using Sat.Recruitment.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sat.Recruitment.Dto.Requests
{
    public class CreateUserRequest : IValidatableObject
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string UserType { get; set; }

        [Required]
        public decimal Money { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            UserType myUserType = Shared.UserType.Normal;
            if (!Enum.TryParse<UserType>(UserType, out myUserType))
            {
                results.Add(new ValidationResult($"The UserType: {UserType}, is not recognized."));
            }

            return results;
        }
    }
}
