using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SQLite; 

namespace team3
{
    class Category
    {
        private long _id;
        private string _name;
        private bool isDirty = false;

        public Category(string name)
        {
            this.Id = 0;
            this.Name = name;
            isDirty = false;
        }

        private Category(long id, string name)
        {
            this.Id = id;
            this.Name = name;
            isDirty = true;
        }

        public long Id
        {
            get { return _id; }
            internal set { this._id = value; }
        } 

        public string Name
        {
            get { return this._name; }
            set {  this._name = value; this.isDirty = true; } 
        }

        public bool IsSaved()
        {
            return this.Id > 0 && this.isDirty == true;
        }

        public bool Save()
        {
            if (IsSaved())
                return true;
           /* else if (GetCategoryByName(this._name) != null)
            {
                return false;
            }*/

            try
            {

                SQLiteConnection con = DatabaseConnection.GetConnection();
                SQLiteCommand cmd = con.CreateCommand();

                if (this.Id >= 0)
                {
                    cmd.CommandText = @"INSERT INTO [categories] (
                                [name]
                               ) VALUES ( @name )";

                    cmd.Parameters.Add(new SQLiteParameter("@name") { Value = this.Name, });
                    cmd.ExecuteNonQuery();

                    string sql = "SELECT last_insert_rowid()";
                    cmd = new SQLiteCommand(sql, con);

                    this.Id = (long)cmd.ExecuteScalar();
                    DatabaseConnection.RemoveConnection(con);
                    return this.Id > 0;
                }
                else
                {
                    cmd.CommandText = @"UPDATE [categories]
                                        SET [name] = @name
                                        WHERE [id] = @id";

                    cmd.Parameters.Add(new SQLiteParameter("@name") { Value = this.Name, });
                    int res = cmd.ExecuteNonQuery();

                    DatabaseConnection.RemoveConnection(con);
                    return true;

                }
             
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }
        
        public static Category GetCategoryById(long CategoryId) {

            SQLiteConnection con = DatabaseConnection.GetConnection();
            SQLiteCommand cmd = con.CreateCommand();

            cmd.CommandText = @"SELECT * 
                                FROM [categories]
                                WHERE [id] = @id";
            cmd.Parameters.Add(new SQLiteParameter("@id") { Value = CategoryId, });
            SQLiteDataReader reader = cmd.ExecuteReader();
            
            if ( reader.Read() )
            {
                Category category = new Category((long)reader["id"],
                                    reader["name"] as String);

                DatabaseConnection.RemoveConnection(con);
                return category;
            }
            else
            {
                return null;
            }

        }

        
        public static Category GetCategoryByName(string CategoryName)
        {

            SQLiteConnection con = DatabaseConnection.GetConnection();
            SQLiteCommand cmd = con.CreateCommand();

            
            cmd.CommandText = @"SELECT * 
                                FROM [categories]
                                WHERE [name] = @name";
            cmd.Parameters.Add(new SQLiteParameter("@name") { Value = CategoryName, });
            SQLiteDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                Category category = new Category((long)reader["id"],
                                    reader["name"] as String);

                DatabaseConnection.RemoveConnection(con);
                return category;
            }
            else
            {
                return null;
            }

        }


    }
}
