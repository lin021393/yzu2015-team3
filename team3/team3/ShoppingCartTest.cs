﻿using System;
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
        public void TestCartProductLimit()//測試購買數量大於商品庫存會return false
        {
            int orderId = 1;//紀錄同一個使用者的購物車他的orderId都相同
            Product product = new Product(
                     "IPHONE 6S",
                     19990,
                     "http://img.technews.tw/wp-content/uploads/2015/06/15233374520_abea3a452a_z-624x416.jpg",
                     "潮到滴水",
                     99
                );
            Assert.IsTrue(product.Save());
            Product productLoaded = Product.GetProductById(product.Id);
            Assert.IsNotNull(productLoaded);

            ShoppingCart cart = new ShoppingCart(orderId, productLoaded.Id, productLoaded.Name, productLoaded.Price, 100);//買100隻但商品只有99隻
            Assert.IsFalse(cart.IsLimitProduct(productLoaded.Remain));
        }
        [TestMethod]
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

            ShoppingCart cart = new ShoppingCart(orderId, productLoaded.Id, productLoaded.Name, productLoaded.Price, 1);
            Assert.IsTrue(cart.IsLimitProduct(productLoaded.Remain));
            Assert.IsTrue(cart.SaveDirectly());

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
            Assert.IsTrue(product.Save());
            Product productLoaded = Product.GetProductById(product.Id);
            Assert.IsNotNull(productLoaded);

            ShoppingCart cart = new ShoppingCart(orderId, productLoaded.Id, productLoaded.Name, productLoaded.Price, 1);
            Assert.IsTrue(cart.IsLimitProduct(productLoaded.Remain));
            cart.AddToCart();
            //
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

            ShoppingCart cart2 = new ShoppingCart(orderId, productLoaded2.Id, productLoaded2.Name, productLoaded2.Price, 3);
            Assert.IsTrue(cart2.IsLimitProduct(productLoaded2.Remain));
            cart2.AddToCart();

            List<ShoppingCart> cartLoaded= ShoppingCart.GetCartInfo();
            Assert.AreEqual(cartLoaded[0].Id, cart.Id);
            Assert.AreEqual(cartLoaded[0].Orderid, cart.Orderid);
            Assert.AreEqual(cartLoaded[0].Productid, cart.Productid);
            Assert.AreEqual(cartLoaded[0].Productname, cart.Productname);
            Assert.AreEqual(cartLoaded[0].Unitprice, cart.Unitprice);
            Assert.AreEqual(cartLoaded[0].Quantity, cart.Quantity);
            Assert.AreEqual(cartLoaded[0].Total, cart.Total);

            Assert.AreEqual(cartLoaded[1].Id, cart2.Id);
            Assert.AreEqual(cartLoaded[1].Orderid, cart2.Orderid);
            Assert.AreEqual(cartLoaded[1].Productid, cart2.Productid);
            Assert.AreEqual(cartLoaded[1].Productname, cart2.Productname);
            Assert.AreEqual(cartLoaded[1].Unitprice, cart2.Unitprice);
            Assert.AreEqual(cartLoaded[1].Quantity, cart2.Quantity);
            Assert.AreEqual(cartLoaded[1].Total, cart2.Total);
            cart2.empty_cart();
            
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
            Assert.IsTrue(product.Save());
            Product productLoaded = Product.GetProductById(product.Id);
            Assert.IsNotNull(productLoaded);

            ShoppingCart cart = new ShoppingCart(orderId, productLoaded.Id, productLoaded.Name, productLoaded.Price, 1);
            Assert.IsTrue(cart.IsLimitProduct(productLoaded.Remain));
            cart.AddToCart();
            //
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

            ShoppingCart cart2 = new ShoppingCart(orderId, productLoaded2.Id, productLoaded2.Name, productLoaded2.Price, 3);
            Assert.IsTrue(cart2.IsLimitProduct(productLoaded2.Remain));
            cart2.AddToCart();

            Assert.IsTrue(cart2.SaveFromCart());
            cart2.empty_cart();

        }
        [TestMethod]
        public void TestGetCartByOrderId()//藉由orderId從DB撈出同樣orderId所有的購買商品資訊
        {
            int orderId = 2;//orderId是table [orderDetail]中的外來鍵, orderid是table [詳細訂單資訊] 中的主鍵
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

            ShoppingCart cart = new ShoppingCart(orderId, productLoaded.Id, productLoaded.Name, productLoaded.Price, 1);
            Assert.IsTrue(cart.IsLimitProduct(productLoaded.Remain));
            cart.AddToCart();
            //
            Product product2 = new Product(
                     "IPHONE 7S",
                     29990,
                     "http://img.technews.tw/wp-content/uploads/2015/06/15233374520_abea3a452a_z-624x416.jpg",
                     "潮到滴水",
                     99
                );
            Assert.IsTrue(product2.Save());
            Product productLoaded2 = Product.GetProductById(product2.Id);
            Assert.IsNotNull(productLoaded2);

            ShoppingCart cart2 = new ShoppingCart(orderId, productLoaded2.Id, productLoaded2.Name, productLoaded2.Price, 3);
            Assert.IsTrue(cart2.IsLimitProduct(productLoaded2.Remain));
            cart2.AddToCart();

            //save
            Assert.IsTrue(cart2.SaveFromCart());

            //get cartinfo from table orderDetail
            ShoppingCart[] cartlist = ShoppingCart.GetCartByOrderId(orderId);
            Assert.IsNotNull(cartlist);
           
            //test get two different cart info use orderId
            //Assert.AreEqual(cartlist[0].Id, cart.Id); 因為從購物車結帳的時候一次把購物車的不同商品資訊丟到DB,所以沒辦法分別驗證不同商品的ID
            Assert.AreEqual(cartlist[0].Orderid, cart.Orderid);
            Assert.AreEqual(cartlist[0].Productid, cart.Productid);
            Assert.AreEqual(cartlist[0].Productname, cart.Productname);
            Assert.AreEqual(cartlist[0].Unitprice, cart.Unitprice);
            Assert.AreEqual(cartlist[0].Quantity, cart.Quantity);
            Assert.AreEqual(cartlist[0].Total, cart.Total);

            //Assert.AreEqual(cartlist[1].Id, cart2.Id);
            Assert.AreEqual(cartlist[1].Orderid, cart2.Orderid);
            Assert.AreEqual(cartlist[1].Productid, cart2.Productid);
            Assert.AreEqual(cartlist[1].Productname, cart2.Productname);
            Assert.AreEqual(cartlist[1].Unitprice, cart2.Unitprice);
            Assert.AreEqual(cartlist[1].Quantity, cart2.Quantity);
            Assert.AreEqual(cartlist[1].Total, cart2.Total);
            cart2.empty_cart();
        }
        [TestMethod]
        public void TestEmptyCart()//測試購物車為空的時候不能購買
        {
            Product product = new Product(
                     "IPHONE 6S",
                     19990,
                     "http://img.technews.tw/wp-content/uploads/2015/06/15233374520_abea3a452a_z-624x416.jpg",
                     "潮到滴水",
                     99
                );
            Assert.IsTrue(product.Save());
            Product productLoaded = Product.GetProductById(product.Id);
            Assert.IsNotNull(productLoaded);

            ShoppingCart cart = new ShoppingCart(1, productLoaded.Id, productLoaded.Name, productLoaded.Price, 1);
            Assert.IsTrue(cart.IsLimitProduct(productLoaded.Remain));
            cart.AddToCart();
            cart.empty_cart();
            Assert.IsFalse(cart.SaveFromCart());
        }
    }
}