using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Hipotecas.Model.Pocos
{
    public class PagedMessagesResult
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
        ///     Obtiene los mensajes del catálogo 
        /// </summary>
        public List<Model.Message> Messages { get; set; }
    }
}