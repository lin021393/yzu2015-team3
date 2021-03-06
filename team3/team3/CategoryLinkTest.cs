﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace team3
{
    /// <summary>
    /// UnitTest1 的摘要描述
    /// </summary>
    [TestClass]
    public class CategoryLinkTest
    {
        [TestInitialize]
        public void Init()
        {
            DatabaseConnection.DropTable("categories");
            DatabaseConnection.DropTable("category_link");
            DatabaseConnection.Init();
        }

        public CategoryLinkTest()
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
        public void TestAddCategoryLink()
        {
            Category category = new Category("手機");
            bool result1 = category.Save().Success;

            Product product = new Product(
                     "Nokia 3310",
                     111,
                     "http://p1-news.yamedia.tw/NTAyMjY3bmV3cw==/55950483e199d61f.jpg",
                     "地表最強手機!!!!!!",
                     99
                );
            bool result2 = product.Save();

            CategoryLink category_link = new CategoryLink("Nokia 3310", "手機");
            Assert.IsTrue(category_link.Save().Success);
        }

        [TestMethod]
        public void TestRemoveCategoryLink()
        {
            Category category = new Category("手機");
            bool result1 = category.Save().Success;

            Product product = new Product(
                     "IPHONE 6S",
                     19990,
                     "http://img.technews.tw/wp-content/uploads/2015/06/15233374520_abea3a452a_z-624x416.jpg",
                     "潮到滴水",
                     99
                );
            bool result2 = product.Save();

            CategoryLink category_link = new CategoryLink("IPHONE 6S", "手機");
            category_link.Save();
            Assert.IsTrue(CategoryLink.Remove(product.Id,category.Id).Success);
        }

        [TestMethod]
        public void TestGetProductListByCategory()
        {
            Category category = new Category("ABC");
            bool result1 = category.Save().Success;

            Product product = new Product(
                     "AAA",
                     111,
                     "http://A.jpg",
                     "A",
                     99
                );
            bool result2 = product.Save();

            Product product2 = new Product(
                     "BBB",
                     19990,
                     "http://B.jpg",
                     "B",
                     99
                );
            bool result3 = product2.Save();

            CategoryLink category_link = new CategoryLink("AAA", "ABC");
            category_link.Save();
            CategoryLink category_link2 = new CategoryLink("BBB", "ABC");
            category_link2.Save();

            List<long> product_list = CategoryLink.GetProductListByCategory(category.Id);
            List<long> test_data = new List<long> { product.Id, product2.Id };
            CollectionAssert.AreEqual(test_data, product_list);
        }

        [TestMethod]
        public void TestGetCategoryListByProduct()
        {
            Category category = new Category("ABC");
            bool result = category.Save().Success;

            Category category2 = new Category("C_");
            bool result2 = category2.Save().Success;

            Product product = new Product(
                     "CCC",
                     0,
                     "http://C.jpg",
                     "C",
                     1
                );
            bool result3 = product.Save();

            CategoryLink category_link = new CategoryLink("CCC", "ABC");
            category_link.Save();
            CategoryLink category_link2 = new CategoryLink("CCC", "C_");
            category_link2.Save();

            List<long> category_list = CategoryLink.GetCategoryListByProduct(product.Id);
            List<long> test_data = new List<long> { category.Id, category2.Id };
            CollectionAssert.AreEqual(test_data, category_list);
        }
    }
}
