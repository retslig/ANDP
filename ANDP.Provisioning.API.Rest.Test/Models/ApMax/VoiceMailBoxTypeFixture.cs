using System.Collections.Generic;
using System.Reflection;
using ANDP.Provisioning.API.Rest.Models.ApMax;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ANDP.Provisioning.API.Rest.Test.Models.ApMax
{
    [TestClass]
    public class VoiceMailBoxTypeFixture
    {

        private ICommonMapper _commonMapper;

        [TestInitialize]
        public void Initialize()
        {
            var settings = new List<CommonMapperProfileResolutionSettings>
            {
                new CommonMapperProfileResolutionSettings
                    {
                        AssemblyName = Assembly.GetAssembly(typeof(VoiceMailBoxType)).GetName().Name,
                        Namespace = null
                    }
            };

            _commonMapper = new CommonMapper(settings);
        }


        [TestMethod]
        public void Can_Map_Provisioning_API_VoiceMailBoxType_To_ApMax_VoicemailV3_VoiceMailBoxType()
        {
            //*** Arrange ***
            var voiceMailBoxType = new VoiceMailBoxType
            {
                    Description = "test mailbox description",
                    MailBoxType = MailboxType.Normal,
                    MaxMailBoxTime = 45,
                    MaxMessageLength = 12,
                    MessageCount = 14,
                    Notifications = new List<NotificationInfoType>
                    {
                        new NotificationInfoType
                        {
                            Address = "ddddddd",
                            Center = 23,
                            Enabled = false
                        },
                        new NotificationInfoType
                        {
                            Address = "23232323",
                            Center = 213,
                            Enabled = true
                        }
                    }
            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<VoiceMailBoxType, Common.VoicemailV3.VoiceMailBoxType>(_commonMapper, voiceMailBoxType);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Can_Map_Provisioning_API_VoiceMailBoxType_To_ApMax_VoicemailV4_VoiceMailBoxType()
        {
            //*** Arrange ***
            var voiceMailBoxType = new VoiceMailBoxType
            {
                Description = "test mailbox description",
                MailBoxType = MailboxType.Normal,
                MaxMailBoxTime = 45,
                MaxMessageLength = 12,
                MessageCount = 14,
                Notifications = new List<NotificationInfoType>
                    {
                        new NotificationInfoType
                        {
                            Address = "ddddddd",
                            Center = 23,
                            Enabled = false
                        },
                        new NotificationInfoType
                        {
                            Address = "23232323",
                            Center = 213,
                            Enabled = true
                        }
                    }
            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<VoiceMailBoxType, Common.VoicemailV4.VoiceMailBoxType>(_commonMapper, voiceMailBoxType);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Can_Map_Provisioning_API_VoiceMailBoxType_To_ApMax_VoicemailV5_VoiceMailBoxType()
        {
            //*** Arrange ***
            var voiceMailBoxType = new VoiceMailBoxType
            {
                Description = "test mailbox description",
                MailBoxType = MailboxType.Normal,
                MaxMailBoxTime = 45,
                MaxMessageLength = 12,
                MessageCount = 14,
                Notifications = new List<NotificationInfoType>
                    {
                        new NotificationInfoType
                        {
                            Address = "ddddddd",
                            Center = 23,
                            Enabled = false
                        },
                        new NotificationInfoType
                        {
                            Address = "23232323",
                            Center = 213,
                            Enabled = true
                        }
                    }
            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<VoiceMailBoxType, Common.VoicemailV5.VoiceMailBoxType>(_commonMapper, voiceMailBoxType);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Can_Map_ApMax_VoicemailV3_VoiceMailBoxType_To_Provisioning_API_VoiceMailBoxType()
        {
            //*** Arrange ***
            var voiceMailBoxType = new Common.VoicemailV3.VoiceMailBoxType
            {
                MessageCountField = 3,
                MaxMessageLengthField = 2,
                DescriptionField = "sssssss",

            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.VoicemailV3.VoiceMailBoxType, VoiceMailBoxType>(_commonMapper, voiceMailBoxType);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Can_Map_ApMax_VoicemailV4_VoiceMailBoxType_To_Provisioning_API_VoiceMailBoxType()
        {
            //*** Arrange ***
            var voiceMailBoxType = new Common.VoicemailV4.VoiceMailBoxType
            {
                MessageCountField = 3,
                MaxMessageLengthField = 2,
                DescriptionField = "sssssss",

            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.VoicemailV4.VoiceMailBoxType, VoiceMailBoxType>(_commonMapper, voiceMailBoxType);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Can_Map_ApMax_VoicemailV5_VoiceMailBoxType_To_Provisioning_API_VoiceMailBoxType()
        {
            //*** Arrange ***
            var voiceMailBoxType = new Common.VoicemailV5.VoiceMailBoxType
            {
                MessageCountField = 3,
                MaxMessageLengthField = 2,
                DescriptionField = "sssssss",

            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.VoicemailV5.VoiceMailBoxType, VoiceMailBoxType>(_commonMapper, voiceMailBoxType);
            Assert.IsNotNull(result);
        }
    }
}
