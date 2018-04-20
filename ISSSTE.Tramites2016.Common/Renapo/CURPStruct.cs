namespace ISSSTE.Tramites2016.Common.Renapo
{
    /// <summary>
    ///     Estrucutura de respuesta del servicio de renapo
    /// </summary>
    public class CURPStruct
    {
        /// <summary>
        /// CURp devuelto por WS Renapo
        /// </summary>
        public string CURP { get; set; }

        /// <summary>
        /// Apellido Paterno
        /// </summary>
        public string apellido1 { get; set; }

        /// <summary>
        /// Apellido Materno
        /// </summary>
        public string apellido2 { get; set; }

        /// <summary>
        /// Nombre (s)
        /// </summary>
        public string nombres { get; set; }

        /// <summary>
        /// sexo
        /// </summary>
        public string sexo { get; set; }

        /// <summary>
        /// Fecha Nacimiento
        /// </summary>
        public string fechaNac { get; set; }

        /// <summary>
        /// Nacionalidad
        /// </summary>
        public string nacionalidad { get; set; }

        /// <summary>
        /// Entidad de Nacimiento
        /// </summary>
        public string cveEntNacimiento { get; set; }

        /// <summary>
        /// Estatus del Curpkdd
        /// </summary>
        public string statusCurp { get; set; }

        /// <summary>
        ///     clave de estatus de operacion
        /// </summary>
        public string statusOper { get; set; }

        /// <summary>
        ///     mensaje del resultado
        /// </summary>
        public string message { get; set; }

        /// <summary>
        ///     tipo de error
        /// </summary>
        public string TipoError { get; set; }

        /// <summary>
        ///     codigo de error
        /// </summary>
        public string CodigoError { get; set; }

        /// <summary>
        ///     id de la session
        /// </summary>
        public string SessionID { get; set; }

        /// <summary>
        /// Bit de Estatus 
        /// </summary>
        public bool statusOperBit { get; set; }
    }
}