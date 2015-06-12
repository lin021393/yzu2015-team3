using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SQLite; 

namespace team3
{
    class CategoryLink
    {
        private long _productid;
        private long _categoryid;
        private bool isDirty = false;

        public CategoryLink(long product, long category)
        {
            this._productid = product;
            this._categoryid = category;
            isDirty = true;
        }

        public CategoryLink(string product, string category)
        {
            this._productid = Product.GetProductByName(product).Id;
            this._categoryid = Category.GetCategoryByName(category).Id;
            isDirty = true;
        }

        public long ProductId
        {
            get { return _productid; }
            internal set { this._productid = value; }
        }

        public long CategoryId
        {
            get { return _categoryid; }
            internal set { this._categoryid = value; }
        }

        public bool IsSaved()
        {
            return this.isDirty == false;
        }

        public bool Save()
        {
            if (IsSaved())
                return true;

            try
            {
                SQLiteConnection con = DatabaseConnection.GetConnection();
                SQLiteCommand cmd = con.CreateCommand();

                if (this.ProductId > 0 && this.CategoryId > 0)
                {
                    cmd.CommandText = @"INSERT INTO [category_link] (
                                [product_id], 
                                [category_id]
                               ) VALUES ( @productid, @categoryid )";
                    
                    cmd.Parameters.Add(new SQLiteParameter("@productid") { Value = this.ProductId, });
                    cmd.Parameters.Add(new SQLiteParameter("@categoryid") { Value = this.CategoryId, });

                    cmd.ExecuteNonQuery();

                    string sql = "SELECT last_insert_rowid()";
                    cmd = new SQLiteCommand(sql, con);

                    DatabaseConnection.RemoveConnection(con);

                    this.isDirty = false;

                    return true;
                }
                else
                {
                    DatabaseConnection.RemoveConnection(con);
                    return false;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public static List<long> GetProductListByCategory(long CategoryId)
        {
            List<long> productlist = new List<long>();
            SQLiteConnection con = DatabaseConnection.GetConnection();
            SQLiteCommand cmd = con.CreateCommand();

            cmd.CommandText = @"SELECT * 
                                FROM [category_link]
                                WHERE [category_id] = @id";
            cmd.Parameters.Add(new SQLiteParameter("@id") { Value = CategoryId, });
            SQLiteDataReader reader = cmd.ExecuteReader();


            if (reader.Read())
            {
                while (reader.Read())
                {
                    productlist.Add((long)reader["product_id"]);
                }

                DatabaseConnection.RemoveConnection(con);
                return productlist;
            }
            else
            {
                DatabaseConnection.RemoveConnection(con);
                return null;
            }

        }

        public static List<long> GetCategoryListByProduct(long ProductId)
        {
            try
            {
                List<long> categorylist = new List<long>();
                SQLiteConnection con = DatabaseConnection.GetConnection();
                SQLiteCommand cmd = con.CreateCommand();

                cmd.CommandText = @"SELECT * 
                                FROM [category_link]
                                WHERE [product_id] = @id";
                cmd.Parameters.Add(new SQLiteParameter("@id") { Value = ProductId, });
                SQLiteDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    while (reader.Read())
                    {
                        categorylist.Add((long)reader["category_id"]);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static bool Remove(long productid, long categoryid)
        {
            return false;
        }

    }
}
