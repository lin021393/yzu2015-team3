using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration.Assemblies;
using System.Data.SQLite;


namespace team3
{
    class DatabaseConnection
    {

        private const string dbName = "database.db";

        private static List<SQLiteConnection> connections = new List<SQLiteConnection>();


        public static void Init()
        {
            SQLiteConnection con = GetConnection();

            SQLiteCommand cmd = con.CreateCommand();

            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS [products] (
                                    [id] INTEGER PRIMARY KEY ,
                                    [name] TEXT ,
                                    [price] INTEGER ,
                                    [imgUrl] TEXT ,
                                    [description] TEXT ,
                                    [remain] INTEGER );";

            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS [categories] (
                                    [id] INTEGER PRIMARY KEY ,
                                    [name] TEXT );";
            
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS [category_link] (
                                    [product_id] INTEGER ,
                                    [category_name] INTEGER );";

            cmd.ExecuteNonQuery();
            
        }

        public static SQLiteConnection GetConnection()
        {
            SQLiteConnection connection = new SQLiteConnection("Data source=" + dbName);

            connection.Open();
            connections.Add(connection);
            return connection;
        }

        public static Boolean RemoveConnection(SQLiteConnection connection) 
        {
            connection.Close();
            return connections.Remove(connection);
        }
        
    }
}
