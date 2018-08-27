using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using Common.Lib.Security;
using Newtonsoft.Json;

namespace Common.NeoNova.Services
{
    public class NeoNovaService
    {
        private readonly string _url;
        private readonly string _username;
        private readonly string _password;
        private readonly string _domain;

        public NeoNovaService(string url, string username, string password, string domain)
        {
            _url = url;
            _username = username;
            _password = password;
            _domain = domain;
        }

        private string ConvertXmlStringToJsonString(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var json = JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        private string ConvertJsonStringToXmlString(string json)
        {
            //var json = JObject.Parse(tokenResponse);
            //return json["access_token"].ToString();

            var xml = JsonConvert.DeserializeXmlNode(json).OuterXml;
            return xml;
        }

        public string PostMessage(string jsonRequest)
        {
            var auth = "<?xml version=\x221.0\x22 encoding=\x22UTF-8\x22 ?>" +
                       "<authenticate>" +
                       "<user>" + _username + "</user>" +
                       "<pass>" + _password + "</pass>" +
                       "<domain>" + _domain + "</domain>" +
                       "</authenticate>";

            string xml = auth + ConvertJsonStringToXmlString(jsonRequest);

            using (var handler = new WebRequestHandler())
            {
                handler.ServerCertificateValidationCallback = CertificateHelper.ServerCertificateValidationCallback;

                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.BaseAddress = new Uri(_url);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                    //httpClient.SetBasicAuthentication(_username, _password);
                    HttpResponseMessage response = httpClient.PostAsync("", new StringContent(xml, Encoding.UTF8, "text/xml")).Result;
                    var result = response.Content.ReadAsStringAsync().Result;
                    return ConvertXmlStringToJsonString(result);
                }
            }
        }
    }
}
