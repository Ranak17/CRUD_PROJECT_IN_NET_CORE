using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly ICountriesRepository _countriesRepository;

        public CountriesService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }

        public async Task<CountryResponse?> AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null) return null;
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException();
            }
            if (await _countriesRepository.GetCountryByName(countryAddRequest.CountryName)==null)
            {
                throw new ArgumentException("Given Country Name is Already Present");

            }
            Country country = countryAddRequest.ToCountry();
            country.CountryID = Guid.NewGuid();
            await _countriesRepository.AddCountry(country);
            return country.ToCountryResponse();
        }

        public async Task<IList<CountryResponse>> GetAllCountries()
        {
            return (await _countriesRepository.GetAllCountries()).Select(temp=>temp.ToCountryResponse()).ToList();
        }
        public async Task<CountryResponse?> GetCountryByCountryID(Guid? countryId)
        {
            if(countryId == null) return null;
            Country? countryResponseFromDB =await _countriesRepository.GetCountryByCountryId(countryId.Value);
            return countryResponseFromDB?.ToCountryResponse();
        }
    }
}
