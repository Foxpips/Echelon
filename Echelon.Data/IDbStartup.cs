namespace Echelon.Data
{
    public interface IDbStartup
    {
        void ExecuteInternal(object store);
    }
}