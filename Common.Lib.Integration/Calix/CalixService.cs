using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Profile;
using System.Xml;
using System.Xml.Linq;
using Common.Lib.Common.Enums;
using Common.Lib.Domain.Common.Models;
using Common.Lib.Interfaces;
using Common.Lib.Utility;
using MySql.Data.MySqlClient.Memcached;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Formatting = Newtonsoft.Json.Formatting;

namespace Common.Calix
{

    public class CalixService
    {
        private readonly string _url;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _action;
        private readonly ILogger _logger;
        private readonly string _tenantId;

        public CalixService(Guid tenantId, EquipmentConnectionSetting setting, ILogger logger)
        {
            _url = setting.Url;
            _userName = setting.Username;
            _password = setting.Password;
            _action = "";
            _logger = logger;
            _tenantId = tenantId.ToString();

            _logger.WriteLogEntry(_tenantId, new List<object> { _url }, "CalixService URL:", LogLevelType.Info);
        }

        public JObject PostMessage(JToken jobject)
        {
            var response = CallWebService(jobject);
            return response;
        }

        public JObject PostMessageRaw(string soapMessage)
        {
            var doc = new XmlDocument();
            doc.LoadXml(soapMessage);
            var jsonResponse = JsonConvert.SerializeXmlNode(doc);

            var response = CallWebService(JObject.Parse(jsonResponse));
            return response;
        }

        public JObject Login()
        {
            //var resultCode = string.Empty;
            //var sessionId = string.Empty;

            //We have to have a seperate login and logout call since the username and password are being
            //passed within the message and that is stored in equipment settings.

            //example response:
            //<?xml version="1.0" encoding="UTF-8"?>
            //<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/">
            // <Header/>
            // <Body>
            // <auth-reply xmlns=””>
            // <ResultCode>0</ResultCode>
            // <SessionID>6</SessionID>
            // </auth-reply>
            // </Body>
            //</soapenv:Envelope>

            const string xml =
              @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <soapenv:Body>
                    <auth message-id=""1"">
                            <login>
                                <UserName>udpuser</UserName>
                                <Password>JgKSRsZVVM</Password>
                            </login>
                        </auth>
                    </soapenv:Body>
                </soapenv:Envelope>";

            var result = PostMessageRaw(xml);
            return result;
        }

        public JObject Logout(int sessionId)
        {
            //We have to have a seperate login and logout call since the username and password are being
            //passed within the message and that is stored in equipment settings.

            var xml =
              @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <soapenv:Body>
                        <auth message-id=""1"">
                            <logout>
                                <UserName>udpuser</UserName>
                                <SessionId>" + sessionId + @"</SessionId>
                            </logout>
                        </auth>
                    </soapenv:Body>
                </soapenv:Envelope>";

            var result = PostMessageRaw(xml);
            return result;
        }

        private JObject CallWebService(JToken content)
        {
            var doc = JsonConvert.DeserializeXmlNode(content.ToString(Formatting.None));
            
            _logger.WriteLogEntry(_tenantId, new List<object> { _url, content }, "CalixService CallWebService:", LogLevelType.Info);

            var response = CreateWebRequest(doc.OuterXml);
            return response;
        }

        private JObject CreateWebRequest(string content)
        {
            using (var client = new HttpClient())
            {
                //httpClient.BaseAddress = new Uri(url);
                using (var request = new HttpRequestMessage(HttpMethod.Post, _url))
                {
                    request.Headers.Add("Accept", "text/xml");
                    request.Headers.Add("ContentType", "text/xml;charset=\"utf-8\"");
                    request.Content = new StringContent(content);

                    using (var response = client.SendAsync(request))
                    {
                        using (var result = response.Result.Content.ReadAsStringAsync())
                        {
                            if (!response.Result.IsSuccessStatusCode)
                            {
                                throw new Exception("Calix Error: " + result.Result);
                            }

                            if (string.IsNullOrEmpty(result.Result))
                            {
                                throw new Exception("No response from Calix.");
                            }

                            var doc = new XmlDocument();
                            doc.LoadXml(result.Result);
                            return JObject.Parse(JsonConvert.SerializeXmlNode(doc));

                            //var res = result.Result;
                            //return res;
                            //var response = client.PostAsync(uri, new StringContent(content, new UTF8Encoding()));
                        }
                    }
                }
            }
        }

//        private XmlDocument CreateSoapEnvelope(string xmlDocument)
//        {
//              var xml = 
//                @"<?xml version=""1.0"" encoding=""UTF-8""?>
//                <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
//                    <soapenv:Body>" +
//                        //<auth message-id=""2"">" + 
//                            xmlDocument + 
//                      //@"</auth>" +
//                  @"</soapenv:Body>
//                </soapenv:Envelope>";

//            var soapEnvelop = new XmlDocument();
//            soapEnvelop.LoadXml(xml);
//            return soapEnvelop;
//        }

        //private static string ConvertJsonStringToXmlString(string json)
        //{
        //    //var json = JObject.Parse(tokenResponse);
        //    //return json["access_token"].ToString();

        //    var xml = JsonConvert.DeserializeXmlNode(json).OuterXml;
        //    return xml;
        //}

    }
}
