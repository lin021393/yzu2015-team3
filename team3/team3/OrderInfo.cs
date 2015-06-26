using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SQLite; 
namespace team3
{
    class OrderResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
    class OrderInfo
    {
        
        private long _id;
        private long _total;
        private long _deliverfee;
        private long _grandtotal;
        private string _customername = "";
        private string _customeraddress = "";
        private string _customerphone = "";
        private string _receivename = "";
        private string _receiveaddress = "";
        private string _receivephone = "";
        private string _paytype = "";
        private string _receivetype = "";
        private List<OrderDetail> _details = new List<OrderDetail>();
        
        private bool isDirty = false;



        public OrderInfo(long deliverfee,  string customername, string customeraddress, string customerphone, string receivename, string receiveaddress, string receivephone, string paytype, string receivetype, long grandtotal=0)
        {
            this.Id = 0;
            this.Deliverfee = deliverfee;
            this.Grandtotal = deliverfee;
            this.Customername = customername;
            this.Customeraddress = customeraddress;
            this.Customerphone = customerphone;
            this.Receivename = receivename;
            this.Receiveaddress = receiveaddress;
            this.Receivephone = receivephone;
            this.Paytype = paytype;
            this.Receivetype = receivetype;
            isDirty = false;
        }

        private OrderInfo(long id, long total, long deliverfee,  string customername, string customeraddress, string customerphone, string receivename, string receiveaddress, string receivephone, string paytype, string receivetype, long grandtotal = 0)
        {
            this.Id = id;
            this.Total = total;
            this.Deliverfee = deliverfee;
            this.Grandtotal = deliverfee;
            this.Customername = customername;
            this.Customeraddress = customeraddress;
            this.Customerphone = customerphone;
            this.Receivename = receivename;
            this.Receiveaddress = receiveaddress;
            this.Receivephone = receivephone;
            this.Paytype = paytype;
            this.Receivetype = receivetype;
            this._details = OrderDetail.getDetailsByOrderId(Id);
            
            isDirty = false;
        }

        public long Id
        {
            get { return _id; }
            internal set { this._id = value; }
        }

        public long Total
        {
            get { return this._total; }
            set { this._total = value; this.isDirty = true; }
        }

        public long Deliverfee
        {
            get { return this._deliverfee; }
            set { this._deliverfee = value; this.isDirty = true; }
        }

        public long Grandtotal
        {
            get { return this._grandtotal; }
            set { this._grandtotal = value; this.isDirty = true; }
        }

        public string Customername
        {
            get { return this._customername; }
            set { this._customername = value; this.isDirty = true; }
        }

        public string Customeraddress
        {
            get { return this._customeraddress; }
            set { this._customeraddress = value; this.isDirty = true; }
        }

        public string Customerphone
        {
            get { return this._customerphone; }
            set { this._customerphone = value; this.isDirty = true; }
        }

        public string Receivename
        {
            get { return this._receivename; }
            set { this._receivename = value; this.isDirty = true; }
        }

        public string Receiveaddress
        {
            get { return this._receiveaddress; }
            set { this._receiveaddress = value; this.isDirty = true; }
        }

        public string Receivephone
        {
            get { return this._receivephone; }
            set { this._receivephone = value; this.isDirty = true; }
        }

        public string Paytype
        {
            get { return this._paytype; }
            set { this._paytype = value; this.isDirty = true; }
        }

        public string Receivetype
        {
            get { return this._receivetype; }
            set { this._receivetype = value; this.isDirty = true; }
        }

        public List<OrderDetail> Details
        {
            get { return this._details; }
            internal set { this._details = value ; }
        }

        public void addDetails(List<ShoppingCart> carts)
        {
            foreach(ShoppingCart cart in carts)
            {
                OrderDetail item = new OrderDetail(this.Id, cart.Productid, cart.Quantity);
                this.Total += cart.Unitprice;
                this.Grandtotal = Total + this.Deliverfee;
                item.Save();
                this._details.Add(item);
            }
        }

        public bool IsSaved()
        {
            return this.Id > 0 && this.isDirty == false;
        }

        public OrderResult Save()
        {
            if (IsSaved())
                return new OrderResult { Success = true, Message = "訂單儲存成功" }; 

            try
            {

                SQLiteConnection con = DatabaseConnection.GetConnection();
                SQLiteCommand cmd = con.CreateCommand();

                if (this.Id == 0)
                {
                    cmd.CommandText = @"INSERT INTO [order] (
                                [total], 
                                [deliverFee],
                                [grandTotal], 
                                [customerName],
                                [customerAddress],
                                [customerPhone],
                                [receiveName],
                                [receiveAddress],
                                [receivePhone],
                                [payType],
                                [receiveType]
                               ) VALUES ( @total, @deliverFee, @grandTotal, @customerName, @customerAddress, @customerPhone, @receiveName, @receiveAddress, @receivePhone, @payType, @receiveType)";

                    cmd.Parameters.Add(new SQLiteParameter("@total") { Value = this.Total, });
                    cmd.Parameters.Add(new SQLiteParameter("@deliverFee") { Value = this.Deliverfee, });
                    cmd.Parameters.Add(new SQLiteParameter("@grandTotal") { Value = this.Grandtotal, });
                    cmd.Parameters.Add(new SQLiteParameter("@customerName") { Value = this.Customername, });
                    cmd.Parameters.Add(new SQLiteParameter("@customerAddress") { Value = this.Customeraddress, });
                    cmd.Parameters.Add(new SQLiteParameter("@customerPhone") { Value = this.Customerphone, });
                    cmd.Parameters.Add(new SQLiteParameter("@receiveName") { Value = this.Receivename, });
                    cmd.Parameters.Add(new SQLiteParameter("@receiveAddress") { Value = this.Receiveaddress, });
                    cmd.Parameters.Add(new SQLiteParameter("@receivePhone") { Value = this.Receivephone, });
                    cmd.Parameters.Add(new SQLiteParameter("@payType") { Value = this.Paytype, });
                    cmd.Parameters.Add(new SQLiteParameter("@receiveType") { Value = this.Receivetype, });
                    cmd.ExecuteNonQuery();

                    string sql = "SELECT last_insert_rowid()";
                    cmd = new SQLiteCommand(sql, con);

                    this.Id = (long)cmd.ExecuteScalar();
                    DatabaseConnection.RemoveConnection(con);
                    this.isDirty = false;

                    if(this.Id>0)
                       return new OrderResult { Success = true, Message = "訂單儲存成功" }; 
                    else
                       return new OrderResult { Success = false, Message = "訂單儲存失敗" }; 
                }
                else
                {
                    cmd.CommandText = @"UPDATE [order]
                                        SET [total] = @total ,
                                            [deliverFee] = @deliverFee ,
                                            [grandTotal] = @grandTotal ,
                                            [customerName] = @customerName ,
                                            [customerAddress] = @customerAddress, 
                                            [customerPhone] = @customerPhone, 
                                            [receiveName] = @receiveName, 
                                            [receiveAddress] = @receiveAddress, 
                                            [receivePhone] = @receivePhone, 
                                            [payType] = @payType, 
                                            [receiveType] = @receiveType
                                        WHERE [id] = @id";

                    cmd.Parameters.Add(new SQLiteParameter("@total") { Value = this.Total, });
                    cmd.Parameters.Add(new SQLiteParameter("@deliverFee") { Value = this.Deliverfee, });
                    cmd.Parameters.Add(new SQLiteParameter("@grandTotal") { Value = this.Grandtotal, });
                    cmd.Parameters.Add(new SQLiteParameter("@customerName") { Value = this.Customername, });
                    cmd.Parameters.Add(new SQLiteParameter("@customerAddress") { Value = this.Customeraddress, });
                    cmd.Parameters.Add(new SQLiteParameter("@customerPhone") { Value = this.Customerphone, });
                    cmd.Parameters.Add(new SQLiteParameter("@receiveName") { Value = this.Receivename, });
                    cmd.Parameters.Add(new SQLiteParameter("@receiveAddress") { Value = this.Receiveaddress, });
                    cmd.Parameters.Add(new SQLiteParameter("@receivePhone") { Value = this.Receivephone, });
                    cmd.Parameters.Add(new SQLiteParameter("@payType") { Value = this.Paytype, });
                    cmd.Parameters.Add(new SQLiteParameter("@receiveType") { Value = this.Receivetype, });
                    int res = cmd.ExecuteNonQuery();
                    this.isDirty = false;
                    DatabaseConnection.RemoveConnection(con);
                    return new OrderResult { Success = true, Message = "訂單儲存成功" }; ;

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new OrderResult { Success = false, Message = "資料庫存取失敗" }; ;
            }

        }

        public static OrderInfo getOrderInfoById(long OrederId)
        {
            SQLiteConnection con = DatabaseConnection.GetConnection();
            SQLiteCommand cmd = con.CreateCommand();

            cmd.CommandText = @"SELECT * 
                                FROM [order]
                                WHERE [id] = @id";
            cmd.Parameters.Add(new SQLiteParameter("@id") { Value = OrederId, });
            SQLiteDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                OrderInfo order = new OrderInfo((long)reader["id"],
                                                (long)reader["total"],
                                                (long)reader["deliverFee"],
                                                reader["customerName"] as string,
                                                reader["customerAddress"] as string,
                                                reader["customerPhone"] as string,
                                                reader["receiveName"] as string,
                                                reader["receiveAddress"] as string,
                                                reader["receivePhone"] as string,
                                                reader["payType"] as string,
                                                reader["receiveType"] as string,
                                                (long)reader["grandTotal"]);

                DatabaseConnection.RemoveConnection(con);
                return order;
            }
            else
            {
                return null;
            }
        }
    }
}
