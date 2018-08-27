using System;
using System.Collections.Generic;
using Common.Lib.Common.Enums;

namespace Common.Lib.Data.Repositories.ApplicationLog
{
    public interface IApplicationLogRepository : IDisposable
    {
        IEnumerable<ApplicationLog> RetrieveApplicationLogsByDate(DateTime dateTime);
        ApplicationLog RetrieveApplicationLogBySearchKey(Guid searchKey);
        IEnumerable<ApplicationLog> RetrieveApplicationLogsByMessageType(LogLevelType type);
        IEnumerable<ApplicationLog> RetrieveApplicationLogsByAppCode(string appCode);
        IEnumerable<ApplicationLog> RetrieveApplicationLogsBySourceMachine(string sourceMachine);
    }
}
