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
       
        /*[TestMethod]
        public void TestBuyItDirectly()//直接購買
        {
            int orderId = 2;//is foreign key for table [orderDetail] and is primary key for table [訂單資訊]
            Product product = new Product(
                     "Nokia 3310",
                     1000,
                     "http://p1-news.yamedia.tw/NTAyMjY3bmV3cw==/55950483e199d61f.jpg",
                     "地表最強手機!!!!!!",
                     99
                );
            Assert.IsTrue(product.Save());
            Product productLoaded = Product.GetProductById(product.Id);
            Assert.IsNotNull(productLoaded);

            ShoppingCart cart = new ShoppingCart(productLoaded.Id, productLoaded.Name, productLoaded.Price, 1);
            CartResult result = cart.IsLimitProduct(productLoaded.Remain);
            Assert.IsTrue(result.Success);
            Assert.AreEqual("庫存足夠", result.Message);

            CartResult result2 = cart.SaveDirectly(orderId);
            Assert.IsTrue(result2.Success);
            Assert.AreEqual("單品儲存成功", result2.Message);

        }*/


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

             Assert.AreEqual(19990, order.Total);
         }
        
         [TestMethod]
         public void TestEmptyCart()//測試購物車為空的時候不能購買
         {
             int orderId = 1;
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
