using AutoFixture;
using AutoFixture.Kernel;
using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
namespace CRUDTests
{
    public class PersonServiceTest
    {
        //Unit test tools
        //AutoFixture to automatically generate model classes
        //Moq and EntityFrameworkMock


        private readonly IPersonsService _personService;
        private readonly ICountriesService _countryService;
        private readonly IFixture _fixture;

        private readonly IPersonsRepository _personRepository;
        private readonly Mock<IPersonsRepository> _personRepositoryMock;

        public PersonServiceTest()
        {
            _fixture = new Fixture();
            //_personService = new PersonsService();
            //var countriesInitialData = new List<Country>();
            //var personsInitialData = new List<Person>();

            //DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);

            //ApplicationDbContext dbContext = dbContextMock.Object;
            //dbContextMock.CreateDbSetMock(temp => temp.Persons, personsInitialData);
            //dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData); 
            _personRepositoryMock = new Mock<IPersonsRepository>();
            _personRepository = _personRepositoryMock.Object;
            //_countryService = new CountriesService(dbContext);
            _personService = new PersonsService(_personRepository);
        }

        #region AddPerson

        [Fact]
        public async Task AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;
            //Act
            PersonResponse? personResponse = await _personService.AddPerson(personAddRequest);
            //Assert
            Assert.Null(personResponse);
        }
        [Fact]
        public async Task AddPerson_PersonNameIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "someone@email.com")
                .With(temp => temp.PersonName, null as string)
                .Create();
            Person person = personAddRequest.ToPerson();
            _personRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _personService.AddPerson(personAddRequest);
            }
            );
        }

        [Fact]
        public async Task AddPerson_ProperPersonDetails_ToBeSuccessFull()
        {
            //Arrange

            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "someone@email.com")
                .Create();
            Person person = personAddRequest.ToPerson();
            PersonResponse personResponseExpected = person.ToPersonResponse();

            _personRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);

            //Act
            PersonResponse? personResponseFromAdd = await _personService.AddPerson(personAddRequest);
            personResponseExpected.PersonID = personResponseFromAdd.PersonID;
            //Assert
            Assert.Equal(personResponseFromAdd, personResponseExpected);
        }

        #endregion

        #region GetPersonByPersonID
        [Fact]
        public async Task GetPersonByPersonID_NullPersonID()
        {
            //Arrange
            Guid? personID = null;
            //Act
            PersonResponse? personResponse = await _personService.GetPersonByPersonID(personID);
            //Assert
            Assert.Null(personResponse);
        }

        [Fact]
        public async Task GetPersonByPersonID_WithPersonID()
        {
            //Arrange
            Person person = _fixture.Build<Person>()
                .With(temp => temp.Email, "someone@email.com")
                .Create();
            PersonResponse personResponseExpected = person.ToPersonResponse();
            _personRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(person);

            //Act

            PersonResponse? personResponseFromGet = await _personService.GetPersonByPersonID(person.PersonID);
            //Assert
            Assert.Equal(personResponseExpected, personResponseFromGet);
        }




        #endregion

        #region GetAllPersons

        [Fact]
        public async Task GetAllPerson_EmptyList()
        {
            //Arrange
            _personRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(new List<Person>());
            //Act
            IList<PersonResponse> personsFromGet = await _personService.GetAllPersons();
            //Assert
            Assert.Empty(personsFromGet);
        }

        [Fact]
        public async Task GetAllPerson_AddFewPerson()
        {
            //Arrange

            IList<Person> persons = new List<Person>(){
                _fixture.Build<Person>()
                .With(temp => temp.Email, "someone@email.com")
                .Create(),
                 _fixture.Build<Person>()
                .With(temp => temp.Email, "someone@email.com")
                .Create()
            };
            _personRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);
            IList<PersonResponse> personResponseFromAdd = persons.Select(temp => temp.ToPersonResponse()).ToList();
            //Act
            IList<PersonResponse> personResponseFromGet = await _personService.GetAllPersons();

            //Assert
            foreach (PersonResponse response in personResponseFromGet)
            {
                Assert.Contains(response, personResponseFromAdd);
            }
        }



        #endregion

        #region PersonUpdateRequest

        [Fact]
        public async Task UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = null;
            //Act
            PersonResponse? personResponse = await _personService.UpdatePerson(personUpdateRequest);
            //Assert
            Assert.Null(personResponse);
        }

        [Fact]
        public async Task UpdatePerson_InvalidPersonID()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = _fixture.Create<PersonUpdateRequest>();
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _personService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public async Task UpdatePerson_PersonNameISNull()
        {
            //Arange
            Person? person = _fixture.Build<Person>()
                .With(temp => temp.PersonName, null as string)
                .With(temp=>temp.Gender,nameof(GenderOptions.Male))
                .Create<Person>();

            PersonResponse? personResponseFromAdd = person.ToPersonResponse();

            PersonUpdateRequest? personUpdateRequest = personResponseFromAdd?.ToPersonUpdateRequest();
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _personService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public async Task UpdatePerson_PersonFullDetailUpdaion()
        {
            //Arange
            Person person = _fixture.Build<Person>()
                .With(temp => temp.Email, "someone@email.com")
                .With(temp=>temp.Gender,nameof(GenderOptions.Male))
                .Create();

            PersonResponse? personResponseExpected = person.ToPersonResponse();
            PersonUpdateRequest? personUpdateRequest = personResponseExpected?.ToPersonUpdateRequest();

            _personRepositoryMock.Setup(temp => temp.UpdatePerson(It.IsAny<Person>()))
                                 .ReturnsAsync(person);
            _personRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
                                 .ReturnsAsync(person);

            //Act
            PersonResponse? personResponseFromUpdate = await _personService.UpdatePerson(personUpdateRequest);
            personResponseExpected.PersonID = personResponseFromUpdate.PersonID;
            //Assert
            Assert.Equal(personResponseFromUpdate, personResponseExpected);
        }

        #endregion

        #region DeletePerson
        [Fact]
        public async Task DeletePerson_ValidPersonID()
        {
            //Arrange
            Person person = _fixture.Build<Person>()
                .With(temp => temp.Email, "someone@gmail.com")
                .With(temp=>temp.Gender,nameof(GenderOptions.Male))
                .With(temp=>temp.PersonID,Guid.NewGuid())
                .Create();
            _personRepositoryMock.Setup(temp => temp.DeletePersonByPersonID(It.IsAny<Guid>()))
                .ReturnsAsync(true);
            _personRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(person);
            //Act
            bool? isDeleted = await _personService.DeletePerson(person.PersonID);
            //Assert
            Assert.True(isDeleted);
        }

        [Fact]
        public async Task DeletePerson_InvalidPersonID()
        {
            //Act
            bool? isDeleted = await _personService.DeletePerson(Guid.NewGuid());
            Assert.False(isDeleted);
        }

        #endregion
    }
}
