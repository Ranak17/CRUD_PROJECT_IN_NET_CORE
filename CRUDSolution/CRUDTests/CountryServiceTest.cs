using ServiceContracts;
using ServiceContracts.DTO;
using Services;
namespace CRUDTests
{
    public class CountryServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountryServiceTest()
        {
            _countriesService = new CountriesService();
        }

        #region AddCountry
        [Fact]
        public void AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = null;
            //Act
            CountryResponse? countryResponse = _countriesService.AddCountry(countryAddRequest);

            //Assert
            Assert.Null(countryResponse);
        }

        [Fact]
        public void AddCountry_CountryNameIsNull()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest()
            {
                CountryName = null,
            };


            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(countryAddRequest);
            });
        }

        [Fact]
        public void AddCountry_DuplicateCountryName()
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
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(countryAddRequest);
                _countriesService.AddCountry(countryAddRequest2);
            });
        }

        [Fact]
        public void AddCountry_ProperCountryDetails()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Japan",
            };

            //Act
            CountryResponse response = _countriesService.AddCountry(countryAddRequest);

            //Assert
            Assert.True(response.CountryID != Guid.Empty);
        }

        #endregion

        #region GetAllCountries

        [Fact]
        public void GetAllCountries_EmptyList()
        {
            //Act
            IList<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(actualCountryResponseList);
        }
        [Fact]
        public void GetAllCountries_AddFewCountries()
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
                countryListFromAddCountry.Add(_countriesService.AddCountry(request));
            }
            //Assert
            IList<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();
            foreach(CountryResponse expectedCountry in countryListFromAddCountry)
            {
                Assert.Contains(expectedCountry, actualCountryResponseList);
            }

        }


        #endregion

        #region GetCountryByCountryID

        [Fact]
        public void GetCountryByCountryID_NullCountryID()
        {
            //Arrange
            Guid? CountryID = null;

            //Act
            CountryResponse response = _countriesService.GetCountryByCountryID(CountryID);

            //Assert
            Assert.Null(response);

        }
        [Fact]
        public void GetCountryByCountryID_ValidCountryID()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName="India"};
            CountryResponse countryResponseFromAdd = _countriesService.AddCountry(countryAddRequest);
            Guid countryIDFromAdd = countryResponseFromAdd.CountryID;


            //Act
            CountryResponse? countryResponseFromGet = _countriesService.GetCountryByCountryID(countryIDFromAdd);

            //Assert
            Assert.Equal(countryResponseFromAdd,countryResponseFromGet);

        }



        #endregion
    }
}