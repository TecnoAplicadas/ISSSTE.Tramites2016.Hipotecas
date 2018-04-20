using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalDecisions.Shared;
using ISSSTE.Tramites2016.Common.Reports.Model;
using ISSSTE.Tramites2016.Common.Reports.Model.Dashboard;
using ISSSTE.Tramites2016.Common.Reports.Model.Dashboard.Report;
using ISSSTE.Tramites2016.Common.Reports.Reports;
using ISSSTE.Tramites2016.Common.Reports.Resources;
using ISSSTE.Tramites2016.Common.Reports.Model.Ebdis;

namespace ISSSTE.Tramites2016.Common.Reports.Implementation
{
    #region Implementation
    public class EbdisReportHelper : IEbdisReportHelper
    {
        /// <summary>
        /// Obtiene el reporte de indicadores en formato pdf o excel
        /// </summary>
        /// <param name="delegation">Delegación donde se consultaran los indicadores</param>
        /// <param name="nameChildCareCenter">Nombre de la estancia donde se consultaran los indicadores</param>
        /// <param name="typeChildCareCenter">Tipo de la estancia donde se consultaran los indicadores</param>
        /// <param name="operador">Operador que realiza el proceso</param>
        /// <param name="starDate">Limite inferior para la consulta de los indicadores</param>
        /// <param name="endDate">Limite superior para la consulta de los indicadores</param>s
        /// <param name="requestWaitingList">Número de solicitudes en lista de espera</param>s
        /// <param name="requestLeaveWaitingList">Número de solicitudes que dejaron de estar en lista de espera</param> 
        /// <param name="requestNotAttended">Número de solicitudes no atendidas</param>
        /// <param name="requestInProcess">Número de solicitudes en proceso</param>
        /// <param name="requestRegistered">Número de solicitudes inscritas</param>
        /// <param name="requestChanneled">Número de solicitudes canalizadas</param>
        /// <param name="requestRejected">Número de solicitudes rechazadas</param>
        /// <param name="requestRuledInTime">Solicitudes revisadas en el tiempo establecido</param>
        /// <param name="clinicalAnalsysRuledInTime">Solicitudes en las que se reviso los analisis clinicos en tiempo</param>
        /// <param name="requestAttendedInTime">Solicitudes atendidas en el tiempo establecido</param>
        /// <param name="dateDictamination">Lista [entero] con el resultado de (Fecha de revisión de documentación - Fecha envío de recepción de documentación)</param>
        /// <param name="dateClinicalAnalisysReview">Lista [entero] con el resultado de (Fecha de revisón de análisis clínicos - Fecha envío de análisis clínicos)</param>
        /// <param name="format">Indica el formato de salida deseado (pdf o excel)</param>
        /// <returns>Arreglo de bytes con el reporte generado</returns>
        public byte[] GetIndicatorsEbdisStream(string delegation, string nameChildCareCenter, string typeChildCareCenter,
            string operador, string starDate, string endDate, int requestWaitingList, int requestLeaveWaitingList,
            int requestNotAttended, int requestInProcess, int requestRegistered, int requestChanneled, int requestRejected,
            int requestRuledInTime, int clinicalAnalsysRuledInTime, int requestAttendedInTime, IEnumerable<int> dateDictamination, IEnumerable<int> dateClinicalAnalisysReview,
            ReportFormat format)
        {
            var report = BuildEbdisIndicators(delegation, nameChildCareCenter, typeChildCareCenter,
            operador, starDate, endDate, requestWaitingList, requestLeaveWaitingList,
            requestNotAttended, requestInProcess, requestRegistered, requestChanneled, requestRejected,
            requestRuledInTime, clinicalAnalsysRuledInTime, requestAttendedInTime, dateDictamination, dateClinicalAnalisysReview);



            var ebdisIndicators = report.ExportToStream(format == ReportFormat.Excel ? ExportFormatType.Excel : ExportFormatType.PortableDocFormat);

            using (MemoryStream ms = new MemoryStream())
            {
                ebdisIndicators.CopyTo(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Obtiene la credencial del niño(a)
        /// </summary>
        /// <param name="numberChildCareCenter">Numero de estancia del niño(a)</param>
        /// <param name="fullNameKid">Nombre completo del niño(a)</param>
        /// <param name="stratum">Estrato donde esatra ubicado el niño(a)</param>
        /// <param name="cycleService">Ciclo de servicio</param>
        /// <param name="fullNameEntitle">Nombre completo del derechohabiente</param>
        /// <returns>Arreglo de bytes con la credencial generada</returns>
        public byte[] GeTCredentialKid(int numberChildCareCenter, string fullNameKid, string stratum, string cycleService,
            string fullNameEntitle)
        {
            var report = BuildEbdisCredentialKid(numberChildCareCenter, fullNameKid, stratum, cycleService, fullNameEntitle);

            var ebdisCredentialKid = report.ExportToStream(ExportFormatType.PortableDocFormat);

            using (MemoryStream ms = new MemoryStream())
            {
                ebdisCredentialKid.CopyTo(ms);

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Obtiene el tablero de control en formato pdf o excel
        /// </summary>
        /// <param name="delegation">Delegación donde se consultara el tablero de control</param>
        /// <param name="nameChildCareCenter">Nombre de la estancia donde se consultaran el tablero de control</param>
        /// <param name="typeChildCareCenter">Tipo de la estancia donde se consultara el tablero de control</param>
        /// <param name="operador">Operador que realiza el proceso</param>
        /// <param name="starDate">Limite inferior para la consulta del tablero de control</param>
        /// <param name="endDate">Limite superior para la consulta del tablero de control</param>s
        /// <param name="data">Objeto con la información con la cual se construye el tablero de control</param>
        /// <param name="format">Indica el formato de salida deseado (pdf o excel)</param>
        /// <returns>Arreglo de bytes con el tablero generado</returns>
        public byte[] GetDashboardEbdis(string delegation, string nameChildCareCenter, string typeChildCareCenter, string operador,
            string starDate, string endDate, DashboardData data, ReportFormat format)
        {
            var report = BuildEbdisDashboard(delegation, nameChildCareCenter,typeChildCareCenter, operador, starDate, endDate, data);

            var dashboard = report.ExportToStream(format == ReportFormat.Excel ? ExportFormatType.Excel : ExportFormatType.PortableDocFormat);

            using (MemoryStream ms = new MemoryStream())
            {
                dashboard.CopyTo(ms);

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Obtiene el certificado de uso de  salas en *.pdf o *.xls
        /// </summary>
        /// <param name="delegation">Delegación donde se consultara el certificado de uso de salas</param>
        /// <param name="nameChildCareCenter">Nombre de la estancia donde se consultaran las salas</param>
        /// <param name="operador">Operardor que realiza el proceso</param>
        /// <param name="city">Ciudada donde se encuantra la estancia</param>
        /// <param name="cycle">Ciclo a consultar</param>
        /// <param name="capacity">Capacidad instalada de la estancia</param>
        /// <param name="listRoom">lista de salas</param>
        /// <param name="format">formato del reporte</param>
        /// <returns>Arreglo de bytes con el certificado de uso de salas generado</returns>
        public byte[] GetCertificateUseRoom(string delegation, string nameChildCareCenter, string operador, string city, string cycle,
            int capacity, IEnumerable<CertificateRoom> listRoom, ReportFormat format)
        {
            var report = BuildEbdisCertificateUseRoom(delegation, nameChildCareCenter, operador, city, cycle, capacity,listRoom);

            var certificate = report.ExportToStream(format == ReportFormat.Excel ? ExportFormatType.Excel : ExportFormatType.PortableDocFormat);

            using (MemoryStream ms = new MemoryStream())
            {
                certificate.CopyTo(ms);

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Obtiene el reporte de servicios de estancias infantiles en formato *-pdf o *.xls
        /// </summary>
        /// <param name="town">Municipio</param>
        /// <param name="delegation">Delegación de la estancia</param>
        /// <param name="nameChildCareCenter">Nombre de la estancia</param>
        /// <param name="numberChildCareCenter">Numero de la estancia</param>
        /// <param name="modality">Modalidad de la estancia</param>
        /// <param name="cycle">Ciclo a consultar</param>
        /// <param name="operador">Operador que realiza el proceso</param>
        /// <param name="registerLastDayBefore">Lista con el total ce niños y niñas inscritos al ultimo dia del mes anterior</param>
        /// <param name="registerInMonth">Lista con el total de niños y niñas dados de alta en el mes</param>
        /// <param name="dropInMonth">Lista con el total de niños y niñas dados de baja en el mes</param>
        /// <param name="registerCurrent">Lista con el total de niños y niñas incritos actualmente al ultimo día del mes</param>
        /// <param name="waitingList">Lista con el total de niños y niñas en lista de espera</param>
        /// <param name="disabilityKids">Lista con el total de niños y niñas con discapacidad</param>
        /// <param name="format">Indica el formato de salida deseado (pdf o excel)</param>
        /// <returns>Arreglo de bytes con el reporte de servicios de estancias infantiles generado</returns>
        public byte[] GetServicesChildCareCenter(string town, 
                                                string delegation, 
                                                string nameChildCareCenter, 
                                                int numberChildCareCenter,
                                                string modality, 
                                                string cycle, 
                                                string operador, 
                                                IEnumerable<ServicesChildCareCenter> registerLastDayBefore, 
                                                IEnumerable<ServicesChildCareCenter> registerInMonth,
                                                IEnumerable<ServicesChildCareCenter> dropInMonth, 
                                                IEnumerable<ServicesChildCareCenter> registerCurrent, 
                                                IEnumerable<ServicesChildCareCenter> waitingList, 
                                                IEnumerable<ServicesChildCareCenter> disabilityKids,
                                                ReportFormat format)
        {
            var report = BuildEbdisServiceChildCareCenter(town,
                                                        delegation,
                                                        nameChildCareCenter,
                                                        numberChildCareCenter,
                                                        modality,
                                                        cycle,
                                                        operador,
                                                        registerLastDayBefore,
                                                        registerInMonth,
                                                        dropInMonth,
                                                        registerCurrent,
                                                        waitingList,
                                                        disabilityKids);

            var certificate = report.ExportToStream(format == ReportFormat.Excel ? ExportFormatType.Excel : ExportFormatType.PortableDocFormat);

            using (MemoryStream ms = new MemoryStream())
            {
                certificate.CopyTo(ms);

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Obtiene el reporte de carta de aceptación
        /// </summary>
        /// <param name="informtion">Objeto que encapsula los parámetros del reporte</param>
        /// <returns></returns>
        public byte[] GetEbdiAcceptanceLetter(EbdisLetterInfo informtion)
        {
            var report = BuildEbdiAcceptanceLetter(informtion);
            var acceptanceLetter = report.ExportToStream(ExportFormatType.PortableDocFormat);
            using (MemoryStream ms = new MemoryStream())
            {
                acceptanceLetter.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public byte[] GetEbdiPostponedLetter(EbdisLetterInfo informtion)
        {
            var report = BuildEbdiPostponedLetter(informtion);
            var postponedLetter = report.ExportToStream(ExportFormatType.PortableDocFormat);
            using (MemoryStream ms = new MemoryStream())
            {
                postponedLetter.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public byte[] GetEbdiRejectLetter(EbdisLetterInfo informtion)
        {
            var report = BuildEbdiRejectLetter(informtion);
            var rejectLetter = report.ExportToStream(ExportFormatType.PortableDocFormat);
            using (MemoryStream ms = new MemoryStream())
            {
                rejectLetter.CopyTo(ms);
                return ms.ToArray();
            }
        }

        #endregion

        #region Helpers
        /// <summary>
        /// Construye el reporte de indicadores en formato pdf o excel
        /// </summary>
        /// <param name="delegation">Delegación donde se consultaran los indicadores</param>
        /// <param name="nameChildCareCenter">Nombre de la estancia donde se consultaran los indicadores</param>
        /// <param name="typeChildCareCenter">Tipo de la estancia donde se consultaran los indicadores</param>
        /// <param name="operador">Operador que realiza el proceso</param>
        /// <param name="starDate">Limite inferior para la consulta de los indicadores</param>
        /// <param name="endDate">Limite superior para la consulta de los indicadores</param>s
        /// <param name="requestWaitingList">Número de solicitudes en lista de espera</param>s
        /// <param name="requestLeaveWaitingList">Número de solicitudes que dejaron de estar en lista de espera</param> 
        /// <param name="requestNotAttended">Número de solicitudes no atendidas</param>
        /// <param name="requestInProcess">Número de solicitudes en proceso</param>
        /// <param name="requestRegistered">Número de solicitudes inscritas</param>
        /// <param name="requestChanneled">Número de solicitudes canalizadas</param>
        /// <param name="requestRejected">Número de solicitudes rechazadas</param>
        /// <param name="requestRuledInTime">Solicitudes revisadas en el tiempo establecido</param>
        /// <param name="clinicalAnalsysRuledInTime">Solicitudes en las que se reviso los analisis clinicos en tiempo</param>
        /// <param name="requestAttendedInTime">Solicitudes atendidas en el tiempo establecido</param>
        /// <param name="dateDictamination">Lista [entero] con el resultado de (Fecha de revisión de documentación - Fecha envío de recepción de documentación)</param>
        /// <param name="dateClinicalAnalisysReview">Lista [entero] con el resultado de (Fecha de revisón de análisis clínicos - Fecha envío de análisis clínicos)</param>
        /// <returns>Objeto con el reporte generado</returns>
        private EbdisIndicators BuildEbdisIndicators(string delegation, string nameChildCareCenter, string typeChildCareCenter,
                                            string operador, string starDate, string endDate, int requestWaitingList,
                                            int requestLeaveWaitingList, int requestNotAttended, int requestInProcess,
                                            int requestRegistered, int requestChanneled, int requestRejected, int requestRuledInTime, int clinicalAnalsysRuledInTime,
                                            int requestAttendedInTime, IEnumerable<int> dateDictamination, IEnumerable<int> dateClinicalAnalisysReview)
        {
            EbdisIndicators report = new EbdisIndicators();

            //Se cargan los datos del titulo de propiedad
            int diffDateDictamination = dateDictamination.Sum();
            int diffDateAssignment = dateClinicalAnalisysReview.Sum();

            report.SetParameterValue(report.Parameter_Delegacion.ParameterFieldName, delegation);
            report.SetParameterValue(report.Parameter_Estancia.ParameterFieldName, delegation);
            report.SetParameterValue(report.Parameter_TipoEstancai.ParameterFieldName, delegation);
            report.SetParameterValue(report.Parameter_Operador.ParameterFieldName, operador);
            report.SetParameterValue(report.Parameter_Desde.ParameterFieldName, starDate ?? ReportValues.DefaultStartDate);
            report.SetParameterValue(report.Parameter_Hasta.ParameterFieldName, endDate ?? ReportValues.DefaultEndDate);
            report.SetParameterValue(report.Parameter_SolicitudesListaEspera.ParameterFieldName, requestWaitingList);
            report.SetParameterValue(report.Parameter_SolicitudesDejaronEstarListaEspera.ParameterFieldName, requestLeaveWaitingList);
            report.SetParameterValue(report.Parameter_SolicitudesNoAtendidas.ParameterFieldName, requestNotAttended);
            report.SetParameterValue(report.Parameter_SolicitudesProceso.ParameterFieldName, requestInProcess);
            report.SetParameterValue(report.Parameter_SolicitudesInscritas.ParameterFieldName, requestRegistered);
            report.SetParameterValue(report.Parameter_SolicitudesCanalizadas.ParameterFieldName, requestChanneled);
            report.SetParameterValue(report.Parameter_SolicitudesRechazadas.ParameterFieldName, requestRejected);
            report.SetParameterValue(report.Parameter_SolicitudesConRevisionDocumentacion.ParameterFieldName, dateDictamination.Count());
            report.SetParameterValue(report.Parameter_SolicitudesConRevisionAnalisis.ParameterFieldName, dateClinicalAnalisysReview.Count());
            report.SetParameterValue(report.Parameter_SolicitudesDictaminadasTiempo.ParameterFieldName, requestRuledInTime);
            report.SetParameterValue(report.Parameter_SolicitudesAnalisisRevisadosTiempo.ParameterFieldName, clinicalAnalsysRuledInTime);
            report.SetParameterValue(report.Parameter_SolicitudesAtendidasTiempo.ParameterFieldName, requestAttendedInTime);            
            report.SetParameterValue(report.Parameter_DiferenciaFechaDictaminacion.ParameterFieldName, diffDateDictamination);
            report.SetParameterValue(report.Parameter_DiferenciaFechaAnalisis.ParameterFieldName, diffDateAssignment);

            return report;
        }

        /// <summary>
        /// Construye la credencial del niño(a)
        /// </summary>
        /// <param name="numberChildCareCenter">Numero de estancia del niño(a)</param>
        /// <param name="fullNameKid">Nombre completo del niño(a)</param>
        /// <param name="stratum">Estrato donde esatra ubicado el niño(a)</param>
        /// <param name="cycleService">Ciclo de servicio</param>
        /// <param name="fullNameEntitle">Nombre completo del derechohabiente</param>
        /// <returns>Objeto con la credencial generada</returns>
        private EbdisCredential BuildEbdisCredentialKid(int numberChildCareCenter, string fullNameKid, string stratum,
            string cycleService, string fullNameEntitle)
        {
            EbdisCredential credential = new EbdisCredential();

            credential.SetParameterValue(credential.Parameter_NumeroEstancia.ParameterFieldName, numberChildCareCenter);
            credential.SetParameterValue(credential.Parameter_NombreNiño.ParameterFieldName, fullNameKid);
            credential.SetParameterValue(credential.Parameter_Estrato.ParameterFieldName, stratum);
            credential.SetParameterValue(credential.Parameter_CicloServicio.ParameterFieldName, cycleService);
            credential.SetParameterValue(credential.Parameter_NombreDerechohabiente.ParameterFieldName, fullNameEntitle);

            return credential;
        }

        /// <summary>
        /// Construye el tablero de control en formato pdf o excel
        /// </summary>
        /// <param name="delegation">Delegación donde se consultara el tablero de control</param>
        /// <param name="nameChildCareCenter">Nombre de la estancia donde se consultaran el tablero de control</param>
        /// <param name="typeChildCareCenter">Tipo de la estancia donde se consultara el tablero de control</param>
        /// <param name="operador">Operador que realiza el proceso</param>
        /// <param name="starDate">Limite inferior para la consulta del tablero de control</param>
        /// <param name="endDate">Limite superior para la consulta del tablero de control</param>s
        /// <param name="data">Objeto con la información con la cual se construye el tablero de control</param>
        /// <returns>Objeto con el tablero generado</returns>
        private EbdisDashboard BuildEbdisDashboard(string delegation, string nameChildCareCenter, string typeChildCareCenter, 
                                                    string operador, string starDate, string endDate, DashboardData data)
        {
            EbdisDashboard dashboard = new EbdisDashboard();
            
            dashboard.Subreports[0].SetDataSource(ConvertToProcedureRequestReport(data.ProcedureRequest));
            dashboard.Subreports[1].SetDataSource(ConvertToProcedureStatusReport(data.ProcedureStatus));

            dashboard.SetParameterValue(dashboard.Parameter_Delegacion.ParameterFieldName, delegation);
            dashboard.SetParameterValue(dashboard.Parameter_Estancia.ParameterFieldName, nameChildCareCenter);
            dashboard.SetParameterValue(dashboard.Parameter_TipoEstancai.ParameterFieldName, typeChildCareCenter);
            dashboard.SetParameterValue(dashboard.Parameter_Operador.ParameterFieldName, operador);
            dashboard.SetParameterValue(dashboard.Parameter_Desde.ParameterFieldName, starDate ?? ReportValues.DefaultStartDate);
            dashboard.SetParameterValue(dashboard.Parameter_Hasta.ParameterFieldName, endDate ?? ReportValues.DefaultEndDate);


            return dashboard;
        }

        /// <summary>
        /// Obtiene una lista de objetos ProcedureStatusHelperReport requeridos para el reporte
        /// </summary>
        /// <param name="procedureStatus">Objeto a convertir en ProcedureStatusHelperReport</param>
        /// <returns>Lista de objetos</returns>
        private IEnumerable<ProcedureStatusHelperReport> ConvertToProcedureStatusReport(IEnumerable<ProcedureStatus> procedureStatus)
        {
            return procedureStatus.Select(item => new ProcedureStatusHelperReport()
            {
                Name = item.Name,
                DueDays = item.DueDays,
                Average = item.Average ?? 0,
                FulfillmentPercentage = (item.FulfillmentPercentage == null) ? 0 : (int)item.FulfillmentPercentage
            }).ToList();
        }

        /// <summary>
        /// Obtiene una lista de objetos ProcedureRequestHelpertReport requeridos para el reporte
        /// </summary>
        /// <param name="procedureRequest">Objeto a convertir en ProcedureRequestHelpertReport</param>
        /// <returns>Lista de objetos</returns>
        private IEnumerable<ProcedureRequestHelpertReport> ConvertToProcedureRequestReport(
            IEnumerable<ProcedureRequest> procedureRequest)
        {
            List<ProcedureRequestHelpertReport> listRequest = new List<ProcedureRequestHelpertReport>();

            if (!procedureRequest.Any())
            {
                var request = new ProcedureRequestHelpertReport()
                {
                    Number = 0,
                    Folio = ReportValues.DefaultEmptyProcedureStatus,
                    IsssteNumber = ReportValues.DefaultEmptyProcedureStatus,
                    EntitleName = ReportValues.DefaultEmptyProcedureStatus,
                    KidName = ReportValues.DefaultEmptyProcedureStatus,
                    DelegationName = ReportValues.DefaultEmptyProcedureStatus,
                    IsCancel = false,
                    SelectionOfChildCareCenter = ReportValues.DefaultEmptyProcedureStatus,
                    StatusSelectionOfChildCareCenter = ReportValues.DefaultEmptyProcedureStatus,
                    AssignmentOfPlace = ReportValues.DefaultEmptyProcedureStatus,
                    StatusAssignmentOfPlace = ReportValues.DefaultEmptyProcedureStatus,
                    DocumentationFirstLoad = ReportValues.DefaultEmptyProcedureStatus,
                    StatusDocumentationFirstLoad = ReportValues.DefaultEmptyProcedureStatus,
                    DocumentInitialValidate = ReportValues.DefaultEmptyProcedureStatus,
                    StatusDocumentInitialValidate = ReportValues.DefaultEmptyProcedureStatus,
                    ClinicalAnalyzesLoad = ReportValues.DefaultEmptyProcedureStatus,
                    StatusClinicalAnalyzesLoad = ReportValues.DefaultEmptyProcedureStatus,
                    ClinicalAnalyzesValidate = ReportValues.DefaultEmptyProcedureStatus,
                    StatusClinicalAnalyzesValidate = ReportValues.DefaultEmptyProcedureStatus,
                    DateRecordInterviews = ReportValues.DefaultEmptyProcedureStatus,
                    StatusDateRecordInterviews = ReportValues.DefaultEmptyProcedureStatus,
                    CloseDate = ReportValues.DefaultEmptyProcedureStatus,
                    StatusCloseDate = ReportValues.DefaultEmptyProcedureStatus,
                    ResponseToRequest = ReportValues.DefaultEmptyProcedureStatus,
                    StatusResponseToRequest = ReportValues.DefaultEmptyProcedureStatus,
                    TotalDays = 0,
                    GeneralStatus = ReportValues.DefaultEmptyProcedureStatus
                };

                listRequest.Add(request);
            }
            else
            {
                listRequest.AddRange(procedureRequest.Select(item => new ProcedureRequestHelpertReport()
                {
                    Number = item.Index,
                    Folio = item.Folio,
                    IsssteNumber = item.IsssteNumber,
                    EntitleName = item.EntitleName,
                    KidName = item.KidName, DelegationName = 
                    item.DelegationName,
                    IsCancel = item.IsCancel,
                    SelectionOfChildCareCenter = 
                    item.StatusEntries[0]?.Date,
                    StatusSelectionOfChildCareCenter = 
                    item.StatusEntries[0]?.Fulfillment.ToString(),
                    AssignmentOfPlace = item.StatusEntries[1]?.Date,
                    StatusAssignmentOfPlace = item.StatusEntries[1]?.Fulfillment.ToString(),
                    DocumentationFirstLoad = item.StatusEntries[2]?.Date,
                    StatusDocumentationFirstLoad = item.StatusEntries[2]?.Fulfillment.ToString(),
                    DocumentInitialValidate = item.StatusEntries[3]?.Date,
                    StatusDocumentInitialValidate = item.StatusEntries[3]?.Fulfillment.ToString(),
                    ClinicalAnalyzesLoad = item.StatusEntries[4]?.Date,
                    StatusClinicalAnalyzesLoad = item.StatusEntries[4]?.Fulfillment.ToString(),
                    ClinicalAnalyzesValidate = item.StatusEntries[5]?.Date,
                    StatusClinicalAnalyzesValidate = item.StatusEntries[5]?.Fulfillment.ToString(),
                    DateRecordInterviews = item.StatusEntries[6]?.Date,
                    StatusDateRecordInterviews = item.StatusEntries[6]?.Fulfillment.ToString(),
                    CloseDate = item.StatusEntries[7]?.Date,
                    StatusCloseDate = item.StatusEntries[7]?.Fulfillment.ToString(),
                    ResponseToRequest = item.StatusEntries[8]?.Date,
                    StatusResponseToRequest = item.StatusEntries[8]?.Fulfillment.ToString(),
                    TotalDays = item.TotalElapsedDays, GeneralStatus = item.Fullfilment.ToString()
                }));
            }
            return listRequest;
        }

        /// <summary>
        /// Construye el certificado de uso de  salas en *.pdf o *.xls
        /// </summary>
        /// <param name="delegation">Delegación donde se consultara el certificado de uso de salas</param>
        /// <param name="nameChildCareCenter">Nombre de la estancia donde se consultaran las salas</param>
        /// <param name="operador">Operardor que realiza el proceso</param>
        /// <param name="city">Ciudada donde se encuantra la estancia</param>
        /// <param name="cycle">Ciclo a consultar</param>
        /// <param name="capacity">Capacidad instalada de la estancia</param>
        /// <returns>Objeto con el certificado de uso de salas generado</returns>
        private EbdisCertificateUseRoom BuildEbdisCertificateUseRoom(string delegation,
                                                                    string nameChildCareCenter,
                                                                    string operador,
                                                                    string city,
                                                                    string cycle,
                                                                    int capacity,
                                                                    IEnumerable<CertificateRoom> listRoom)
        {
            EbdisCertificateUseRoom certificate = new EbdisCertificateUseRoom();

            certificate.SetDataSource(listRoom);

            certificate.SetParameterValue(certificate.Parameter_Delegacion.ParameterFieldName, delegation);
            certificate.SetParameterValue(certificate.Parameter_Estancia.ParameterFieldName, nameChildCareCenter);
            certificate.SetParameterValue(certificate.Parameter_Operador.ParameterFieldName, operador);
            certificate.SetParameterValue(certificate.Parameter_Ciudad.ParameterFieldName, city);
            certificate.SetParameterValue(certificate.Parameter_Ciclo.ParameterFieldName, cycle);
            certificate.SetParameterValue(certificate.Parameter_Capacidad.ParameterFieldName, capacity);

            return certificate;
        }

        /// <summary>
        /// Construye el reporte de servicios de estancias infantiles en formato *-pdf o *.xls
        /// </summary>
        /// <param name="town">Municipio</param>
        /// <param name="delegation">Delegación de la estancia</param>
        /// <param name="nameChildCareCenter">Nombre de la estancia</param>
        /// <param name="numberChildCareCenter">Numero de la estancia</param>
        /// <param name="modality">Modalidad de la estancia</param>
        /// <param name="cycle">Ciclo a consultar</param>
        /// <param name="operador">Operador que realiza el proceso</param>
        /// <param name="registerLastDayBefore">Lista con el total ce niños y niñas inscritos al ultimo dia del mes anterior</param>
        /// <param name="registerInMonth">Lista con el total de niños y niñas dados de alta en el mes</param>
        /// <param name="dropInMonth">Lista con el total de niños y niñas dados de baja en el mes</param>
        /// <param name="registerCurrent">Lista con el total de niños y niñas incritos actualmente al ultimo día del mes</param>
        /// <param name="waitingList">Lista con el total de niños y niñas en lista de espera</param>
        /// <param name="disabilityKids">Lista con el total de niños y niñas con discapacidad</param>
        /// <returns>Objeto con el reporte de servicios de estancias infantiles generado</returns>
        private EbdisStatistic BuildEbdisServiceChildCareCenter(string town,
                                                                string delegation,
                                                                string nameChildCareCenter,
                                                                int numberChildCareCenter,
                                                                string modality,
                                                                string cycle,
                                                                string operador,
                                                                IEnumerable<ServicesChildCareCenter> registerLastDayBefore,
                                                                IEnumerable<ServicesChildCareCenter> registerInMonth,
                                                                IEnumerable<ServicesChildCareCenter> dropInMonth,
                                                                IEnumerable<ServicesChildCareCenter> registerCurrent,
                                                                IEnumerable<ServicesChildCareCenter> waitingList,
                                                                IEnumerable<ServicesChildCareCenter> disabilityKids)
        {
            EbdisStatistic reportServices = new EbdisStatistic();

            //Asignar source a subreportes
            reportServices.Subreports["registerLastDayBefore"].SetDataSource(registerLastDayBefore);
            //reportServices.Subreports["registerInMonth"].SetDataSource(registerInMonth);
            reportServices.Subreports["StaticDetail.rpt"].SetDataSource(registerInMonth);
            //reportServices.Subreports["drop"].SetDataSource(dropInMonth);
            reportServices.Subreports["StaticDetail.rpt - 01"].SetDataSource(dropInMonth);
            //reportServices.Subreports["current"].SetDataSource(registerCurrent);
            reportServices.Subreports["StaticDetail.rpt - 02"].SetDataSource(registerCurrent);
            //reportServices.Subreports["waiting"].SetDataSource(waitingList);
            reportServices.Subreports["StaticDetail.rpt - 03"].SetDataSource(waitingList);
            //reportServices.Subreports["disabilityKids"].SetDataSource(disabilityKids);
            reportServices.Subreports["StaticDetail.rpt - 04"].SetDataSource(disabilityKids);
            //...
            //...

            //Asignar parametros
            reportServices.SetParameterValue(reportServices.Parameter_Municipio.ParameterFieldName, town);
            reportServices.SetParameterValue(reportServices.Parameter_Delegacion.ParameterFieldName, delegation);
            reportServices.SetParameterValue(reportServices.Parameter_Estancia.ParameterFieldName, nameChildCareCenter);
            reportServices.SetParameterValue(reportServices.Parameter_NumeroEstancia.ParameterFieldName, numberChildCareCenter);
            reportServices.SetParameterValue(reportServices.Parameter_Modalidad.ParameterFieldName, modality);
            reportServices.SetParameterValue(reportServices.Parameter_Ciclo.ParameterFieldName, cycle);
            reportServices.SetParameterValue(reportServices.Parameter_Operador.ParameterFieldName, operador);

            return reportServices;
        }

        /// <summary>
        /// Construye el reporte de carta de aeptación
        /// </summary>
        /// <param name="info">Objeto que encapsula la información de la carta</param>
        /// <returns>Objeto con el reporte de carta de aceptación generado</returns>
        private EbdisAcceptance BuildEbdiAcceptanceLetter(EbdisLetterInfo info)
        {
            EbdisAcceptance reportAccpetance = new EbdisAcceptance();

            reportAccpetance.SetParameterValue(reportAccpetance.Parameter_day.ParameterFieldName, info.Day);
            reportAccpetance.SetParameterValue(reportAccpetance.Parameter_month.ParameterFieldName, info.Month);
            reportAccpetance.SetParameterValue(reportAccpetance.Parameter_beneficiaryName.ParameterFieldName, info.BeneficiaryName);
            reportAccpetance.SetParameterValue(reportAccpetance.Parameter_ebdiNumber.ParameterFieldName, info.EbdiNumber);
            reportAccpetance.SetParameterValue(reportAccpetance.Parameter_kidName.ParameterFieldName, info.KidName);
            reportAccpetance.SetParameterValue(reportAccpetance.Parameter_year.ParameterFieldName, info.Year);
            
            return reportAccpetance;
        }

        /// <summary>
        /// Construye el reporte de carta de ingreso pospuesto
        /// </summary>
        /// <param name="info">Objeto que encapsula la información de la carta</param>
        /// <returns>Objeto con el reporte de carta de ingreso pospuesto generado</returns>
        private EbdisPostponed BuildEbdiPostponedLetter(EbdisLetterInfo info)
        {
            EbdisPostponed reportPostponed = new EbdisPostponed();

            reportPostponed.SetParameterValue(reportPostponed.Parameter_day.ParameterFieldName, info.Day);
            reportPostponed.SetParameterValue(reportPostponed.Parameter_month.ParameterFieldName, info.Month);
            reportPostponed.SetParameterValue(reportPostponed.Parameter_year.ParameterFieldName, info.Year);
            reportPostponed.SetParameterValue(reportPostponed.Parameter_beneficiaryName.ParameterFieldName, info.BeneficiaryName);
            reportPostponed.SetParameterValue(reportPostponed.Parameter_ebdiNumber.ParameterFieldName, info.EbdiNumber);
            reportPostponed.SetParameterValue(reportPostponed.Parameter_kidName.ParameterFieldName, info.KidName);

            return reportPostponed;
        }

        /// <summary>
        /// Construye el reporte de carta de rechazo
        /// </summary>
        /// <param name="info">Objeto que encapsula la información de la carta</param>
        /// <returns>Objeto con el reporte de carta de rechazo generado</returns>
        private EbdisReject BuildEbdiRejectLetter(EbdisLetterInfo info)
        {
            EbdisReject reportReject = new EbdisReject();

            reportReject.SetParameterValue(reportReject.Parameter_day.ParameterFieldName, info.Day);
            reportReject.SetParameterValue(reportReject.Parameter_month.ParameterFieldName, info.Month);
            reportReject.SetParameterValue(reportReject.Parameter_year.ParameterFieldName, info.Year);
            reportReject.SetParameterValue(reportReject.Parameter_beneficiaryName.ParameterFieldName, info.BeneficiaryName);
            reportReject.SetParameterValue(reportReject.Parameter_ebdiNumber.ParameterFieldName, info.EbdiNumber);
            reportReject.SetParameterValue(reportReject.Parameter_kidName.ParameterFieldName, info.KidName);

            return reportReject;
        }

        #endregion
    }
}
