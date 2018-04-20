using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Common.Reports.Model;
using ISSSTE.Tramites2016.Common.Reports.Model.Dashboard;
using ISSSTE.Tramites2016.Common.Reports.Model.Ebdis;
using ISSSTE.Tramites2016.Common.Reports.Reports;

namespace ISSSTE.Tramites2016.Common.Reports
{
    public interface IEbdisReportHelper
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
        /// <param name="requestLeaveWaitingList">Número de solicitudes que deC:\Source\Dev-Fase2\Tramites 2015\ISSSTE.Tramites2016.Common.Reports\IEbdisReportHelper.csjaron de estar en lista de espera</param> 
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
        byte[] GetIndicatorsEbdisStream(string delegation, string nameChildCareCenter, string typeChildCareCenter,
                                        string operador, string starDate, string endDate, int requestWaitingList, int requestLeaveWaitingList,
                                        int requestNotAttended, int requestInProcess, int requestRegistered, int requestChanneled, int requestRejected,
                                        int requestRuledInTime, int clinicalAnalsysRuledInTime, int requestAttendedInTime, IEnumerable<int> dateDictamination,
                                        IEnumerable<int> dateClinicalAnalisysReview, ReportFormat format);

        /// <summary>
        /// Obtiene la credencial del niño(a)
        /// </summary>
        /// <param name="numberChildCareCenter">Numero de estancia del niño(a)</param>
        /// <param name="fullNameKid">Nombre completo del niño(a)</param>
        /// <param name="stratum">Estrato donde esatra ubicado el niño(a)</param>
        /// <param name="cycleService">Ciclo de servicio</param>
        /// <param name="fullNameEntitle">Nombre completo del derechohabiente</param>
        /// <returns>Arreglo de bytes con la credencial generada</returns>
        byte[] GeTCredentialKid(int numberChildCareCenter, string fullNameKid, string stratum, string cycleService, string fullNameEntitle);


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
        byte[] GetDashboardEbdis(string delegation, string nameChildCareCenter, string typeChildCareCenter,
                                    string operador, string starDate, string endDate, DashboardData data,
                                    ReportFormat format);

        /// <summary>
        /// Obtiene el certificado de uso de  salas en *.pdf o *.xls
        /// </summary>
        /// <param name="delegation">Delegación donde se consultara el certificado de uso de salas</param>
        /// <param name="nameChildCareCenter">Nombre de la estancia donde se consultaran las salas</param>
        /// <param name="operador">Operardor que realiza el proceso</param>
        /// <param name="city">Ciudada donde se encuantra la estancia</param>
        /// <param name="cycle">Ciclo a consultar</param>
        /// <param name="capacity">Capacidad instalada de la estancia</param>
        /// <param name="format">Indica el formato de salida deseado (pdf o excel)</param>
        /// <returns>Arreglo de bytes con el certificado de uso de salas generado</returns>
        byte[] GetCertificateUseRoom(string delegation, 
                                        string nameChildCareCenter, 
                                        string operador, 
                                        string city, 
                                        string cycle, 
                                        int capacity, 
                                        IEnumerable<CertificateRoom> listRoom, 
                                        ReportFormat format);

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
        byte[] GetServicesChildCareCenter(string town,
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
                                        ReportFormat format);

        
        byte[] GetEbdiAcceptanceLetter(EbdisLetterInfo informtion);
        byte[] GetEbdiPostponedLetter(EbdisLetterInfo informtion);
        byte[] GetEbdiRejectLetter(EbdisLetterInfo informtion);
    }
}
