using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Hipotecas.Model.Pocos
{
    /// <summary>
    ///     Objeto que sirve para mostrar los datos para el reporte de cancelación de hipoteca
    /// </summary>
    public class MortgageCancelReportData
    {
        public MortgageCancelReportData(string name, string city, DateTime date, string writingProperty, string telephone, string mobileTelephone)
        {
            Name = name;
            City = city;
            Date = date;
            WritingProperty = writingProperty;
            Telephone = telephone;
            MobileTelephone = mobileTelephone;
        }
        public MortgageCancelReportData()
        {
            Name = "";
            City = "";
            Date = DateTime.Now;
            WritingProperty = "";
            Telephone = "";
            MobileTelephone = "";
        }
        #region constructores

        #endregion

        #region Propiedades
        /// <summary>
        ///     Nombre del Derechohabiente
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        ///     Ciudad del Derechohabiente
        /// </summary>
        public String City { get; set; }

        /// <summary>
        ///     Fecha de la Solicitud
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     Propiedad de la Solicitud
        /// </summary>
        public String WritingProperty { get; set; }

        /// <summary>
        ///     Teléfono del Derechohabiente
        /// </summary>
        public String Telephone { get; set; }

        /// <summary>
        ///     Teléfono Movil del Derechohabiente
        /// </summary>
        public String MobileTelephone { get; set; }
        #endregion
    }
}
