using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SQLite; 

namespace team3
{
    class CategoryResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        
        
    }

    class Category
    {
        private long _id;
        private string _name;
        private bool isDirty = false;

        public Category(string name)
        {
            this.Id = 0;
            this.Name = name;
            isDirty = true;
        }

        private Category(long id, string name)
        {
            this.Id = id;
            this.Name = name;
            isDirty = false;
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
            return this.Id > 0 && this.isDirty == false;
        }

        public CategoryResult Save()
        {
            if (this.Name.Trim() == "")
                return new CategoryResult {Success = false};

            if (IsSaved())
                return new CategoryResult { Success = true };
            else if (GetCategoryByName(this.Name) != null)
                return new CategoryResult { Success = true };

            try
            {

                SQLiteConnection con = DatabaseConnection.GetConnection();
                SQLiteCommand cmd = con.CreateCommand();

                if (this.Id == 0)
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
                    return new CategoryResult { Success = true };
                }
                else
                {
                    cmd.CommandText = @"UPDATE [categories]
                                        SET [name] = @name
                                        WHERE [id] = @id";

                    cmd.Parameters.Add(new SQLiteParameter("@name") { Value = this.Name, });
                    int res = cmd.ExecuteNonQuery();

                    DatabaseConnection.RemoveConnection(con);
                    return new CategoryResult { Success = true };

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new CategoryResult { Success = false };
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
                DatabaseConnection.RemoveConnection(con);
                return null;
            }

        }

        public static List<string> GetCategoryList()
        {
            List<string> categorylist = new List<string>();
            SQLiteConnection con = DatabaseConnection.GetConnection();
            SQLiteCommand cmd = con.CreateCommand();

            cmd.CommandText = @"SELECT * 
                                FROM [categories]";
            
            SQLiteDataReader reader = cmd.ExecuteReader();


            if (reader.Read())
            {
                categorylist.Add((string)reader["name"]);
                while (reader.Read())
                {
                    categorylist.Add((string)reader["name"]);
                }

                DatabaseConnection.RemoveConnection(con);
                return categorylist;
            }
            else
            {
                DatabaseConnection.RemoveConnection(con);
                return null;
            }
        }

        public static bool Remove(string CategoryName)
        {
            if(GetCategoryByName(CategoryName) != null &&
               CategoryLink.GetProductListByCategory(GetCategoryByName(CategoryName).Id) == null)
            {
                SQLiteConnection con = DatabaseConnection.GetConnection();
                SQLiteCommand cmd = con.CreateCommand();


                cmd.CommandText = @"DELETE 
                                    FROM [categories]
                                    WHERE [name] = @name";
                cmd.Parameters.Add(new SQLiteParameter("@name") { Value = CategoryName, });
                SQLiteDataReader reader = cmd.ExecuteReader();

                DatabaseConnection.RemoveConnection(con);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
