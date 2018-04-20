using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Hipotecas.Model.Modelo
{
    public class Request_Property_Entitles
    {
        public String RequestId { get; set; }
        public String EntitleId { get; set; }
        public String Folio { get; set; }
        public int IsComplete { get; set; }
        public int NeedsAuthorization { get; set; }
        public int IsAuthorized { get; set; }
        public String AuthorizeUserId { get; set; }
        public String AuthorizedComments { get; set; }
        public DateTime Date { get; set; }
        public int StatusNextId { get; set; }
        public int IdLegalUnit { get; set; }
        public int IdProperty { get; set; }
        public int IdTypeOwner { get; set; }


        public int IdPropertyType { get; set; }
        public int IdUrbanCenter { get; set; }
        public string ZipCode { get; set; }
        public string Delegation { get; set; }
        public String Colony { get; set; }
        public String Street { get; set; }
        public String City { get; set; }
        public String NumExt { get; set; }
        public String NumInt { get; set; }
        public String CompleteName { get; set; }
        public String Estatus { get; set; }
        public String PaternalLastName { get; set; }
        public String MaternalLastName { get; set; }
        public String Name { get; set; }
        public String Birthdate { get; set; }
        public String Gender { get; set; }
        public String MaritalStatus { get; set; }
        public String Birthplace { get; set; }

        public string Square { get; set; }
    }
}
