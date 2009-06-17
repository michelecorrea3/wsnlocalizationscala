namespace Elab.Rtls.Engines.WsnEngine.Positioning.Tests
{
    using DatabaseConnection;
    using Elab.Rtls.Engines.WsnEngine.Positioning;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///This is a test class for CentroidLocalizationTest and is intended
    ///to contain all CentroidLocalizationTest Unit Tests
    ///</summary>
    [TestClass]
    public class LocalizationAlgorithmsTest
    {
        #region Fields

        private TestContext testContextInstance;

        #endregion Fields

        #region Properties

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

        #endregion Properties

        #region Methods

        /// <summary>
        ///A test for CalculatePosition
        ///</summary>
        [TestMethod]
        public void ClusteredTriLaterationTest()
        {
            MySQLClass MyDB = new MySQLClass("DRIVER={MySQL ODBC 3.51 Driver};SERVER=localhost;DATABASE=senseless;UID=root;PASSWORD=admin;OPTION=3;");

            Node BlindNode = new Node("Test", MyDB);
            BlindNode.NewAnchor("1", 50.00, 0.00, 0.00, 1);
            BlindNode.NewAnchor("2", 50.00, 2.00, 0.00, 1);
            BlindNode.NewAnchor("3", 50.00, 2.00, 2.00, 1);

            Node.FilterMethod filterMethod = RangeBasedPositioning.AverageFilter;
            Point expected = new Point(1,1);

            Point actual;

            actual = ClusterTrilateration.CalculatePosition(BlindNode, filterMethod, false);
            //Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /*
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
         */
        /// <summary>
        ///A test for CalculatePosition of MinMax
        ///</summary>
        [TestMethod]
        public void MinMaxTest()
        {
            MySQLClass MyDB = new MySQLClass("DRIVER={MySQL ODBC 3.51 Driver};SERVER=localhost;DATABASE=senseless;UID=root;PASSWORD=admin;OPTION=3;");

            Node BlindNode = new Node("Test", MyDB);
            BlindNode.NewAnchor("1", 50.00, 0.00, 0.00, 1);
            BlindNode.NewAnchor("2", 50.00, 15.00, 0.00, 1);
            BlindNode.NewAnchor("3", 50.00, 15.00, 22.00, 1);
            BlindNode.NewAnchor("4", 50.00, 15.00, 15.00, 2);

            //BlindNode.NewAnchor("1", 50.00, 100, 100);

            Point expected = new Point(1, 1);
            Node.FilterMethod filterMethod = RangeBasedPositioning.AverageFilter;

            Point actual;

            actual = MinMax.CalculatePosition(BlindNode, filterMethod, true);
            Assert.AreEqual(expected.x, actual.x);
            Assert.AreEqual(expected.y, actual.y);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        #endregion Methods

        #region Other

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

        #endregion Other
    }
}