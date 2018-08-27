using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ANDP.API.Rest.Models;
using ANDP.API.Rest.Models;
using Common.Lib.Common.Enums;
using Common.Lib.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ANDP.API.Rest.Test.Controllers
{
    [TestClass]
    public class EngineControllerFixture
    {


        private const string BaseRemoteUrl = "http://andprestapi.azurewebsites.net/api/engine/";
        private const string BaseLocalUrl = "http://localhost:56198/api/engine/";
        private HttpClient _client;


        [TestInitialize]
        public void TestInitialize()
        {
            //_container = BootStrapper.Initialize();
            //_iProvisioningEngineService = _container.Resolve<IProvisioningEngineService>();
            _client = new HttpClient();
            _client.BaseAddress = new Uri(BaseRemoteUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); //making sure to accept json
        }

        [TestMethod]
        public void RetrieveEquipmentInformation()
        {

            //*** Arrange ***

            //*** Act ***
            HttpResponseMessage response = _client.GetAsync("equipment/equipmentid/2").Result;

            var reasonPhrase = response.ReasonPhrase;
            var myResponse = response.Content.ReadAsStringAsync();
            var result = myResponse.Result;

            //*** Assert ***
            //Assert.IsNotNull(json);

        }


        [TestMethod]
        [Ignore] //This will actually send an email so have it setup to be ran manually.
        public void CreateGmailEmail()
        {

            //*** Arrange ***
            var mail = new GmailEmaillDto
            {
                ToEmail = "support@qssolutions.net",
                Subject = "SRTC error",
                Body = "skskksssssssssssss"
            };

            var json = mail.SerializeObjectToJsonString();

            //*** Act ***
            HttpResponseMessage response = _client.PostAsync("email/gmail/", new StringContent(json, Encoding.UTF8, "application/json")).Result;

            var reasonPhrase = response.ReasonPhrase;
            var myResponse = response.Content.ReadAsStringAsync();
            var result = myResponse.Result;

            //*** Assert ***
            Assert.IsNotNull(json);

        }

        [TestMethod]
        //[Ignore] //This will actually send an email so have it setup to be ran manually.
        public void CreateLogEntry()
        {

            //*** Arrange ***
            var log = new LogEntryDto
            {
                Entry = "testsksksks",
                LogLevelType = LogLevelType.Info
            };

            var json = log.SerializeObjectToJsonString();

            //*** Act ***
            //HttpResponseMessage response = _client.PostAsync("log/", new StringContent(json, Encoding.UTF8, "application/json")).Result;

            //var reasonPhrase = response.ReasonPhrase;
            //var myResponse = response.Content.ReadAsStringAsync();
            //var result = myResponse.Result;

            //*** Assert ***
            Assert.IsNotNull(json);

        }

    }
}
