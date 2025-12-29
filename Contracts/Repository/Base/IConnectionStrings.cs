namespace Contracts.Repository.Base
{
    public interface IConnectionStrings
    {
        IMSSQLConnection Default { get; }
        IMSSQLConnection Log { get; }
    }
}