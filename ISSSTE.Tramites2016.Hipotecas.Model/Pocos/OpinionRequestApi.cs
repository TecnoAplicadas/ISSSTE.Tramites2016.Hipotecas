#region

using ISSSTE.Tramites2016.Hipotecas.Model.Model;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Pocos
{
    /// <summary>
    ///     Clase que contiene el diagnostico y la solicitud
    /// </summary>
    public class OpinionRequestApi
    {
        /// <summary>
        ///     Solicitud del drechohabiente
        /// </summary>
        public RequestGeneric Request { get; set; }

        /// <summary>
        ///     Diagnostico de la solicitud
        /// </summary>
        public Opinion Opinion { get; set; }
    }
}