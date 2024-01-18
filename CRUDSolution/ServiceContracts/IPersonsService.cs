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
        PersonResponse? AddPerson(PersonAddRequest? personAddRequest);
        List<PersonResponse> GetAllPersons();

        PersonResponse? GetPersonByPersonID(Guid? personID);

        PersonResponse? UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        bool? DeletePerson(Guid? personID);
    }
}
