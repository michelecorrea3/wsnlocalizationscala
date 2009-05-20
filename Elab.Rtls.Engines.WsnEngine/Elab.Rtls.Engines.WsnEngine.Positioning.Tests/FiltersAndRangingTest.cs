namespace Elab.Rtls.Engines.WsnEngine.Positioning.Tests
{
    using System.Collections.Generic;

    using Elab.Rtls.Engines.WsnEngine.Positioning;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///This is a test class for RangeBasedPositioningTest and is intended
    ///to contain all RangeBasedPositioningTest Unit Tests
    ///</summary>
    [TestClass]
    public class FiltersAndRangingTest
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
        ///A test for MedianFilter
        ///</summary>
        [TestMethod]
        public void MedianFilterTest()
        {
            Queue<double> RSS = new Queue<double>(); // TODO: Initialize to an appropriate value
            RSS.Enqueue(10.00);
            RSS.Enqueue(10.00);
            RSS.Enqueue(20.00);
            RSS.Enqueue(25.00);
            RSS.Enqueue(30.00);
            RSS.Enqueue(30.00);
            double expected = 20.00; // TODO: Initialize to an appropriate value
            double actual;
            actual = RangeBasedPositioning.MedianFilter(RSS);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Ranging
        ///</summary>
        [TestMethod]
        public void RangingTest()
        {
            double fRSS = -94.00; // TODO: Initialize to an appropriate value
            double expected = 2.00; // TODO: Initialize to an appropriate value
            double actual;
            actual = RangeBasedPositioning.Ranging(fRSS);
            Assert.AreEqual(expected, actual);
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