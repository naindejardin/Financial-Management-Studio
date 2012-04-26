using System;
using System.Collections.Generic;

namespace FMStudio.SolutionTemplate.Templates
{
    public class TemplateCategory : ITemplateCategory
    {
        private readonly string name;
        private readonly IList<ITemplateCategory> children;
        private int level;

        public TemplateCategory(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentException("name"); }
            this.name = name;
            this.children = new List<ITemplateCategory>();
            this.level = 0;
        }


        public string Name { get { return this.name; } }

        public IList<ITemplateCategory> Children { get { return this.children; } }

        public int Level 
        {
            get { return this.level; }
            set
            {
                if ((value >= 0) && (value < 3))
                {
                    this.level = value;
                }
                else
                    throw new IndexOutOfRangeException();
            }
        }
    }
}
