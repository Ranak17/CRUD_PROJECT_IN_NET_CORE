using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{

    //Repository(or Repository Pattern) is an abstraction between Data Access Layer(EF DbContext) and business logic layer(Service) of the application
    //The biggest difference between the service as well as repository is that repository is the pure representation of the data access logic i.e., its tightly coupled with domain


    /// <summary>
    /// Represents data access logic for managing person entity 
    /// </summary>
    public interface IPersonsRepository
    {
        Task<Person> AddPerson(Person person);
        Task<IList<Person>> GetAllPersons();
        Task<Person> GetPersonByPersonID(Guid personID);

        Task<bool> DeletePersonByPersonID(Guid personID);
        Task<Person> UpdatePerson(Person person);
    }
}
