using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface ICountriesService
    {
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);

        Task<IList<CountryResponse>> GetAllCountries();

        Task<CountryResponse?> GetCountryByCountryID(Guid? countryId);
    }
}
