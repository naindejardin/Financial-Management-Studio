using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using BigEgg.Framework.Applications;
using FMStudio.SolutionTemplate.Properties;
using FMStudio.SolutionTemplate.Services;
using FMStudio.SolutionTemplate.Templates;

namespace FMStudio.SolutionTemplate.Controllers
{
    /// <summary>
    /// Responsible to synchronize template.
    /// </summary>
    [Export]
    public class TemplateController : Controller
    {
        #region Members
        private readonly CompositionContainer container;
        private readonly ITemplateService templateService;
        #endregion

        [ImportingConstructor]
        public TemplateController(CompositionContainer container, ITemplateService templateService)
        {
            this.container = container;
            this.templateService = templateService;

            IsInitialized = false;
        }

        #region Properties
        private IList<ITemplate> Templates { get { return this.templateService.AllTemplates; } }

        private IList<TemplateCategory> TemplateCategories { get { return this.templateService.TemplateCategories; } }

        private ITemplate SelectTemplate
        {
            get { return this.templateService.SelectedTemplate; }
            set { this.templateService.SelectedTemplate = value; }
        }

        public bool IsInitialized { get; set; }
        #endregion

        #region Public Methods
        public void Initialize()
        {
            if (IsInitialized)
                return;

            RegisterTemplateCategory(Resources.CommonCategoryName);
            RegisterTemplateCategory(Resources.PrivateCategoryName, Resources.CommonCategoryName);

            RegisterTemplate(new EmptyTemplate());
            RegisterTemplate(new PersonalBillListTemplate());

            if (Settings.Default.SelectedTemplate != null)
                SelectTemplate = Settings.Default.SelectedTemplate;
            else
                SelectTemplate = Templates.First();

            if (Settings.Default.SelectedCategory != null)
                templateService.SelectedCategory = Settings.Default.SelectedCategory;
            else
                templateService.SelectedCategory = templateService.RootTemplateCategories.First();

            IsInitialized = true;
        }

        internal void RegisterTemplate(ITemplate template)
        {
            bool containsFlag = false;
            foreach (TemplateCategory categories in TemplateCategories)
            {
                if (categories.Name == template.Category) 
                { 
                    containsFlag = true;
                    break;
                }
            }
            if (!containsFlag)
                throw new ArgumentException("Template category is not an item of the Template Categories collection.");

            if (Templates.Contains(template))
                throw new ArgumentException("Template had already added into the Templates collection.");

            foreach (ITemplate temp in Templates)
            {
                if (temp.Name == template.Name)
                {
                    throw new ArgumentException("Template with the same name had already add into the Templates collection.");
                }
            }

            Templates.Add(template);
        }

        internal void RegisterTemplateCategory(string categoryName, string parentName = null)
        {
            foreach (TemplateCategory categories in TemplateCategories)
            {
                if (categories.Name == categoryName)
                {
                    throw new ArgumentException("Template Category with the same name had already add into the Template Categories collection.");
                }
            }

            TemplateCategory newCategory = new TemplateCategory(categoryName);

            if (!string.IsNullOrEmpty(parentName))
            {
                if (!TemplateCategories.Any()) { throw new ArgumentException("Template Category's parent not an item of the Templates Category collection."); }

                try
                {
                    TemplateCategory parentCategory = TemplateCategories.First(c => c.Name == parentName);

                    newCategory.Level = parentCategory.Level + 1;
                    parentCategory.Children.Add(newCategory);
                }
                catch (IndexOutOfRangeException)
                {
                    throw new IndexOutOfRangeException("Only support 3 level categories.");
                }
                catch
                {
                    throw new ArgumentException("Template Category's parent not an item of the Templates Category collection.");
                }
            }
            TemplateCategories.Add(newCategory);
        }
        
        public void Shutdown()
        {
            Settings.Default.SelectedTemplate = SelectTemplate;
            Settings.Default.SelectedCategory = templateService.SelectedCategory;
            Settings.Default.Save();
        }
        #endregion
    }
}
