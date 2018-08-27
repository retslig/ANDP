
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Common.Lib.Extensions;
using Common.ProcessMakerService;

namespace Common.NewNet.Services
{
    public class ProcessMakerService : IProcessMakerService
    {
        private readonly ProcessMakerServiceSoapClient _clientServiceSoapClient;
        private readonly string  _sessionId = "";

        public ProcessMakerService(Lib.Domain.Common.Models.EquipmentConnectionSetting settings)
        {
            string version;
            string timestamp;
            string message;
            //md5:26ee92d7cbf8d81eff53ab0a943edf50
            //wsdl: http://newnetcases.net/sysworkflow/en/classic/services/wsdl2
            //soap endpoint: http://newnetcases.net:80/sysworkflow/en/classic/services/soap2

            var messegeElement = new TextMessageEncodingBindingElement
            {
                MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None),
                //ReaderQuotas =
                //{
                //    MaxArrayLength = 200000,
                //    MaxBytesPerRead = 200000,
                //    MaxDepth = 200000,
                //    MaxNameTableCharCount = 200000,
                //    MaxStringContentLength = 200000
                //}
            };

            CustomBinding binding;
            if (settings.Url.ToLower().Contains("https"))
            {
                binding = new CustomBinding(messegeElement, new HttpsTransportBindingElement())
                {
                    Name = "ProcessMakerServiceSoap",
                };
            }
            else
            {
                binding = new CustomBinding(messegeElement, new HttpTransportBindingElement())
                {
                    Name = "ProcessMakerServiceSoap",
                }; 
            }

            _clientServiceSoapClient = new ProcessMakerServiceSoapClient(binding, new EndpointAddress(settings.Url));
        
            string pass = "md5:" + settings.Password.GetMd5Hash();
            var result = _clientServiceSoapClient.login(settings.Username, pass, out message, out version, out timestamp);
            if (!result.Trim().Equals("0"))
                throw new Exception(message);

            _sessionId = message;
        }

        public void RouteCase(string caseId, string routeIndex)
        {
            string message;
            string timestamp;
            routeListStruct[] routing;
            var result = _clientServiceSoapClient.routeCase(_sessionId, caseId, routeIndex, out message, out timestamp, out routing);
            if (!result.Trim().Equals("0"))
                throw new Exception(message);
        }


        public void ExecuteTrigger(string caseId, string triggerId, string routeIndex)
        {
            string message;
            string timestamp;
            var result = _clientServiceSoapClient.executeTrigger(_sessionId, caseId, triggerId, routeIndex, out message, out timestamp);
            if (!result.Trim().Equals("0"))
                throw new Exception(message);
        }

        public void CreateCaseNote(string caseId, string processid, string taskid, string userid, string note)
        {
            string[] sendEmail = new string[0];
            string message;
            string timestamp;
            var result = _clientServiceSoapClient.addCaseNote(_sessionId, caseId, processid, taskid, userid, note, sendEmail, out message, out timestamp);
            if (!result.Trim().Equals("0"))
                throw new Exception(message);
        }

        public void CreateResponse(string caseId, string responseMessage, string responseCode, List<variableListStruct> variableList)
        {
            string message;
            string timestamp;
            variableList.Add(new variableListStruct
            {
                name = "ResponseCode",
                value = responseCode
            });
            variableList.Add(new variableListStruct
            {
                name = "ResponseMessage",
                value = responseMessage
            });

            var result = _clientServiceSoapClient.sendVariables(_sessionId, caseId, variableList.ToArray(), out message, out timestamp);
            if (!result.Trim().Equals("0"))
                throw new Exception(message);
        }

        public void AttachDocument()
        {
            //<?php
 
            /////Send the file
            //  $params = array (
            //    'ATTACH_FILE'  => '@/Users/newandrew/Desktop/TroubleSMS.txt',
            //    'APPLICATION'  => '23352050753ffa338613b13082225618',
            //    'INDEX'        => '1',
            //    'USR_UID'      => '11230468753f4bbb0c7c743048286547',
            //    'DOC_UID'      => '70159912153ff9c70b80655067185863',
            //    'APP_DOC_TYPE' => 'INPUT',
            //    'TITLE'        => "12345",
            //    'COMMENT'      => "AP Max Output");
            //ob_flush();
            //$ch = curl_init();
            //curl_setopt($ch, CURLOPT_URL, 'http://localhost/syssantarosa/en/green/services/upload');
            ////curl_setopt($ch, CURLOPT_VERBOSE, 1);  //Uncomment to debug
            //curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
            //curl_setopt($ch, CURLOPT_POST, 1);
            //curl_setopt($ch, CURLOPT_POSTFIELDS, $params);
            //// curl_setopt ($ch, CURLOPT_SSL_VERIFYHOST, 1); //Uncomment for SSL
            //// curl_setopt ($ch, CURLOPT_SSL_VERIFYPEER, 1); //Uncomment for SSL
            //$response = curl_exec($ch);
            //curl_close($ch);
            //print $response;
               
            //?>

            throw new NotImplementedException();
        }

        public caseListStruct[] RetrieveCaseList()
        {
            return _clientServiceSoapClient.caseList(_sessionId);
        }
    }
}
