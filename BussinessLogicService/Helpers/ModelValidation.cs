using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicService.Helpers
{
    internal class ModelValidation
    {
        internal static void ValidateRequest<T>(T items)
        {
            if(items == null)
                throw new ArgumentNullException(nameof(items), "Request cannot be null");
            ValidationContext validationContext = new ValidationContext(items);

            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(items, validationContext, validationResults, true);
            if (!isValid)
            {
                var errorMessage = validationResults.FirstOrDefault()?.ErrorMessage;
                throw new ValidationException(errorMessage);
            }
        }
        internal static void ValidateId(Guid? Id)
        {
            if(Id == null)
                throw new ArgumentNullException(nameof(Id), "Id cannot be null");
            if (Id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty", nameof(Id));
        }

    }
}
