namespace GeneticProgramming.Core.Programs
{
    public interface IProgramNode
    {
        double CurrentValue { get; }

        bool RequiresArgument();
    }
}
