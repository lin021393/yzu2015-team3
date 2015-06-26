using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace team3
{
    [TestClass]
    public class ShoppingCartTest
    {
        [TestInitialize]
        public void Init()
        {
            DatabaseConnection.DropTable("orderDetail");
            DatabaseConnection.DropTable("products");
            DatabaseConnection.Init();
        }
       

        [TestMethod]
        public void TestAddToCart()//測試加到購物車
        {
            int orderId = 2;//is foreign key for table [orderDetail] and is primary key for table [訂單資訊]
            Product product = new Product(
                     "IPHONE 7",
                     19990,
                     "http://img.technews.tw/wp-content/uploads/2015/06/15233374520_abea3a452a_z-624x416.jpg",
                     "潮到滴水",
                     99
                );
            product.Save();

            AuthResult regResult = User.Register("testaccountQQ", "MNHUOKJHBNKUH", "MNHUOKJHBNKUH", "exa2eweweemple@gmail.com");
            User user = regResult.User;
            AuthResult loginRes = User.Login("testaccountQQ", "MNHUOKJHBNKUH");

            CartResult res =  user.addProductToCards(product, 1);

            Assert.IsTrue(res.Success);

            res = user.addProductToCards(product, 100);

            Assert.IsFalse(res.Success);
           
            
        }
        
    }
}
