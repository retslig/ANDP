using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Common.Lib.Extensions;
using Newtonsoft.Json.Linq;

namespace Common.Lib.Utility
{
    //Dictionary isn't serializable and I want it to be...

    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {

        #region IXmlSerializable Members
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            bool wasEmpty = reader.IsEmptyElement;

            if (wasEmpty)
                return;

            var xml = reader.ReadOuterXml();
            var doc = XDocument.Parse(xml);
            SpanXDocument(this, doc.Root);
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            foreach (var key in this.Keys)
            {
                try
                {
                    writer.WriteStartElement(key.ToString());
                    TValue value = this[key];
                    var type = value.GetType();
                    if (value == null)
                    {
                        writer.WriteValue("");
                    }
                    else if (type == typeof(JObject))
                    {
                        WriteDictionaryObject(value as JObject, writer);
                    }
                    else if (type == typeof(JArray))
                    {
                        WriteJArray(value as JArray, writer, key.ToString());
                    }
                    else if (type == typeof(JValue))
                    {
                        throw new NotImplementedException("JValue");
                    }
                    else if (type == typeof(JToken))
                    {
                        throw new NotImplementedException("JToken");
                    }
                    else if (type == typeof(SerializableDictionary<TKey, TValue>))
                    {
                        WriteSerializableObject(value as SerializableDictionary<TKey, TValue>, writer);
                    }
                    else
                    {
                        writer.WriteValue(value.ToString());
                    }

                    writer.WriteEndElement();
                }
                catch (Exception ex)
                {
                    throw new Exception("Property: " + key + " cannot be null.", ex);
                }
            }
        }

        private void WriteJArray(JArray obj, System.Xml.XmlWriter writer, string elementName)
        {
            //If the child elements have the same name as parent remove s to make it not plural
            if (elementName.EndsWith("s"))
            {
                elementName = elementName.Substring(0, elementName.Length - 1);
            }
            else
            {
                elementName = elementName + "Child";
            }
            
            //This may not handle every scenario where the child is not jobject but just a value...
            //obj.Children()
            foreach (JObject jobj in obj.Children<JObject>())
            {
                writer.WriteStartElement(elementName);
                WriteDictionaryObject(jobj, writer);
                writer.WriteEndElement();
            }
        }

        private void WriteDictionaryObject(JObject obj, System.Xml.XmlWriter writer)
        {
            var dict = obj.ToDictionary();
            foreach (var key in dict.Keys)
            {
                writer.WriteStartElement(key);
                var value = dict[key];
                value = value ?? "";
                var type = value.GetType();
                if (value == null)
                {
                    writer.WriteValue("");
                }
                else if (type == typeof(JObject))
                {
                    WriteDictionaryObject(value as JObject, writer);
                }
                else if (type == typeof(JArray))
                {
                    WriteJArray(value as JArray, writer, key);
                }
                else if (type == typeof(JValue))
                {
                    throw new NotImplementedException("JValue");
                }
                else if (type == typeof(JToken))
                {
                    throw new NotImplementedException("JToken");
                }
                else if (type == typeof(SerializableDictionary<TKey, TValue>))
                {
                    WriteSerializableObject(value as SerializableDictionary<TKey, TValue>, writer);
                }
                else
                {
                    writer.WriteValue(value.ToString());
                }
                
                writer.WriteEndElement();
            }
        }

        private void WriteSerializableObject(SerializableDictionary<TKey, TValue> obj, System.Xml.XmlWriter writer)
        {
            foreach (var k in obj.Keys)
            {
                writer.WriteStartElement(k.ToString());
                var v = obj[k];
                //Check for dict then loop
                if (v is SerializableDictionary<TKey, TValue>)
                    WriteSerializableObject(v as SerializableDictionary<TKey, TValue>, writer);
                else
                    writer.WriteValue(v);
                writer.WriteEndElement();
            }
        }

        private void SpanXDocument(SerializableDictionary<TKey, TValue> dict, XElement elements)
        {
            foreach (var element in elements.Elements())
            {
                if (dict == null)
                {
                    dict = new SerializableDictionary<TKey, TValue>();
                }

                if (element.Descendants().Any())
                {
                    var newChild = new SerializableDictionary<TKey, TValue>();

                    //Check Descendants and see if there are elements with same name if so create list.
                    if (CheckForRepeatElementNames(element.Descendants()))
                    {
                        CreateListOfDictionaries(dict, element);
                    }
                    else
                    {
                        dict.Add((TKey)(object)element.Name.LocalName, (TValue)(object)newChild);
                        SpanXDocument(newChild, element);
                    }
                }
                else
                {
                    //types like instructor can contain one or more items
                    //when more than one must make a list
                    if (dict.ContainsKey((TKey)(object)element.Name.LocalName))
                    {
                        CreateListOfDictionaries(dict, element);
                    }
                    else
                    {
                        //add to a dictionary
                        dict.Add((TKey)(object)element.Name.LocalName, GetValue(element));
                    }
                }
            }
        }

        private TValue GetValue(XElement element)
        {
            Int64 intValue;
            if (Int64.TryParse(element.Value, out intValue))
            {
                return (TValue)(object)intValue;
            }

            bool boolValue;
            if (bool.TryParse(element.Value, out boolValue))
            {
                return (TValue)(object)boolValue;
            }

            DateTime dtValue;
            if (DateTime.TryParse(element.Value, out dtValue))
            {
                return (TValue)(object)dtValue;
            }

            decimal decValue;
            if (decimal.TryParse(element.Value, out decValue))
            {
                return (TValue)(object)decValue;
            }

            return (TValue)(object)element.Value;
        }

        private void CreateListOfDictionaries(SerializableDictionary<TKey, TValue> dict, XElement elements)
        {
            var listElements = new List<SerializableDictionary<TKey, TValue>>();

            foreach (var element in elements.Elements())
            {
                if (element.Descendants().Any())
                {
                    var newChild = new SerializableDictionary<TKey, TValue>();

                    SpanXDocument(newChild, element);

                    listElements.Add(newChild);
                }
                else
                {
                    listElements.Add(new SerializableDictionary<TKey, TValue>
                    {
                        {
                            (TKey) (object) element.Name.LocalName,
                            GetValue(element)
                        }
                    });
                }

                //Note use elements.Name.LocalName not element.Name.LocalName the elements is the parent.
                dict[(TKey) (object) elements.Name.LocalName] = (TValue) (object) listElements;
            }
        }

        private bool CheckForRepeatElementNames(IEnumerable<XElement> descendants)
        {
            return descendants.GroupBy(x => new {x.Name.LocalName}).Any(x => x.Skip(1).Any());
        }

        #endregion
    }
}
