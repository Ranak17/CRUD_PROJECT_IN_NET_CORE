using Entities;

namespace RepositoryContracts
{
    //Repository(or Repository Pattern) is an abstraction between Data Access Layer(EF DbContext) and business logic layer(Service) of the application
    //The biggest difference between the service as well as repository is that repository is the pure representation of the data access logic i.e., its tightly coupled with domain

    public interface ICountriesRepository
    {
        Task<Country> AddCountry(Country country);
        Task<IList<Country>> GetAllCountries();
        Task<Country?> GetCountryByCountryId(Guid countryID);
        Task<Country?> GetCountryByName(string countryName);

    }
}
