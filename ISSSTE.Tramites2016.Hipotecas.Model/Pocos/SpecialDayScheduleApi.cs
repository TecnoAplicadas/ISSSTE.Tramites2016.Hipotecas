#region

using System.Collections.Generic;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Pocos
{
    /// <summary>
    ///     Representacion de Horarios en dias especiales
    /// </summary>
    public class SpecialDayScheduleApi
    {
        /// <summary>
        ///     Dia especial
        /// </summary>
        public SpecialDay SpecialDay { get; set; }

        /// <summary>
        ///     Lsita de citas por dia
        /// </summary>
        public List<SpecialDaysSchedule> Schedules { get; set; }
    }
}