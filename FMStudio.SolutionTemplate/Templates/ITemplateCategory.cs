namespace FMStudio.SolutionTemplate.Templates
{
    public interface ITemplateCategory
    {
        string Name { get;}

        ITemplateCategory Parent { get;} 
    }
}
