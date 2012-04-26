using System.Collections.Generic;
using System.ComponentModel;
using FMStudio.SolutionTemplate.Templates;

namespace FMStudio.SolutionTemplate.Services
{
    public interface ITemplateService  : INotifyPropertyChanged
    {
        IList<ITemplate> AllTemplates { get; }

        IList<ITemplate> TemplatesInCategory { get; }

        IList<TemplateCategory> RootTemplateCategories { get; }

        IList<TemplateCategory> TemplateCategories { get; }

        ITemplate SelectedTemplate { get; set; }

        TemplateCategory SelectedCategory { get; set; }
    }
}
