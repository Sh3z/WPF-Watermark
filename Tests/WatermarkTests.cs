using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Windows.Documents;

namespace Watermark.Tests
{
    /// <summary>
    /// Summary description for WatermarkTests
    /// </summary>
    [TestClass]
    public class WatermarkTests
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
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void Test_GetWatermark_NullElement()
        {
            object watermark = Watermark.GetWatermark( null );
        }

        [TestMethod]
        public void Test_GetWatermark_UnsetValue()
        {
            FrameworkElement e = new FrameworkElement();
            object watermark = Watermark.GetWatermark( e );
            Assert.IsNull( watermark );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void Test_SetWatermark_NullElement()
        {
            Watermark.SetWatermark( null, new object() );
        }

        [TestMethod]
        public void Test_SetWatermark()
        {
            object watermark = new object();
            FrameworkElement e = new FrameworkElement();
            Watermark.SetWatermark( e, watermark );
            object retrievedWatermark = Watermark.GetWatermark( e );
            Assert.AreEqual( watermark, retrievedWatermark );

            watermark = null;
            Watermark.SetWatermark( e, watermark );
            retrievedWatermark = Watermark.GetWatermark( e );
            Assert.AreEqual( watermark, retrievedWatermark );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void Test_GetWatermarkVisibility_NullElement()
        {
            Visibility v = Watermark.GetWatermarkVisibility( null );
        }

        [TestMethod]
        public void Test_GetWatermarkVisibility_UnsetValue()
        {
            FrameworkElement e = new FrameworkElement();
            Visibility v = Watermark.GetWatermarkVisibility( e );
            Assert.AreEqual( Visibility.Hidden, v );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void Test_SetWatermarkVisibility_NullElement()
        {
            Watermark.SetWatermarkVisibility( null, Visibility.Hidden );
        }

        [TestMethod]
        public void Test_SetWatermarkVisibility()
        {
            FrameworkElement e = new FrameworkElement();
            Visibility visibility = Visibility.Collapsed;
            Watermark.SetWatermarkVisibility( e, visibility );
            Visibility retrievedVisibility = Watermark.GetWatermarkVisibility( e );
            Assert.AreEqual( visibility, retrievedVisibility );
        }

        [TestMethod]
        public void Test_GetWatermarkAdorner_NullElement()
        {
            WatermarkAdorner a = Watermark.AdornerForElement( null );
            Assert.IsNull( a );
        }

        [TestMethod]
        public void Test_GetWatermarkAdorner_UnsetElement()
        {
            FrameworkElement e = new FrameworkElement();
            WatermarkAdorner a = Watermark.AdornerForElement( e );
            Assert.IsNull( a );
        }

        [TestMethod]
        public void Test_GetWatermarkAdorner()
        {
            FrameworkElement e = new FrameworkElement();
            AdornerDecorator d = new AdornerDecorator();
            d.Child = e;
            Watermark.SetWatermark( e, new object() );
            WatermarkAdorner a = Watermark.AdornerForElement( e );
            Assert.IsNotNull( a );
        }
    }
}
