using FMStudio.Documents;

namespace FMStudio.SolutionTemplate.Templates
{
    public interface ITemplate
    {
        string Name { get; }

        string Description { get; }

        string Category { get; }

        bool CanNewSolution();

        SolutionDocument NewSolution(string fullFilePath);
    }
}
