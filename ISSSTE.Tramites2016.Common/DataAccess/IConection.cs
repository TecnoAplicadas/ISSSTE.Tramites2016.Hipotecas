using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Common.DataAccess
{
    public interface IConection
    {
        SqlConnection obtenConexion();
        DataSet obtenConsulta(string Consulta);
    }
}
