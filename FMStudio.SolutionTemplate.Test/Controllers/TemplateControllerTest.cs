using System;
using System.Linq;
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

            Assert.AreEqual(2, templateService.AllTemplates.Count);
            Assert.AreEqual(1, templateService.RootTemplateCategories.Count);
            Assert.AreEqual(2, templateService.TemplateCategories.Count);
            Assert.IsNotNull(templateService.SelectedTemplate);
            Assert.IsNotNull(templateService.SelectedCategory);
        }

        [TestMethod()]
        public void RegisterTemplateTest()
        {
            TemplateController templateController = Container.GetExportedValue<TemplateController>();
            ITemplateService templateService = Container.GetExportedValue<ITemplateService>();

            MockTemplate template = new MockTemplate("Test", "Test Description", "Common");
            templateController.RegisterTemplate(template);
            Assert.AreEqual(3, templateService.AllTemplates.Count);

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

            AssertHelper.ExpectedException<ArgumentException>(() => templateController.RegisterTemplateCategory("Common"));
            AssertHelper.ExpectedException<ArgumentException>(() =>
                templateController.RegisterTemplateCategory("NewCategory", "AnotherCategory"));
            
            templateController.RegisterTemplateCategory("CategoryA");
            templateController.RegisterTemplateCategory("CategoryB", "CategoryA");
            Assert.AreEqual(2, templateService.RootTemplateCategories.Count);

            ITemplateCategory category = templateService.RootTemplateCategories.First(c => c.Name == "CategoryA");
            Assert.AreEqual(0, category.Level);
            Assert.AreEqual(1, category.Children.First().Level);
            Assert.AreEqual("CategoryB", category.Children.First().Name);

            templateController.RegisterTemplateCategory("CategoryC", "CategoryB");
            Assert.AreEqual("CategoryC", category.Children.First().Children.First().Name);

            AssertHelper.ExpectedException<IndexOutOfRangeException>(() => templateController.RegisterTemplateCategory("CategoryD", "CategoryC"));
        }
    }
}
