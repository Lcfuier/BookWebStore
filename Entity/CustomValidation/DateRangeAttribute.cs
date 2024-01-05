using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.CustomValidation
{
    public class DateRangeAttribute : ValidationAttribute
    {
        private readonly DateTime _MinDate;
        private readonly DateTime _MaxDate;
        public DateRangeAttribute()
        {
            _MinDate = new DateTime(1990, 1, 1);
            _MaxDate = DateTime.Now;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateValue)
            {
                if (dateValue < _MinDate || dateValue > _MaxDate)
                {
                    return new ValidationResult(ErrorMessage);
                }

                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}
