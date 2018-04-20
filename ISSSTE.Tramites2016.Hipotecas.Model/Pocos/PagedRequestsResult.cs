#region

using System.Collections.Generic;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Pocos
{
    /// <summary>
    ///     objeto que representa la consulta paginada
    /// </summary>
    public class PagedRequestsResult
    {
        /// <summary>
        ///     Obtiene el total de paginas en base a los criterios de busqueda
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        ///     Obtiene el tamaño de cada página
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        ///     Obtiene la pagina actual
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        ///     Obtiene las solicitudes
        /// </summary>
        public List<RequestGeneric> Requests { get; set; }
    }
}