using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PersonsRepository : IPersonsRepository
    {
        private readonly ApplicationDbContext _db;
        public PersonsRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Person> AddPerson(Person person)
        {
            _db.Persons.Add(person);
            await _db.SaveChangesAsync();
            return person;
        }

        public async Task<bool> DeletePersonByPersonID(Guid personID)
        {
            _db.Persons.RemoveRange(_db.Persons.Where(temp=>temp.PersonID==personID));
            int rowDeleted =await _db.SaveChangesAsync();
            return rowDeleted>0;
        }

        public async Task<IList<Person>> GetAllPersons()
        {
            return await _db.Persons.Include("Country").ToListAsync();
        }

        public async Task<Person?> GetPersonByPersonID(Guid personID)
        {
            return await _db.Persons.Include("Country").FirstOrDefaultAsync(temp => temp.PersonID == personID);
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            Person? matchingperson = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonID == person.PersonID);
            if(matchingperson == null)
                return person;
            matchingperson.PersonName = person.PersonName;
            matchingperson.Email = person.Email;
            matchingperson.DateOfBirth = person.DateOfBirth;
            matchingperson.Gender = person.Gender;
            matchingperson.Address = person.Address;
            matchingperson.ReceiveNewsLetter = person.ReceiveNewsLetter;
            matchingperson.CountryID = person.CountryID;

            int countryUpdated = await _db.SaveChangesAsync();
            return matchingperson;
        
        
        }

    }
}
