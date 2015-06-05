﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data.SQLite;

namespace team3
{
     class AuthResult 
     {
        private bool _result = false;
        private List<string> _messages;
        private User _user = null;

        public AuthResult (bool Result, List<string> Messages, User RetUser)
        {
            this._result = Result;
            this._messages = Messages;
            this._user = RetUser;
        }
        public bool Result
        {
            get { return _result; }
        }

        public User User
        {
            get { return _user; }
        }

        public List<string> Messages
        {
            get { return _messages; }
        }
     }

    class User
    {
        private long _id;
        private string _account = "";
        private string _email = "";
        private bool isDirty = false;
        
        private User(long Id, string Account, string Email)
        {
            this._id = Id;
            this._account = Account;
            this._email = Email;
            this.isDirty = false;
        }

        public long ID
        {
            get { return this._id; }
        }

        public string Account
        {
            get { return this._account;  }
        }

        public string Email
        {
            get { return this._email; }
            set { this._email = value; this.isDirty = true; }
        }

        public bool Save()
        {
            if (!isDirty)
                return true;
            SQLiteConnection con = DatabaseConnection.GetConnection();
            SQLiteCommand cmd = con.CreateCommand();

            if (this._id > 0)
            {
                cmd.CommandText = @"UPDATE [users] 
                                    SET [email] = @email
                                    WHERE [id] = @id;";

                cmd.Parameters.Add(new SQLiteParameter("@email") { Value = Email, });
                cmd.Parameters.Add(new SQLiteParameter("@id") { Value = ID, });
                cmd.ExecuteNonQuery();

                return true;
            }
            else
            {
                return false;
            }
        }

        public static User GetUserByAccount(string Account)
        {
            SQLiteConnection con = DatabaseConnection.GetConnection();
            SQLiteCommand cmd = con.CreateCommand();
            User retUser = null;

            cmd.CommandText = @"SELECT * 
                                FROM [users]
                                WHERE [account] = @account;";
            cmd.Parameters.Add(new SQLiteParameter("@account") { Value = Account, });

            SQLiteDataReader reader = cmd.ExecuteReader();

            if(reader.Read())
            {
               retUser =  new User(
                      (long)reader["id"],
                      (string)reader["account"],
                      (string)reader["email"]
                  );
                
            }
            DatabaseConnection.RemoveConnection(con);

            return retUser;

        }

        public static User GetUserByEmail(string Email)
        {
            SQLiteConnection con = DatabaseConnection.GetConnection();
            SQLiteCommand cmd = con.CreateCommand();
            User retUser = null;

            cmd.CommandText = @"SELECT * 
                                FROM [users]
                                WHERE [email] = @email;";
            cmd.Parameters.Add(new SQLiteParameter("@email") { Value = Email, });

            SQLiteDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                retUser = new User(
                       (long)reader["id"],
                       (string)reader["account"],
                       (string)reader["email"]
                   );

            }
            DatabaseConnection.RemoveConnection(con);

            return retUser;

        }

        public static User GetUserById(long ID)
        {
            SQLiteConnection con = DatabaseConnection.GetConnection();
            SQLiteCommand cmd = con.CreateCommand();
            User retUser = null;

            cmd.CommandText = @"SELECT * 
                                FROM [users]
                                WHERE [id] = @id;";
            cmd.Parameters.Add(new SQLiteParameter("@id") { Value = ID, });

            SQLiteDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                retUser = new User(
                       (long)reader["id"],
                       (string)reader["account"],
                       (string)reader["email"]
                   );

            }
            DatabaseConnection.RemoveConnection(con);

            return retUser;

        }

        public static AuthResult Login(string Account, string Password)
        {
            SQLiteConnection con = DatabaseConnection.GetConnection();
            SQLiteCommand cmd = con.CreateCommand();
            AuthResult retResult = null;
            List<string> messages = new List<string>();
            
            cmd.CommandText = @"SELECT * 
                                FROM [user]
                                WHERE [account] = @account AND [password] = @password;";
            cmd.Parameters.Add(new SQLiteParameter("@account") { Value = Account, });
            cmd.Parameters.Add(new SQLiteParameter("@password") { Value = HashUtil.EncryptoSHA1(Password), });

            SQLiteDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                User retUser = new User(
                        (long)reader["id"],
                        (string)reader["account"],
                        (string)reader["email"]
                    );
                messages.Add(Strings.USER_LOGIN_SUCESSFULLY);
                retResult = new AuthResult(true, messages, retUser);
                
            }
            else
            {
                messages.Add(Strings.USER_LOGIN_FAIL);

                retResult = new AuthResult(false, messages , null);
            }

            DatabaseConnection.RemoveConnection(con);
            return retResult;
        }
        
        public static AuthResult Register(string Account, string Password, string ConfirmPassword, string Email)
        {
            List<string> errorMessage = new List<string>();
            AuthResult retResult = null;

            if (!(Account.Length >= 6 && Account.Length <= 40))
                errorMessage.Add(Strings.USER_REGISTER_ACCOUNT_LENGTH_ERROR);

            if (!StringUtil.isVaildAccountFormat(Account))
                errorMessage.Add(Strings.USER_REGISTER_ACCOUNT_FORMAT_ERROR);

            if (!(Password.Length >= 6 && Password.Length <= 40))
                errorMessage.Add(Strings.USER_REGISTER_PASSWORD_LENGTH_ERROR);

            if (!Password.Equals(ConfirmPassword))
                errorMessage.Add(Strings.USER_REGISTER_PASSWORD_CONFIRM_ERROR);

            if (!StringUtil.isValidEmail(Email))
                errorMessage.Add( Strings.USER_REGISTER_EMAIL_FORMAT_ERROR);

            if( GetUserByAccount(Account) != null)
                errorMessage.Add(Strings.USER_REGISTER_ACCOUNT_EXISTS);

            if (GetUserByEmail(Email) != null)
                errorMessage.Add(Strings.USER_REGISTER_EMAIL_EXISTS_ERROR);
            

            if( errorMessage.Count == 0)
            {
                SQLiteConnection con = DatabaseConnection.GetConnection();
                SQLiteCommand cmd = con.CreateCommand();

                cmd.CommandText = @"INSERT INTO [users] ( 
                                    [account] ,
                                    [password] ,
                                    [email] ) VALUES (
                                     @account ,
                                     @password ,
                                     @email );";

                cmd.Parameters.Add(new SQLiteParameter("@account") { Value = Account, });
                cmd.Parameters.Add(new SQLiteParameter("@password") { Value = HashUtil.EncryptoSHA1(Password), });
                cmd.Parameters.Add(new SQLiteParameter("@email") { Value = Email, });

                cmd.ExecuteNonQuery();

                string sql = "SELECT last_insert_rowid()";
                cmd = new SQLiteCommand(sql, con);
                long id = (long)cmd.ExecuteScalar();

                if (id > 0)
                {
                    List<string> messages = new List<string>();
                    messages.Add(Strings.USER_REGISTER_SUCCESSFULLY);

                    User retUser = new User(id, Account, Email);
                    retResult = new AuthResult(true, messages, retUser);
                }
                else
                {
                    errorMessage.Add(Strings.USER_REGISTER_FAIL);
                    retResult = new AuthResult(false, errorMessage, null);
                }

                DatabaseConnection.RemoveConnection(con);
            }
            else
            {
                errorMessage.Insert(0, Strings.USER_REGISTER_FAIL);
                retResult = new AuthResult(false, errorMessage, null);
            }

            return retResult;
        }
    }

}