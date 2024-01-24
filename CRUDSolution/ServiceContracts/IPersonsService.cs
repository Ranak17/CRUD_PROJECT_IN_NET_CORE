using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IPersonsService
    {
        Task<PersonResponse?> AddPerson(PersonAddRequest? personAddRequest);
        Task<List<PersonResponse>> GetAllPersons();

        Task<PersonResponse?> GetPersonByPersonID(Guid? personID);

        Task<PersonResponse?> UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        Task<bool?> DeletePerson(Guid? personID);
    }
}
