﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace team3
{
    /// <summary>
    /// CategoryTest 的摘要描述
    /// </summary>
    [TestClass]
    public class CategoryTest
    {
        [TestInitialize]
        public void Init()
        {
            DatabaseConnection.Init();
        }

        public CategoryTest()
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
        public void TestAddCategory()
        {
            Category category = new Category( "ABC" );
            Assert.IsTrue(category.Save());
            Category.Remove("ABC");
        }

        [TestMethod]
        public void TestRemoveCategory()
        {
            Category category = new Category("123");
            category.Save();
            Assert.IsTrue(Category.Remove("123"));
            Assert.IsFalse(Category.Remove("123"));
        }

        [TestMethod]
        public void TestGetCategoryById()
        {
            Category category = new Category( "手機" );

            bool result = category.Save();

            if (result)
            {
                Category categoryLoaded = Category.GetCategoryById(category.Id);

                Assert.IsNotNull(categoryLoaded);

                if (category != null)
                {
                    Assert.AreEqual(category.Id, categoryLoaded.Id);
                    Assert.AreEqual(category.Name, categoryLoaded.Name);
                }
            }
            else
            {
                Assert.Fail("Get Category Fail.");
            }
            Category.Remove("手機");
            Assert.IsNull(Category.GetCategoryById(category.Id));
        }
        
        
        [TestMethod]
        public void TestGetCategoryByName()
        {
            Category category = new Category("手機");

            bool result = category.Save();

            if (result)
            {
                Category categoryLoaded = Category.GetCategoryByName(category.Name);

                Assert.IsNotNull(categoryLoaded);

                if (category != null)
                {
                    Assert.AreEqual(category.Id, categoryLoaded.Id);
                    Assert.AreEqual(category.Name, categoryLoaded.Name);
                }
            }
            else
            {
                Assert.Fail("Get Category Fail.");
            }
            Category.Remove("手機");
            Assert.IsNull(Category.GetCategoryByName("手機"));
        }

        [TestMethod]
        public void TestAddCategoryLink()
        {
            CategoryLink category_link = new CategoryLink(2, 3);
            Assert.IsTrue(category_link.Save());
        }

        /*
        [TestMethod]
        public void TestGetProductListByCategory()
        {
            List<long> product_list = CategoryLink.GetProductListByCategory(3);
            List<long> test_data = new List<long> { 2 };
            CollectionAssert.AreEqual(test_data, product_list);
        }*/
        
    }
}
