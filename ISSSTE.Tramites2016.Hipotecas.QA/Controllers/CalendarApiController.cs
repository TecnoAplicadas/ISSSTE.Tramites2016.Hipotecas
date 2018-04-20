#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ISSSTE.Tramites2016.Common.Util;
using ISSSTE.Tramites2016.Common.Web;
using ISSSTE.Tramites2016.Hipotecas.Domian;
using ISSSTE.Tramites2016.Hipotecas.Model.Api;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Pocos;
using ISSSTE.Tramites2016.Hipotecas.Domians;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Controllers
{
    /// <summary>
    ///     Controllador del api para el calendario
    /// </summary>
    [RoutePrefix("api/Calendar")]
    public class CalendarApiController : BaseApiController
    {
        #region Constants

        /// <summary>
        ///     Nombre del encabezado utilizado para enviar la identidad del usuario
        /// </summary>
        private const string UserIdentityHeader = "Issste-Tramites2016-UserIdentity";

        #endregion

        #region Constructor

        /// <summary>
        ///     Constructor del Calendario
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="requestDomainService"></param>
        /// <param name="entitleDomainService"></param>
        /// <param name="calendarDomainService"></param>
        /// <param name="commonDomainService"></param>
        public CalendarApiController(ILogger logger, IRequestDomainService requestDomainService,
            IEntitleDomainService entitleDomainService, ICalendarDomainService calendarDomainService,
            ICommonDomainService commonDomainService) : base(logger)
        {
            _requestDomainService = requestDomainService;
            _entitleDomainService = entitleDomainService;
            _calendarDomainService = calendarDomainService;
            _commonDomainService = commonDomainService;
        }

        #endregion

        #region Fields

        /// <summary>
        ///     Dominio de la solicitud
        /// </summary>
        private readonly IRequestDomainService _requestDomainService;

        /// <summary>
        ///     Dominio del derechohabiente
        /// </summary>
        private readonly IEntitleDomainService _entitleDomainService;

        /// <summary>
        ///     Dominio del calendario
        /// </summary>
        private readonly ICalendarDomainService _calendarDomainService;

        /// <summary>
        ///     Dominio comun
        /// </summary>
        private readonly ICommonDomainService _commonDomainService;

        /// <summary>
        ///     Identidad del usuario enviado en la petición
        /// </summary>
        private string _userIdentity;

        #endregion

        #region Properties

        /// <summary>
        ///     Valida si en la petición se envío la identidad del usuario
        /// </summary>
        private bool HasUserIdentityInRequest
        {
            get
            {
                if (String.IsNullOrEmpty(_userIdentity))
                    return false;
                return true;
            }
        }

        /// <summary>
        ///     Se obtiene la identidad del usuario que realizo la petición
        /// </summary>
        private string UserIdentity
        {
            get { return _userIdentity; }
        }

        #endregion

        #region Actions


        /// <summary>
        ///     Obtiene las citas por id Derechohabiente
        /// </summary>
        /// <param name="RequestId"></param>
        /// <returns></returns>
        [ResponseType(typeof(List<Appoinment>))]
        [HttpGet, Route("CurrentsAppointmentsByRequestId/{RequestId}")]
        public async Task<HttpResponseMessage> GetCurrentAppointmentsByRequestId(Guid RequestId)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.GetCurrentsAppointmentsByRequestId(RequestId);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene las citas canceladas/No atendidas por id Derechohabiente
        /// </summary>
        /// <param name="RequestId"></param>
        /// <returns></returns>
        [ResponseType(typeof(List<Appoinment>))]
        [HttpGet, Route("CancelAndNotAttendedAppointmentsByRequestId/{RequestId}")]
        public async Task<HttpResponseMessage> GetCancelAndNotAttendedAppointmentsByEntitleId(Guid RequestId)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.GetCancelAndNotAttendedAppointmentsByRequestId(RequestId);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene la lista de Citas por fecha y delegacion
        /// </summary>
        /// <param name="date"></param>
        /// <param name="delegationId"></param>
        /// <returns></returns>
        [ResponseType(typeof(List<Schedule>))]
        [HttpGet, Route("Appointments/{date}/{delegationId}")]
        public async Task<HttpResponseMessage> GetAppointments(DateTime date, int delegationId)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.GetAppointments(date, delegationId);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene las citas por solicitud
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [ResponseType(typeof(List<Appoinment>))]
        [HttpGet, Route("AppointmentsByRequest/{requestId}")]
        public async Task<HttpResponseMessage> GetAppointmentsByRequestId(Guid requestId)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.GetAppointmentsByRequestId(requestId);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene los dias feriados
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(List<Holyday>))]
        [HttpGet, Route("Holydays")]
        public async Task<HttpResponseMessage> GetHolydays()
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.GetHolydays();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene la configuracion de dias por delegación
        /// </summary>
        /// <param name="delegationId"></param>
        /// <returns></returns>
        [ResponseType(typeof(List<Schedule>))]
        [HttpGet, Route("Schedule/{delegationId}")]
        public async Task<HttpResponseMessage> GetScheduleByDelegationId(int delegationId)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.GetScheduleByDelegationId(delegationId);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene los dias especiales por delegacion
        /// </summary>
        /// <param name="delegationId"></param>
        /// <returns></returns>
        [ResponseType(typeof(List<SpecialDay>))]
        [HttpGet, Route("SpecialDay/{delegationId}")]
        public async Task<HttpResponseMessage> GetSpecialDayByDelegationId(int delegationId)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.GetSpecialDayByDelegationId(delegationId);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene los horarios por dia especial por delegacion
        /// </summary>
        /// <param name="delegationId"></param>
        /// <returns></returns>
        [ResponseType(typeof(List<SpecialDaysSchedule>))]
        [HttpGet, Route("SpecialDaySchedule/{delegationId}")]
        public async Task<HttpResponseMessage> GetSpecialDayScheduleByDelegationId(int delegationId)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.GetSpecialDayScheduleByDelegationId(delegationId);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene los dias de la semanada
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(List<Weekday>))]
        [HttpGet, Route("WeekDays/{delegationId}")]
        public async Task<HttpResponseMessage> GetWeekDays()
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.GetWeekDays();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Guarda la configuracion de citas
        /// </summary>
        /// <param name="schedules"></param>
        /// <returns></returns>
        [ResponseType(typeof(int))]
        [HttpPost, Route("SaveSchedules")]
        public async Task<HttpResponseMessage> SaveSchedule(List<Schedule> schedules)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = 0;
                foreach (var sche in schedules)
                {
                    result = await _calendarDomainService.SaveSchedule(sche);
                }
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Borra las configuraciones por id de delegacion
        /// </summary>
        /// <param name="delegationId"></param>
        [ResponseType(typeof(void))]
        [HttpPost, Route("DeleteSchedules")]
        public void DeleteSchedulesByDelegation(int delegationId)
        {
            _calendarDomainService.DeleteSchedulesByDelegation(delegationId);
        }

        /// <summary>
        ///     Guarda la cita para la solicitud
        /// </summary>
        /// <param name="appoinment"></param>
        /// <returns></returns>
        [ResponseType(typeof(int))]
        [HttpPost, Route("Appointment")]
        public async Task<HttpResponseMessage> SaveAppointment(Appoinment appoinment)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.SaveAppointment(appoinment);
                var request = await _requestDomainService.GetRequestByRequestId(appoinment.RequestId);
                var entitle = await _entitleDomainService.GetEntitleByCurp(request.EntitleId.ToString());
                var res =
                    await
                        _requestDomainService.SaveStatusRequestByEntitle(request, true, 202,
                            entitle.CURP); // 340 => Cita agendada
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Regresa las citas no atendidas por id
        /// </summary>
        /// <param name="appointmenId"></param>
        /// <returns></returns>
        [ResponseType(typeof(int))]
        [HttpPost, Route("NotAttendedAppointment/{appointmenId}")]
        public async Task<HttpResponseMessage> NotAttendedAppointment(Guid appointmenId)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var app = await _calendarDomainService.GetAppointmentsById(appointmenId);
                app.IsAttended = false;
                var result = await _calendarDomainService.SaveAppointment(app);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene la informacion de las citas por solicitud
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [ResponseType(typeof(DatesApi))]
        [HttpGet, Route("DatesRequest/{requestId}")]
        public async Task<HttpResponseMessage> GetDatesRequest(Guid requestId)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var datesApi = new DatesApi();
                var app = await _calendarDomainService.GetAllAppointmentsbyRequest(requestId);
                var conf = await _commonDomainService.GetConfiguration("NumberOfDates");
                datesApi.NumberAppointments = int.Parse(conf.Value);
                datesApi.CanceledAppoinments = app.Where(r => r.Date < DateTime.Now.Date || r.IsCancelled).ToList();
                datesApi.CurrentdAppoinments = app.Where(r => r.Date >= DateTime.Now.Date && !r.IsCancelled).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, datesApi);
            });
        }

        /// <summary>
        ///     Obtiene las citas canceladas
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns></returns>
        [ResponseType(typeof(DatesApi))]
        [HttpGet, Route("CancelAppointment/{appointmentId}")]
        public async Task<HttpResponseMessage> CancelAppointment(Guid appointmentId)
        {
            //var req = new Request();

            //req.RequestId = appointmentId;
            //req.IsComplete = true;
            var conf = await _commonDomainService.GetConfigurationAsync("NumberOfDates");

            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var res = await _calendarDomainService.CancelAppointment(appointmentId, Convert.ToInt32(conf.Value.ToString()));
                //  var result = await _requestDomainService.SaveRequest(req);
                return Request.CreateResponse(HttpStatusCode.OK, res);
            });
        }

        /// <summary>
        ///     Obtiene el calendario por delegacion y mes
        /// </summary>
        /// <param name="dateDelegation"></param>
        /// <returns></returns>
        [ResponseType(typeof(CalendarApi))]
        [HttpPost, Route("CalendarByMonthAndDelegation")]
        public async Task<HttpResponseMessage> GetCalendarByMonthAndDelegation(DateDelegationApi dateDelegation)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var res =
                    await
                        _calendarDomainService.GetCalendarByMonthAndDelegation(dateDelegation.DelegationId,
                            dateDelegation.Date);
                return Request.CreateResponse(HttpStatusCode.OK, res);
            });
        }

        /// <summary>
        ///     Obtiene el espacio libre por configuracion
        /// </summary>
        /// <param name="dateDelegation"></param>
        /// <returns></returns>
        [ResponseType(typeof(int))]
        [HttpPost, Route("FreeSpace/{appointmentId}")]
        public async Task<HttpResponseMessage> GetFreeSpace(DateDelegationApi dateDelegation)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var res = _calendarDomainService.GetFreeSpace(dateDelegation.Date, dateDelegation.DelegationId);
                return Request.CreateResponse(HttpStatusCode.OK, res);
            });
        }

        /// <summary>
        ///     Obtiene la configuracion de espacios ocupados por id
        /// </summary>
        /// <param name="dateDelegation"></param>
        /// <returns></returns>
        [ResponseType(typeof(int))]
        [HttpPost, Route("OccupiedSpace/{appointmentId}")]
        public async Task<HttpResponseMessage> GetOccupiedSpace(DateDelegationApi dateDelegation)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var res = _calendarDomainService.GetOccupiedSpace(dateDelegation.Date, dateDelegation.DelegationId);
                return Request.CreateResponse(HttpStatusCode.OK, res);
            });
        }

        /// <summary>
        ///     Obtiene los tiempos  por dia
        /// </summary>
        /// <param name="dateDelegation"></param>
        /// <returns></returns>
        [ResponseType(typeof(DatesApi))]
        [HttpPost, Route("TimesCalendar")]
        public async Task<HttpResponseMessage> GetTimeCalendar(DateDelegationApi dateDelegation)
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var res = await _calendarDomainService.GetTimeCalendar(dateDelegation.Date, dateDelegation.DelegationId);
                return Request.CreateResponse(HttpStatusCode.OK, res);
            });
        }

        /// <summary>
        ///     OBtiene los meses del año
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(List<KeyValue>))]
        [HttpGet, Route("Months")]
        public async Task<HttpResponseMessage> GetMonths()
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var res = await _calendarDomainService.GetMonths();
                return Request.CreateResponse(HttpStatusCode.OK, res);
            });
        }

        /// <summary>
        ///     Obtiene una lista de años
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(List<KeyValue>))]
        [HttpGet, Route("Years")]
        public async Task<HttpResponseMessage> GetYears()
        {
            return await ValidateAndHandleOperationExecutionAsync(async () =>
            {
                var res = await _calendarDomainService.GetYears();
                return Request.CreateResponse(HttpStatusCode.OK, res);
            });
        }

        #endregion

        #region Helper Methods

        /// <summary>
        ///     Valida que la petición contenga la identidad del usuario, a la vez que ejecuta cóigo asíncrono para manejar
        ///     excepciones no controladas
        /// </summary>
        /// <param name="operationBody">Cuerpo a ejecutar si se valido la petición</param>
        /// <returns>Resultado del cuerpo a ejecutar</returns>
        private async Task<HttpResponseMessage> ValidateAndHandleOperationExecutionAsync(
            Func<Task<HttpResponseMessage>> operationBody)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                HttpResponseMessage response = null;

                //Se obtiene la identidad del usuario desde el header
                if (Request != null && Request.Headers.Any(h => h.Key == UserIdentityHeader))
                    _userIdentity = Request.Headers.GetValues(UserIdentityHeader).First();

                if (!HasUserIdentityInRequest)
                    response = CreateStringResponseMessage(HttpStatusCode.Unauthorized, "Usuario no válido.");
                else
                    response = await operationBody();

                return response;
            });
        }

        /// <summary>
        ///     Valida que la petición contenga la identidad del usuario, a la vez que ejecuta cóigo para manejar excepciones no
        ///     controladas
        /// </summary>
        /// <param name="operationBody">Cuerpo a ejecutar si se valido la petición</param>
        /// <returns>Resultado del cuerpo a ejecutar</returns>
        private HttpResponseMessage ValidateAndHandleOperationExecution(Func<HttpResponseMessage> operationBody)
        {
            var validationTask = ValidateAndHandleOperationExecutionAsync(async () => operationBody());

            validationTask.Wait();

            return validationTask.Result;
        }

        /// <summary>
        ///     Valida que la petición contenga la identidad del usuario, a la vez que ejecuta cóigo asíncrono para manejar
        ///     excepciones no controladas y envía al código a ejecutar los valores obtenidos del multipart
        /// </summary>
        /// <param name="operationBody">Cuerpo a ejecutar si se valido la petición</param>
        /// <returns>Resultado del cuerpo a ejecutar</returns>
        protected async Task<HttpResponseMessage> ValidateAndHandleMultipartOperationExecutionAsync(
            Func<HttpPostedData, Task<HttpResponseMessage>> operationBody)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                HttpResponseMessage response = null;

                //Se obtiene el Expediente del multipart
                var multipartData = await Request.Content.ParseMultipartAsync();

                _userIdentity = multipartData.Fields[UserIdentityHeader].Value;

                if (!HasUserIdentityInRequest)
                {
                    response = CreateStringResponseMessage(HttpStatusCode.Unauthorized, "Usuario no válido.");

                    return response;
                }
                response = await operationBody(multipartData);

                return response;
            });
        }

        #endregion
    }
}