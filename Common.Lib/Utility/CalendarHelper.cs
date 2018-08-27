using System;
using System.IO;
using System.Net.Mail;
using System.Collections.Generic;
using Common.Lib.Common.Email;

namespace Common.Lib.Utility
{
	public static class CalendarHelper
	{
		//Could use Exchange Web Services Operations to add it directly to the user...
		//http://msdn.microsoft.com/en-us/library/aa564690(v=exchg.140).aspx


		/// <summary>
		/// Creates the ICS calendar event.
		/// Note by click on it this will import it to outlook.
		/// to import to gmail see this instructions: http://support.google.com/calendar/bin/answer.py?hl=en&answer=37118
		/// standards info : http://en.wikipedia.org/wiki/ICalendar
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="body">The body.</param>
		/// <param name="priority">The priority.</param>
		/// <param name="busy">If you want the meeting to show busy </param>
		/// <param name="beginDate">The begin date.</param>
		/// <param name="endDate">The end date.</param>
		/// <returns>The File path</returns>
		public static string CreateICSCalendarEvent(string location, string subject, string body, int priority, bool busy, DateTime beginDate, DateTime endDate)
		{
			string[] contents = GenerateCalendarICSFormatString(location, subject, body, priority, busy, beginDate, endDate, 0);
			string fileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".ics");
			File.WriteAllLines(fileName, contents);
			return fileName;
		}

		/// <summary>
		/// Creates the ICS calendar event.
		/// Note by click on it this will import it to outlook.
		/// to import to gmail see this instructions: http://support.google.com/calendar/bin/answer.py?hl=en&answer=37118
		/// standards info : http://en.wikipedia.org/wiki/ICalendar
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="body">The body.</param>
		/// <param name="priority">The priority.</param>
		/// <param name="busy">If you want the meeting to show busy </param>
		/// <param name="beginDate">The begin date.</param>
		/// <param name="endDate">The end date.</param>
		/// <param name="reminderTime"> </param>
		/// <returns>The File path</returns>
		public static string CreateICSCalendarEvent(string location, string subject, string body, int priority, bool busy, DateTime beginDate, DateTime endDate, int reminderTime)
		{
			string[] contents = GenerateCalendarICSFormatString(location, subject, body, priority, busy, beginDate, endDate, reminderTime);
			string fileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".ics");
			File.WriteAllLines(fileName, contents);
			return fileName;
		}

		/*/// <summary>
		/// Emails the calendar event.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="body">The body.</param>
		/// <param name="priority">The priority.</param>
		/// <param name="busy">if set to <c>true</c> [busy].</param>
		/// <param name="beginDate">The begin date.</param>
		/// <param name="endDate">The end date.</param>
		/// <param name="reminderTime">The reminder time.</param>
		/// <param name="emailTo">The email to.</param>
		public static void EmailICSCalendarEvent(string location, string subject, string body, int priority, bool busy, DateTime beginDate, DateTime endDate, int reminderTime, string emailTo)
		{
			using (MemoryStream ms = CalendarHelper.GenerateCalendarICSMemoryStream("Desk", "test", "bodytest", 3, true, DateTime.Now, DateTime.Now.AddHours(1), 15) as MemoryStream)
			{
				Emailer.SendMessage(emailTo, "", "test", "subject", "body", false, ".com", new List<Attachment> { new Attachment(ms, Guid.NewGuid() + ".ics") });
			}
		}*/

	    /// <summary>
	    /// Emails the calendar event.
	    /// </summary>
	    /// <param name="location">The location.</param>
	    /// <param name="subject">The subject.</param>
	    /// <param name="body">The body.</param>
	    /// <param name="priority">The priority.</param>
	    /// <param name="busy">if set to <c>true</c> [busy].</param>
	    /// <param name="beginDate">The begin date.</param>
	    /// <param name="endDate">The end date.</param>
	    /// <param name="reminderTime">The reminder time.</param>
	    /// <param name="emailTo">The email to.</param>
	    /// <param name="emailFrom">The email from. </param>
	    /// <param name="emailCc">The email CC. </param>
	    /// <param name="inviteName"> Name of calendar event</param>
	    public static void EmailIcsCalendarEvent(string location, string subject, string body, int priority, bool busy, DateTime beginDate, DateTime endDate, int reminderTime, string emailFrom, string emailTo, string emailCc, string inviteName)
        {
            using (var ms = CalendarHelper.GenerateCalendarIcsMemoryStream(location, subject, body, priority, true, beginDate, endDate, reminderTime) as MemoryStream)
            {
                var toEmailsList = new MailAddressCollection();
                foreach (var email in emailTo.Split(';'))
                {
                    toEmailsList.Add(new MailAddress(email));
                }

                Emailer.SendGmailSupportMessage(toEmailsList, subject, body, new List<string> { inviteName + ".ics" });
            }
        }

		/// <summary>
		/// Generates the calendar ICS memory stream.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="body">The body.</param>
		/// <param name="priority">The priority.</param>
		/// <param name="busy">if set to <c>true</c> [busy].</param>
		/// <param name="beginDate">The begin date.</param>
		/// <param name="endDate">The end date.</param>
		/// <param name="reminderTime">The reminder time.</param>
		/// <returns></returns>
		public static Stream GenerateCalendarIcsMemoryStream(string location, string subject, string body, int priority, bool busy, DateTime beginDate, DateTime endDate, int reminderTime)
		{
			string[] contents = GenerateCalendarICSFormatString(location, subject, body, priority, busy, beginDate, endDate, reminderTime);

			var ms = new MemoryStream(); 
			TextWriter tw = new StreamWriter(ms);
			foreach (var content in contents)
			{
				tw.WriteLine(content);
			}
			tw.Flush();
			ms.Seek(0, SeekOrigin.Begin);
			return ms;
		}

		/// <summary>
		/// Generates the calendar ICS format string.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="body">The body.</param>
		/// <param name="priority">The priority.</param>
		/// <param name="busy">if set to <c>true</c> [busy].</param>
		/// <param name="beginDate">The begin date.</param>
		/// <param name="endDate">The end date.</param>
		/// <param name="reminderTime">if greater than 0 than add reminder fo this time. </param>
		/// <returns></returns>
		private static string[] GenerateCalendarICSFormatString(string location, string subject, string body, int priority, bool busy, DateTime beginDate, DateTime endDate, int reminderTime)
		{ 
			return new[]
					   {
						   "BEGIN:VCALENDAR",
						   //"PRODID:-//Microsoft Corporation//Outlook 14.0 MIMEDIR//EN",
						   "PRODID:-//Raven//Test App//EN",
						   "VERSION:2.0",
						   "CALSCALE:GREGORIAN",
						   "METHOD:PUBLISH",
						   //"X-WR-CALNAME:Calendar - Nathan.Gilster@ravenind.com",
						   "X-WR-TIMEZONE:Central Standard Time",
						   "X-WR-CALDESC:",
						   //"BEGIN:VTIMEZONE",
						   //"TZID:Central Standard Time",
						   "BEGIN:VEVENT",
						   "CLASS:PUBLIC",
						   "DTSTART:" + beginDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
						   "DTEND:" + endDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
						   "DTSTAMP:" + DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
						   "SEQUENCE:0",
						   "STATUS:CONFIRMED",
						   "TRANSP:OPAQUE",
						   "LOCATION:" + location,
						   "DESCRIPTION;ENCODING=QUOTED-PRINTABLE:" + body,
						   "SUMMARY:" + subject, "PRIORITY:" + priority,
						   //"SUMMARY;LANGUAGE=en-us:Busy",
						   "UID:" + Guid.NewGuid(),
						   busy ? "X-MICROSOFT-CDO-BUSYSTATUS:BUSY" : "",
						   reminderTime > 0 ? "BEGIN:VALARM" : "",
						   reminderTime > 0 ?"TRIGGER:-PT" + reminderTime + "M" : "", 
						   reminderTime > 0 ?"ACTION:DISPLAY" : "",
						   reminderTime > 0 ?"DESCRIPTION:Reminder" : "",
						   reminderTime > 0 ?"END:VALARM" : "",
						   "END:VEVENT",
						   "END:VCALENDAR"
					   };

			//Example Format
			//http://msdn.microsoft.com/en-us/library/ee158284(v=exchg.80).aspx
			//BEGIN:VCALENDAR
			//PRODID:-//Microsoft Corporation//Outlook 14.0 MIMEDIR//EN
			//VERSION:2.0
			//METHOD:PUBLISH
			//X-CALSTART:20120406T200000Z
			//X-CALEND:20120406T220000Z
			//X-CLIPSTART:20120406T050000Z
			//X-CLIPEND:20120407T050000Z
			//X-WR-RELCALID:{0000002E-8126-E659-6F4E-C8C0FF35C8BE}
			//X-WR-CALNAME:Nathan Gilster
			//X-PRIMARY-CALENDAR:TRUE
			//X-OWNER;CN="Nathan Gilster":mailto:Nathan.Gilster@ravenind.com
			//X-MS-OLK-WKHRSTART;TZID="Central Standard Time":080000
			//X-MS-OLK-WKHREND;TZID="Central Standard Time":170000
			//X-MS-OLK-WKHRDAYS:MO,TU,WE,TH,FR
			//BEGIN:VTIMEZONE
			//TZID:Central Standard Time
			//BEGIN:STANDARD
			//DTSTART:16011104T020000
			//RRULE:FREQ=YEARLY;BYDAY=1SU;BYMONTH=11
			//TZOFFSETFROM:-0500
			//TZOFFSETTO:-0600
			//END:STANDARD
			//BEGIN:DAYLIGHT
			//DTSTART:16010311T020000
			//RRULE:FREQ=YEARLY;BYDAY=2SU;BYMONTH=3
			//TZOFFSETFROM:-0600
			//TZOFFSETTO:-0500
			//END:DAYLIGHT
			//END:VTIMEZONE
			//BEGIN:VEVENT
			//DTEND:20120406T210000Z
			//DTSTAMP:20120405T161330Z
			//DTSTART:20120406T200000Z
			//SEQUENCE:0
			//SUMMARY;LANGUAGE=en-us:Busy
			//TRANSP:OPAQUE
			//UID:MtMLQHCj8k26LEdyE4bVTA==
			//X-MICROSOFT-CDO-BUSYSTATUS:BUSY
			//END:VEVENT
			//BEGIN:VEVENT
			//DTEND:20120406T220000Z
			//DTSTAMP:20120405T161330Z
			//DTSTART:20120406T214500Z
			//SEQUENCE:0
			//SUMMARY;LANGUAGE=en-us:Busy
			//TRANSP:OPAQUE
			//UID:ouIYEMc9R0uPaPw+sKxN0w==
			//X-MICROSOFT-CDO-BUSYSTATUS:BUSY
			//END:VEVENT
			//END:VCALENDAR
		}
	}
}
