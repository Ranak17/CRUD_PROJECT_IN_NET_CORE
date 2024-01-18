using System;
using System.Runtime.CompilerServices;
using Entities;

namespace ServiceContracts.DTO
{
    public class CountryResponse
    {
        public Guid CountryID { get; set; } 
        public string? CountryName { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(CountryResponse)) return false;
            CountryResponse objectToCompare = (CountryResponse)obj;
            return this.CountryID == objectToCompare.CountryID && this.CountryName==objectToCompare.CountryName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class CountryExtension
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse() { CountryID=country.CountryID,CountryName=country.CountryName};
        }
    }
}
