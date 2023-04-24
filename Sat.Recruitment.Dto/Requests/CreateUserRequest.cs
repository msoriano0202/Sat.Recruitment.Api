using Sat.Recruitment.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sat.Recruitment.Dto.Requests
{
    public class CreateUserRequest : IValidatableObject
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string UserType { get; set; }
        public decimal Money { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(Name))
            {
                results.Add(new ValidationResult("The name is required."));
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                results.Add(new ValidationResult("The email is required."));
            }

            if (string.IsNullOrWhiteSpace(Address))
            {
                results.Add(new ValidationResult("The address is required."));
            }

            if (string.IsNullOrWhiteSpace(Phone))
            {
                results.Add(new ValidationResult("The phone is required."));
            }

            UserType myUserType = Shared.UserType.Normal;
            if (!Enum.TryParse<UserType>(UserType, out myUserType))
            {
                results.Add(new ValidationResult($"The UserType: {UserType}, is not recognized."));
            }

            return results;
        }
    }
}
