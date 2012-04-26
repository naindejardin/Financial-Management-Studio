using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using BigEgg.Framework.Applications;
using FMStudio.SolutionTemplate.Templates;

namespace FMStudio.SolutionTemplate.Services
{
    [Export(typeof(ITemplateService)), Export]
    public class TemplateService : DataModel, ITemplateService
    {
        #region Members
        private readonly IList<ITemplate> alltemplates;
        private readonly IList<TemplateCategory> templateCategories;
        private ITemplate selectedTemplate;
        private TemplateCategory selectedCategory;
        #endregion

        [ImportingConstructor]
        public TemplateService()
        {
            this.alltemplates = new List<ITemplate>();
            this.templateCategories = new List<TemplateCategory>();
            this.selectedTemplate = null;
            this.selectedCategory = null;
            TemplatesInCategory = AllTemplates;
        }

        #region Properties
        public IList<ITemplate> AllTemplates { get { return this.alltemplates; } }

        public IList<ITemplate> TemplatesInCategory { get; private set; }

        public IList<TemplateCategory> TemplateCategories { get { return this.templateCategories; } }

        public IList<TemplateCategory> RootTemplateCategories 
        { 
            get 
            {
                try
                {
                    return this.templateCategories.Where(c => c.Level == 0).ToList();
                }
                catch
                {
                    return null;
                }
            } 
        }

        public ITemplate SelectedTemplate
        {
            get { return this.selectedTemplate; }
            set
            {
                if (this.selectedTemplate != value)
                {
                    if (value != null && !this.alltemplates.Contains(value))
                    {
                        throw new ArgumentException("value is not an item of the Templates collection.");
                    }
                    this.selectedTemplate = value;
                    RaisePropertyChanged("SelectedTemplate");
                }
            }
        }

        public TemplateCategory SelectedCategory
        {
            get { return this.selectedCategory; }
            set
            {
                if (this.selectedCategory != value)
                {
                    this.selectedCategory = value;
                    SelectedCategoryChanged();
                }
            }
        }
        #endregion

        private void SelectedCategoryChanged()
        {
            List<ITemplate> templateinCategory = new List<ITemplate>();

            templateinCategory.AddRange(GetTemplateInCategory(SelectedCategory));

            foreach (ITemplateCategory childCategory in SelectedCategory.Children)
            {
                templateinCategory.AddRange(GetTemplateInCategory(childCategory));
                if (childCategory.Level == 1)
                {
                    foreach (ITemplateCategory secondLevelChildCategory in childCategory.Children)
                    {
                        templateinCategory.AddRange(GetTemplateInCategory(secondLevelChildCategory));
                    }
                }
            }

            TemplatesInCategory = templateinCategory;
        }

        private IList<ITemplate> GetTemplateInCategory(ITemplateCategory category)
        {
            try
            {
                return AllTemplates.Where(t => t.Category == category.Name).ToList();
            }
            catch
            {
                return new List<ITemplate>();
            }
        }
    }
}
