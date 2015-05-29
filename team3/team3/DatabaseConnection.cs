using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration.Assemblies;


namespace team3
{
    class DatabaseConnection
    {
        private static List<MySqlConnection> connections = new List<MySqlConnection>();

        public static MySqlConnection GetConnection()
        {

        }
    }
}
