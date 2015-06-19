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


        public static string DBName
        {
            get { return dbName;  }
        }

        public static void DropTable(String TableName)
        {
            SQLiteConnection con = GetConnection();

            SQLiteCommand cmd = con.CreateCommand();

            cmd.CommandText = @"DROP TABLE IF EXISTS [" + TableName + "];";
            cmd.ExecuteNonQuery();

            RemoveConnection(con);
        }

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
            cmd.ExecuteNonQuery();

            
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS [categories] (
                                    [id] INTEGER PRIMARY KEY ,
                                    [name] TEXT );";
            cmd.ExecuteNonQuery();

            
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS [category_link] (
                                    [product_id] INTEGER ,
                                    [category_id] INTEGER,
                                    FOREIGN KEY (product_id) REFERENCES products(id),
                                    FOREIGN KEY (category_id) REFERENCES categories(id) );";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS [users] (
                                    [id] INTEGER PRIMARY KEY ,
                                    [account] TEXT NOT NULL UNIQUE ,
                                    [password] TEXT ,
                                    [email] TEXT NOT NULL UNIQUE);";

            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS [order] (
                                    [id] INTEGER PRIMARY KEY ,
                                    [total] INTEGER ,
                                    [deliverFee] INTEGER ,
                                    [grandTotal] INTEGER ,
                                    [customerName] TEXT ,
                                    [customerAddress] TEXT,
                                    [customerPhone] TEXT,
                                    [receiveName] TEXT ,
                                    [receiveAddress] TEXT,
                                    [receivePhone] TEXT,
                                    [payType] TEXT, 
                                    [receiveType] TEXT) ;";

            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS [orderDetail] (
                                    [id] INTEGER PRIMARY KEY ,
                                    [orderId] INTEGER ,
                                    [productId] INTEGER ,
                                    [productName] TEXT ,
                                    [unitPrice] INTEGER ,
                                    [quantity] INTEGER ,
                                    [total] INTEGER) ;";//FOREIGN KEY(orderId) REFERENCES order(id)

            cmd.ExecuteNonQuery();


            RemoveConnection(con);

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
