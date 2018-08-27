using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Common.Lib.Common.Enums;

namespace Common.Lib.Data.Repositories.ApplicationLog
{
    public class ApplicationLogRepository : IApplicationLogRepository
    {
        private readonly ICommon_ApplicationLog_Entities _iRavenApplicationLogEntities;

        protected ApplicationLogRepository()
        {
        }

        public ApplicationLogRepository(ICommon_ApplicationLog_Entities iRavenApplicationLogEntities)
        {
            _iRavenApplicationLogEntities = iRavenApplicationLogEntities;
        }

        public IEnumerable<ApplicationLog> RetrieveApplicationLogsByDate(DateTime dateTime)
        {
            return _iRavenApplicationLogEntities.ApplicationLogs.Where(p => DbFunctions.TruncateTime(p.LoggedDateTime) == DbFunctions.TruncateTime(dateTime));
        }

        public ApplicationLog RetrieveApplicationLogBySearchKey(Guid searchKey)
        {
            return _iRavenApplicationLogEntities.ApplicationLogs.FirstOrDefault(p => p.SearchKey == searchKey);
        }

        public IEnumerable<ApplicationLog> RetrieveApplicationLogsByMessageType(LogLevelType type)
        {
            return _iRavenApplicationLogEntities.ApplicationLogs.Where(p => p.MessageType == type.ToString());
        }

        public IEnumerable<ApplicationLog> RetrieveApplicationLogsByAppCode(string appCode)
        {
            return _iRavenApplicationLogEntities.ApplicationLogs.Where(p => p.AppCode == appCode);
        }

        public IEnumerable<ApplicationLog> RetrieveApplicationLogsBySourceMachine(string sourceMachine)
        {
            return _iRavenApplicationLogEntities.ApplicationLogs.Where(p => p.SourceMachineName == sourceMachine);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //this is not really neccesary since the above classes are static and dispose within them.
        }
    }
}
