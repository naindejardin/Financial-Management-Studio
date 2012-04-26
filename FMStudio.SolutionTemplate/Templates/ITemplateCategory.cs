using System.Collections.Generic;

namespace FMStudio.SolutionTemplate.Templates
{
    public interface ITemplateCategory
    {
        string Name { get;}

        IList<ITemplateCategory> Children { get; }

        int Level { get; set; }
    }
}
