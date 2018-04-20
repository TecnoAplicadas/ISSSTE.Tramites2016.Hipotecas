#region

using System;
using System.Configuration;
using System.Net;
using System.Xml.Linq;
using ISSSTE.Tramites2016.Common.RenapoService;
using Newtonsoft.Json;

#endregion

namespace ISSSTE.Tramites2016.Common.Renapo
{
    /// <summary>
    ///     Implementación WebService Renapo para validar CURP
    /// </summary>
    public static class Renapo
    {
        /// <summary>
        ///     Valida la curp via renapo
        /// </summary>
        /// <param name="curp"></param>
        /// <returns></returns>
        public static CURPStruct ValidateCurp(string curp)
        {
            var result = new CURPStruct();
            try
            {
                var client = new ConsultaPorCurpServicePortTypeClient();
                var dtos = new DatosConsultaCurp();
                dtos.cveCurp = curp;
                dtos.cveEntidadEmisora = ConfigurationManager.AppSettings["EntidadEmisora"];
                dtos.direccionIp = ConfigurationManager.AppSettings["Ip"];
                dtos.password = ConfigurationManager.AppSettings["Password"];
                dtos.usuario = ConfigurationManager.AppSettings["Usuario"];
                dtos.tipoTransaccion = 5;
                dtos.tipoTransaccionSpecified = true;

                ServicePointManager.ServerCertificateValidationCallback =
                    (senderX, certificate, chain, sslPolicyErrors) => true;
                var ress = client.consultarPorCurp(dtos);

                var x = XElement.Parse(ress);
                if (x.Name == "CURPStruct")
                {
                    result.statusOper = x.Attribute("statusOper").Value;
                    result.message = x.Attribute("message").Value;
                    result.TipoError = x.Attribute("TipoError").Value;
                    result.CodigoError = x.Attribute("CodigoError").Value;
                    result.SessionID = x.Attribute("SessionID").Value;

                    result.CURP = x.Element("CURP").Value;
                    result.nombres = x.Element("nombres").Value;
                    result.apellido1 = x.Element("apellido1").Value;
                    result.apellido2 = x.Element("apellido2").Value;
                    result.fechaNac = x.Element("fechNac").Value;
                    result.nacionalidad = x.Element("nacionalidad").Value;
                    result.statusCurp = x.Element("statusCurp").Value;
                    result.sexo = x.Element("sexo").Value;
                    result.cveEntNacimiento = x.Element("cveEntidadNac").Value;

                }
                if (result.statusOper.ToLower() == "EXITOSO".ToLower())
                {
                    result.statusOperBit = true;
                }

                else {
                    result.statusOperBit = false;
                }

                return result;

                //return false;
            }
            catch (Exception exception)
            {
                result.statusOperBit = false;

                return result;
            }

        }
    }
}