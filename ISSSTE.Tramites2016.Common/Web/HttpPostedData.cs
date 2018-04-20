#region

using System.Collections.Generic;

#endregion

namespace ISSSTE.Tramites2016.Common.Web
{
    /// <summary>
    /// Representa información enviada en una petición como multipart
    /// </summary>
    public class HttpPostedData
    {
        #region Properties

        /// <summary>
        /// Obtiene los campos enviados en la petición
        /// </summary>
        public IDictionary<string, HttpPostedField> Fields { get; private set; }

        /// <summary>
        /// Obtiene la lista de archivos enviados en la petición
        /// </summary>
        public IDictionary<string, HttpPostedFile> Files { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="fields">Lista de campos</param>
        /// <param name="files">Lista de archivos</param>
        public HttpPostedData(IDictionary<string, HttpPostedField> fields, IDictionary<string, HttpPostedFile> files)
        {
            Fields = fields;
            Files = files;
        }

        #endregion       
    }
}