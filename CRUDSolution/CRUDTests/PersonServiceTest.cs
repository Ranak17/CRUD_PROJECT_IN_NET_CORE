using ServiceContracts;
using System;
using Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDTests
{
    public class PersonServiceTest
    {
        private readonly IPersonsService _personService;
        private readonly ICountriesService _countryService;

        public PersonServiceTest()
        {
            _personService = new PersonsService();
            _countryService = new CountriesService();
        }

        #region AddPerson

        [Fact]
        public void AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;
            //Act
            PersonResponse? personResponse = _personService.AddPerson(personAddRequest);
            //Assert
            Assert.Null(personResponse);
        }
        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = null,
                Address = "c.p colony",
                Email = "karan@gmail.com"
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personService.AddPerson(personAddRequest);
            }
            );
        }

        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            //Arrange

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "karan",
                Address = "c.p colony",
                Email = "person@gmail.com",
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("12-12-2000"),
                CountryID = Guid.NewGuid(),
                ReceiveNewsLetter = true
            };
            //Act
            PersonResponse personResponseFromAdd = _personService.AddPerson(personAddRequest);

            //Assert
            Assert.True(personResponseFromAdd.PersonID != Guid.Empty);
        }

        #endregion

        #region GetPersonByPersonID
        [Fact]
        public void GetPersonByPersonID_NullPersonID()
        {
            //Arrange
            Guid? personID = null;
            //Act
            PersonResponse? personResponse = _personService.GetPersonByPersonID(personID);
            //Assert
            Assert.Null(personResponse);
        }

        [Fact]
        public void GetPersonByPersonID_WithPersonID()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryResponse countryResponse = _countryService.AddCountry(countryAddRequest);
            //Act
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "perosonName",
                Address = "address",
                CountryID = countryResponse.CountryID,
                DateOfBirth = DateTime.Parse("17-07-2000"),
                Email = "email@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetter = true
            };
            PersonResponse? personResponseFromAdd = _personService.AddPerson(personAddRequest);
            PersonResponse? personResponseFromGet = _personService.GetPersonByPersonID(personResponseFromAdd?.PersonID);
            //Assert
            Assert.Equal(personResponseFromAdd, personResponseFromGet);
        }




        #endregion

        #region GetAllPersons

        [Fact]
        public void GetAllPerson_EmptyList()
        {
            //Act
            IList<PersonResponse> personsFromGet = _personService.GetAllPersons();
            //Assert
            Assert.Empty(personsFromGet);
        }

        [Fact]
        public void GetAllPerson_AddFewPerson()
        {
            //Arrange
            CountryAddRequest countryAddRequest1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest() { CountryName = "UK" };
            CountryResponse countryResponse1 = _countryService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = _countryService.AddCountry(countryAddRequest2);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "karan",
                Address = "address",
                Email = "email@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetter = true,
                CountryID = countryResponse1?.CountryID,
                DateOfBirth = DateTime.Parse("12-02-2000")
            };
            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "paran",
                Address = "address",
                Email = "email2@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetter = true,
                CountryID = countryResponse2?.CountryID,
                DateOfBirth = DateTime.Parse("13-02-2000")
            };
            PersonResponse? personResponse = _personService.AddPerson(personAddRequest);
            PersonResponse? personResponse2 = _personService.AddPerson(personAddRequest2);
            IList<PersonResponse> personResponseFromAdd = new List<PersonResponse>() { personResponse, personResponse2 };
            //Act
            IList<PersonResponse> personResponseFromGet = _personService.GetAllPersons();

            //Assert
            foreach (PersonResponse response in personResponseFromGet)
            {
                Assert.Contains(response, personResponseFromAdd);
            }
        }



        #endregion

        #region PersonUpdateRequest

        [Fact]
        public void UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = null;
            //Act
            PersonResponse? personResponse = _personService.UpdatePerson(personUpdateRequest);
            //Assert
            Assert.Null(personResponse);
        }

        [Fact]
        public void UpdatePerson_InvalidPersonID()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest()
            {
                PersonID = Guid.NewGuid()
            };
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public void UpdatePerson_PersonNameISNull()
        {
            //Arange
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "karan",
                Address = "address",
                DateOfBirth = DateTime.Parse("12-12-2012"),
                Email = "email@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetter = true,
            };

            PersonResponse? personResponseFromAdd = _personService.AddPerson(personAddRequest);
            PersonUpdateRequest? personUpdateRequest = personResponseFromAdd?.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public void UpdatePerson_PersonFullDetailUpdaion()
        {
            //Arange
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "karan",
                Address = "address",
                DateOfBirth = DateTime.Parse("12-12-2012"),
                Email = "email@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetter = true,
            };

            PersonResponse? personResponseFromAdd = _personService.AddPerson(personAddRequest);
            PersonUpdateRequest? personUpdateRequest = personResponseFromAdd?.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "Tarun";
            personUpdateRequest.Email = "tarun@gmail.com";

            //Act
            PersonResponse? personResponseFromUpdate = _personService.UpdatePerson(personUpdateRequest);
            PersonResponse? personResponseFromGet = _personService.GetPersonByPersonID(personResponseFromUpdate.PersonID);

            //Assert
            Assert.Equal(personResponseFromUpdate, personResponseFromGet);
        }

        #endregion

        #region DeletePerson
        [Fact]
        public void DeletePerson_ValidPersonID()
        {
            //Arrange
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Karan",
                Address = "address",
                DateOfBirth = DateTime.Parse("02-02-2022"),
                Email="email@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetter = true            
            };
            PersonResponse? personResponseFromAdd = _personService.AddPerson(personAddRequest);

            //Act
            bool? isDeleted =_personService.DeletePerson(personResponseFromAdd?.PersonID);
            //Assert
            Assert.True(isDeleted);
        }

        [Fact]
        public void DeletePerson_InvalidPersonID()
        {
            //Act
            bool? isDeleted = _personService.DeletePerson(Guid.NewGuid());
            Assert.False(isDeleted);
        }

        #endregion
    }
}
