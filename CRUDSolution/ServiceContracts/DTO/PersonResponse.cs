using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public bool? ReceiveNewsLetter { get; set; }
        public double? Age { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(PersonResponse)) return false;
            PersonResponse person = (PersonResponse)obj;
            return person.PersonID == PersonID &&
                    person.PersonName == PersonName &&
                    person.DateOfBirth == DateOfBirth &&
                    person.Email == Email &&
                    person.Address == Address &&
                    person.ReceiveNewsLetter == ReceiveNewsLetter &&
                    person.Gender == Gender &&
                    person.CountryID == CountryID;

        }

        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonID = PersonID,
                PersonName = PersonName,
                DateOfBirth = DateOfBirth,
                Gender = (GenderOptions)(Enum.Parse(typeof(GenderOptions), Gender, true)),
                Email = Email,
                Address = Address,
                CountryID = CountryID,
                ReceiveNewsLetter = ReceiveNewsLetter

            };
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class PersonExtensions
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonID = person.PersonID,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Address = person.Address,
                ReceiveNewsLetter = person.ReceiveNewsLetter,
                Gender = person.Gender,
                CountryID = person.CountryID,
                Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null
            };
        }
    }
}
