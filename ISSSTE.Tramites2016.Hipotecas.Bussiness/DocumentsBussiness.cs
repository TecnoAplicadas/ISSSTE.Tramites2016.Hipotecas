using ISSSTE.Tramites2016.Hipotecas.DataAccess;
using ISSSTE.Tramites2016.Hipotecas.Model.Modelo;
using System;
using System.IO;
using System.Web;

namespace ISSSTE.Tramites2016.Escrituracion.Bussines
{
    public class DocumentsBussiness
    {

        DocumentsDataAccess docs = new DocumentsDataAccess();
        public void CargarDocumento(RequestDocuments RD, String Archivo)
        {

            docs.uploadFile(RD.Data, RD.FileExtension, RD.RequestId, (int)RD.DocumentTypeId);
        }

        public void SaveDocument(HttpPostedFileBase file, string requestId, int documentType)
        {

            byte[] archivo = ConvertToBytes(file);
            if (archivo.Length <= 5242880)//menor a 5mb
            {
                if (file.FileName != string.Empty)
                {
                    string extension = Path.GetExtension(file.FileName);//obtiene la extencion del archivo 
                    docs.uploadFile(archivo, extension, requestId, documentType);
                }
            }
        }

        private byte[] ConvertToBytes(HttpPostedFileBase file)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(file.InputStream);
            imageBytes = reader.ReadBytes((int)file.ContentLength);
            return imageBytes;
        }
    }
}
