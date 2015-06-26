using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace team3
{
    [TestClass]
    public class AdministratorTest
    {
        [TestInitialize]
        public void Init()
        {
            DatabaseConnection.DropTable("administrators");
            DatabaseConnection.Init();

        }
        [TestMethod]
        public void TestRegisterAdm()
        {
            /* 登錄 */
            AuthResult regResult = User.Login("accou___n_t1", "passowrd");
            Assert.IsTrue(regResult.Messages.Contains(Message.USER_LOGIN_SUCESSFULLY));
            Assert.IsTrue(regResult.Result);

            /* 設置管理員 */
            Administrator administrator = new Administrator(regResult.User.Account);
            var result = Administrator.Register(regResult.User.Account);
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Admin註冊成功", result.Message); 
        }
    }
}
