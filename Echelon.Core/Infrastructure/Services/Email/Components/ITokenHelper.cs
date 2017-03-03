namespace Echelon.Core.Infrastructure.Services.Email.Components
{
    public interface ITokenHelper
    {
        string Replace(object tokens, string content);
    }
}