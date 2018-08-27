using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Common.Lib.Common.Log
{
    public class LogObject : IXmlSerializable
    {
        public LogObject()
        {
            this.CurrentDataObjects = new List<object>();
        }

        public string Message
        {
            get;
            set;
        }       

        public List<object> CurrentDataObjects
        {
            get;
            set;
        }


        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            //Dump out message, then unique items in the list.
            writer.WriteStartElement("Message");
            writer.WriteString(this.Message);
            writer.WriteEndElement();

            writer.WriteStartElement("CurrentDataObjects");

            if (this.CurrentDataObjects != null)
            {
                XmlSerializer serializer;
                foreach (object o in this.CurrentDataObjects)
                {
                    serializer = new XmlSerializer(o.GetType());
                    serializer.Serialize(writer, o, namespaces);
                }
            }

            writer.WriteEndElement();
        }
    }
}
