using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;

        public PersonsService()
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();
        }

        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countriesService.GetCountryByCountryID(person.CountryID)?.CountryName;
            return personResponse;
        }
        public PersonResponse? AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest == null) return null;
            //Model Validation
            ValidationHelper.ModelValidation(personAddRequest);

            if (personAddRequest.PersonName == null) throw new ArgumentException();
            Person person = personAddRequest.ToPerson();
            person.PersonID = Guid.NewGuid();
            _persons.Add(person);
            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetAllPersons()
        {
            return _persons.Select(temp => temp.ToPersonResponse()).ToList();
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            if (personID == null) return null;
            return _persons.FirstOrDefault(p => p.PersonID == personID)?.ToPersonResponse(); ;
        }

        public PersonResponse? UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null) return null;
            ValidationHelper.ModelValidation(personUpdateRequest);
            Person? matchingPerson = _persons.FirstOrDefault(temp => temp.PersonID == personUpdateRequest.PersonID);
            if (matchingPerson == null)
            {
                throw new ArgumentException("Invalid Person Id");
            }

            //update all details
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;

            return matchingPerson.ToPersonResponse();
        }

        public bool? DeletePerson(Guid? personID)
        {
            if (personID == null) return null;
            Person? person = _persons.FirstOrDefault(temp => temp.PersonID == personID);
            if (person == null) return false;
            _persons.RemoveAll(temp => temp.PersonID == personID);
            return true;
        }
    }
}
