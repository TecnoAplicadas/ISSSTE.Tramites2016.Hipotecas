using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Common.Reports.Model.Ebdis
{
    public class CertificateRoom
    {
        /// <summary>
        /// Nombre de la sala
        /// </summary>
        public string NameRoom { get; set; }

        /// <summary>
        /// Metros cuadrados netos de cada sala
        /// </summary>
        public decimal DimensionM2 { get; set; }

        /// <summary>
        /// Numero de Lactantes A por estancia
        /// </summary>
        public int LactantesA { get; set; }

        /// <summary>
        /// Numero de Lactantes B por estancia
        /// </summary>
        public int LactantesB { get; set; }

        /// <summary>
        /// Numero de Lactantes C por estancia
        /// </summary>
        public int LactantesC { get; set; }

        /// <summary>
        /// Numero de Maternales A por estancia
        /// </summary>
        public int MaternalA { get; set; }

        /// <summary>
        /// Numero de Maternales B por estancia
        /// </summary>
        public int MaternalB { get; set; }

        /// <summary>
        /// Numero de Preescolar 1 por estancia
        /// </summary>
        public int Preescolar1 { get; set; }

        /// <summary>
        /// Numero de Preescolar 2 por estancia
        /// </summary>
        public int Preescolar2{ get; set; }

        /// <summary>
        /// Numero de Preescolar 3 por estancia
        /// </summary>
        public int Preescolar3 { get; set; }
    }
}
