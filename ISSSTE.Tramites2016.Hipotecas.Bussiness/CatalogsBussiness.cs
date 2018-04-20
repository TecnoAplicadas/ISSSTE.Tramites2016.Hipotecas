using ISSSTE.Tramites2016.Hipotecas.DataAccess;
using ISSSTE.Tramites2016.Hipotecas.Model.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;

namespace ISSSTE.Tramites2016.Hipotecas.Bussines
{
    public class CatalogsBussiness
    {
        CatalogsDataAccess catDA = new CatalogsDataAccess();
        public List<TypeOwner> GetTypeOwner()
        {
            List<TypeOwner> dt = catDA.SelectTypeOwner();
            return dt;
        }

        public  List<PropertType> GetPropertType()
        {
            List<PropertType> dt = catDA.SelectPropertType();
            return dt;
        }
        public List<UrbanCenter> GetUrbanCenters()
        {
            List<UrbanCenter> dt = catDA.SelectUrbanCenter();
            return dt;
        }

        public List<UrbanCenter> GetUrbanCenterByID(string idUrbanCenter)
        {
            List<UrbanCenter> dt = catDA.SelectUrbanCenterByID(idUrbanCenter);
            return dt;
        }
    

        public List<DocumentTypes> showDocumentTypes()
        {
            List<DocumentTypes> dt = new List<DocumentTypes>();
            dt = catDA.selectDocumentTypes();
            return dt;
        }

    }

}