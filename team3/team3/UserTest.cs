using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace team3
{
    [TestClass]
    public class UserTest
    {
        [TestMethod]
        public void RegisterUser()
        {
            DatabaseConnection.DropTable("users");
            DatabaseConnection.Init();

            /* 合法帳號,合法密碼,合法信箱 */
            AuthResult regResult = User.Register("accou___n_t1", "passowrd", "passowrd", "example@gmail.com");
            Assert.IsNotNull(regResult);
            Assert.IsNotNull(regResult.User);
            Assert.IsNotNull(regResult.Messages);
            Assert.IsTrue(regResult.Messages.Contains(Strings.USER_REGISTER_SUCCESSFULLY));
            Assert.IsTrue(regResult.Result);
            Assert.IsTrue(regResult.User.ID > 0);
            Assert.AreEqual(regResult.User.Account, "accou___n_t1");
            Assert.AreEqual(regResult.User.Email,  "example@gmail.com");

             /* 長度過短帳號,合法密碼,合法信箱 */
            AuthResult regResult2 = User.Register("asd", "passowrd", "passowrd", "example2@gmail.com");
            Assert.IsNotNull(regResult2);
            Assert.IsNull(regResult2.User);
            Assert.IsNotNull(regResult2.Messages);
            Assert.IsTrue(regResult2.Messages.Contains(Strings.USER_REGISTER_FAIL));
            Assert.IsTrue(regResult2.Messages.Contains(Strings.USER_REGISTER_ACCOUNT_LENGTH_ERROR));
            Assert.IsFalse(regResult2.Result);

            /* 格式不符合帳號(除了底線以外的特殊符號),合法密碼,合法信箱 */
            AuthResult regResult3 = User.Register("SDJA(DUAS*DJ(Q@)", "passowrd", "passowrd", "example3@gmail.com");
            Assert.IsNotNull(regResult3);
            Assert.IsNull(regResult3.User);
            Assert.IsNotNull(regResult3.Messages);
            Assert.IsTrue(regResult3.Messages.Contains(Strings.USER_REGISTER_FAIL));
            Assert.IsTrue(regResult3.Messages.Contains(Strings.USER_REGISTER_ACCOUNT_FORMAT_ERROR));
            Assert.IsFalse(regResult3.Result);


            /* 合法帳號,合法密碼,已存在信箱 */
            AuthResult regResult4 = User.Register("ssadadasdasdadsa", "passowrd", "passowrd", "example@gmail.com");
            Assert.IsNotNull(regResult4);
            Assert.IsNull(regResult4.User);
            Assert.IsNotNull(regResult4.Messages);
            Assert.IsTrue(regResult4.Messages.Contains(Strings.USER_REGISTER_FAIL));
            Assert.IsTrue(regResult4.Messages.Contains(Strings.USER_REGISTER_EMAIL_EXISTS_ERROR));
            Assert.IsFalse(regResult4.Result);

            /* 已存在帳號,合法密碼,合法信箱 */
            AuthResult regResult5 = User.Register("accou___n_t1", "passowrd", "passowrd", "example5@gmail.com");
            Assert.IsNotNull(regResult5);
            Assert.IsNull(regResult5.User);
            Assert.IsNotNull(regResult5.Messages);
            Assert.IsTrue(regResult5.Messages.Contains(Strings.USER_REGISTER_FAIL));
            Assert.IsTrue(regResult5.Messages.Contains(Strings.USER_REGISTER_ACCOUNT_EXISTS));
            Assert.IsFalse(regResult5.Result);

            /* 已存在帳號,過長密碼,合法信箱 */
            AuthResult regResult6 = User.Register("ssadadasdasdadsa", "passowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrd",
                "passowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrdpassowrd", 
                "example6@gmail.com");
            Assert.IsNotNull(regResult6);
            Assert.IsNull(regResult6.User);
            Assert.IsNotNull(regResult6.Messages);
            Assert.IsTrue(regResult6.Messages.Contains(Strings.USER_REGISTER_FAIL));
            Assert.IsTrue(regResult6.Messages.Contains(Strings.USER_REGISTER_PASSWORD_LENGTH_ERROR));
            Assert.IsFalse(regResult6.Result);

            /* 已存在帳號,確認密碼不一致,合法信箱 */
            AuthResult regResult7 = User.Register("qweqweqwqweqwe", "passowrd", "passowrd2", "example7@gmail.com");
            Assert.IsNotNull(regResult7);
            Assert.IsNull(regResult7.User);
            Assert.IsNotNull(regResult7.Messages);
            Assert.IsTrue(regResult7.Messages.Contains(Strings.USER_REGISTER_FAIL));
            Assert.IsTrue(regResult7.Messages.Contains(Strings.USER_REGISTER_PASSWORD_CONFIRM_ERROR));
            Assert.IsFalse(regResult7.Result);

            /* 合法帳號,合法密碼,空白信箱 */
            AuthResult regResult8 = User.Register("asdasdadad2", "passowrd", "passowrd", "");
            Assert.IsNotNull(regResult8);
            Assert.IsNull(regResult8.User);
            Assert.IsNotNull(regResult8.Messages);
            Assert.IsTrue(regResult8.Messages.Contains(Strings.USER_REGISTER_FAIL));
            Assert.IsTrue(regResult8.Messages.Contains(Strings.USER_REGISTER_EMAIL_FORMAT_ERROR));
            Assert.IsFalse(regResult8.Result);

            /* 空白帳號,合法密碼,合法信箱 */
            AuthResult regResult9 = User.Register("", "passowrd", "passowrd", "examp23le3@gmail.com");
            Assert.IsNotNull(regResult9);
            Assert.IsNull(regResult9.User);
            Assert.IsNotNull(regResult9.Messages);
            Assert.IsTrue(regResult9.Messages.Contains(Strings.USER_REGISTER_FAIL));
            Assert.IsTrue(regResult9.Messages.Contains(Strings.USER_REGISTER_ACCOUNT_FORMAT_ERROR));
            Assert.IsFalse(regResult9.Result);

            /* 合法帳號,空白密碼,合法信箱 */
            AuthResult regResult10 = User.Register("ms8901231235sd", "", "", "examp23le3@gmail.com");
            Assert.IsNotNull(regResult10);
            Assert.IsNull(regResult10.User);
            Assert.IsNotNull(regResult10.Messages);
            Assert.IsTrue(regResult10.Messages.Contains(Strings.USER_REGISTER_FAIL));
            Assert.IsTrue(regResult10.Messages.Contains(Strings.USER_REGISTER_PASSWORD_LENGTH_ERROR));
            Assert.IsFalse(regResult10.Result);

        }
    }
}
