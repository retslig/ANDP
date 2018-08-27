
using System.Collections.Generic;
using Common.ProcessMakerService;

namespace Common.NewNet.Services
{
    public interface IProcessMakerService
    {
        void RouteCase(string caseId, string delIndex);
        void CreateCaseNote(string caseId, string processid, string taskid, string userid, string note);
        void CreateResponse(string caseId, string responseMessage, string responseCode, List<variableListStruct> variableList);
        void AttachDocument();
        caseListStruct[] RetrieveCaseList();
        void ExecuteTrigger(string caseId, string triggerId, string routeIndex);
    }
}
