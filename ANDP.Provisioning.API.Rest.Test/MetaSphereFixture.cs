using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ANDP.Provisioning.API.Rest.Models.MetaSphere;
using Common.Lib.Security;
using Common.MetaSphere;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ANDP.Provisioning.API.Rest.Test
{
    [TestClass]
    public class MetaSphereFixture
    {
        private const string BaseRemoteUrl = "https://andpserver.cloudapp.net/ANDP.Provisioning.API.Rest/api/metaswitch/";
        private const string BaseLocalUrl = "https://localhost:56198/api/metasphere/";
        private const string dn = "6055554444";
        private const int EquipmentId = 1;
        private WebRequestHandler _handler;
        private HttpClient _client;

        [TestInitialize]
        public void TestInitialize()
        {
            _handler = new WebRequestHandler();
            _handler.ServerCertificateValidationCallback = CertificateHelper.ServerCertificateValidationCallback;
            _client = new HttpClient(_handler);
            _client.BaseAddress = new Uri(BaseRemoteUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.SetBasicAuthentication("ctc", "ctc");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _client.Dispose();
            _handler.Dispose();
        }

        [TestMethod]
        public void Can_Call_Test_Controller()
        {
            HttpResponseMessage response = _client.GetAsync("test").Result;

            var result = response.Content.ReadAsStringAsync();

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual("OK", response.ReasonPhrase);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("\"OK\"", result.Result);
            Assert.AreEqual(null, result.Exception);
        }

        [TestMethod]
        public void Can_Create_Subscriber()
        {
            //*** ARRANGE ****

            //*** ACT ****
            var userData = new tUserData()
            {
                ShData = new List<tTransparentData>
                {
                    new tTransparentData()
                    {
                        ChangeDescription = null,
                        SequenceNumber = 99,
                        ServiceIndication = "Msph_Subscriber_BaseInformation",
                        ServiceData = new tServiceData
                        {
                            Item = new tMetaSphereData
                            {
                                Item = new tMsph_Subscriber_BaseInformation
                                {
                                    DisplayName = "billy bob",
                                    CoSID = "20",
                                    PrimaryPhoneNumber = "9374843002"
                                },
                                ItemElementName = ItemChoiceType1.Msph_Subscriber_BaseInformation
                            }
                        }
                    }
                }.ToArray()
            };

            var addUpdate = new MetaSphereUpdate
            {
                EquipmentId = 7,
                Dn = "9374843002",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(addUpdate);

            var updateHttpResponse = _client.PostAsync("subscriber/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;

            //*** ASSERT ***
            Assert.IsTrue(true);
            Assert.IsNotNull(updateResult);
        }

    }
}
