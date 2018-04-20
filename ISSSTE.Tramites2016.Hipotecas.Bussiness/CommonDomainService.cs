using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Common.Model;
using ISSSTE.Tramites2016.Common.ServiceAgents.Implementation;
using ISSSTE.Tramites2016.Hipotecas.Model;
using ISSSTE.Tramites2016.Hipotecas.DataAccess;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;


namespace ISSSTE.Tramites2016.Hipotecas.Bussiness
{
    public class CommonDomainService
    {
        BasesDatos db = new BasesDatos();
        private SipeAvDataServiceAgent sipeAvServiceAgent = new SipeAvDataServiceAgent();
        public Entitle GetEntitledByCURP(string curp)
        {
            return db.GetEntitleByCurp(curp);

        }

        public void UpdateEntitledFromInformix(Entitle entitledDataBase, EntitleSipeInformation entitledInformix, RegimenInformation regimenInformation)
        {

            entitledDataBase.Age = entitledInformix.Age;
            entitledDataBase.Birthdate = entitledInformix.BirthDate;
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
            entitledDataBase.RegimeType = (regimenInformation != null && !string.IsNullOrEmpty(regimenInformation.RegimenKey)) ? regimenInformation.RegimenKey.Trim() : string.Empty;
            entitledDataBase.RFC = entitledInformix.Rfc.Trim();
            entitledDataBase.Street = entitledInformix.Street.Trim();
            entitledDataBase.ZipCode = entitledInformix.PostalCode.Trim();
            entitledDataBase.IsActive = sipeAvServiceAgent.GetStateEntitle(entitledInformix);

            db.UpdateEntitle(entitledDataBase);
            
        }

        public void SaveEntitledFromInformix(Entitle entitled)
        {

            db.SaveEntitle(entitled);

        }

    }
}
