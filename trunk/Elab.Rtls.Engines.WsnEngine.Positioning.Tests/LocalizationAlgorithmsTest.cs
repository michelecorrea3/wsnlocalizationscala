using Elab.Rtls.Engines.WsnEngine.Positioning;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using DatabaseConnection;
using Elab.Rtls.Engines.WsnEngine.Positioning;
using System.Collections.Generic;

namespace Elab.Rtls.Engines.WsnEngine.Positioning.Tests
{


    /// <summary>
    ///This is a test class for CentroidLocalizationTest and is intended
    ///to contain all CentroidLocalizationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LocalizationAlgorithmsTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for CalculatePosition
        ///</summary>
        [TestMethod()]
        public void CentroidLocalizationCalculatePositionTest()
        {
            MySQLClass MyDB = new MySQLClass("DRIVER={MySQL ODBC 3.51 Driver};SERVER=localhost;DATABASE=senseless;UID=root;PASSWORD=admin;OPTION=3;");

            Node BlindNode = new Node("Test", MyDB);
            BlindNode.NewAnchor("1", 50.00, 0, 0);
            BlindNode.NewAnchor("2", 50.00, 100, 0);
            BlindNode.NewAnchor("3", 50.00, 50, 60);

            Point expected = new Point(50, 20); // TODO: Initialize to an appropriate value
            Point actual;
            actual = CentroidLocalization.CalculatePosition(BlindNode);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CalculatePosition
        ///</summary>
        [TestMethod()]
        public void MinMaxTest()
        {
            MySQLClass MyDB = new MySQLClass("DRIVER={MySQL ODBC 3.51 Driver};SERVER=localhost;DATABASE=senseless;UID=root;PASSWORD=admin;OPTION=3;");

            Node BlindNode = new Node("Test", MyDB);
            BlindNode.NewAnchor("1", 50.00, 0.00, 0.00);
            BlindNode.NewAnchor("2", 50.00, 2.00, 0.00);
            BlindNode.NewAnchor("3", 50.00, 2.00, 2.00);
            //BlindNode.NewAnchor("1", 50.00, 100, 100);

            Point expected = new Point(1, 1); 
            Node.FilterMethod filterMethod = RangeBasedPositioning.AverageFilter; 

            Point actual;

            actual = MinMax.CalculatePosition(BlindNode, filterMethod);
            Assert.AreEqual(expected.x, actual.x);
            Assert.AreEqual(expected.y, actual.y);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }


        /// <summary>
        ///A test for CalculatePosition
        ///</summary>
        [TestMethod()]
        public void ClusteredTriLaterationTest()
        {
            MySQLClass MyDB = new MySQLClass("DRIVER={MySQL ODBC 3.51 Driver};SERVER=localhost;DATABASE=senseless;UID=root;PASSWORD=admin;OPTION=3;");

            Node BlindNode = new Node("Test", MyDB);
            BlindNode.NewAnchor("1", 50.00, 0.00, 0.00);
            BlindNode.NewAnchor("2", 50.00, 2.00, 0.00);
            BlindNode.NewAnchor("3", 50.00, 2.00, 2.00);

            Node.FilterMethod filterMethod = RangeBasedPositioning.AverageFilter; 
            Point expected = new Point(1,1); 

            Point actual;

            actual = ClusterTrilateration.CalculatePosition(BlindNode, filterMethod);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
