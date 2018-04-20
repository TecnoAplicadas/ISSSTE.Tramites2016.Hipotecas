#region

using System.Collections.Generic;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Pocos
{
    /// <summary>
    ///     Objeto que contiene las listas para la configuracion de citas para una delegacion
    /// </summary>
    public class CalendarList
    {
        /// <summary>
        ///     Lista de citas por dia de la semana y horarios
        /// </summary>
        public List<ScheduleAdminApi> Schedules { get; set; }

        /// <summary>
        ///     Lista que contiene los dias no laborables
        /// </summary>
        public List<SpecialDay> NonLaborableDays { get; set; }

        /// <summary>
        ///     Lista que contiene los dias especiales con los horarios especificos
        /// </summary>
        public List<SpecialDayScheduleApi> SpecialSchedules { get; set; }
    }
}