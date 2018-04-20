using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Common.Model
{

    /// <summary>
    /// MFP 04/01/2017 Clase Generica para manejar la paginacion.
    /// </summary>
    public class PagedInformation<T> where T : class
    {

        public PagedInformation()
        {
            CurrentPage = 1;
            PageSize = 1;
            queryString = null;           
        }

        //public PagedInformation(int tamañoPagina) { PageSize = tamañoPagina; CurrentPage = 1; }

        public List<T> ResultList { get; set; }

        public string queryString { get; set; }
        public int resultCount { get; set; } // Total de elementos (sin considerar los filtros de pagnacion Skip y Take.
        public  int PageSize { get; set; } // <Tamaño de la pagina o elementos por pagina - PageSize > Necesario del lado del cliente para la paginacion
        public int CurrentPage { get; set; } // <CurrentPage >  Necesario del lado del cliente para la paginacion


        public int TotalPages // <TotalPages> Necesario del lado del cliente para la paginacion
        {
            get
            {
                return PageSize > 0 ? (int)Math.Ceiling((decimal)resultCount / PageSize) : 1;
            }
        }

        public int SetElementosPorPagina
        {
            set { PageSize = value; }
        }

        public int GetElementosPorPagina
        {
            get { return PageSize; }
        }

        public string GetFiltroBusqueda
        {

            get
            {
                return queryString = string.IsNullOrEmpty(queryString) || string.IsNullOrWhiteSpace(queryString) ? null :
                queryString;
            }
        }





    }
}
