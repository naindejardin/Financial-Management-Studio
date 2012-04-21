using System;
using System.Windows;
using BigEgg.Framework.UnitTesting;
using FMStudio.Presentation.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Writer.Presentation.Test.Converters
{
    [TestClass]
    public class BoolToVisibilityConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            BoolToVisibilityConverter converter = BoolToVisibilityConverter.Default;

            Assert.AreEqual(Visibility.Visible, converter.Convert(true, typeof(Visibility), null, null));
            Assert.AreEqual(Visibility.Collapsed, converter.Convert(false, typeof(Visibility), null, null));

            AssertHelper.ExpectedException<NotImplementedException>(() =>
                converter.ConvertBack(null, null, null, null));
        }
    }
}
