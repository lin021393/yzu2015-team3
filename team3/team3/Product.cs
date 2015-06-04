using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SQLite; 
namespace team3
{
    
    public class Product
    {
        private long _id = 0;
        private string _name;
        private long _price;
        private string _img_Url;
        private string _description;
        private long _remain;
        private List<Category> _catogorys = new List<Category>();
        private bool isDirty = false;

        public Product(string name, long price, string imgUrl, string description, long remain)
        {
            this.Id = 0;
            this.Name = name;
            this.Price = price;
            this.ImageUrl = imgUrl;
            this.Description = description;
            this.Remain = remain;
            isDirty = false;
        }

        private Product(long id, string name, long price, string imgUrl, string description, long remain)
        {
            this.Id = id;
            this.Name = name;
            this.Price = price;
            this.ImageUrl = imgUrl;
            this.Description = description;
            this.Remain = remain;
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

        public long Price
        {
            get { return this._price; }
            set { this._price = value; this.isDirty = true; }
        }

        public string ImageUrl 
        {
            get { return this._img_Url; }
            set { this._img_Url = value; this.isDirty = true; } 
        }

        public string Description
        {
            get { return this._description; }
            set { this._description = value; this.isDirty = true;  } 
        }

        public long Remain
        {
            get { return this._remain; }
            set { this._remain = value; this.isDirty = true; } 
        }


        public bool IsSaved()
        {
            return this.Id > 0 && this.isDirty == true;
        }

        public bool Save()
        {
            if (IsSaved())
                return true;

            try
            {

                SQLiteConnection con = DatabaseConnection.GetConnection();
                SQLiteCommand cmd = con.CreateCommand();

                if (this.Id >= 0)
                {
                    cmd.CommandText = @"INSERT INTO [products] (
                                [name], 
                                [price],
                                [imgUrl], 
                                [description],
                                [remain]
                               ) VALUES ( @name, @price, @imgUrl, @description, @remain )";

                    cmd.Parameters.Add(new SQLiteParameter("@name") { Value = this.Name, });
                    cmd.Parameters.Add(new SQLiteParameter("@price") { Value = this.Price, });
                    cmd.Parameters.Add(new SQLiteParameter("@imgUrl") { Value = this.ImageUrl, });
                    cmd.Parameters.Add(new SQLiteParameter("@description") { Value = this.Description, });
                    cmd.Parameters.Add(new SQLiteParameter("@remain") { Value = this.Remain, });
                    cmd.ExecuteNonQuery();

                    string sql = "SELECT last_insert_rowid()";
                    cmd = new SQLiteCommand(sql, con);

                    this.Id = (long)cmd.ExecuteScalar();
                    DatabaseConnection.RemoveConnection(con);
                    return this.Id > 0;
                }
                else
                {
                    cmd.CommandText = @"UPDATE [products]
                                        SET [name] = @name ,
                                            [price] = @price ,
                                            [imgUrl] = @imgUrl ,
                                            [description] = @description ,
                                            [remain] = @remain 
                                        WHERE [id] = @id";

                    cmd.Parameters.Add(new SQLiteParameter("@name") { Value = this.Name, });
                    cmd.Parameters.Add(new SQLiteParameter("@price") { Value = this.Price, });
                    cmd.Parameters.Add(new SQLiteParameter("@imgUrl") { Value = this.ImageUrl, });
                    cmd.Parameters.Add(new SQLiteParameter("@description") { Value = this.Description, });
                    cmd.Parameters.Add(new SQLiteParameter("@remain") { Value = this.Remain, });
                    cmd.Parameters.Add(new SQLiteParameter("@id") { Value = this.Remain, });
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
        
        public static Product GetProductById(long ProductId) {

            SQLiteConnection con = DatabaseConnection.GetConnection();
            SQLiteCommand cmd = con.CreateCommand();

            cmd.CommandText = @"SELECT * 
                                FROM [products]
                                WHERE [id] = @id";
            cmd.Parameters.Add(new SQLiteParameter("@id") { Value = ProductId, });
            SQLiteDataReader reader = cmd.ExecuteReader();
            
            if ( reader.Read() )
            {
                return new Product((long)reader["id"], 
                                    reader["name"] as String,
                                    (long)reader["price"],
                                    reader["imgUrl"] as String,
                                    reader["description"] as String,
                                    (long)reader["remain"]);
            }
            else
            {
                return null;
            }
            

        }


        
  
    }
}
