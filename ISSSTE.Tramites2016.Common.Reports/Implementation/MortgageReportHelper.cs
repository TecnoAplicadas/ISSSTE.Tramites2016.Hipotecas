using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalDecisions.Shared;
using System.IO;
using ISSSTE.Tramites2016.Common.Reports.Reports;
using ISSSTE.Tramites2016.Common.Reports.Model.Hipoteca;

namespace ISSSTE.Tramites2016.Common.Reports.Implementation
{
    public class MortgageReportHelper : IMortgageReportHelper
    {
        /// <summary>
        /// obtiene el reporte de cancelación de hipoteca en formato pdf
        /// </summary>
        /// </summary>
        /// <param name="date">Fecha de realizacion del trámite</param>
        /// <param name="mortgage">descripcion del inmueble hipotecado</param>
        /// <param name="name">Nombre del derechohabiente</param>
        /// <param name="telephone">telefono del derechohabiente</param>
        /// <returns></returns>
        public byte[] GetMortgageCancel(MortgageCancel mortgage)
        {
            var report = BuildMortgageCancel(mortgage);
            var mortageCancel = report.ExportToStream(ExportFormatType.PortableDocFormat);

            using (MemoryStream ms = new MemoryStream())
            {
                mortageCancel.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private MortgageCancelReport BuildMortgageCancel(MortgageCancel mortgage)
        {
            MortgageCancelReport report = new MortgageCancelReport();

            report.SetParameterValue(report.Parameter_Nombre.ParameterFieldName, mortgage.Name);
            report.SetParameterValue(report.Parameter_Telefono.ParameterFieldName, mortgage.Telephone);
            report.SetParameterValue(report.Parameter_Inmueble.ParameterFieldName, mortgage.Property);
            report.SetParameterValue(report.Parameter_Cuidad.ParameterFieldName, mortgage.City);
            report.SetParameterValue(report.Parameter_Fecha.ParameterFieldName, mortgage.Date);
            report.SetParameterValue(report.Parameter_TelefonoMovil.ParameterFieldName, mortgage.MobileTelephone ?? "Sin teléfono móvil");
            return report;
        }


    }
}
