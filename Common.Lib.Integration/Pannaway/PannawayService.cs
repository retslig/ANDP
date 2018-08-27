using System;
using System.Collections.Generic;
using System.Xml;
using Common.Lib.Common.Enums;
using Common.Lib.Domain.Common.Models;
using Common.Lib.Extensions;
using Common.Lib.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Formatting = Newtonsoft.Json.Formatting;

namespace Common.Pannaway
{
    public class PannawayService
    {
        private readonly string _url;
        private readonly string _userName;
        private readonly string _password;
        private readonly ILogger _logger;
        private readonly string _tenantId;

        public PannawayService(Guid tenantId, EquipmentConnectionSetting setting, ILogger logger)
        {
            _url = setting.Url;
            _userName = setting.Username;
            _password = setting.Password;
            _logger = logger;
            _tenantId = tenantId.ToString();

            _logger.WriteLogEntry(_tenantId, new List<object> { _url }, "PannawayService URL:", LogLevelType.Info);
        }

        public JObject PostMessage(JToken jobject)
        {
            var doc = JsonConvert.DeserializeXmlNode(jobject.ToString(Formatting.None));
            var client = new RestClient(_url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "text/xml");
            request.AddHeader("authorization",
                "Basic " +
                Convert.ToBase64String(
                    System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(_userName + ":" + _password)));
            request.AddParameter("text/xml", doc.OuterXml, ParameterType.RequestBody);
            var response = client.Execute(request);

            if (!response.IsSuccessful())
            {
                throw new Exception("Content: " + response.Content + 
                    "\r\nError Message:" + response.ErrorMessage + 
                    "\r\nError Exception:" + response.ErrorException +
                    "\r\nStatus Code:" + response.StatusCode +
                    "\r\nStatus Description:" + response.StatusDescription
                    );
            }
            
            if (string.IsNullOrEmpty(response.Content))
                throw new Exception("No response from Pannaway.");

            doc = new XmlDocument();
            doc.LoadXml(response.Content);
            return JObject.Parse(JsonConvert.SerializeXmlNode(doc));
        }
    }
}
