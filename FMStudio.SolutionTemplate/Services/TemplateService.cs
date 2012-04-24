using System.Collections.Generic;
using System.ComponentModel.Composition;
using BigEgg.Framework.Applications;
using FMStudio.SolutionTemplate.Templates;
using System;
using System.Linq;
using System.Collections.ObjectModel;

namespace FMStudio.SolutionTemplate.Services
{
    [Export(typeof(ITemplateService)), Export]
    public class TemplateService : DataModel, ITemplateService
    {
        #region Members
        private readonly List<ITemplate> templates;
        private readonly List<ITemplateCategory> templateCategories;
        private ITemplate selectedTemplate;
        #endregion

        [ImportingConstructor]
        public TemplateService()
        {
            this.templates = new List<ITemplate>();
            this.templateCategories = new List<ITemplateCategory>();
            this.selectedTemplate = null;
        }

        #region Properties
        public List<ITemplate> Templates { get { return this.templates; } }

        public List<ITemplateCategory> TemplateCategories { get { return this.templateCategories; } }

        public ITemplate SelectedTemplate
        {
            get { return this.selectedTemplate; }
            set
            {
                if (this.selectedTemplate != value)
                {
                    if (value != null && !this.templates.Contains(value))
                    {
                        throw new ArgumentException("value is not an item of the Templates collection.");
                    }
                    this.selectedTemplate = value;
                    RaisePropertyChanged("SelectedTemplate");
                }
            }
        }
        #endregion
    }
}
