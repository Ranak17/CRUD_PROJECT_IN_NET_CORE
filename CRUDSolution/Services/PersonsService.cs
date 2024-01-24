using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        //private readonly List<Person> _persons;
        private readonly IPersonsRepository _personRepository;
        private readonly ILogger<PersonsService> _logger;

        public PersonsService(IPersonsRepository personRepository,ILogger<PersonsService> logger)
        {
            _personRepository=personRepository;
            _logger=logger;
        }

        public async Task<PersonResponse?> AddPerson(PersonAddRequest? personAddRequest)
        {
            _logger.LogInformation("AddPerson Service Execution started");
            if (personAddRequest == null) return null;
            //Model Validation
            ValidationHelper.ModelValidation(personAddRequest);

            if (personAddRequest.PersonName == null) throw new ArgumentException();
            Person person = personAddRequest.ToPerson();
            person.PersonID = Guid.NewGuid();
            await _personRepository.AddPerson(person);
            _logger.LogInformation("AddPerson Service Execution finished");
            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
            //Include() -  Navigation property.  "Country" is the property name in Person Class is called naviagation property
            IList<Person> personsFromDB = await _personRepository.GetAllPersons();
            return personsFromDB.Select(temp => temp.ToPersonResponse()).ToList();
        }

        public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        {
            if (personID == null) return null;
            //Include() -> Navigation Property
            Person? personFromDB = await _personRepository.GetPersonByPersonID(personID.Value);
            return personFromDB?.ToPersonResponse() ;
        }

        public async Task<PersonResponse?> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null) return null;
            ValidationHelper.ModelValidation(personUpdateRequest);
            Person? matchingPerson = await _personRepository.GetPersonByPersonID(personUpdateRequest.PersonID);
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

        public async Task<bool?> DeletePerson(Guid? personID)
        {
            if (personID == null) return null;
            Person? person = await _personRepository.GetPersonByPersonID(personID.Value);
            if (person == null) return false;
            await _personRepository.DeletePersonByPersonID(personID.Value);
            return true;
        }
    }
}
