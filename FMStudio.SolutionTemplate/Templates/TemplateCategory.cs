using System;

namespace FMStudio.SolutionTemplate.Templates
{
    public class TemplateCategory : ITemplateCategory
    {
        private string name;
        private ITemplateCategory parent;

        public TemplateCategory(string name, ITemplateCategory parent = null)
        {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentException("name"); }
            this.name = name;
            this.parent = parent;
        }


        public string Name { get { return this.name; } }

        public ITemplateCategory Parent { get { return this.parent; } }

    }
}
