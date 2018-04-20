#region

using System;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Pocos
{
    /// <summary>
    ///     Clase que contiene una fecha y un idDelegacion
    /// </summary>
    public class DateDelegationApi
    {
        /// <summary>
        ///     Fecha que se utiliza como parametro
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     Id de la delegacion
        /// </summary>
        public int DelegationId { get; set; }
    }
}