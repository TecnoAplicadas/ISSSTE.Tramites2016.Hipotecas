using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Hipotecas.Model.Pocos
{
    public class AppointmentsResult
    {
        /// <summary>
        ///     Id de la solicitud
        /// </summary>
        public Guid RequestId { get; set; }

        /// <summary>
        ///     Folio de la solicitud
        /// </summary>
        public string Folio { get; set; }

        /// <summary>
        ///     Bandera que indica si la solicitud esta asignada
        /// </summary>
        public bool IsAssigned { get; set; }

        /// <summary>
        ///     Descripcion de la Solicitud
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Direccion del Derechohaniente
        /// </summary>
        public string Direccion { get; set; }

        public string DelegationName { get; set; }

        public string DelegationAddress{ get; set; }

        /// <summary>
        ///     Fecha de la Solicitud
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     Fecha de la cita
        /// </summary>
        public DateTime AppointmentDate { get; set; }

        public TimeSpan AppointmentTime{ get; set; }

        /// <summary>
        ///     Id del Usuario que tiene asignada la solicitud
        /// </summary>
        public String UserId { get; set; }

        /// <summary>
        ///     NoIssste del derechohabiente
        /// </summary>
        public String NoISSSTE { get; set; }

        /// <summary>
        ///     Rfc del derechohabiente
        /// </summary>
        public String RFC { get; set; }

        /// <summary>
        ///     CURP del derechohabiente
        /// </summary>
        public String CURP { get; set; }

        /// <summary>
        ///     Nombre del derechohabiente
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        ///     Id de la pension seleccionada
        /// </summary>
        public int PensionId { get; set; }

        /// <summary>
        ///     Nombre de la Pension
        /// </summary>
        public String Pension { get; set; }

        /// <summary>
        ///     Id del Estatus
        /// </summary>
        public int StatusId { get; set; }

        /// <summary>
        ///     Nombre del estatus actual
        /// </summary>
        public String StatusDescription { get; set; }

        /// <summary>
        ///     Role del Estatus
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        ///     Descripción del Rol
        /// </summary>
        public String RoleDescription { get; set; }

        /// <summary>
        ///     Fecha de la Cita
        /// </summary>
        public DateTime DateApp { get; set; }

        /// <summary>
        ///     Hora de la Cita
        /// </summary>
        public TimeSpan TimeApp { get; set; }

        /// <summary>
        ///     Id de la cita
        /// </summary>
        public Guid AppointmentId { get; set; }

        /// <summary>
        ///     Bandera que indica si es atendida
        /// </summary>
        public bool IsAttendent { get; set; }

        /// <summary>
        ///     Id de la delegación
        /// </summary>
        public int DelegationId { get; set; }

        /// <summary>
        ///     Bandera que indica si la cita esta atendida
        /// </summary>
        public bool IsAttended { get; set; }

        /// <summary>
        ///     Fecha del Status
        /// </summary>
        public String StatusDate { get; set; }

        /// <summary>
        ///     Id de la delgacion de la cita
        /// </summary>
        public int DelegationIdApp { get; set; }

        /// <summary>
        ///     Bandera indica si hoy es el dia de la cita
        /// </summary>
        public bool IsinDay { get; set; }

        /// <summary>
        ///     Bandera indica si hoy es el dia de la cita
        /// </summary>
        public bool? IsComplete { get; set; }



    }
}
