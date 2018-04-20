using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Common.ServiceAgents.Implementation;
using ISSSTE.Tramites2016.Common.Model;
using ISSSTE.Tramites2016.Hipotecas.Bussiness;
using ISSSTE.Tramites2016.Hipotecas.DataAccess;
using ISSSTE.Tramites2016.Hipotecas.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;


namespace ISSSTE.Tramites2016.Hipotecas.Domain.Implementation
{
    public class EntitleDomain
    {
        private SipeAvDataServiceAgent sipeAvServiceAgent = new SipeAvDataServiceAgent();
        private CommonDomainService commonDomainService = new CommonDomainService();
        private EntitleDataAccess entitleDataAccess = new EntitleDataAccess();

        
        public Entitle GetEntitle(string noIssste)
        {
            Entitle entitledInfo = null;
            EntitleSipeInformation entitledInformix =  sipeAvServiceAgent.GetEntitleByNoIssste(noIssste);
            RegimenInformation entitledRegimen = sipeAvServiceAgent.GetRegimenByNoIssste(noIssste);
            bool isActive = sipeAvServiceAgent.GetStateEntitle(entitledInformix);
            if (entitledInformix != null)
            {
                entitledInfo = new Entitle()
                {
                    Age = entitledInformix.Age,
                    Birthdate = entitledInformix.BirthDate,
                    Birthplace = entitledInformix.EntityBirth.Trim(),
                    City = entitledInformix.Population.Trim(),
                    Colony = entitledInformix.Colony.Trim(),
                    CURP = entitledInformix.Curp.Trim(),
                    Gender = entitledInformix.Genger.Trim(),
                    MaritalStatus = entitledInformix.MaritalStatus.Trim().ToUpper(),
                    MaternalLastName = entitledInformix.SecondSurname.Trim(),
                    Name = entitledInformix.Name.Trim(),
                    NoISSSTE = entitledInformix.NumIssste.Trim(),
                    NumExt = entitledInformix.ExteriorNumber.Trim(),
                    NumInt = entitledInformix.InteriorNumber.Trim(),
                    PaternalLastName = entitledInformix.FirstSurname.Trim(),
                    RegimeType = (entitledRegimen != null && !string.IsNullOrEmpty(entitledRegimen.RegimenKey)) ? entitledRegimen.RegimenKey.Trim() : string.Empty,
                    RFC = entitledInformix.Rfc.Trim(),
                    Street = entitledInformix.Street.Trim(),
                    ZipCode = entitledInformix.PostalCode.Trim(),
                    EntitleId = entitledInformix.Curp.Trim(),
                    IsActive = isActive,
                    State = entitledInformix.State
                };


                Entitle existingEntitled = commonDomainService.GetEntitledByCURP(entitledInformix.Curp);
                if (existingEntitled != null && !string.IsNullOrEmpty(existingEntitled.CURP))
                {
                    commonDomainService.UpdateEntitledFromInformix(existingEntitled, entitledInformix, entitledRegimen);
                   
                }
                else
                {
                    commonDomainService.SaveEntitledFromInformix(entitledInfo);
                }

                entitledInfo = commonDomainService.GetEntitledByCURP(entitledInformix.Curp);
            }

            if(entitledInformix.State == "")
            {
                entitledInfo.State = "BAJA";
            }else if(entitledInformix.State == "F")
            {
                entitledInfo.State = "FINADO";
            }else
            {
                entitledInfo.State = "ACTIVO";
            }

            return entitledInfo;
        }

        public bool GetStatusEntitle(string noIssste)
        {
            Entitle entitledInfo = null;
            EntitleSipeInformation entitledInformix = sipeAvServiceAgent.GetEntitleByNoIssste(noIssste);
            bool isActive = sipeAvServiceAgent.GetStateEntitle(entitledInformix);
            return isActive;
        }

        public string saveInformationEntitle(string noIssste, string lada, string telefono, string email, string mobile)
        {
            string rpta = "";

            try
            {
                entitleDataAccess.ActualizarEntitle(noIssste, lada, telefono, email, mobile);
                rpta = "OK";
            }
            catch (Exception ex)
            {
                rpta = ex.Message;
            }
            return rpta;
        }

    }
}
