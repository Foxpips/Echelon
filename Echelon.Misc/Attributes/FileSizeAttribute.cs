using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Echelon.Misc.Attributes
{
    public class FileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxSize;

        public FileSizeAttribute(int maxSize)
        {
            _maxSize = maxSize;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            return (value as HttpPostedFileBase)?.ContentLength <= _maxSize;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The file size should not exceed {_maxSize}";
        }
    }
}