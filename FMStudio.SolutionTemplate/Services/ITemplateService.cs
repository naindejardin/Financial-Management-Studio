using System.Collections.Generic;
using System.ComponentModel;
using FMStudio.SolutionTemplate.Templates;

namespace FMStudio.SolutionTemplate.Services
{
    public interface ITemplateService  : INotifyPropertyChanged
    {
        List<ITemplate> Templates { get; }

        List<ITemplateCategory> TemplateCategories { get; }

        ITemplate SelectTemplate { get; set; }
    }
}
