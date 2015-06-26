using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace team3
{
    [TestClass]
    public class OrderInfoTest
    {
        [TestInitialize]
        public void Init()
        {
            DatabaseConnection.DropTable("order");
            DatabaseConnection.DropTable("orderDetail");
            DatabaseConnection.DropTable("products");
            DatabaseConnection.Init();
        }
        [TestMethod]
        public void TestBuyItFromCart()//從購物車結帳
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
            user = loginRes.User;


            CartResult res = user.addProductToCards(product, 1);

            OrderInfo order = user.BuyFromCarts();

            Assert.AreEqual(1, order.Details.Count);
            Assert.AreEqual(19990, order.Total);
        }

        [TestMethod]
        public void TestBuyDirectly()
        {
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
            user = loginRes.User;


            OrderInfo order = user.buyFromProduct(product.Id, 1);
            Assert.IsNotNull(order);

            OrderInfo order2 = user.buyFromProduct(product.Id, 100);
            Assert.IsNull(order2);

        }

        [TestMethod]
        public void TestBuyEmptyCart()//測試購物車為空的時候不能購買
        {
            Product product = new Product(
                     "IPHONE 6S",
                     19990,
                     "http://img.technews.tw/wp-content/uploads/2015/06/15233374520_abea3a452a_z-624x416.jpg",
                     "潮到滴水",
                     99
                );
            product.Save();

            AuthResult regResult = User.Register("testaccountQQ", "MNHUOKJHBNKUH", "MNHUOKJHBNKUH", "exa2eweweemple@gmail.com");
            User user = regResult.User;
            AuthResult loginRes = User.Login("testaccountQQ", "MNHUOKJHBNKUH");
            user = loginRes.User;

            user.ClearCarts();

            Assert.IsNull(user.BuyFromCarts());

        }
    }
}
