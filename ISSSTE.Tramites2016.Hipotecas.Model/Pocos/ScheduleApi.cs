#region

using System;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Pocos
{
    /// <summary>
    ///     Representacion de una cita
    /// </summary>
    public class ScheduleApi
    {
        /// <summary>
        ///     Hora de la cita
        /// </summary>
        public TimeSpan Time { get; set; }

        /// <summary>
        ///     Fecha de la cita
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     Id de la delegación
        /// </summary>
        public int DelegationId { get; set; }

        /// <summary>
        ///     Capacidad del horario
        /// </summary>
        public int Capacity { get; set; }
    }
}