using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Windows.Documents;

namespace Watermark.Tests
{
    /// <summary>
    /// Summary description for WatermarkAdornerTests
    /// </summary>
    [TestClass]
    public class WatermarkAdornerTests
    {
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get;
            set;
        }


        [TestMethod]
        public void Test_Constructor_WatermarkValue()
        {
            FrameworkElement e = new FrameworkElement();
            WatermarkAdorner a = new WatermarkAdorner( e );
            Assert.IsNull( a.WatermarkContent );

            object watermark = new object();
            a = new WatermarkAdorner( e, watermark );
            Assert.AreEqual( watermark, a.WatermarkContent );
        }

        [TestMethod]
        public void Test_WatermarkContentUpdated()
        {
            AdornerDecorator d = new AdornerDecorator();
            FrameworkElement e = new FrameworkElement();
            d.Child = e;
            object watermark = new object();
            Watermark.SetWatermark( e, watermark );
            WatermarkAdorner a = Watermark.AdornerForElement( e );
            Assert.AreEqual( watermark, a.WatermarkContent );

            watermark = new object();
            Watermark.SetWatermark( e, watermark );
            Assert.AreEqual( watermark, a.WatermarkContent );
        }
    }
}
