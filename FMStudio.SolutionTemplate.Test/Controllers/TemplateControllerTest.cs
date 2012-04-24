using System;
using BigEgg.Framework.UnitTesting;
using FMStudio.SolutionTemplate.Controllers;
using FMStudio.SolutionTemplate.Services;
using FMStudio.SolutionTemplate.Templates;
using FMStudio.SolutionTemplate.Test.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FMStudio.SolutionTemplate.Test.Controllers
{
    [TestClass]
    public class TemplateControllerTest : TestClassBase
    {
        [TestMethod]
        public void InitializeTest()
        {
            TemplateController templateController = Container.GetExportedValue<TemplateController>();
            ITemplateService templateService = Container.GetExportedValue<ITemplateService>();

            Assert.IsNotNull(templateController);
            Assert.IsNotNull(templateService);

            Assert.AreEqual(1, templateService.Templates.Count);
            Assert.AreEqual(1, templateService.TemplateCategories.Count);
            Assert.IsNotNull(templateService.SelectedTemplate);
        }

        [TestMethod()]
        public void RegisterTemplateTest()
        {
            TemplateController templateController = Container.GetExportedValue<TemplateController>();
            ITemplateService templateService = Container.GetExportedValue<ITemplateService>();

            MockTemplate template = new MockTemplate("Test", "Test Description", "Common");
            templateController.RegisterTemplate(template);
            Assert.AreEqual(2, templateService.Templates.Count);

            //  Template had already been added.
            template = new MockTemplate("Test", "Test Description", "Common");
            AssertHelper.ExpectedException<ArgumentException>(() => templateController.RegisterTemplate(template));

            //  Category not exist.
            template = new MockTemplate("Test", "Test Description", "AnotherCategory8");
            AssertHelper.ExpectedException<ArgumentException>(() => templateController.RegisterTemplate(template));

            //  Template with same name.
            template = new MockTemplate("Test", "Another Description", "AnotherCategory8");
            AssertHelper.ExpectedException<ArgumentException>(() => templateController.RegisterTemplate(template));
        }


        [TestMethod()]
        public void RegisterTemplateCategoryTest()
        {
            TemplateController templateController = Container.GetExportedValue<TemplateController>();
            ITemplateService templateService = Container.GetExportedValue<ITemplateService>();
            templateController.RegisterTemplateCategory("CategoryA");
            templateController.RegisterTemplateCategory("CategoryB", new TemplateCategory("CategoryA"));
            Assert.AreEqual(3, templateService.TemplateCategories.Count);

            AssertHelper.ExpectedException<ArgumentException>(() => templateController.RegisterTemplateCategory("Common"));

            AssertHelper.ExpectedException<ArgumentException>(() => 
                templateController.RegisterTemplateCategory("NewCategory", new TemplateCategory("AnotherCategory")));
        }
    }
}
