#region

using System;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Pocos
{
    /// <summary>
    ///     Representacion de el diagnostico y estatus
    /// </summary>
    public class StatusOpinionApi
    {
        /// <summary>
        ///     Id de la solicitud
        /// </summary>
        public Guid requestId { get; set; }

        /// <summary>
        ///     Id del status
        /// </summary>
        public int? idStatus { get; set; }

        /// <summary>
        ///     Indica si va por el camino optimo
        /// </summary>
        public bool happy { get; set; }

        /// <summary>
        ///     Diagnostico
        /// </summary>
        public string opinion { get; set; }
    }
}