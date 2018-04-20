#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Pocos;
using ISSSTE.Tramites2016.Hipotecas.Model.Enums;
using ISSSTE.Tramites2016.Hipotecas.Model.Modelo;
using ISSSTE.Tramites2016.Hipotecas.Model.Api;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Domian
{
    public interface IRequestDomainService
    {
        /// <summary>
        ///     Gaurda la solicitud
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<int> SaveRequest(Request request);

        /// <summary>
        ///     Asigna la solicitud a un usuario
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> AsingRequest(Guid requestId, Guid userId);
        /// <summary>
        /// Obtiene las alertas del derechohabiente
        /// </summary>
        /// <param name="noIssste">Número de issste</param>
        /// <returns>Lista de alertas</returns>
        Task<IEnumerable<Alert>> GetAlertsAsync(string noIssste);


        /// <summary>
        ///     Obtiene el siguiente estatus de la solicitud
        /// </summary>
        /// <param name="happy"></param>
        /// <param name="isAssigned"></param>
        /// <param name="isEntitleStatus"></param>
        /// <param name="actualStatus"></param>
        /// <param name="statusChangue"></param>
        /// <returns></returns>
        Task<List<Status>> GetNextStatus(bool happy, bool isEntitleStatus, int? actualStatus,
            int? statusChangue);
        /// <summary>
        /// Obtiene los posibles estatus a donde se puede mover una solicitud
        /// </summary>
        /// <param name="requestId">RequestId de quien se quiere realizar una petición</param>
        /// <param name="roleNames">Roles de la persona que intenta modificar el estatus de la solicitud</param>
        /// <param name=""></param>
        /// <returns></returns>
        Task<List<Status>> GetAvailableNextStatus(Guid requestId, List<String> roleNames);

        Task<String> ChangeRequestStatusAsync(Guid requestId, int nextStatusId, List<String> roleNames);

        /// <summary>
        ///     Guarda el estatus de la solicitud por derechohabiente
        /// </summary>
        /// <param name="request"></param>
        /// <param name="happy"></param>
        /// <param name="statusChague"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> SaveStatusRequestByEntitle(Request request, bool happy, int? statusChague, String userId);

        /// <summary>
        ///     Guarda el estatud de la solicitud por jefe de operador
        /// </summary>
        /// <param name="request"></param>
        /// <param name="happy"></param>
        /// <param name="statusChague"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> SaveStatusRequestByChief(Request request, bool happy, int? statusChague, String userId);

        /// <summary>
        ///     OBtienen la solicitud con infomracion complementaria por derechohabiente
        /// </summary>
        /// <param name="entitleId"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        Task<List<RequestGeneric>> GetRequestsByEntitleId(String entitleId, bool actual);

        /// <summary>
        ///     Obtiene la solicitud por Id
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        Task<Request> GetRequestByRequestId(Guid requestId);

        /// <summary>
        ///     Obtiene  las solicitudes por delegacion
        /// </summary>
        /// <param name="filtro"></param>
        /// <param name="delegationId"></param>
        /// <param name="status"></param>
        /// <param name="isChief"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<RequestGeneric>> GetRequestsByDelegation(String filtro, int delegationId, Status status, bool isChief,
            string userId);

        /// <summary>
        ///     guarda el dictamen
        /// </summary>
        /// <param name="opinion"></param>
        /// <returns></returns>
        Task<int> SaveOpinion(Opinion opinion);

        /// <summary>
        ///     Obitiene el dictamen por solicitud
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        Task<Opinion> GetOpinionByRequest(Guid requestId);

        /// <summary>
        ///     Obtiene la solicitud con informacion complemtaria obtenida por id
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        Task<RequestGeneric> GetRequestCompleteEntitleByRequestId(Guid requestId, string state);

        /// <summary>
        /// Ontiene informacion de la documentacion
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<IEnumerable<DocumentTypes>>> GetDocumentsForInfoAsync();

        /// <summary>
        ///     Obtiene los estatus por rol
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<List<Status>> GetStatusByRole(string role);

        /// <summary>
        ///     Obtiene los estatus de cita por rol
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<List<Status>> GetDateStatusByRole(string role);

        /// <summary>
        ///     Obtiene las delegaciones 
        /// </summary>
        /// <returns></returns>
        Task<List<Delegation>> GetAllDelegation();

        /// <summary>
        ///     Obtiene la consulta pagina de solicitudes
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <param name="roleName"></param>
        /// <param name="delegationId"></param>
        /// <param name="username"></param>
        /// <param name="statusId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedRequestsResult> GetPagedRequestsAsync(int pageSize, int page, string roleName, List<int> delegationId,
            string username, bool orden,int? statusId = default(int?), string query = null);

        /// <summary>
        ///     Obtiene la consulta para los reportes
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <param name="Gender"></param>
        /// <param name="delID"></param>
        /// <param name="pensionId"></param>
        /// <param name="statusId"></param>
        /// <param name="nameEntiti"></param>
        /// <param name="numIssste"></param>
        /// <param name="inicio"></param>
        /// <param name="final"></param>
        /// <returns></returns>
        //Task<PagedReportResult> GetPagedRequestsRepo(int pageSize, int page,
        // string Gender = null, int? delID = default(int?),
        // int? pensionId = default(int?), int? statusId = default(int?), string nameEntiti = null,
        //  string numIssste = null, DateTime? inicio = default(DateTime?), DateTime? final = default(DateTime?), bool? banderaSoloDelegacion = default(bool?));

        /// <summary>
        ///     Obitinene el detalle de solicitud
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<RequestGeneric> GetRequestCompleteByRequestId(Guid requestId, string role);

        /// <summary>
        ///     Obtienen los mensajes para el dictamen
        /// </summary>
        /// <returns></returns>
        Task<List<OpinionMessage>> GetOpinionMessages();

        /// <summary>
        ///     Guarda el estatus de ls solicitud
        /// </summary>
        /// <param name="request"></param>
        /// <param name="happy"></param>
        /// <param name="statusChague"></param>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<int> SaveStatusRequest(Request request, bool happy, int? statusChague, String userId, string role);

        /// <summary>
        ///     OBtienene la lista paginada de citas por delegacion
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <param name="delegationId"></param>
        /// <param name="username"></param>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <param name="rolename"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedRequestsResult> GetPagedDatesAsync(int pageSize, int page, List<int> delegationId,
            string username, DateTime? date, TimeSpan? time, string rolename, string query = null);

        /// <summary>
        ///     Cmabia el status de la cita
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <param name="isAttended"></param>
        /// <returns></returns>
        Task<int> ChangueStatusAppointment(Guid appointmentId, bool isAttended);

        /// <summary>
        ///     Obtiene  el rol por nombre
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Task<Role> GetRole(string roleName);

        /// <summary>
        ///     Obtiene la configuracion de mensajes por webconfig
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        Task<List<OpinionMessage>> GetOpinionMessagesByConfig(EnumConfigMessages config);

        /// <summary>
        ///     Obtiene los usuarios operadores
        /// </summary>
        /// <returns></returns>
        Task<List<AspNetUser>> GetUsersOperator();

        /// <summary>
        ///     Envia el Email de Validacion
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        Task<int> SendEmailValidation(Guid requestId);

        /// <summary>
        ///     Envia la encuesta
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        Task<int> SendEmailQuiz(Guid requestId);

        /// <summary>
        ///     Obtienen la solicitud con informacion complementaria por derechohabiente
        /// </summary>
        /// <param name="curp"></param>
        /// <returns></returns>
        Task<List<RequestGeneric>> GetCurpRequest(String curp, bool actual);

        /// <summary>
        /// Ejecución del servicio batch
        /// </summary>
     //   void BatchService();

        /// <summary>
        ///
        /// </summary>
        /// <param name="Pension"></param>
        /// <param name="Relationship"></param>
        /// <returns></returns>
        Task<List<RelationshipGeneric>> GetDocumentsByRelationship(int Pension, string Relationship);

        /// <summary>
        /// Actualiza el estado del documento por RequestID
        /// </summary>
        /// <param name="RequestId"></param>
        /// <param name="Observations"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        Task<bool> UpdateDocumentByRequestId(DocumentData DocumentData);

        /// <summary>
        /// Obtiene la modalidad de la pensión
        /// </summary>
        /// <param name="Relationship"></param>
        /// <returns></returns>
        Task<List<RelationshipGeneric>> GetModaliadPension(string Relationship);

        /// <summary>
        /// Obtiene la solicitudes con creadas por el Deudo 1.
        /// </summary>
        /// <param name="entitleId"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        Task<List<RequestGeneric>> GetRequestsDebtor(String entitleId, bool actual);

        /// <summary>
        /// Obtiene las solicitudes creadas por el Deudo.
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        Task<List<RequestGeneric>> GetRequestDebtorsRequestId(Guid requestId, bool actual);
    }
}