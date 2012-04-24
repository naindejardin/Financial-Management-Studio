using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using FMStudio.SolutionTemplate.Properties;
using FMStudio.SolutionTemplate.Services;
using FMStudio.SolutionTemplate.Templates;

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
            get { return this.templateService.SelectedTemplate; }
            set { this.templateService.SelectedTemplate = value; }
        }
        #endregion

        #region Public Methods
        public void Initialize()
        {
            RegisterTemplateCategory(Resources.CommonCategoryName);

            RegisterTemplate(new EmptyTemplate());

            if (Settings.Default.SelectTemplate != null)
                SelectTemplate = Settings.Default.SelectTemplate;
            else
                SelectTemplate = Templates.First();
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

        internal void RegisterTemplateCategory(string name, ITemplateCategory templateCategoryParent = null)
        {
            RegisterTemplateCategory(new TemplateCategory(name, templateCategoryParent));
        }

        internal void RegisterTemplateCategory(ITemplateCategory templateCategory)
        {
            bool containsFlag = false;
            foreach (TemplateCategory categories in TemplateCategories)
            {
                if (categories.Name == templateCategory.Name)
                {
                    throw new ArgumentException("Template Category with the same name had already add into the Template Categories collection.");
                }
                
                if (templateCategory.Parent != null)
                {
                    if (categories.Name == templateCategory.Parent.Name)
                    {
                        containsFlag = true;
                    }
                }
                else if (templateCategory.Parent == null)
                {
                    containsFlag = true;
                }
            }

            if ((!containsFlag) && (TemplateCategories.Any()))
            {
                throw new ArgumentException("Template Category's parent not an item of the Templates Category collection.");
            }

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
