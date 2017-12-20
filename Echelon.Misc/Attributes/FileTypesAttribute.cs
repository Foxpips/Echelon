using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using Echelon.Misc.Enums;

namespace Echelon.Misc.Attributes
{
    public class FileTypesAttribute : ValidationAttribute
    {
        private readonly List<FileType> _types;

        public FileTypesAttribute(params FileType[] type)
        {
            _types = type.ToList();
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            FileType fileExtEnum;
            var fileExtString = Path.GetExtension(((HttpPostedFileBase) value).FileName)?.Substring(1);
            var hasFileType = Enum.TryParse(fileExtString, true, out fileExtEnum);

            return hasFileType && _types.Contains<FileType>(fileExtEnum);
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Invalid file type. Only the following types {string.Join(", ", _types)} are supported.";
        }
    }
}