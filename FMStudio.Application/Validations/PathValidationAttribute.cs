using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace FMStudio.Applications.Validations
{
    [AttributeUsage(AttributeTargets.Property |
      AttributeTargets.Field, AllowMultiple = false)]
    sealed public class PathCheckValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string path = value as String;
            if (String.IsNullOrWhiteSpace(path))
                return false;

            return Path.IsPathRooted(path);
        }
    }
}
