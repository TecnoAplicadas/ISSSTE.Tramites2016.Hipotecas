using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Hipotecas.Model.Api
{
    public class DownloadFileResult
    {
        /// <summary>
        /// Nombre del archivo a descargar
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Contenido del archivo a descargar
        /// </summary>
        public byte[] Data { get; set; }
    }
}
