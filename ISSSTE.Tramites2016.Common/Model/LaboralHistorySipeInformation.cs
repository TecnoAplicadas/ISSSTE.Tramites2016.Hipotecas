#region

using System;

#endregion

namespace ISSSTE.Tramites2016.Common.Model
{
    /// <summary>
    /// Modelo que representa la información de una entrada en el historial laboral de un derechohabiente
    /// </summary>
    public class LaboralHistorySipeInformation
    {
        /// <summary>
        ///     Numero de ramo
        /// </summary>
        public string BranchNumber { get; set; }

        /// <summary>
        ///     Numero de pagaduria
        /// </summary>
        public string PayMaster { get; set; }

        /// <summary>
        ///     Numero de ISSSTE
        /// </summary>
        public string IsssteNumber { get; set; }

        /// <summary>
        ///     Modalidad
        /// </summary>
        public string Mode { get; set; }

        /// <summary>
        ///     Fecha de inicio del periodo
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        ///     Fecha de termino de periodo
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        ///     Motivo de inicio
        /// </summary>
        public string ReasonStart { get; set; }

        /// <summary>
        ///     Motivo de cierre
        /// </summary>
        public string ReasonEnd { get; set; }

        /// <summary>
        ///     Sueldo que cotizo en el periodo
        /// </summary>
        public string IsssteSalary { get; set; }

        public string YearsOfContributions { get; set; }
        public string Dependency { get; set; }

        public string UsoPen { get; set; }
    }
}