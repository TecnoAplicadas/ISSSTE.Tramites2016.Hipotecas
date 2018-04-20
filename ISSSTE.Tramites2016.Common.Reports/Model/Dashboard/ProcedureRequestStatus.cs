namespace ISSSTE.Tramites2016.Common.Reports.Model.Dashboard
{
    /// <summary>
    /// Representa un la información de un estado por la que paso una solicitud
    /// </summary>
    public class ProcedureRequestStatus
    {
        /// <summary>
        /// Obtiene o asigna el estatus al que representa esta entrada
        /// </summary>
        public ProcedureStatus ProcedureStatus { get; set; }

        /// <summary>
        /// Obtiene o asigna la fecha en la que paso a este estado la solicitud
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Obtiene o asigna los días transucrridos en este estado
        /// </summary>
        public int ElapsedWorkDays { get; set; }

        /// <summary>
        /// Obtiene o asigna el nivel de cumplimiento de la solicitud en su paso en este estado
        /// </summary>
        public FulfillmentCategory Fulfillment { get; set; }
    }
}