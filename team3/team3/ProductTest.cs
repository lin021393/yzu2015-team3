using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
namespace team3
{
    /// <summary>
    /// ProductTest 的摘要描述
    /// </summary>
    [TestClass]
    public class ProductTest
    {
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
        public void ShowProductInfo()
        {
            //    productid = 0;
            //    categoryid = 1;
            //    productname = 2;
            //    productprice = 3;
            //    productimages = 4;
            //    description = 5;
            //    productremain = 6;
            Product info= new Product();
            List<string> DataInfo= new List<string>();
            int productID = 1;//search ID=1 product Info
            DataInfo = info.ShowProduct(productID);//用productID來查商品的欄位資訊

            //productID沒有資料時候傳productID=0 在實作部份取最後一筆資料 
            /*if (DataInfo.Count == 0)
            {
                DataInfo = info.ShowProduct(0);
            }
            */
            Assert.AreEqual("1", DataInfo[0]);
            Assert.AreEqual("3", DataInfo[1]);
            Assert.AreEqual("ASUS ZENBOOK UX305", DataInfo[2]);
            Assert.AreEqual("22900", DataInfo[3]);
            Assert.AreEqual("123.jpg", DataInfo[4]);
            Assert.AreEqual("輕盈靈巧的ZenBook UX305沉穩紮實的陶瓷白或黑曜岩外觀色彩、加上同心圓髮絲紋表面處理，無論在遠處欣賞或近距離接觸，都展現出獨一無二的質感；精巧的鑽石切邊，更在細微處流露精品格調。", DataInfo[5]);
            Assert.AreEqual("50", DataInfo[6]);
        }
        [TestMethod]
        public void AddProductInfo()
        {
            Product info = new Product();
            info.AddProduct(0, 3, "APPLE Macbook Air", 19999, "456.jpg", "APPLE 就是潮", 69);
        }
    }
}
