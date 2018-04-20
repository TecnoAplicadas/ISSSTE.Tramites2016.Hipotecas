using ISSSTE.Tramites2016.Hipotecas.Model.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;

namespace ISSSTE.Tramites2016.Hipotecas.DataAccess
{
    public class CatalogsDataAccess
    {

        Conection con = new Conection();
        public List<TypeOwner> SelectTypeOwner()
        {

            DataSet ds = new DataSet();
            List<TypeOwner> listTO = new List<TypeOwner>();

            string query = "exec spS_TypeOwner";

            ds = con.ObtenerConsulta(query);

            foreach (DataRow DR in ds.Tables[0].Rows)
            {
                TypeOwner to = new TypeOwner();
                to.IdTypeOwner = Convert.ToInt32(DR["IdTypeOwner"].ToString());
                to.Description = DR["Description"].ToString();
                to.Active = Convert.ToBoolean(DR["Active"].ToString());

                listTO.Add(to);

            }

            return listTO;
        }


        public List<PropertType> SelectPropertType()
        {

            DataSet ds = new DataSet();
            List<PropertType> listPT = new List<PropertType>();

            string query = "exec spS_PropertType";

            ds = con.ObtenerConsulta(query);

            foreach (DataRow DR in ds.Tables[0].Rows)
            {
                PropertType pt = new PropertType();
                pt.IdPropertyType = Convert.ToInt32(DR["IdPropertyType"].ToString());
                pt.Description = DR["Description"].ToString();
                pt.Active = Convert.ToBoolean(DR["Active"].ToString());

                listPT.Add(pt);

            }

            return listPT;

        }



        public List<UrbanCenter> SelectUrbanCenterByID(string idUrbanCenter)
        {

            DataSet ds = new DataSet();
            List<UrbanCenter> listUC = new List<UrbanCenter>();

            string query = "exec sps_UrbanCenterByID @idUrbanCenter =  " + idUrbanCenter + "";

            ds = con.ObtenerConsulta(query);

            foreach (DataRow DR in ds.Tables[0].Rows)
            {
                UrbanCenter uc = new UrbanCenter();
                uc.IdUrbanCenter = Convert.ToInt32(DR["IdUrbanCenter"].ToString());
                uc.Description = DR["Description"].ToString();
                uc.NoDeptos = DR["NoDeptos"].ToString();
                uc.NoEdif = DR["NoEdif"].ToString();
                uc.Ubication = DR["Ubication"].ToString();
                uc.ComercialLocals = DR["ComercialLocals"].ToString();
                uc.Active = Convert.ToBoolean(DR["Active"].ToString());
                uc.Locality = DR["Locality"].ToString();
                listUC.Add(uc);

            }

            return listUC;

        }
        public List<UrbanCenter> SelectUrbanCenter()
        {

            DataSet ds = new DataSet();
            List<UrbanCenter> listUC = new List<UrbanCenter>();

            string query = "exec spS_UrbanCenter";

            ds = con.ObtenerConsulta(query);

            foreach (DataRow DR in ds.Tables[0].Rows)
            {
                UrbanCenter uc = new UrbanCenter();
                uc.IdUrbanCenter = Convert.ToInt32(DR["IdUrbanCenter"].ToString());
                uc.Description = DR["Description"].ToString();
                uc.NoDeptos = DR["NoDeptos"].ToString();
                uc.NoEdif = DR["NoEdif"].ToString();
                uc.Ubication = DR["Ubication"].ToString();
                uc.ComercialLocals = DR["ComercialLocals"].ToString();
                uc.Active = Convert.ToBoolean(DR["Active"].ToString());

                listUC.Add(uc);

            }

            return listUC;

        }


        public DataTable SelectRequirements()
        {

            DataSet ds = new DataSet();

            string query = "exec spS_Requirements";

            ds = con.ObtenerConsulta(query);

            return ds.Tables[0];

        }

        public List<DocumentTypes> selectDocumentTypes()
        {
            List<DocumentTypes> Documentos = new List<DocumentTypes>();



            DataSet ds = new DataSet();
            string query = "exec spS_Requirements";

            ds = con.ObtenerConsulta(query);


            foreach (DataRow DR in ds.Tables[0].Rows)
            {
                DocumentTypes dtype = new DocumentTypes();
                dtype.Name = DR["Name"].ToString();
                dtype.DocumentTypeId = Convert.ToInt32(DR["DocumentTypeId"].ToString());
                dtype.Name = DR["Name"].ToString();
                dtype.Required = Convert.ToBoolean(DR["Required"].ToString());
                Documentos.Add(dtype);
            }








            return Documentos;
        }


    }
}
