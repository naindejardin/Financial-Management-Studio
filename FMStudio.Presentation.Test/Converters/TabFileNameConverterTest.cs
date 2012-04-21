using System;
using System.Windows;
using BigEgg.Framework.UnitTesting;
using FMStudio.Presentation.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Writer.Presentation.Test.Converters
{
    [TestClass]
    public class TabFileNameConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            TabFileNameConverter converter = TabFileNameConverter.Default;

            Assert.AreEqual("Document 1", 
                converter.Convert(new object[] { "Document 1.rtf", false }, null, null, null));
            Assert.AreEqual("Document 1*",
                converter.Convert(new object[] { "Document 1.rtf", true }, null, null, null));
            Assert.AreEqual("This is a document with a very long f...",
                converter.Convert(new object[] { "This is a document with a very long file name.rtf", false }, null, null, null));

            Assert.AreEqual(DependencyProperty.UnsetValue, converter.Convert(new object[] { new object(), new object() }, 
                typeof(string), null, null));

            AssertHelper.ExpectedException<NotImplementedException>(() =>
                converter.ConvertBack(null, null, null, null));
        }
    }
}
