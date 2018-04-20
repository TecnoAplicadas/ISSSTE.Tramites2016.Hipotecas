
using ISSSTE.Tramites2016.Hipotecas.DataAccess;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using System;


namespace ISSSTE.Tramites2016.Hipotecas.Bussines
{
    public class EntitleBussiness
    {

        EntitleDataAccess entDA = new EntitleDataAccess();
        public void SaveEntitle(Entitle entitle)
        {


            ////entDA.saveInformationEntitle();
        }
        //    private Tramites2016.Common.Service.Implementation.SipeAvDataService sipeAvServiceAgent = new SipeAvDataService();
        // private CommonBussines commonDomainService = new CommonBussines();


        public void SendMail(string RequestId, int status)
        {
            entDA.SendMail(RequestId, status);
        }

        public string saveInformationEntitle(string noIssste, string lada, string telefono, string email, string mobile)
        {
            string rpta = "";

            try
            {
                entDA.ActualizarEntitle(noIssste, lada, telefono, email, mobile);
                rpta = "OK";
            }
            catch (Exception ex)
            {
                rpta = ex.Message;
            }
            //finally
            //{
            //    DB.Desconectar();
            //}

            return rpta;
        }
    }
}
