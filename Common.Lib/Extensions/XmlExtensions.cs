using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Common.Lib.Extensions
{
    public static class XmlSerializerExtension
    {
        public static T DeSerializeStringToObject<T>(this string sxml)
        {
            using (var xreader = new XmlTextReader(new StringReader(sxml.Replace("&", "&amp;"))))
            {
                var xs = new XmlSerializer(typeof(T));
                return (T)xs.Deserialize(xreader);
            }
        }

        public static string SerializeObjectToString(this object obj)
        {
            using (var stream = new MemoryStream())
            {
                var x = new XmlSerializer(obj.GetType());
                x.Serialize(stream, obj);
                return Encoding.Default.GetString(stream.ToArray());
            }
        }

        public static string SerializeObjectWithNoNameSpaceToString(this object obj)
        {
            using (var stream = new StringWriter())
            {
                var settings = new XmlWriterSettings
                    {
                        Indent = true,
                        OmitXmlDeclaration = true,
                        NewLineOnAttributes = true
                    };

                using (var writer = XmlWriter.Create(stream, settings))
                {
                    var ns = new XmlSerializerNamespaces();
                    ns.Add(string.Empty, string.Empty);
                    var serializer = new XmlSerializer(obj.GetType());
                    serializer.Serialize(writer, obj, ns);
                    return stream.ToString();
                }
            }
        }

        public static XDocument SerializeObjectToXdocumnet(this object obj)
        {
            var xmlSerializer = new XmlSerializer(obj.GetType());

            var doc = new XDocument();
            using (var writer = doc.CreateWriter())
            {
                xmlSerializer.Serialize(writer, obj);
            }

            return doc;
        }

        public static void FillDynamicObject(this XElement node, dynamic parent)
        {
            if (node.HasElements)
            {
                if (node.Elements(node.Elements().First().Name.LocalName).Count() > 1)
                {
                    //list
                    var item = new ExpandoObject();
                    var list = new List<dynamic>();
                    foreach (var element in node.Elements())
                    {
                        FillDynamicObject(element, list);
                    }

                    AddProperty(item, node.Elements().First().Name.LocalName, list);
                    AddProperty(parent, node.Name.ToString(), item);
                }
                else
                {
                    var item = new ExpandoObject();

                    foreach (var attribute in node.Attributes())
                    {
                        AddProperty(item, attribute.Name.ToString(), attribute.Value.Trim());
                    }

                    //element
                    foreach (var element in node.Elements())
                    {
                        FillDynamicObject(element, item);
                    }

                    AddProperty(parent, node.Name.ToString(), item);
                }
            }
            else
            {
                AddProperty(parent, node.Name.ToString(), node.Value.Trim());
            }
        }

        private static void AddProperty(dynamic parent, string name, object value)
        {
            if (parent is List<dynamic>)
            {
                (parent as List<dynamic>).Add(value);
            }
            else
            {
// ReSharper disable once PossibleNullReferenceException
                (parent as IDictionary<String, object>)[name] = value;
            }
        }
    }
}