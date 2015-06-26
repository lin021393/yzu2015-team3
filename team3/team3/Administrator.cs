using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data.SQLite;

namespace team3
{
    class AdminResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
    class Administrator
    {
        private string _account = "";
        private bool isDirty = false;

        public Administrator(string Account)
        {
            this._account = Account;
            this.isDirty = false;
        }
        public static AdminResult Register(string Account)
        {
            List<string> errorMessage = new List<string>();
            AdminResult admResult = null;

            if (errorMessage.Count == 0)
            {
                SQLiteConnection con = DatabaseConnection.GetConnection();
                SQLiteCommand cmd = con.CreateCommand();

                cmd.CommandText = @"INSERT INTO [administrators] ( 
                                    [account] ) VALUES (
                                     @account );";

                cmd.Parameters.Add(new SQLiteParameter("@account") { Value = Account, });

                cmd.ExecuteNonQuery();

                string sql = "SELECT last_insert_rowid()";
                cmd = new SQLiteCommand(sql, con);
                long id = (long)cmd.ExecuteScalar();

                if (id > 0)
                {

                    Administrator retAdm = new Administrator(Account);
                    admResult = new AdminResult{ Success = true, Message = "Admin註冊成功" };
                }
                else
                {
                    admResult = new AdminResult{ Success = false, Message = "Admin註冊失敗" };
                }

                DatabaseConnection.RemoveConnection(con);
            }
            else
            {
                admResult = new AdminResult { Success = false, Message = "Admin註冊失敗" };
            }

            return admResult;
        }
    }
}
