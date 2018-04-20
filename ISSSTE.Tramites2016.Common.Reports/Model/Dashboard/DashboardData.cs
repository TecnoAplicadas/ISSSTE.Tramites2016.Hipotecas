using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Common.Reports.Model.Dashboard
{
    /// <summary>
    /// Representa la información necesaria para generar un tablero de indicadores
    /// </summary>
    public class DashboardData
    {
        #region Properties

        /// <summary>
        /// Obtiene o asigna la lista de estatus que a desplegar en el encabezado del tablero
        /// </summary>
        public List<ProcedureStatus> ProcedureStatus { get; set; }        

        /// <summary>
        /// Obtiene o asigna la lista de registroa a mostrar en el cuerpo del tablero
        /// </summary>
        public List<ProcedureRequest> ProcedureRequest { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public DashboardData()
        {
            ProcedureStatus = new List<ProcedureStatus>();
            ProcedureRequest = new List<ProcedureRequest>();
        }

        #endregion
    }
}
