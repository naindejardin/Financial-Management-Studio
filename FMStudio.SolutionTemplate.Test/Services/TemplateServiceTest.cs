using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FMStudio.SolutionTemplate.Controllers;
using FMStudio.SolutionTemplate.Services;

namespace FMStudio.SolutionTemplate.Test.Services
{
    [TestClass]
    public class TemplateServiceTest : TestClassBase
    {
        [TestMethod]
        public void CategoryChangeTest()
        {
            TemplateController templateController = Container.GetExportedValue<TemplateController>();
            ITemplateService templateService = Container.GetExportedValue<ITemplateService>();

            Assert.AreEqual(2, templateService.TemplatesInCategory.Count);

            templateService.SelectedCategory = templateService.TemplateCategories.First(c => c.Name == "Private Financial Solution");
            Assert.AreEqual(1, templateService.TemplatesInCategory.Count);
        }
    }
}
