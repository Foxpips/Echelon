using System.Text;

namespace Echelon.Core.Infrastructure.Services.Email.Components
{
    public class EmailTokenHelper : ITokenHelper
    {
        public string Replace(object tokens, string content)
        {
            var type = tokens.GetType();
            var propertyInfos = type.GetProperties();
            var stringBuilder = new StringBuilder(content);
            foreach (var propertyInfo in propertyInfos)
            {
                var token = propertyInfo.Name;
                var value = propertyInfo.GetValue(tokens);
                stringBuilder.Replace("{{" + token + "}}", value.ToString());
            }

            return stringBuilder.ToString();
        }
    }
}