using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Common.Reports.Model.Dashboard.Report
{
    /// <summary>
    /// Representa la información de un trámite a mostrar en el cuerpo del tablero de indicadores
    /// </summary>
    public class ProcedureRequestHelpertReport
    {
        /// <summary>
        /// Obtiene o asigna  el número de la fila
        /// </summary>
        public int Number { get; set; }

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
        /// Obtiene o asigna el nombre del niño(a)
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
        /// Obtiene o asigna el valor para el campo 'Solicitud de estancia y solicitud de lugar' del reporte
        /// </summary>
        public string SelectionOfChildCareCenter { get; set; }

        /// <summary>
        /// Obtiene o asigna el estatus del valor para el campo 'Solicitud de estancia y solicitud de lugar' del reporte
        /// </summary>
        public string  StatusSelectionOfChildCareCenter { get; set; }

        /// <summary>
        /// Obtiene o asigna el valor para el campo 'Asignacion de lugar' del reporte
        /// </summary>
        public string AssignmentOfPlace { get; set; }

        /// <summary>
        /// Obtiene o asigna el estatus para el valor para el campo 'Asignacion de lugar' del reporte
        /// </summary>
        public string StatusAssignmentOfPlace { get; set; }

        /// <summary>
        /// Obtiene o asigna el valor para el campo 'Carga de documentacion inicial' del reporte
        /// </summary>
        public string DocumentationFirstLoad { get; set; }

        /// <summary>
        /// Obtiene o asigna el estatus para el valor para el campo 'Carga de documentacion inicial' del reporte
        /// </summary>
        public string StatusDocumentationFirstLoad { get; set; }

        /// <summary>
        /// Obtiene o asigna el valor para el campo 'Validacion de documentacion inicial' del reporte
        /// </summary>
        public string DocumentInitialValidate { get; set; }

        /// <summary>
        /// Obtiene o asigna el estatus para el valor para el campo 'Validacion de documentacion inicial' del reporte
        /// </summary>
        public string StatusDocumentInitialValidate { get; set; }

        /// <summary>
        /// Obtiene o asigna el valor para el campo 'Carga de analisis clinicos' del reporte
        /// </summary>
        public string ClinicalAnalyzesLoad { get; set; }

        /// <summary>
        /// Obtiene o asigna el estatus para el valor para el campo 'Carga de analisis clinicos' del reporte
        /// </summary>
        public string StatusClinicalAnalyzesLoad { get; set; }

        /// <summary>
        /// Obtiene o asigna el valor para el campo 'Validacion de analisis clinicos' del reporte
        /// </summary>
        public string ClinicalAnalyzesValidate { get; set; }

        /// <summary>
        /// Obtiene o asigna el estatus para el valor para el campo 'Validacion de analisis clinicos' del reporte
        /// </summary>
        public string StatusClinicalAnalyzesValidate { get; set; }

        /// <summary>
        /// Obtiene o asigna el estatus para el valor para el campo 'Registro de fecha para entrevistas' del reporte
        /// </summary>
        public string DateRecordInterviews { get; set; }

        /// <summary>
        /// Obtiene o asigna el estatus para el valor para el campo 'Registro de fecha para entrevistas' del reporte
        /// </summary>
        public string StatusDateRecordInterviews { get; set; }

        /// <summary>
        /// Obtiene o asigna el valor para el campo 'Cierre de cita' del reporte
        /// </summary>
        public string CloseDate { get; set; }

        /// <summary>
        /// Obtiene o asigna el estatus para el valor para el campo 'Cierre de cita' del reporte
        /// </summary>
        public string StatusCloseDate { get; set; }

        /// <summary>
        /// Obtiene o asigna el valor para el campo 'Respuesta a solicitud' del reporte
        /// </summary>
        public string ResponseToRequest { get; set; }

        /// <summary>
        /// Obtiene o asigna el estatus para el valor para el campo 'Respuesta a solicitud' del reporte
        /// </summary>
        public string StatusResponseToRequest { get; set; }

        /// <summary>
        /// Obtiene o asigna el valor para el campo 'Días totales' del reporte
        /// </summary>
        public int TotalDays { get; set; }

        /// <summary>
        /// Obtiene o asigna el valor para el campo 'Estatus general del tramite' del reporte
        /// </summary>
        public string GeneralStatus { get; set; }
    }
}
