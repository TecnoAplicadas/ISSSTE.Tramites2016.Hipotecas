using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Common.Reports.Model.Dashboard.Report
{
    /// <summary>
    /// Representa un estado a mostrar en el reporte de indicadores
    /// </summary>
    public class ProcedureStatusHelperReport
    {
        /// <summary>
        /// Obtiene o asigna el Id del estado
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o asigna el nombre del estado
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Obtiene o asigna los días límite del estado
        /// </summary>
        public int DueDays { get; set; }

        /// <summary>
        /// Días promedio que se tomarón los trámites en este estado
        /// </summary>
        public double Average { get; set; }

        /// <summary>
        /// Porcentaje de cumplimiento de este estado por todos los trámites
        /// </summary>
        public double FulfillmentPercentage { get; set; }
    }
}
