using BussinessLogicService.Helpers;
using Entities;
using ModelDto;
using ServiceContracts.Dto.CountryDto;

namespace BussinessLogicService
{
    public class CountryServiceLogic : ICountriesService
    {
        private List<CountryModel> _countries;

        public CountryServiceLogic()
        {
            _countries = new List<CountryModel>();
        }
        private CountryResponseDto CountryIntoCountryResponse(CountryModel country)
        {
            return country.ToCountryResponse();
        }

        public CountryResponseDto AddCountry(CountryRequest AddCountryRequest)
        {
            if (AddCountryRequest == null) 
                throw new ArgumentNullException(nameof(AddCountryRequest));

            ModelValidation.ValidateRequest(AddCountryRequest);

            if (_countries.Any(country => 
                country.CountryName.Equals(AddCountryRequest.CountryName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Country already exists");
            }

            var newCountry = AddCountryRequest.ToCountry();
            newCountry.CountryId = Guid.NewGuid();

            _countries.Add(newCountry);

            return CountryIntoCountryResponse(newCountry);
        }

        public CountryResponseDto GetCountry(Guid? CountryId)
        {
            ModelValidation.ValidateId(CountryId);

            var country = _countries.FirstOrDefault(x => x.CountryId == CountryId);

            if (country == null)
                throw new KeyNotFoundException($"Country with ID {CountryId} not found.");

            return CountryIntoCountryResponse(country);
        }

        public List<CountryResponseDto> ListCountry()
        {
           return _countries.Select(x => x.ToCountryResponse()).ToList();
        }
    }
}
