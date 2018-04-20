using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Common.Reports.Model.Dashboard
{
    /// <summary>
    /// Representa la información de un trámite a mostrar en el cuerpo del tablero de indicadores
    /// </summary>
    public class ProcedureRequest
    {
        #region Properties

        /// <summary>
        /// Obtiene o asigna el índice de la fila
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Obtiene o asigna el folio del trámite
        /// </summary>
        public string Folio { get; set; }

        /// <summary>
        /// Obtiene o asigna el número ISSSTE del derechohabiente
        /// </summary>
        public string IsssteNumber { get; set; }

        /// <summary>
        /// Obtiene o asigna el nombre del derechohabiente
        /// </summary>
        public string EntitleName { get; set; }

        /// <summary>
        /// Obtiene o asigna el nombre del niño
        /// </summary>
        public string KidName { get; set; }

        /// <summary>
        /// Obtiene o asigna el nombre de la delegación a la que pertenece el derechohabiente
        /// </summary>
        public string DelegationName { get; set; }

        /// <summary>
        /// Obtiene o asigna el valor que idnica si el trámite fue o no cancelado o rechazado antes de finalizar todos los estados
        /// </summary>
        public bool IsCancel { get; set; }

        /// <summary>
        /// Obtiene o asigna la lista de estados por lo que paso el trámite
        /// </summary>
        public List<ProcedureRequestStatus> StatusEntries { get; set; }

        /// <summary>
        /// Obtiene o asigna el total de días por los que ha pasado el trámite
        /// </summary>
        public int TotalElapsedDays { get; set; }

        /// <summary>
        /// Obtiene o asigna el nivel de cumplimiento del total del trámite
        /// </summary>
        public FulfillmentCategory Fullfilment { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constrcutor de la clase
        /// </summary>
        public ProcedureRequest()
        {
            StatusEntries = new List<ProcedureRequestStatus>();
            Fullfilment = FulfillmentCategory.NotApply;
        }

        #endregion
    }
}
