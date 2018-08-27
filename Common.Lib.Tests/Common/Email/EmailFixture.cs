using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Common.Lib.Common.Email;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.Lib.Tests.Common.Email
{
    [TestClass]
    public class EmailFixture
    {
        [TestMethod]
        [Ignore]
        //This test is meant to be ran manually since it'll actually send out a email.
        public void Can_Send_Gmail_Email()
        {
            //*** Arrange ***

            //*** Act ***
            Emailer.SendGmailSupportMessage(new MailAddressCollection{ new MailAddress("support@qssolutions.net", "support")}, "mysubject", "How about you actually get some work done!");

            //*** Assert ***
            //Assert.IsNotNull(result);
        }

        
    }
}
