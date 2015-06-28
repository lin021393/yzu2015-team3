using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace team3
{
    class OrderDetail
    {
        private long id;
        private long _order_id;
        private long _product_id;
        private long _quantity;
       
        private OrderDetail(long id, long order_id, long product_id, int quantity)
        {
            this.Id = id;
            this.OrderID = order_id;
            this.ProductID = product_id;
            this.Quantity = quantity;
        }

        public OrderDetail(long order_id, long product_id, long quantity)
        {
            this.OrderID = order_id;
            this.ProductID = product_id;
            this.Quantity = quantity;
        }

        public long Id
        {
            get { return id; }
            internal set { id = value; }
        }

        public long OrderID
        {
            get { return _order_id;  }
            internal set { _order_id = value; }
        }

        public long ProductID
        {
            get { return _product_id; }
            internal set { _product_id = value; }
        }

        public long Quantity
        {
            get { return _quantity; }
            internal set { _quantity = value; }
        }

        public String getProductName()
        {
            Product pro = Product.GetProductById(this._product_id);
            return pro.Name;
        }

        public long getPriceProductPrice()
        {
            Product pro = Product.GetProductById(this._product_id);
            return pro.Price;
        }

        public void Save()
        {
            SQLiteConnection con = DatabaseConnection.GetConnection();
            SQLiteCommand cmd = con.CreateCommand();
            cmd.CommandText = @"INSERT INTO [orderDetail] (
                                [orderId], 
                                [productId],
                                [productName], 
                                [unitPrice],
                                [quantity]
                               ) VALUES ( @orderId, @productId, @productName, @unitPrice, @quantity )";
            Product pro = Product.GetProductById(this.ProductID);
            cmd.Parameters.Add(new SQLiteParameter("@orderId") { Value = this.OrderID, });
            cmd.Parameters.Add(new SQLiteParameter("@productId") { Value = this.ProductID, });
            cmd.Parameters.Add(new SQLiteParameter("@productName") { Value = pro.Name, });
            cmd.Parameters.Add(new SQLiteParameter("@unitPrice") { Value = pro.Price, });
            cmd.Parameters.Add(new SQLiteParameter("@quantity") { Value = this.Quantity, });
            cmd.ExecuteNonQuery();

            string sql = "SELECT last_insert_rowid()";
            cmd = new SQLiteCommand(sql, con);

            this.Id = (long)cmd.ExecuteScalar();
            DatabaseConnection.RemoveConnection(con);
            
        }


        public static List<OrderDetail> getDetailsByOrderId(long orderId)
        {
            OrderInfo orderInfo = OrderInfo.getOrderInfoById(orderId) ;

            if (orderInfo == null)
                return null;

            List<OrderDetail> details = new List<OrderDetail>();

            SQLiteConnection con = DatabaseConnection.GetConnection();
            SQLiteCommand cmd = con.CreateCommand();

            cmd.CommandText = @"SELECT * 
                                FROM [orderDetail]
                                WHERE [orderId] = @orderId";
            cmd.Parameters.Add(new SQLiteParameter("@orderId") { Value = orderId, });
            SQLiteDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                details.Add(new OrderDetail((long)reader["id"],
                    (long)reader["orderId"],
                    (long)reader["productId"],
                    (int)reader["quantity"]));
            }

            DatabaseConnection.RemoveConnection(con);

            return null;
        }

        public static void RemoveAllByOrderId(long orderId)
        {
            SQLiteConnection con = DatabaseConnection.GetConnection();
            SQLiteCommand cmd = con.CreateCommand();

            cmd.CommandText = @"DELETE FROM [orderDetail] WHERE [orderId] = @orderId";
            cmd.Parameters.Add(new SQLiteParameter("@orderId") { Value = orderId, });
            int rows = cmd.ExecuteNonQuery();

            DatabaseConnection.RemoveConnection(con);
        }

    }

}
