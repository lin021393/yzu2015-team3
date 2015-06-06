using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace team3
{
    /// <summary>
    /// ProductTest 的摘要描述
    /// </summary>
    [TestClass]
    public class ProductTest
    {
        [TestInitialize]
        public void Init()
        {
            DatabaseConnection.Init();
        }

        public ProductTest()
        {
            //
            // TODO:  在此加入建構函式的程式碼
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///取得或設定提供目前測試回合
        ///的相關資訊與功能的測試內容。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 其他測試屬性
        //
        // 您可以使用下列其他屬性撰寫您的測試: 
        //
        // 執行該類別中第一項測試前，使用 ClassInitialize 執行程式碼
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在類別中的所有測試執行後，使用 ClassCleanup 執行程式碼
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在執行每一項測試之前，先使用 TestInitialize 執行程式碼 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在執行每一項測試之後，使用 TestCleanup 執行程式碼
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestAddProduct()
        {
            Product product = new Product(
                     "Nokia 3310" ,
                     111 ,
                     "http://p1-news.yamedia.tw/NTAyMjY3bmV3cw==/55950483e199d61f.jpg" ,
                     "地表最強手機!!!!!!" ,
                     99
                );
            Assert.IsTrue(product.Save());
        }


        [TestMethod]
        public void TestGetProductById()
        {
            Product product = new Product(
                     "Nokia 3310",
                     111,
                     "http://p1-news.yamedia.tw/NTAyMjY3bmV3cw==/55950483e199d61f.jpg",
                     "地表最強手機!!!!!!",
                     99
                );

            bool result = product.Save();

            if(result)
            {
                Product productLoaded = Product.GetProductById(product.Id);

                Assert.IsNotNull(productLoaded);

                if( product != null )
                {
                    Assert.AreEqual(product.Id, productLoaded.Id);
                    Assert.AreEqual(product.Name, productLoaded.Name);
                    Assert.AreEqual(product.Price, productLoaded.Price);
                    Assert.AreEqual(product.Remain, productLoaded.Remain);
                    Assert.AreEqual(product.ImageUrl, productLoaded.ImageUrl);
                    Assert.AreEqual(product.Description, productLoaded.Description);

                }
            }
            else
            {
                Assert.Fail("Add Product Fail.");
            }
        }


        [TestMethod]
        public void TestDeleteProductById()
        {
            Product product = new Product(
                     "Nokia 3310",
                     111,
                     "http://p1-news.yamedia.tw/NTAyMjY3bmV3cw==/55950483e199d61f.jpg",
                     "地表最強手機!!!!!!",
                     99
                );
            product.Save();

            Product productLoaded = Product.GetProductById(product.Id);

            Assert.IsNotNull(productLoaded);
            Assert.IsTrue(productLoaded.DeleteProductById(product.Id));
        }
    }
}
