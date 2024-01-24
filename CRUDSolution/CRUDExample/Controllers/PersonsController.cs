using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
namespace CRUDExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonsService _personsService;
        private readonly ILogger<PersonsController> _logger;

        public PersonsController(IPersonsService personsService, ILogger<PersonsController> logger)
        {
            _personsService= personsService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<PersonResponse>> GetAllPerson()
        {
            _logger.LogInformation("GetAllPerson Action Executed");
            return await _personsService.GetAllPersons();
        }

        [HttpPost]
        public async Task<PersonResponse?> AddPerson(PersonAddRequest personAddRequest)
        {
            _logger.LogInformation("AddPerson Action Executed");
            return await _personsService.AddPerson(personAddRequest);
        }
    }
}
