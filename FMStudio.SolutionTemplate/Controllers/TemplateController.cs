using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using BigEgg.Framework.Applications.Services;
using FMStudio.SolutionTemplate.Services;
using System.Collections.ObjectModel;
using FMStudio.SolutionTemplate.Templates;
using FMStudio.SolutionTemplate.Properties;

namespace FMStudio.SolutionTemplate.Controllers
{
    /// <summary>
    /// Responsible to synchronize template.
    /// </summary>
    [Export]
    public class TemplateController
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

        }

        #region Properties
        private List<ITemplate> Templates { get { return this.templateService.Templates; } }

        private List<ITemplateCategory> TemplateCategories { get { return this.templateService.TemplateCategories; } }

        private ITemplate SelectTemplate
        {
            get { return this.templateService.SelectTemplate; }
            set { this.templateService.SelectTemplate = value; }
        }
        #endregion

        #region Public Methods
        public void Initialize()
        {
            TemplateCategories.Add(new TemplateCategory(Resources.CommonCategoryName));

            RegisterTemplate(new EmptyTemplate());

            if (Settings.Default.SelectTemplate != null)
                SelectTemplate = Settings.Default.SelectTemplate;
            else
                SelectTemplate = Templates.First();
        }

        internal void RegisterTemplate(ITemplate template)
        {
            bool contains = false;
            foreach (TemplateCategory categories in TemplateCategories)
            {
                if (categories.Name == template.Category) 
                { 
                    contains = true;
                    break;
                }
            }
            if (!contains)
                throw new ArgumentException("Template category is not an item of the Template Categories collection.");

            if (Templates.Contains(template))
                throw new ArgumentException("Template had already added into the Templates collection.");

            contains = false;
            foreach (ITemplate temp in Templates)
            {
                if (temp.Name == template.Name)
                {
                    contains = true;
                    break;
                }
            }
            if (!contains)
                throw new ArgumentException("Template with the same name had already add into the Templates collection.");

            Templates.Add(template);
        }

        internal void RegisterTemplateCategory(string name, ITemplateCategory templateCategoryParent = null)
        {
            RegisterTemplateCategory(new TemplateCategory(name, templateCategoryParent));
        }

        internal void RegisterTemplateCategory(ITemplateCategory templateCategory)
        {
            bool contains = false;
            foreach (TemplateCategory categories in TemplateCategories)
            {
                if (categories.Name == templateCategory.Name)
                {
                    contains = true;
                    break;
                }
            }
            if (!contains)
                throw new ArgumentException("Template Category with the same name had already add into the Template Categories collection.");


            if (templateCategory.Parent != null) 
                if (TemplateCategories.Contains(templateCategory.Parent))
                    throw new ArgumentException("Template Category's parent not an item of the Templates Category collection.");

            TemplateCategories.Add(templateCategory);
        }
        
        public void Shutdown()
        {
            Settings.Default.SelectTemplate = SelectTemplate;
            Settings.Default.Save();
        }
        #endregion
    }
}
