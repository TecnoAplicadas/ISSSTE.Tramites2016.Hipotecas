using System.Collections.Generic;

namespace ISSSTE.Tramites2016.Common.Reports.Model.Dashboard
{
    /// <summary>
    /// Representa un estado a mostrar en el reporte de indicadores
    /// </summary>
    public class ProcedureStatus
    {
        #region Properties

        /// <summary>
        /// Obtiene o asigna el nombre del estado
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Obtiene o asigna los días límite del estado
        /// </summary>
        public int DueDays { get; set; }

        /// <summary>
        /// Obtiene o asigna  los días promedio que se tomarón los trámites en este estado
        /// </summary>
        public double? Average { get; set; }

        /// <summary>
        /// Obtiene o asigna el porcentaje de cumplimiento de este estado por todos los trámites
        /// </summary>
        public double? FulfillmentPercentage { get; set; }

        /// <summary>
        /// Obtiene o asigna la lista de estatus origen con los que se genera este estatus
        /// </summary>
        public List<int> SourceStatuses { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ProcedureStatus()
        {
            SourceStatuses = new List<int>();
        }

        #endregion
    }
}