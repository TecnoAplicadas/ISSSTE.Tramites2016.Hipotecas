#region

using System;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Pocos
{
    /// <summary>
    ///     Parametros para la busqueda de solicitud en administrador
    /// </summary>
    public class SearchParameterApi
    {
        /// <summary>
        ///     Tamaño de pagina
        /// </summary>
        public int pageSize { get; set; }

        /// <summary>
        ///     Pagina
        /// </summary>
        public int page { get; set; }

        /// <summary>
        ///     filtro
        /// </summary>
        public string query { get; set; }

        /// <summary>
        ///     Fecha
        /// </summary>
        public DateTime? date { get; set; }

        /// <summary>
        ///     Hora
        /// </summary>
        public TimeSpan? time { get; set; }
    }
}