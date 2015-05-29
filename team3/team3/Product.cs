using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

using System.Data.Common; 
namespace team3
{
    
    public class Product
    {
        public int productid { get; private set; }
        public int categoryid { get; private set; }
        public string productname { get; private set; }
        public int productprice { get; private set; }
        public string productimages { get; private set; }
        public string description { get; private set; }
        public int productremain { get; private set; }

        
        public List<string> ShowProduct(int productID)
        {
            MySqlConnection conn = DatabaseConnection.GetConnection();
            MySqlCommand command = conn.CreateCommand();
            conn.Open(); 
          
          
            ////計算總資料數
            //command.CommandText = "SELECT  count(*) FROM product";
            //int count = Convert.ToInt32(command.ExecuteScalar());

            //拿最後一筆資料(因為是auto_increment所以ID有可能是空的)
           if(productID==0)
                command.CommandText = "SELECT  * FROM product ORDER by productid desc LIMIT  1 " ;
            //根據productid拿資料
            else
               command.CommandText = "SELECT  * FROM product WHERE productid = " + productID.ToString();
            MySqlDataReader data = command.ExecuteReader();


            List<string> NameList = new List<string>();
            if (data.HasRows) 
            {
                while (data.Read())
                {
                    NameList.Add(Convert.ToString(data["productid"]));
                    NameList.Add(Convert.ToString(data["categoryid"]));
                    NameList.Add(Convert.ToString(data["productname"]));
                    NameList.Add(Convert.ToString(data["productprice"]));
                    NameList.Add(Convert.ToString(data["productimages"]));
                    NameList.Add(Convert.ToString(data["description"]));
                    NameList.Add(Convert.ToString(data["productremain"]));
                }
            }
            data.Close();
            conn.Close();
            return NameList;
        }
        public void AddProduct(int productID, int categoryid, string productname, int productprice, string productimages, string describes, int productremain)
        {
            string dbHost = "127.0.0.1";//資料庫位址
            string dbUser = "root";//資料庫使用者帳號
            string dbPass = "AgileTeam3";//資料庫使用者密碼
            string dbName = "Team3";//資料庫名稱

            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();
            conn.Open();

          
            command.CommandText = "Insert into product(productid,categoryid,productname,productprice,productimages,description,productremain) values(" + productid + "," + categoryid + ",'" + productname + "'," + productprice + ",'" + productimages + "','" + describes + "'," + productremain + ")";
            command.ExecuteNonQuery();

            DatabaseConnection.RemoveConnection(conn);
        }
    }
    public class Category
    {
        public int categoryid { get; private set; }
        public string categoryname { get; private set; }
        public string categoryproduct { get; private set; }

        public void AddCategory(int categoryId, string categoryName)
        {
            string categoryProduct = "0";

            MySqlConnection conn = DatabaseConnection.GetConnection();
            MySqlCommand command = conn.CreateCommand();
            conn.Open();

            command.CommandText = "Insert into category(categoryid,categoryname,categoryproduct) values(" + categoryId + ",'" + categoryName + "','" + categoryProduct + "')";
            command.ExecuteNonQuery();

            DatabaseConnection.RemoveConnection(conn);
        }

        public void AddProduct(int CategoryId, int ProductId)
        {
            List<int> ProductList = new List<int>();

            string dbHost = "127.0.0.1";//資料庫位址
            string dbUser = "root";//資料庫使用者帳號
            string dbPass = "AgileTeam3";//資料庫使用者密碼
            string dbName = "Team3";//資料庫名稱

            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();
            conn.Open();

            List<int> categoryProduct = ShowProductList(CategoryId);

            if(categoryProduct.Contains(ProductId) == false)
            {
                categoryProduct.Add(ProductId);
            }

            categoryProduct.Sort();

            string NewCategoryProduct = null;

            for (int i = 0; i < categoryProduct.Count(); i++ )
            {
                if (i != 0)
                    NewCategoryProduct += ",";
                NewCategoryProduct += categoryProduct[i].ToString();
            }
            

            command.CommandText = "Update category SET categoryproduct='" + NewCategoryProduct + "' WHERE categoryid = " + CategoryId.ToString();
            command.ExecuteNonQuery();

            conn.Close();
            
        }

        public void DeleteCategory(int CategoryId)
        {
            MySqlConnection conn = DatabaseConnection.GetConnection();
            MySqlCommand command = conn.CreateCommand();
            conn.Open();

            List<int> categoryProduct = ShowProductList(CategoryId);

            if (categoryProduct.Count() == 1)
            {
                command.CommandText = "Delete FROM category WHERE CategoryId='" + CategoryId.ToString() + "'";
            }


            DatabaseConnection.RemoveConnection(conn);
        }

        public void DeleteProduct(int CategoryId, int ProductId)
        {
            MySqlConnection conn = DatabaseConnection.GetConnection();
            MySqlCommand command = conn.CreateCommand();
            conn.Open();

            List<int> categoryProduct = ShowProductList(CategoryId);

            if(categoryProduct.Contains(ProductId) == false)
            {
            }
            else
            {
                categoryProduct.Remove(ProductId);
            }

            string NewCategoryProduct = null;

            for (int i = 0; i < categoryProduct.Count(); i++ )
            {
                if (i != 0)
                    NewCategoryProduct += ",";
                NewCategoryProduct += categoryProduct[i].ToString();
            }
            

            command.CommandText = "Update category SET categoryproduct='" + NewCategoryProduct + "' WHERE categoryid = " + CategoryId.ToString();
            command.ExecuteNonQuery();

            DatabaseConnection.RemoveConnection(conn);
        }

        public List<int> ShowProductList(int CategoryId)
        {
            List<int> ProductList = new List<int>();

            MySqlConnection conn = DatabaseConnection.GetConnection();
            MySqlCommand command = conn.CreateCommand();
            conn.Open();

            command.CommandText = "SELECT  * FROM category WHERE categoryid = " + CategoryId.ToString();
            MySqlDataReader data = command.ExecuteReader();

            string products = "";

            if (data.HasRows)
            {
                while (data.Read())
                {
                    if (!data.IsDBNull(data.GetOrdinal("categoryproduct")))
                        products = data.GetString(2);
                    else
                        products = null;
                }
            }
            data.Close();

            DatabaseConnection.RemoveConnection(conn);

             string[] sArray1 = products.Split(new char[1] { ',' });

            if (sArray1[0] != null)
            {
                for (int i = 0; i < sArray1.Count(); i++)
                {
                    ProductList.Add(Convert.ToInt32(sArray1[i].ToString()));
                }
            }
        

            return ProductList;
        }
    }
}
