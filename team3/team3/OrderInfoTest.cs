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
        public void TestAddOrderInfo()
        {
            Product product = new Product(
                     "IPHONE 7",
                     19990,
                     "http://img.technews.tw/wp-content/uploads/2015/06/15233374520_abea3a452a_z-624x416.jpg",
                     "潮到滴水",
                     99
                );
            Assert.IsTrue(product.Save());
            Product productLoaded = Product.GetProductById(product.Id);
            Assert.IsNotNull(productLoaded);
            
            /*
            ShoppingCart cart = new ShoppingCart(productLoaded.Id, productLoaded.Name, productLoaded.Price, 1);
            CartResult result = cart.IsLimitProduct(productLoaded.Remain);
            Assert.IsTrue(result.Success);
            Assert.AreEqual("庫存足夠", result.Message);

            cart.AddToCart();
            //以上是第一筆購物資訊先加入購物車
            Product product2 = new Product(
                    "Nokia 3310",
                    1000,
                    "http://p1-news.yamedia.tw/NTAyMjY3bmV3cw==/55950483e199d61f.jpg",
                    "地表最強手機!!!!!!",
                    99
               );
            Assert.IsTrue(product2.Save());
            Product productLoaded2 = Product.GetProductById(product2.Id);
            Assert.IsNotNull(productLoaded2);

            ShoppingCart cart2 = new ShoppingCart(productLoaded2.Id, productLoaded2.Name, productLoaded2.Price, 3);
            CartResult result2 = cart2.IsLimitProduct(productLoaded2.Remain);
            Assert.IsTrue(result2.Success);
            Assert.AreEqual("庫存足夠", result2.Message);

            cart2.AddToCart();
            //以上是第二筆購物資訊先加入購物車
           
            List<ShoppingCart> cartLoaded = ShoppingCart.GetCartInfo();
            long total = OrderInfo.CaculateTotal(cartLoaded, ShoppingCart.Count);//算出購物車內商品的總和(丟進去購物車內的商品資訊&商品數目)
            long deliverfee=100;//運費100
            OrderInfo orderinfo = new OrderInfo(total, deliverfee, "Kevin", "桃園市中壢區遠東路135號", "0912345678", "David", "桃園市中壢區元智大學","0987654321", "ATM轉帳", "郵局包裹");
            OrderResult result3 = orderinfo.Save();
            Assert.IsTrue(result3.Success);
            Assert.AreEqual("訂單儲存成功", result3.Message);
            //以上是先取出購物車內的商品資訊(取得總金額) 先建立訂單並存在order資料表 

            CartResult result4 = cart2.SaveFromCart(orderinfo.Id);
            Assert.IsTrue(result4.Success);
            Assert.AreEqual("購物車商品儲存成功", result4.Message);
            //以上是利用資料表order的Id  將不同商品分別存在orderDetail資料表(若orderId相同則為同一筆訂單)
            cart2.empty_cart();*/
        }
    }
}
