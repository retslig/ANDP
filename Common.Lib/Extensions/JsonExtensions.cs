using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;

namespace Common.Lib.Extensions
{
    public static class JsonSerializerExtension
    {
        public static string SerializeObjectToJsonString<T>(this T obj)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                var encoding = new UTF8Encoding();
                return encoding.GetString(stream.ToArray());
            }
        }

        public static T DeserializeJsonStringToObject<T>(this string stringJson)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(stringJson)))
            {
                return (T)serializer.ReadObject(stream);
            }
        }

        public static bool ValidateJson(this string stringJson)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(stringJson)))
            {
                stream.Position = 0;
                using (var jsonReader = JsonReaderWriterFactory.CreateJsonReader(stream, XmlDictionaryReaderQuotas.Max))
                {
                    try
                    {
                        jsonReader.Read();
                        var myXmlDocument = new XmlDocument();
                        myXmlDocument.LoadXml(jsonReader.ReadOuterXml());
                        return true;
                    }
                    catch (XmlException)
                    {
                        return false;
                    }
                }
            }
        }

        public static string FormatJson(this string stringJson)
        {
            //Example Output:
            //{
            //  "code":"example-subscriber",
            //  "attributes": 
            //  {
            //    "Subscriber.Address.1.City":"Vancouver",
            //    "Subscriber.Address.1.PostalCode":"98682",
            //    "Subscriber.Address.1.State":"WA",
            //    "Subscriber.Address.1.Street":"123 Main St.",
            //    "Subscriber.Address.1.Type":"Work",
            //    "Subscriber.EmailAddress":"subscriber.x@example.com",
            //    "Subscriber.FullName":"Example Subscriber",
            //    "Subscriber.Phone.1.Number":"111-222-3333",
            //    "Subscriber.Phone.1.Type":"Work"
            //  },
            //  "credentials": 
            //  {
            //    "login":"1112223333",
            //    "password":"secret-shhh!"
            //  },
            //  "labels":[
            //  {
            //    "name":"active"
            //  }],
            //  "subscriptions": [
            //  {
            //    "device":
            //    {
            //       "sn":"000000BAD917",
            //       "oui":"000000",
            //       "labels": []
            //    },
            //    "settings":{ },
            //    "services":[]
            //  }]
            //}

            if (!ValidateJson(stringJson))
                throw new Exception("Json must be valid prior to formating.");

            var outputstring = new StringBuilder();
            int indexIndent = 0;

            foreach (char mychar in stringJson)
            {
                switch (mychar)
                {
                    case '{':
                        indexIndent++;
                        outputstring.Append(mychar + Environment.NewLine + JsonIndentfiller(indexIndent));
                        break;
                    case '}':
                        indexIndent--;
                        outputstring.Append(Environment.NewLine + JsonIndentfiller(indexIndent) + mychar);
                        break;
                    case ',':
                        outputstring.Append(mychar + Environment.NewLine + JsonIndentfiller(indexIndent));
                        break;
                    case '\r':
                        //Get rid of this character.
                        break;
                    case '\n':
                        //Get rid of this character.
                        break;
                    case ' ':
                        //Get rid of this character.
                        break;
                    default:
                        outputstring.Append(mychar);
                        break;
                }
            }

            return outputstring.ToString();
        }

        internal static string JsonIndentfiller(int indentLevel)
        {
            return new string(' ', indentLevel * 2);
            //string sPadding = "";
            //return sPadding.PadLeft(IndentLevel * 2, ' ');
        }
    }
}
