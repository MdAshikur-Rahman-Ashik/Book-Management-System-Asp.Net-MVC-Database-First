using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookManagement.Models
{
    public class ValidDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime CurrentDate = DateTime.Now;
            string message = "";
            if (Convert.ToDateTime(value) >= CurrentDate)
            {
                return ValidationResult.Success;
            }
            else
            {
                message = "join date connot be less than current date";
                return new ValidationResult(message);
            }
        }
    }
}