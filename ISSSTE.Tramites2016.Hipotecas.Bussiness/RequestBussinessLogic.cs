using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Hipotecas.DataAccess;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Api;
using System.Configuration;

namespace ISSSTE.Tramites2016.Hipotecas.Domain.Implementation
{
    public class RequestBussinessLogic
    {
        private RequestDataAccess requestDataAccess = new RequestDataAccess();
        private RequestStatusDataAccess requestStatusDataAccess = new RequestStatusDataAccess();

        public string SaveRequest(Request request, string entitleId)
        {
            string status = "";
            Random folioR = new Random();
            try
            {
                var newRequestId = request.RequestId;
                request.EntitleId = entitleId;
                request.IsComplete = false;
                //  request.IdLegalUnit = 3;
                request.Folio = ConfigurationManager.AppSettings["RequestPrefix"] + DateTime.Now.ToString("ddMMyyyy");
                request.Date = DateTime.Now;
                requestDataAccess.SaveRequest(request);
                requestStatusDataAccess.SaveRequestStatus(newRequestId, (int)StatusEnum.EnesperaderevisiondedocumentacionDer);
                requestStatusDataAccess.RelatedRequestStatus(newRequestId, (int)StatusEnum.EnesperaderevisiondedocumentacionDer);
                   
                status = "OK";
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }
            return status;
        }


        public void UpdateRequestNoComplete(string RequestId)
        {
            try
            {
                var UpdRequest = RequestId;

                requestDataAccess.UpdateRequestNoComplete(RequestId);
                requestStatusDataAccess.UpdateRequestStatus((int)StatusEnum.EnesperadeAgendarCiraDer, RequestId);

                requestStatusDataAccess.SaveRequestStatus(new Guid(RequestId), (int)StatusEnum.EnesperadeAgendarCiraDer);
                requestStatusDataAccess.RelatedRequestStatus(new Guid(RequestId), (int)StatusEnum.EnesperadeAgendarCiraDer);
                // requestStatusDataAccess.RelatedRequestStatus(Guid.Parse(RequestId), (int)StatusEnum.EnesperadeAgendarCiraDer);
            }
            catch (Exception ex)
            {

            }
        }

        public string UpdateRequest(string RequestId, string Writing, bool TypeCredit)
        {
            string status = "";
            try
            {
                var UpdRequest = RequestId;
                requestDataAccess.UpdateRequest(UpdRequest, Writing, TypeCredit);
                status = "OK";
            }

            catch (Exception ex)
            {
                status = ex.Message;
            }
            return status;
        }
        public Request GetRequestByEntitleId(string entitleId)
        {
            return requestDataAccess.GetRequestByEntitleId(entitleId);
        }

        public Request GetRequestByRequestId(Guid RequestId)
        {
            return requestDataAccess.GetRequestByRequestId(RequestId);
        }

        public string GetRequestIdByNoIssste(string NoIssste)
        {
            return requestDataAccess.GetRequestIdByNoIssste(NoIssste);
        }

        public List<RequestAdministrator> GetAllRequest()
        {
            return requestDataAccess.GetAllRequest();
        }

        public List<RequestAdministrator> GetAllRequestHistoria(string NoIssste)
        {
            return requestDataAccess.GetAllRequestHistoria(NoIssste);
        }
        public List<CivilState> GetCivilState()

        {
            return requestDataAccess.GetCivilState();
        }
        public RequestAdministrator GetHistoryRequestByRequestId(string requestId)
        {
            return requestDataAccess.GetHistoryRequestByRequestId(requestId);
        }

        public void UpdateRlequestStatusStatus(int status, string requestId)
        {
            requestStatusDataAccess.UpdateRequestStatus(status, requestId);
        }

        public void SaveStatusNext(int status, string requestId)
        {
            requestStatusDataAccess.SaveStatusNext(status, requestId);
        }

        public int GetActivesRequests(string noissste)
        {
            return requestStatusDataAccess.GetActivesRequests(noissste);
        }
    }
}
