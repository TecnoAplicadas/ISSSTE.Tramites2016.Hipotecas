using ISSSTE.Tramites2016.Common.Model;
using ISSSTE.Tramites2016.Common.Service.Implementation;
using ISSSTE.Tramites2016.Escrituracion.DataAccess;
using ISSSTE.Tramites2016.Escrituracion.Model.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Escrituracion.Bussines
{
    public class CommonBussines
    {
        EntitleDataAccess entDA = new EntitleDataAccess();
        private SipeAvDataService sipeAvServiceAgent = new SipeAvDataService();
        public Entitles GetEntitledByCURP(string curp)
        {
            return entDA.GetEntitleByCurp(curp);

        }

        public void UpdateEntitledFromInformix(Entitles entitledDataBase, EntitleSipeInformation entitledInformix)//, RegimenInformation regimenInformation)
        {

            entitledDataBase.Age = entitledInformix.Age;
            entitledDataBase.Birthdate = entitledInformix.BirthDate.ToString();
            entitledDataBase.Birthplace = entitledInformix.EntityBirth.Trim();
            entitledDataBase.City = entitledInformix.Population.Trim();
            entitledDataBase.Colony = entitledInformix.Colony.Trim();
            entitledDataBase.CURP = entitledInformix.Curp.Trim();
            entitledDataBase.Gender = entitledInformix.Genger.Trim();
            entitledDataBase.MaritalStatus = entitledInformix.MaritalStatus.Trim();
            entitledDataBase.MaternalLastName = entitledInformix.SecondSurname.Trim();
            entitledDataBase.Name = entitledInformix.Name.Trim();
            entitledDataBase.NoISSSTE = entitledInformix.NumIssste.Trim();
            entitledDataBase.NumExt = entitledInformix.ExteriorNumber.Trim();
            entitledDataBase.NumInt = entitledInformix.InteriorNumber.Trim();
            entitledDataBase.PaternalLastName = entitledInformix.FirstSurname.Trim();
           // entitledDataBase.RegimeType = (regimenInformation != null && !string.IsNullOrEmpty(regimenInformation.RegimenKey)) ? regimenInformation.RegimenKey.Trim() : string.Empty;
            entitledDataBase.RFC = entitledInformix.Rfc.Trim();
            entitledDataBase.Street = entitledInformix.Street.Trim();
            entitledDataBase.ZipCode = entitledInformix.PostalCode.Trim();
            // entitledDataBase.IsActive = sipeAvServiceAgent.GetStateEntitle(entitledInformix);

            entDA.saveInformationEntitle(entitledDataBase);

        }

        public void SaveEntitledFromInformix(Entitles entitled)
        {

            entDA.SaveEntitle(entitled);

        }

    }
}
