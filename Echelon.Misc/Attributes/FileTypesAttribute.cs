using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace Echelon.Misc.Attributes
{
    public class FileTypesAttribute : ValidationAttribute
    {
        private readonly List<FileTypes> _types;

        public FileTypesAttribute(params FileTypes[] types)
        {
            _types = types.ToList();
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            FileTypes fileExtEnum;
            var fileExtString = Path.GetExtension(((HttpPostedFileBase) value).FileName)?.Substring(1);
            var hasFileType = Enum.TryParse(fileExtString, true, out fileExtEnum);

            return hasFileType && _types.Contains<FileTypes>(fileExtEnum);
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Invalid file type. Only the following types {string.Join(", ", _types)} are supported.";
        }
    }

    public enum FileTypes
    {
        Jpg,
        Jpeg,
        Gif,
        Png,
        Svg
    }
}