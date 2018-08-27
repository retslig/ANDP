using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Common.Lib.Common.Enums;
using Common.Lib.Common.Log;
using Common.Lib.Interfaces;
using Newtonsoft.Json;
using NLog;
using System.Xml.Serialization;
using System.IO;
using NLog.Targets;

namespace Common.Lib.Services
{
    /// <summary>
    /// Helper class that writes log entries out using NLog as the delivery mechanism.
    /// </summary>
    public class NLogWriterService : ILogger
    {
        private readonly SerializerType _type;
        private readonly Logger _curLogger;
        private readonly List<LogLevelType> _enabledLogLevels = new List<LogLevelType>(); 
        private readonly XmlSerializer _curSerializer;

        public NLogWriterService(SqlConnectionStringBuilder sqlBuilder)
        {
            var config = LogManager.Configuration;
            if (config == null)
                throw new Exception("Cannot load Nlog config. Please check and make sure NLog.config exists on server.");

            var dbTarget = (DatabaseTarget)config.FindTargetByName("MsqlDatabase");
            dbTarget.ConnectionString = sqlBuilder.ConnectionString;
            LogManager.ReconfigExistingLoggers();

            foreach (var level in config.LoggingRules.SelectMany(rule => rule.Levels))
            {
                _enabledLogLevels.Add(FromNlogLevel(level));
            } 

            _curLogger = LogManager.GetCurrentClassLogger();
            _type = SerializerType.Xml;
            _curSerializer = new XmlSerializer(typeof(LogObject));
        }

        public NLogWriterService(bool useFileLogging)
        {
            var config = LogManager.Configuration;
            if (config == null)
                throw new Exception("Cannot load Nlog config. Please check and make sure NLog.config exists on server.");

            LogManager.ReconfigExistingLoggers();

            foreach (var level in config.LoggingRules.SelectMany(rule => rule.Levels))
            {
                _enabledLogLevels.Add(FromNlogLevel(level));
            }

            _curLogger = LogManager.GetCurrentClassLogger();
            _curSerializer = new XmlSerializer(typeof(LogObject));
        }

        public NLogWriterService(bool useFileLogging, SerializerType type)
        {
            _type = type;
            var config = LogManager.Configuration;
            if (config == null)
                throw new Exception("Cannot load Nlog config. Please check and make sure NLog.config exists on server.");

            LogManager.ReconfigExistingLoggers();

            foreach (var level in config.LoggingRules.SelectMany(rule => rule.Levels))
            {
                _enabledLogLevels.Add(FromNlogLevel(level));
            }

            _curLogger = LogManager.GetCurrentClassLogger();
            switch (type)
            {
                case SerializerType.Json:
                    break;
                case SerializerType.Xml:
                    _curSerializer = new XmlSerializer(typeof(LogObject));
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        public List<LogLevelType> RetrieveEnabledLogLevels()
        {
            return _enabledLogLevels;
        }

        public string WriteLogEntry(string logMessage, LogLevelType curLevel)
        {
            return WriteLogEntry(null, logMessage, curLevel, null, "", "", "", "", "");
        }

        public string WriteLogEntry(string logMessage, LogLevelType curLevel, Exception curException)
        {
            return WriteLogEntry(null, logMessage, curLevel, curException, "", "", "", "", "");
        }

        public string WriteLogEntry(List<object> curObjects, string logMessage, LogLevelType curLevel)
        {
            return WriteLogEntry(curObjects, logMessage, curLevel, null, "", "", "", "", "");
        }

        public string WriteLogEntry(List<object> curObjects, string logMessage, LogLevelType curLevel, Exception curException)
        {
            return WriteLogEntry(curObjects, logMessage, curLevel, curException, "", "", "", "", "");
        }

        public string WriteLogEntry(List<object> curObjects, string logMessage, LogLevelType curLevel, Exception curException, Guid searchKey)
        {
            return WriteLogEntry(curObjects, logMessage, curLevel, curException, "", "", searchKey.ToString(), "", "");
        }

        public string WriteLogEntry(List<object> curObjects, string logMessage, LogLevelType curLevel, Exception curException, string destinationMachineName)
        {
            return WriteLogEntry(curObjects, logMessage, curLevel, curException, destinationMachineName, "", "", "", "");
        }

        public string WriteLogEntry(List<object> curObjects, string logMessage, LogLevelType curLevel, Exception curException, 
            string destinationMachineName, string applicationName)
        {
            return WriteLogEntry(curObjects, logMessage, curLevel, curException, destinationMachineName, applicationName, "", "", "");
        }

        public string WriteLogEntry(List<object> curObjects, string logMessage, LogLevelType curLevel, Exception curException, 
            string destinationMachineName, string applicationName, Guid searchKey)
        {
            return WriteLogEntry(curObjects, logMessage, curLevel, curException, destinationMachineName, applicationName, searchKey.ToString(), "", "");
        }

        public string WriteLogEntry(List<object> curObjects, string logMessage, LogLevelType curLevel, Exception curException,
                                    string destinationMachineName, string applicationName, string winUser, Guid searchKey)
        {
            return WriteLogEntry(curObjects, logMessage, curLevel, curException, destinationMachineName, applicationName, searchKey.ToString(), winUser, "");
        }

        public string WriteLogEntry(string tenantId, List<object> curObjects, string logMessage, LogLevelType curLevel, Exception curException,
            string destinationMachineName, string applicationName, string winUser, Guid searchKey)
        {
            return WriteLogEntry(curObjects, logMessage, curLevel, curException, destinationMachineName, applicationName, searchKey.ToString(), winUser, tenantId);
        }

        public string WriteLogEntry(string tenantId, List<object> curObjects, string logMessage, LogLevelType curLevel, Exception curException)
        {
            return WriteLogEntry(curObjects, logMessage, curLevel, curException, "", "", "", "", tenantId);
        }

        public string WriteLogEntry(string tenantId, List<object> curObjects, string logMessage, LogLevelType curLevel)
        {
            return WriteLogEntry(curObjects, logMessage, curLevel, null, "", "", "", "", tenantId);
        }

        private string WriteLogEntry(List<object> curObjects, string logMessage, LogLevelType curLevel, Exception curException, 
            string destinationMachineName, string applicationName, string uniqueId, string winUser, string tenantId)
        {
            if (string.IsNullOrEmpty(uniqueId))
                uniqueId = Guid.NewGuid().ToString();

            var nLogLevel = LogLevel.FromString(curLevel.ToString());
            var curLogObject = new LogObject { Message = logMessage, CurrentDataObjects = curObjects };

            string message;
            switch (_type)
            {
                case SerializerType.Json:
                    message = JsonConvert.SerializeObject(curLogObject);
                    break;
                case SerializerType.Xml:
                    using (var sw = new StringWriter())
                    {
                        _curSerializer.Serialize(sw, curLogObject);
                        message = sw.ToString();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("_type");
            }

            var logRecord = curException != null ? new LogEventInfo(nLogLevel, null, null, message, null, curException) : new LogEventInfo(nLogLevel, null, message);

            logRecord.Properties["DestinationMachineName"] = destinationMachineName;
            logRecord.Properties["UniqueID"] = uniqueId;
            logRecord.Properties["TenantId"] = tenantId;
            if (!string.IsNullOrEmpty(applicationName))
                logRecord.Properties["AppCode"] = applicationName;
            else
                logRecord.Properties["AppCode"] = Process.GetCurrentProcess().ProcessName;

            if (!string.IsNullOrEmpty(winUser))
                logRecord.Properties["WinUser"] = winUser;
            else
                logRecord.Properties["WinUser"] = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            
            _curLogger.Log(logRecord);

            return uniqueId;
        }

        private LogLevelType FromNlogLevel(LogLevel logLevel)
        {
            switch (logLevel.Name)
            {
                case "Debug":
                    return LogLevelType.Debug;
                case "Error":
                    return LogLevelType.Error;
                case "Fatal":
                    return LogLevelType.Fatal;
                case "Info":
                    return LogLevelType.Info;
                case "Off":
                    return LogLevelType.Off;
                case "Trace":
                    return LogLevelType.Trace;
                case "Warn":
                    return LogLevelType.Warn;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
