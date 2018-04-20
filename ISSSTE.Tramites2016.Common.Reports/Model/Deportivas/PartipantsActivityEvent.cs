using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Common.Reports.Model.Deportivas
{
    public class PartipantsActivityEvent
    {
        /// <summary>
        /// Nombre completo del particiante
        /// </summary>
        public string NameFull { get; set; }

        /// <summary>
        /// Numero de issste del participante
        /// </summary>
        public string NoIssste { get; set; }

        /// <summary>
        /// Curp del participante
        /// </summary>
        public string Curp { get; set; }

        /// <summary>
        /// Nombre del equipo del particpante en caso de aplicar
        /// </summary>
        public string Team { get; set; }
    }
}
