using LATravelManager.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LATravelManager.Model.Extensions
{
    public static class ValidationCheck
    {
        public static bool IsValid<T>(this T obj, ref List<string> errors)
        {
            //If metadata class type has been passed in that's different from the class to be validated, register the association
            //if (typeof(T) != typeof(U))
            //{
            //    TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(T), typeof(U)), typeof(T));
            //}
            var validationContext = new ValidationContext(obj, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(obj, validationContext, validationResults, true);

            if (validationResults.Count > 0)
                errors = new List<string>();

            foreach (var validationResult in validationResults)
            {
                errors.Add(validationResult.ErrorMessage);
            }

            if (validationResults.Count > 0)
                return false;
            else
                return true;
        }
    }
}