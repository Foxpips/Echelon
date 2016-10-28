using System.Linq;
using System.Security.Cryptography;
using System.Text;
using static System.String;

namespace Echelon.Core.Helpers
{
    public class HashHelper
    {
        public static string CreateHash(string input)
        {
            using (var sha1Managed = new SHA1Managed())
            {
                var computeHash = sha1Managed.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Concat(computeHash.Select(b => b.ToString("x2")));
            }
        }
    }
}