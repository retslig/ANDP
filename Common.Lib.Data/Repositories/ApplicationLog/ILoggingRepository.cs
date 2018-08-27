
namespace Common.Lib.Data.Repositories.ApplicationLog
{
    public interface ILoggingRepository
    {
        void LogUserActivity(string userId, int activityId, bool isAuthorized);
    }
}
