using System;
using System.Collections.Generic;
using Common.Lib.Common.Enums;

namespace Common.Lib.Interfaces
{
    public interface ILogger
    {
        List<LogLevelType> RetrieveEnabledLogLevels();
        string WriteLogEntry(string logMessage, LogLevelType curLevel);
        string WriteLogEntry(string logMessage, LogLevelType curLevel, Exception curException);
        string WriteLogEntry(List<object> curObjects, string logMessage, LogLevelType curLevel);
        string WriteLogEntry(List<object> curObjects, string logMessage, LogLevelType curLevel, Exception curException);
        string WriteLogEntry(List<object> curObjects, string logMessage, LogLevelType curLevel, Exception curException, Guid searchKey);
        string WriteLogEntry(List<object> curObjects, string logMessage, LogLevelType curLevel, Exception curException, string destinationMachineName);
        string WriteLogEntry(List<object> curObjects, string logMessage, LogLevelType curLevel, Exception curException, string destinationMachineName, string applicationName);
        string WriteLogEntry(List<object> curObjects, string logMessage, LogLevelType curLevel, Exception curException, string destinationMachineName, string applicationName, Guid searchKey);
        string WriteLogEntry(List<object> curObjects, string logMessage, LogLevelType curLevel, Exception curException, string destinationMachineName, string applicationName, string winUser, Guid searchKey);
        string WriteLogEntry(string tenantId, List<object> curObjects, string logMessage, LogLevelType curLevel, Exception curException, string destinationMachineName, string applicationName, string winUser, Guid searchKey);
        string WriteLogEntry(string tenantId, List<object> curObjects, string logMessage, LogLevelType curLevel, Exception curException);
        string WriteLogEntry(string tenantId, List<object> curObjects, string logMessage, LogLevelType curLevel);
    }
}
