using ServiceContracts.Dto.CountryDto;

namespace ModelDto
{
    /// <summary>
    /// Add Country Object to list for Countries
    /// </summary>
    /// <param name="AddCountryRequest">Country Created Dto</param>
    /// <return>Return the country Response as new object</return>
    public interface ICountriesService
    {
        /// <summary>
        /// Adding Country Add Request 
        /// </summary>
        /// <param name="AddCountryRequest"></param>
        /// <returns></returns>
       CountryResponseDto AddCountry(CountryRequest AddCountryRequest);


        /// <summary>
        /// Get the list of Country 
        /// </summary>  
        /// <returns></returns>
        List<CountryResponseDto> ListCountry();

        /// <summary>
        /// Get Country By Id
        /// </summary>
        /// <param name="CountryId"></param>
        /// <returns>Get Country ID</returns>
        CountryResponseDto GetCountry(Guid? CountryId);
    }
}
