#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Common.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Pocos;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Domian
{
    public interface ICalendarDomainService
    {
        /// <summary>
        ///     Obtiene los dias de la semana desde la BD
        /// </summary>
        /// <returns></returns>
        Task<List<Weekday>> GetWeekDays();

        /// <summary>
        ///     Obtienen los dias inhabiles
        /// </summary>
        /// <returns></returns>
        Task<List<Holyday>> GetHolydays();

        /// <summary>
        ///     Identifica si una fecha es dia inhabil
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<bool> IsHolyday(DateTime date);

        /// <summary>
        /// Obtiene las citas por id Derechohabiente 
        /// </summary>
        /// <param name="entitleId"></param>
        /// <returns></returns>
        Task<List<AppointmentsResult>> GetCurrentsAppointmentsByRequestId(Guid RequestId);

        Task<List<AppointmentsResult>> GetCancelAndNotAttendedAppointmentsByRequestId(Guid RequestId);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <param name="roleName"></param>
        /// <param name="delegationId"></param>
        /// <param name="username"></param>
        /// <param name="statusId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInformation<AppointmentsResult>> GetPagetAppointmentsAsync(int pageSize, int page, string roleName,
            List<int> delegationId, string username, DateTime? date = default(DateTime?), string query = null, TimeSpan? time = default(TimeSpan?), int? statusId = default(int?));

        Task<PagedInformation<AppointmentsResult>> GetPagetAppointmentsByRequestAsync(string roleName,
            List<int> delegationId, Guid requestId);

        /// <summary>
        ///     Obtiene todas citas por fecha y delegacion
        /// </summary>
        /// <param name="date"></param>
        /// <param name="delegationId"></param>
        /// <returns></returns>
        Task<List<Schedule>> GetAppointments(DateTime date, int delegationId);

        /// <summary>
        ///     Obtiene las citas por solicitud
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        Task<List<Appoinment>> GetAppointmentsByRequestId(Guid requestId);

        /// <summary>
        ///     Guarda un dia special
        /// </summary>
        /// <param name="specialDay"></param>
        /// <returns></returns>
        Task<int> SaveSpecialDay(SpecialDay specialDay);

        /// <summary>
        ///     Guarda un horario de un dia en especial
        /// </summary>
        /// <param name="specialDaysSchedule"></param>
        /// <returns></returns>
        Task<int> SaveSpecialDaySchedule(SpecialDaysSchedule specialDaysSchedule);

        /// <summary>
        ///     Guarda una configuracion de calendario
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        Task<int> SaveSchedule(Schedule schedule);

        /// <summary>
        ///     Borra las configuraciones por delegacion
        /// </summary>
        /// <param name="delegationId"></param>
        void DeleteSchedulesByDelegation(int delegationId);

        /// <summary>
        ///     Obtiene los dias especiales por delegación
        /// </summary>
        /// <param name="delegationId"></param>
        /// <returns></returns>
        Task<List<SpecialDay>> GetSpecialDayByDelegationId(int delegationId);

        /// <summary>
        ///     Obtiene los horarios de dias especiales por delegación
        /// </summary>
        /// <param name="delegationId"></param>
        /// <returns></returns>
        Task<List<SpecialDaysSchedule>> GetSpecialDayScheduleByDelegationId(int delegationId);

        /// <summary>
        ///     Obtiene la lista de configuraciones por delegación
        /// </summary>
        /// <param name="delegationId"></param>
        /// <returns></returns>
        Task<List<Schedule>> GetScheduleByDelegationId(int delegationId);

        /// <summary>
        ///     Guarda una cita
        /// </summary>
        /// <param name="appoinment"></param>
        /// <returns></returns>
        Task<int> SaveAppointment(Appoinment appoinment);

        /// <summary>
        ///     Obtiene una cita por su Id
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns></returns>
        Task<Appoinment> GetAppointmentsById(Guid appointmentId);

        /// <summary>
        ///     Obtiene todas las citas por id de la solicitud
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        Task<List<Appoinment>> GetAllAppointmentsbyRequest(Guid requestId);

        /// <summary>
        ///     Cancela la cita
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        Task<int> CancelAppointment(Guid requestId, int conf);

        /// <summary>
        ///     Obtiene el calendario de configuraciones por mes y delegación
        /// </summary>
        /// <param name="delegationId"></param>
        /// <param name="montYear"></param>
        /// <returns></returns>
        Task<CalendarApi> GetCalendarByMonthAndDelegation(int delegationId, DateTime montYear);

        /// <summary>
        ///     OBtiene el espacio libre por fecha y delegacion
        /// </summary>
        /// <param name="date"></param>
        /// <param name="delegation"></param>
        /// <returns></returns>
        int GetFreeSpace(DateTime date, int delegation);

        /// <summary>
        ///     Obtiene el espacio ocupado por delegacion y fecha
        /// </summary>
        /// <param name="date"></param>
        /// <param name="delegation"></param>
        /// <returns></returns>
        int GetOccupiedSpace(DateTime date, int delegation);

        /// <summary>
        ///     Obtiene la lista de  configuraciones por fecha y delegacio
        /// </summary>
        /// <param name="date"></param>
        /// <param name="delegation"></param>
        /// <returns></returns>
        Task<List<ScheduleApi>> GetTimeCalendar(DateTime date, int delegation);

        /// <summary>
        ///     OBtiene los meses
        /// </summary>
        /// <returns></returns>
        Task<List<KeyValue>> GetMonths();

        /// <summary>
        ///     Obtiene los años
        /// </summary>
        /// <returns></returns>
        Task<List<KeyValue>> GetYears();

        /// <summary>
        ///     Obtiene la configuracion por id de delegacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<ScheduleAdminApi>> GetScheduleByAdministrator(int id);

        /// <summary>
        ///     Obtiene los dias no laborales por id de delegacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<SpecialDay>> GetNonLaborableDaysAdministrator(int id);

        /// <summary>
        ///     Obtiene los dias especiales con las configuraciones de sus horarios
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<SpecialDayScheduleApi>> GetSpecialDaysDateAdministrator(int id);

        /// <summary>
        ///     Borrra la configuracion de horario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteSchedule(Guid id);

        /// <summary>
        ///     Borra el dia no laborable
        /// </summary>
        /// <param name="id"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<int> DeleteNonLaboraleDays(int id, DateTime date);

        /// <summary>
        ///     Borra las configuraciones de fechas especiales
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteSpecialScheduleDays(Guid id);

        /// <summary>
        ///     Guarda las configuraciones
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> SaveSchedules(List<Schedule> saves);

        /// <summary>
        ///     Guarda los dias no laborables
        /// </summary>
        /// <param name="specialDays"></param>
        /// <returns></returns>
        Task<int> SaveNonLaborableDays(List<SpecialDay> specialDays);

        /// <summary>
        ///     Guarda los dias especiales y sus configuraciones
        /// </summary>
        /// <param name="specialDaysapi"></param>
        /// <returns></returns>
        Task<int> SaveSpecialDayAndSchedule(List<SpecialDayScheduleApi> specialDaysapi);

        /// <summary>
        ///     Guarda las configuraciones de dias especiales
        /// </summary>
        /// <param name="specialSchedules"></param>
        /// <returns></returns>
        Task<int> SaveSpecialSchedules(List<SpecialDaysSchedule> specialSchedules);

        /// <summary>
        ///     Obitne elas citas por delegacion
        /// </summary>
        /// <param name="Delegationid"></param>
        /// <returns></returns>
        Task<IEnumerable<Appoinment>> GetAppointmentsForDelegation(int Delegationid);

        /// <summary>
        /// Elimina el horario del día especial.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteSpecialDaysSchedules(Guid id);

    }
}