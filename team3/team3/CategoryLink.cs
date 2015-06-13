using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SQLite; 

namespace team3
{
    class CategoryLinkResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    class CategoryLink
    {
        private long _productid;
        private long _categoryid;
        private bool isDirty = false;

        public CategoryLink(long product, long category)
        {
            Product product_temp = Product.GetProductById(product);
            Category category_temp = Category.GetCategoryById(category);

            if (product_temp == null)
                this._productid = 0;
            else
                this._productid = product_temp.Id;

            if (category_temp == null)
                this._categoryid = 0;
            else
                this._categoryid = category_temp.Id;

            isDirty = true;
        }

        public CategoryLink(string product, string category)
        {
            Product product_temp = Product.GetProductByName(product);
            Category category_temp = Category.GetCategoryByName(category);

            if (product_temp == null)
                this._productid = 0;
            else
                this._productid = product_temp.Id;

            if (category_temp == null)
                this._categoryid = 0;
            else
                this._categoryid = category_temp.Id;
            
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

        public CategoryLinkResult Save()
        {
            if ( this._productid == 0)
                return new CategoryLinkResult { Success = false, Message = "商品不存在" };
            else if (this._categoryid == 0 )
                return new CategoryLinkResult { Success = false, Message = "分類不存在" };
            else if (IsSaved())
                return new CategoryLinkResult { Success = true, Message = "分類已儲存" };
            else
            {
                if (GetProductListByCategory(this.CategoryId) != null)
                {
                    if (GetProductListByCategory(this.CategoryId).Contains(this.ProductId))
                    {
                        this.isDirty = false;
                        return new CategoryLinkResult { Success = true, Message = "分類已儲存" };
                    }
                }
            }

            try
            {
                SQLiteConnection con = DatabaseConnection.GetConnection();
                SQLiteCommand cmd = con.CreateCommand();


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

                return new CategoryLinkResult { Success = true, Message = "分類儲存成功" };

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new CategoryLinkResult { Success = false, Message = "資料庫存取失敗" };
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
                productlist.Add((long)reader["product_id"]);
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
                    categorylist.Add((long)reader["category_id"]);
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
            try
            {
                SQLiteConnection con = DatabaseConnection.GetConnection();
                SQLiteCommand cmd = con.CreateCommand();
                cmd.CommandText = @"DELETE 
                                    FROM [category_link]
                                    WHERE [product_id] = @product
                                    AND [category_id] = @category";

                cmd.Parameters.Add(new SQLiteParameter("@product") { Value = productid, });
                cmd.Parameters.Add(new SQLiteParameter("@category") { Value = categoryid, });
                SQLiteDataReader reader = cmd.ExecuteReader();
                DatabaseConnection.RemoveConnection(con);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}
