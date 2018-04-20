using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Common.Mail;
using ISSSTE.Tramites2016.Common.Util;
using ISSSTE.Tramites2016.Escrituracion.DataAccess;
using ISSSTE.Tramites2015.Escrituracion.Domian.Implementation;

namespace ISSSTE.Tramites2016.Escrituracion.Task
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ejecutando proceso de Escrituracion");
            ExecuteEscrituracion();
            Console.WriteLine();           
            Console.WriteLine("Procesos ejecutados satisfactoriamente");
        }

        public static void ExecuteEscrituracion()
        {
            IUnitOfWork unitOfWork = new EscrituracionContext();
            IMailService mailService = new MailService(); 
            ILogger logger = new Logger();
            RequestDomainService service = new RequestDomainService(unitOfWork, mailService, logger); 
            service.BatchService();
        }
    }
}
