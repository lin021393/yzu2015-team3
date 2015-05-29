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
            String connectionStr =  String.Format("server={0};uid={1};pwd={2};database={3}",
                                        DBSettings.Default.dbHost, 
                                        DBSettings.Default.dbUser,
                                        DBSettings.Default.dbPassword,
                                        DBSettings.Default.dbName);

            MySqlConnection connection = new MySqlConnection(connectionStr);

            connections.Add(connection);

            return connection;
        }

        public static Boolean RemoveConnection(MySqlConnection connection) 
        {
            connection.Close();
            return connections.Remove(connection);
        }
        
    }
}
