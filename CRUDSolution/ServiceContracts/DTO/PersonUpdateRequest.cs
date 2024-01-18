using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class PersonUpdateRequest
    {
        [Required(ErrorMessage ="Person ID Can't be blank")]
        public Guid PersonID { get; set; }

        [Required(ErrorMessage = "Person Name Can't be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email Can't be blank")]
        [EmailAddress(ErrorMessage = "Email should be valid")]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Address { get; set; }
        public bool? ReceiveNewsLetter { get; set; }

        public Person ToPerson()
        {
            return new Person()
            {
                PersonID = PersonID,
                Address = Address,
                CountryID = CountryID,
                DateOfBirth = DateOfBirth,
                Email = Email,
                Gender = Gender.ToString(),
                PersonName = PersonName,
                ReceiveNewsLetter = ReceiveNewsLetter,
            };
        }
    }
}
