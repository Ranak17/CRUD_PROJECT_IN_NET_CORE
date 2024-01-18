using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;

        public CountriesService()
        {
            _countries = new List<Country>();
        }

        public CountryResponse? AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null) return null;
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException();
            }
            if (_countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Any())
            {
                throw new ArgumentException("Given Country Name is Already Present");

            }
            Country country = countryAddRequest.ToCountry();
            country.CountryID = Guid.NewGuid();
            _countries.Add(country);
            return country.ToCountryResponse();
        }

        public IList<CountryResponse> GetAllCountries()
        {
            return _countries.Select(temp=>temp.ToCountryResponse()).ToList();
        }
        public CountryResponse? GetCountryByCountryID(Guid? countryId)
        {
            if(countryId == null) return null;
            return _countries.FirstOrDefault(temp=>temp.CountryID == countryId)?.ToCountryResponse();
        }
    }
}
