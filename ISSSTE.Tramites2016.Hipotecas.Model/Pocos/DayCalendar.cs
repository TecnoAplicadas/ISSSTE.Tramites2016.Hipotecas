#region

using System;
using ISSSTE.Tramites2016.Hipotecas.Model.Enums;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Pocos
{
    /// <summary>
    ///     Representacion de un dia para el calendario de citas
    /// </summary>
    public class DayCalendar
    {
        /// <summary>
        ///     Fecha del dia
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        ///     Nombre del dia
        /// </summary>
        public String Day { get; set; }

        /// <summary>
        ///     Espacios libres para citas
        /// </summary>
        public decimal FreeSpace { get; set; }

        /// <summary>
        ///     Espacios ocupados para citas
        /// </summary>
        public decimal OccupiedSpace { get; set; }

        /// <summary>
        ///     Disponibilidad en el dia para citas
        /// </summary>
        public AvailabilityEnum Availability { get; set; }

        /// <summary>
        ///     Porcentaje de ocupacion
        /// </summary>
        public decimal Rate { get; set; }
    }
}