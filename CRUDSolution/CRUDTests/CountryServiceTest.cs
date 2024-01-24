using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Entities;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using Moq;
using RepositoryContracts;
using AutoFixture;
namespace CRUDTests
{
    public class CountryServiceTest
    {
        private readonly ICountriesService _countriesService;
        private readonly ICountriesRepository _countriesRepository;
        private readonly Mock<ICountriesRepository> _countriesRepositoryMock;
        private readonly IFixture _fixture;

        public CountryServiceTest()
        {
            var countriesInitialData = new List<Country>();
            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            dbContextMock.CreateDbSetMock(temp => temp.Countries,countriesInitialData);
            var dbContext = dbContextMock.Object;
            _countriesRepositoryMock = new Mock<ICountriesRepository>();
            _countriesRepository = _countriesRepositoryMock.Object;
            _countriesService = new CountriesService(_countriesRepository);
            _fixture = new Fixture();

        }

        #region AddCountry
        [Fact]
        public async Task AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = null;
            //Act
            CountryResponse? countryResponse = await _countriesService.AddCountry(countryAddRequest);

            //Assert
            Assert.Null(countryResponse);
        }

        [Fact]
        public async Task AddCountry_CountryNameIsNull()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest()
            {
                CountryName = null,
            };


            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _countriesService.AddCountry(countryAddRequest);
            });
        }

        [Fact]
        public async Task AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest()
            {
                CountryName = "USA",
            };
            CountryAddRequest? countryAddRequest2 = new CountryAddRequest()
            {
                CountryName = "USA",
            };


            //Assert
            await Assert.ThrowsAsync<ArgumentException>( async () =>
            {
                //Act
                await _countriesService.AddCountry(countryAddRequest);
                await _countriesService.AddCountry(countryAddRequest2);
            });
        }

        //[Fact]
        //public async Task AddCountry_ProperCountryDetails()
        //{
        //    //Arrange
        //    CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
        //    Country country = countryAddRequest.ToCountry();

        //    _countriesRepositoryMock.Setup(temp => temp.AddCountry(It.IsAny<Country>()))
        //        .ReturnsAsync(country);
        //    //Act
        //    CountryResponse response =await _countriesService.AddCountry(countryAddRequest);

        //    //Assert
        //    Assert.True();
        //}

        #endregion

        #region GetAllCountries

        [Fact]
        public async Task GetAllCountries_EmptyList()
        {
            //Act
            IList<CountryResponse> actualCountryResponseList =await _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(actualCountryResponseList);
        }
        [Fact]
        public async Task GetAllCountries_AddFewCountries()
        {
            //Arrange
                IList<CountryAddRequest> countryRequestList = new List<CountryAddRequest>()
                {
                    new CountryAddRequest() {CountryName ="USA"},
                    new CountryAddRequest() {CountryName ="UK"}
                };
            //Act
            IList<CountryResponse> countryListFromAddCountry = new List<CountryResponse>();
            foreach(CountryAddRequest request  in countryRequestList)
            {
                countryListFromAddCountry.Add(await _countriesService.AddCountry(request));
            }
            //Assert
            IList<CountryResponse> actualCountryResponseList = await _countriesService.GetAllCountries();
            foreach(CountryResponse expectedCountry in countryListFromAddCountry)
            {
                Assert.Contains(expectedCountry, actualCountryResponseList);
            }

        }


        #endregion

        #region GetCountryByCountryID

        [Fact]
        public async Task GetCountryByCountryID_NullCountryID()
        {
            //Arrange
            Guid? CountryID = null;

            //Act
            CountryResponse response = await _countriesService.GetCountryByCountryID(CountryID);

            //Assert
            Assert.Null(response);

        }
        [Fact]
        public async Task GetCountryByCountryID_ValidCountryID()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName="India"};
            CountryResponse countryResponseFromAdd = await _countriesService.AddCountry(countryAddRequest);
            Guid countryIDFromAdd = countryResponseFromAdd.CountryID;


            //Act
            CountryResponse? countryResponseFromGet = await _countriesService.GetCountryByCountryID(countryIDFromAdd);

            //Assert
            Assert.Equal(countryResponseFromAdd,countryResponseFromGet);

        }



        #endregion
    }
}