namespace Echelon.Core.Features.Email
{
    public interface ITokenHelper
    {
        string Replace(object tokens, string content);
    }
}