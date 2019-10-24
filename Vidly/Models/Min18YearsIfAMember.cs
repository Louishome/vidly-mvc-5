using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Vidly.Models.MembershipType;

namespace Vidly.Models
{
    public class Min18YearsIfAMember : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var customer = (Customer) validationContext.ObjectInstance;
            if (customer.MembershipTypeId == (byte) MembershipTypeEnum.Unknown || customer.MembershipTypeId == (byte) MembershipTypeEnum.PayAsYouGo)
            {
                return ValidationResult.Success;
            }

            if (customer.BirthDay == null)
            {

                return new ValidationResult("Birthday is required.");
            }

            var age = DateTime.Today.Year - customer.BirthDay.Value.Year;
            return (age >= 18)
                ? ValidationResult.Success
                : new ValidationResult("Customer should be at least 18 years old to go on membership");
        }
    }
}