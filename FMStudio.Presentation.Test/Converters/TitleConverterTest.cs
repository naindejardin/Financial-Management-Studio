using System;
using System.Globalization;
using System.Windows;
using BigEgg.Framework.UnitTesting;
using FMStudio.Presentation.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FMStudio.Presentation.Test.Converters
{
    [TestClass]
    public class TitleConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            TitleConverter converter = TitleConverter.Default;

            Assert.AreEqual("App Title", converter.Convert(new[] { "App Title", null }, null, null, CultureInfo.InvariantCulture));
            Assert.AreEqual("Document1.rtf - App Title",
                converter.Convert(new[] { "App Title", "Document1.rtf" }, null, null, CultureInfo.InvariantCulture));

            Assert.AreEqual(DependencyProperty.UnsetValue, converter.Convert(null, null, null, null));
            Assert.AreEqual(DependencyProperty.UnsetValue, converter.Convert(new[] { "Wrong" }, null, null, null));
            Assert.AreEqual(DependencyProperty.UnsetValue, converter.Convert(new object[] { 4, 2 }, null, null, null));

            AssertHelper.ExpectedException<NotSupportedException>(() => converter.ConvertBack(null, null, null, null));
        }
    }
}
