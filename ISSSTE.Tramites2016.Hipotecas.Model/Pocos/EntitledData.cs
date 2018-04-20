#region

using System;
using System.Collections.Generic;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Pocos
{
    /// <summary>
    ///     Objeto que represneta los datos de un derechohabiente para el api
    /// </summary>
    public class EntitledData
    {
        /// <summary>
        ///     Id de la solicitud
        /// </summary>
        public Guid RequestId { get; set; }

        /// <summary>
        ///     Id del tipo de pension
        /// </summary>
        public int PensionId { get; set; }

        /// <summary>
        ///     OBjeto que representa al derechohabiente
        /// </summary>
        public Entitle Entitle { get; set; }

     
        /// <summary>
        ///     Lista de Deudos
        /// </summary>
        public List<Debtor> Debtors { get; set; }

       
        /// <summary>
        ///     Respues de la validacion de informacion
        /// </summary>
        //public Validation Validation { get; set; }

        /// <summary>
        ///     Datos de la solicitud
        /// </summary>
        public Request Request { get; set; }

        public string ErrorMesage { get; set; }

        public string InconsistenciesLHMessages { get; set; }

        public string MessageInfoRO { get; set; }

        public string totalContributionTime { get; set; }

        public string totalLicensesTime { get; set; }

        public string totalTime { get; set; }

        public string ApplicationMessages { get; set; }

        public TimeContribution timeContributions { get; set; }


    }
}