using System;
using System.Collections.Generic;
using Common.Lib.Common.Enums;
using Common.Lib.Common.Log;
using NLog;
using System.Xml.Serialization;
using System.IO;

namespace Common.Lib.Utility
{
    /// <summary>
    /// Helper class that writes log entries out using NLog as the delivery mechanism.
    /// </summary>
    public static class LogWriter
    {
        private static Logger _curLogger;
        private static XmlSerializer _curSerializer;

        static LogWriter()
        {
            _curLogger = LogManager.GetCurrentClassLogger();
            _curSerializer = new XmlSerializer(typeof(LogObject));
        }

        public static string WriteLogEntry(string logMessage, LogLevelType curLevel)
        {
            return WriteLogEntry(null, logMessage, curLevel, null, "");
        }

        public static string WriteLogEntry(string logMessage, LogLevelType curLevel, Exception curException)
        {
            return WriteLogEntry(null, logMessage, curLevel, curException, "");
        }

        public static string WriteLogEntry(List<object> curObjects, string logMessage, LogLevelType curLevel)
        {
            return WriteLogEntry(curObjects, logMessage, curLevel, null, "");
        }

        public static string WriteLogEntry(List<object> curObjects, string logMessage, LogLevelType curLevel, Exception curException)
        {
            return WriteLogEntry(curObjects, logMessage, curLevel, curException, "");
        }

        public static string WriteLogEntry(List<object> curObjects, string logMessage, LogLevelType curLevel, Exception curException, string destinationMachineName)
        {
            LogLevel nLogLevel = LogLevel.FromString(curLevel.ToString());

            LogObject curLogObject = new LogObject { Message = logMessage, CurrentDataObjects = curObjects };

            Guid logGUID = Guid.NewGuid();
            string uniqueID = logGUID.ToString();

            string xmlMessage;

            using (StringWriter sw = new StringWriter())
            {
                _curSerializer.Serialize(sw, curLogObject);
                xmlMessage = sw.ToString();
            }

            LogEventInfo logRecord;

            if (curException != null)
            {
                logRecord = new LogEventInfo(nLogLevel, null, null, xmlMessage, null, curException);
            }
            else
            {
                logRecord = new LogEventInfo(nLogLevel, null, xmlMessage);
            }

            logRecord.Properties["DestinationMachineName"] = destinationMachineName;
            logRecord.Properties["UniqueID"] = uniqueID;
            _curLogger.Log(logRecord);

            return uniqueID;
        }
    }
}
