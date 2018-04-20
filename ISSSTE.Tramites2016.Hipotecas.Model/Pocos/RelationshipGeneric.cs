using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Hipotecas.Model.Pocos
{
    public class RelationshipGeneric
    {
        /// <summary>
        ///     Descripción de la solicitud
        /// </summary>
        public int RelationshipDocumentsId { get; set; }
        public int PensionId { get; set; }
        public int RelationshipId { get; set; }
        public int RelationshipTitleId { get; set; }
        public int DocumentsId { get; set; }
        public Guid DocumentId { get; set; }
        public string TypeDocument { get; set; }
        public string Description { get; set; }
        public string NamePensions { get; set; }
        public string NameRelationships { get; set; }
        public string NameRelationshipTitles { get; set; }
        public string type { get; set; }


    }
}