using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Common.Reports.Model.Ebdis
{
    /// <summary>
    /// Clase que encapsula la información de la carta de aceptación.
    /// </summary>
    public class EbdisLetterInfo
    {
        /// <summary>
        /// Dia 
        /// </summary>
        public string Day { get; set; }
        /// <summary>
        /// Mes
        /// </summary>
        public string Month { get; set; }
        /// <summary>
        /// Año
        /// </summary>
        public string Year { get; set; }
        /// <summary>
        /// Nombre del beneficiario
        /// </summary>
        public string BeneficiaryName { get; set; }
        /// <summary>
        /// Número de estancia
        /// </summary>
        public string EbdiNumber { get; set; }
        /// <summary>
        /// Nombre del niño
        /// </summary>
        public string KidName { get; set; }
    }
}
