using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface ICountriesService
    {
        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);

        IList<CountryResponse> GetAllCountries();

        CountryResponse? GetCountryByCountryID(Guid? countryId);
    }
}
