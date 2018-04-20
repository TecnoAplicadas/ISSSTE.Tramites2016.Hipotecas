#region

using System;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Pocos
{
    /// <summary>
    ///     Objeto para paso de parametros
    /// </summary>
    public class UserRequestApi
    {
        /// <summary>
        ///     Nombre de usuario
        /// </summary>
        public string User { get; set; }

        /// <summary>
        ///     Id de la solicitud
        /// </summary>
        public Guid RequestId { get; set; }
    }
}