using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite; 

namespace team3
{
    class CartResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
    class ShoppingCart
    {
        private long _id;
        private long _orderid;
        private long _productid;
        private string _productname = "";
        private long _unitprice ;
        private long _quantity;
        private long _total;
        private bool isDirty = false;
        private bool isEmpty = false;
        private static int count = 0;
        private static List<ShoppingCart> listCart = new List<ShoppingCart>();

        public ShoppingCart(long orderid, long productid, string productname, long unitprice, long quantity, long total = 0)
        {
            this.Id = 0;
            this.Orderid = orderid;
            this.Productid = productid;
            this.Productname = productname;
            this.Unitprice = unitprice;
            this.Quantity = quantity;
            this.Total = unitprice * quantity;
            isDirty = false;
            isEmpty = false;
        }

        private ShoppingCart(long id, long orderid, long productid, string productname, long unitprice, long quantity, long total=0)
        {
            this.Id = id;
            this.Orderid = orderid;
            this.Productid = productid;
            this.Productname = productname;
            this.Unitprice = unitprice;
            this.Quantity = quantity;
            this.Total = unitprice * quantity;
            isDirty = false;
            isEmpty = false;
        }

        public long Id
        {
            get { return _id; }
            internal set { this._id = value; }
        }

        public long Orderid
        {
            get { return this._orderid; }
            set { this._orderid = value; this.isDirty = true; }
        }

        public long Productid
        {
            get { return this._productid; }
            set { this._productid = value; this.isDirty = true; }
        }

        public string Productname
        {
            get { return this._productname; }
            set { this._productname = value; this.isDirty = true; }
        }

        public long Unitprice
        {
            get { return this._unitprice; }
            set { this._unitprice = value; this.isDirty = true; }
        }

        public long Quantity
        {
            get { return this._quantity; }
            set { this._quantity = value; this.isDirty = true; }
        }

        public long Total
        {
            get { return this._total; }
            set { this._total = value; this.isDirty = true; }
        }

        public void empty_cart()
        {
            this.Id = 0;
            this.Orderid = 0;
            this.Productid = 0;
            this.Productname = "";
            this.Unitprice = 0;
            this.Quantity = 0;
            this.Total = 0;
            this.isEmpty = true;
            listCart = new List<ShoppingCart>();
            count = 0;
        }

        public bool IsSaved()
        {
            return this.Id > 0 && this.isDirty == false;
        }

        public CartResult IsLimitProduct(long productremain)
        {
            if (productremain < this.Quantity)
                return new CartResult { Success = false, Message = "購買商品數量超出庫存" };
            return new CartResult { Success = true, Message = "庫存足夠" }; ;
        }

        public void AddToCart()
        {
            ShoppingCart temp = new ShoppingCart(this.Orderid, this.Productid, this.Productname, this.Unitprice, this.Quantity, this.Total);
            count++;
            listCart.Add(temp);
        }
        
        public static List<ShoppingCart> GetCartInfo()
        {
            return listCart;
        }

        public CartResult SaveDirectly()
        {
            if (IsSaved())
                return new CartResult { Success = true, Message = "單品結帳成功" };
            try
            {

                SQLiteConnection con = DatabaseConnection.GetConnection();
                SQLiteCommand cmd = con.CreateCommand();

                if (this.Id == 0)
                {
                    cmd.CommandText = @"INSERT INTO [orderDetail] (
                                [orderId], 
                                [productId],
                                [productName], 
                                [unitPrice],
                                [quantity],
                                [total]
                               ) VALUES ( @orderId, @productId, @productName, @unitPrice, @quantity, @total )";

                    cmd.Parameters.Add(new SQLiteParameter("@orderId") { Value = this.Orderid, });
                    cmd.Parameters.Add(new SQLiteParameter("@productId") { Value = this.Productid, });
                    cmd.Parameters.Add(new SQLiteParameter("@productName") { Value = this.Productname, });
                    cmd.Parameters.Add(new SQLiteParameter("@unitPrice") { Value = this.Unitprice, });
                    cmd.Parameters.Add(new SQLiteParameter("@quantity") { Value = this.Quantity, });
                    cmd.Parameters.Add(new SQLiteParameter("@total") { Value = this.Total, });
                    cmd.ExecuteNonQuery();

                    string sql = "SELECT last_insert_rowid()";
                    cmd = new SQLiteCommand(sql, con);

                    this.Id = (long)cmd.ExecuteScalar();
                    DatabaseConnection.RemoveConnection(con);
                    this.isDirty = false;

                    if (this.Id > 0)
                        return new CartResult { Success = true, Message = "單品結帳成功" };
                    else
                        return new CartResult { Success = false, Message = "單品結帳失敗" };
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

                    cmd.Parameters.Add(new SQLiteParameter("@orderId") { Value = this.Orderid, });
                    cmd.Parameters.Add(new SQLiteParameter("@productId") { Value = this.Productid, });
                    cmd.Parameters.Add(new SQLiteParameter("@productName") { Value = this.Productname, });
                    cmd.Parameters.Add(new SQLiteParameter("@unitPrice") { Value = this.Unitprice, });
                    cmd.Parameters.Add(new SQLiteParameter("@quantity") { Value = this.Quantity, });
                    cmd.Parameters.Add(new SQLiteParameter("@total") { Value = this.Total, });
                    cmd.Parameters.Add(new SQLiteParameter("@id") { Value = this.Id, });
                    int res = cmd.ExecuteNonQuery();
                    this.isDirty = false;
                    DatabaseConnection.RemoveConnection(con);
                    return new CartResult { Success = true, Message = "單品結帳成功" };

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new CartResult { Success = false, Message = "資料庫存取失敗" };
            }
        }

        public CartResult SaveFromCart()
        {
            if (isEmpty)
                return new CartResult { Success = false, Message = "購物車內無商品" };
            try
            {

                SQLiteConnection con = DatabaseConnection.GetConnection();
                SQLiteCommand cmd = con.CreateCommand();

                for (int i = 0; i < count; i++)
                {
                    if (listCart[i].Id == 0)
                    {
                        cmd.CommandText = @"INSERT INTO [orderDetail] (
                                [orderId], 
                                [productId],
                                [productName], 
                                [unitPrice],
                                [quantity],
                                [total]
                               ) VALUES ( @orderId, @productId, @productName, @unitPrice, @quantity, @total )";

                        cmd.Parameters.Add(new SQLiteParameter("@orderId") { Value = listCart[i].Orderid, });
                        cmd.Parameters.Add(new SQLiteParameter("@productId") { Value = listCart[i].Productid, });
                        cmd.Parameters.Add(new SQLiteParameter("@productName") { Value = listCart[i].Productname, });
                        cmd.Parameters.Add(new SQLiteParameter("@unitPrice") { Value = listCart[i].Unitprice, });
                        cmd.Parameters.Add(new SQLiteParameter("@quantity") { Value = listCart[i].Quantity, });
                        cmd.Parameters.Add(new SQLiteParameter("@total") { Value = listCart[i].Total, });
                        cmd.ExecuteNonQuery();

                        string sql = "SELECT last_insert_rowid()";
                        cmd = new SQLiteCommand(sql, con);

                        listCart[i].Id = (long)cmd.ExecuteScalar();
                        listCart[i].isDirty = false;

                        if (listCart[i].Id <= 0)
                            return new CartResult { Success = false, Message = "購物車結帳失敗" };
                    }
                    else
                    {
                        cmd.CommandText = @"UPDATE [orderDetail]
                                        SET [orderId] = @orderId ,
                                            [productId] = @productId ,
                                            [productName] = @productName ,
                                            [unitPrice] = @unitPrice ,
                                            [quantity] = @quantity ,
                                            [total] = @total 
                                        WHERE [id] = @id";

                        cmd.Parameters.Add(new SQLiteParameter("@orderId") { Value = listCart[i].Orderid, });
                        cmd.Parameters.Add(new SQLiteParameter("@productId") { Value = listCart[i].Productid, });
                        cmd.Parameters.Add(new SQLiteParameter("@productName") { Value = listCart[i].Productname, });
                        cmd.Parameters.Add(new SQLiteParameter("@unitPrice") { Value = listCart[i].Unitprice, });
                        cmd.Parameters.Add(new SQLiteParameter("@quantity") { Value = listCart[i].Quantity, });
                        cmd.Parameters.Add(new SQLiteParameter("@total") { Value = listCart[i].Total, });
                        cmd.Parameters.Add(new SQLiteParameter("@id") { Value = listCart[i].Id, });
                        int res = cmd.ExecuteNonQuery();
                        listCart[i].isDirty = false;

                    }
                }

                DatabaseConnection.RemoveConnection(con);
                return new CartResult { Success = true, Message = "購物車結帳成功" };

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new CartResult { Success = false, Message = "資料庫存取失敗" };
            }
        }
        public static ShoppingCart[] GetCartByOrderId(long OrderId)
        {
            SQLiteConnection con = DatabaseConnection.GetConnection();
            SQLiteCommand cmd = con.CreateCommand();

            cmd.CommandText = @"SELECT * 
                                FROM [orderDetail]
                                WHERE [orderId] = @OrderId";
            cmd.Parameters.Add(new SQLiteParameter("@OrderId") { Value = OrderId, });
            SQLiteDataReader reader = cmd.ExecuteReader();

            int idx = 0;
            ShoppingCart[] cart = new ShoppingCart[100];
            while (reader.Read())
            {
                    cart[idx] = new ShoppingCart((long)reader["id"],
                                        (long)reader["orderId"],
                                        (long)reader["productId"],
                                        reader["productName"] as String,
                                        (long)reader["unitPrice"],
                                        (long)reader["quantity"],
                                        (long)reader["total"]);
                    idx++;
            }
            DatabaseConnection.RemoveConnection(con);
            if (cart.Count() != 0)
                return cart;
            else
                return null;
            
           
        }
    }
}
