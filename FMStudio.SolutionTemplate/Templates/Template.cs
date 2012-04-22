using System;
using System.Globalization;
using System.IO;
using FMStudio.Applications.Documents;
using FMStudio.SolutionTemplate.Properties;

namespace FMStudio.SolutionTemplate.Templates
{
    public abstract class Template : ITemplate
    {
        private string name;
        private string description;
        private string category;

        public Template(string name, string description, string category)
        {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentException("name"); }
            if (string.IsNullOrEmpty(description)) { throw new ArgumentException("description"); }
            if (string.IsNullOrEmpty(category)) { throw new ArgumentException("category"); }

            this.name = name;
            this.description = description;
            this.category = category;
        }


        public string Name { get { return this.name; } }

        public string Description { get { return this.description; } }

        public string Category { get { return this.category; } }


        public virtual bool CanNewSolution() { return false; }

        public SolutionDocument NewSolution(string fullFilePath)
        {
            if (!CanNewSolution()) { throw new NotSupportedException("The NewSolution operation is not supported. CanNewSolution returned false."); }
            if (string.IsNullOrEmpty(fullFilePath)) 
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.NewSolutionPathInvalid,
                    Path.GetFileNameWithoutExtension(fullFilePath)));
            }
            if (Directory.Exists(Path.GetDirectoryName(fullFilePath)))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.SolutionAlreadyExisted, 
                    Path.GetFileNameWithoutExtension(fullFilePath)));
            }

            SolutionDocument solutionDocument = NewCore(fullFilePath);

            return solutionDocument;
        }


        protected virtual SolutionDocument NewCore(string fullFilePath)
        {
            throw new NotSupportedException();
        }
    }
}
