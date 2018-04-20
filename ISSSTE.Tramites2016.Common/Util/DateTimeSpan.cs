#region

using System;

#endregion

namespace ISSSTE.Tramites2016.Common.Util
{
    /// <summary>
    /// Representa una fecha incluyendo tiempo, y permite la más facil manipulación de cada parte que la compone
    /// </summary>
    public struct DateTimeSpan
    {
        #region Properties

        /// <summary>
        /// Obtiene el año
        /// </summary>
        public int Years { get; }

        /// <summary>
        /// Obtiene el mes
        /// </summary>
        public int Months { get; }

        /// <summary>
        /// Obtiene el día
        /// </summary>
        public int Days { get; }

        /// <summary>
        /// Obtiene la hora
        /// </summary>
        public int Hours { get; }

        /// <summary>
        /// Obtiene el minuto
        /// </summary>
        public int Minutes { get; }

        /// <summary>
        /// Obtiene el segundo
        /// </summary>
        public int Seconds { get; }

        /// <summary>
        /// Obtiene el milisegundo
        /// </summary>
        public int Milliseconds { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Contructor de la clas
        /// </summary>
        /// <param name="year">Año</param>
        /// <param name="month">Mes</param>
        /// <param name="day">Día</param>
        /// <param name="hour">Hora</param>
        /// <param name="minute">Minuto</param>
        /// <param name="second">Segundo</param>
        /// <param name="millisecond">Milisegundo</param>
        public DateTimeSpan(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            this.Years = year;
            this.Months = month;
            this.Days = day;
            this.Hours = hour;
            this.Minutes = minute;
            this.Seconds = second;
            this.Milliseconds = millisecond;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Compara dos fechas y regresa el resultado entre ellas
        /// </summary>
        /// <remarks>Se utiliza en lugar de <see cref="DateTime.Compare(DateTime, DateTime)"/>cuando se necesita más precición en la comparación de fechas llegando a días, horas, minutos, segundo o milisegundos</remarks>
        /// <param name="mainDate">Fecha base de la comparación</param>
        /// <param name="dateToCompare">Fecha contra la cual comparar</param>        
        /// <returns>Diferencia entre las fechas</returns>
        public static DateTimeSpan CompareDates(DateTime mainDate, DateTime dateToCompare)
        {
            if (mainDate < dateToCompare)
            {
                var sub = dateToCompare;
                dateToCompare = mainDate;
                mainDate = sub;
            }

            var current = dateToCompare;
            var years = 0;
            var months = 0;
            var days = 0;

            var phase = Phase.Years;
            var span = new DateTimeSpan();

            while (phase != Phase.Done)
            {
                switch (phase)
                {
                    case Phase.Years:
                        if (current.AddYears(years + 1) > mainDate)
                        {
                            phase = Phase.Months;
                            current = current.AddYears(years);
                        }
                        else
                        {
                            years++;
                        }
                        break;
                    case Phase.Months:
                        if (current.AddMonths(months + 1) > mainDate)
                        {
                            phase = Phase.Days;
                            current = current.AddMonths(months);
                            //if (months >= 12)
                            //    months = months - 1;
                        }
                        else
                        {
                            months++;
                        }
                        break;
                    case Phase.Days:
                        if (current.AddDays(days + 1) > mainDate)
                        {
                            current = current.AddDays(days);
                            var timespan = mainDate - current;
                            span = new DateTimeSpan(years, months, days, timespan.Hours, timespan.Minutes,
                                timespan.Seconds, timespan.Milliseconds);
                            phase = Phase.Done;
                        }
                        else
                        {
                            days++;
                        }
                        break;
                }
            }

            return span;
        }

        #endregion

        #region Inner Classes

        /// <summary>
        /// Representa las fases de una comparacíón de fechas
        /// </summary>
        private enum Phase
        {
            Years,
            Months,
            Days,
            Done
        }

        #endregion
    }
}