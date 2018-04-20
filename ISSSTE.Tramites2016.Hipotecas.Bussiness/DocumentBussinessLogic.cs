using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using ISSSTE.Tramites2016.Hipotecas.DataAccess;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using ISSSTE.Tramites2016.Escrituracion.Models.Model;

namespace ISSSTE.Tramites2016.Hipotecas.Domain.Implementation
{
    public class DocumentBussinessLogic
    {
        private DocumentTypeDataAccess documentTypeData = new DocumentTypeDataAccess();
        private DocumentDataAccess documentsDataAccess = new DocumentDataAccess();
        private RequestBussinessLogic requestBussinessLogic = new RequestBussinessLogic();

        public List<DocumentsAdministrator> GetListDocumentsByRequestId(Guid requestId)
        {
            return documentsDataAccess.GetListDocumentsByRequestId(requestId);
        }

        public Documents GetImage(Guid documentId, int documentTupeId)
        {
            return documentsDataAccess.GetImage(documentId, documentTupeId);
        }

        public List<DocumentType> GeDocuments(string tipoCred)
        {

            return documentTypeData.GetDocuments(tipoCred);
        }
        public List<DocumentType> GeDocuments()
        {

            return documentTypeData.GetDocuments();
        }

        //public void SaveDocument(HttpPostedFileBase file,byte[] archivo, string numeroIssste, int documentType,string requestId)
        //{


        //        string extension = Path.GetExtension(file.FileName);//obtiene la extencion del archivo 
        //        documentsDataAccess.SaveDocument(archivo, extension, requestId, documentType);


        //}

        public void SaveDocument(byte[] file, string extension, string numeroIssste, int documentType, string requestId)
        {

            //byte[] archivo = ConvertToBytes(file);
            //if (archivo.Length <= 5242880)//menor a 5mb
            //{
            // string extension = Path.GetExtension(file.FileName);//obtiene la extencion del archivo 
            documentsDataAccess.SaveDocument(file, extension, requestId, documentType);
            //  }

        }
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }



        public List<Seguimiento> GetResult(Guid RequestId)
        {
            return documentsDataAccess.GetResult(RequestId);
        }
    }
}
