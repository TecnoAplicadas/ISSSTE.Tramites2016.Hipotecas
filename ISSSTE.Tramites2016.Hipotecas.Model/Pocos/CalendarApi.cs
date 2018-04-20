namespace ISSSTE.Tramites2016.Hipotecas.Model.Pocos
{
    /// <summary>
    ///     Objeto que representa un calendario
    /// </summary>
    public class CalendarApi
    {
        /// <summary>
        ///     Id de la delegación
        /// </summary>
        public int DelegationId { get; set; }

        /// <summary>
        ///     Entero que representa un mes
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        ///     Arreglo que representa los dias del mes
        /// </summary>
        public DayCalendar[,] Days { get; set; }
    }
}