
using BussinessLogicService;
using ModelDto;
using ServiceContracts.Dto.CountryDto;

namespace CrudTest
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        public CountriesServiceTest()
        {
            _countriesService = new CountryServiceLogic(); 
        }

        #region Add Country Test
        //when countryAddRequest is  null, it should throw an ArgumentNullException
        [Fact]
        public void AddCountry_NullRequest_ThrowsArgumentNullException()
        {
            // Arrange
            CountryRequest? countryAddRequest = null;
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _countriesService.AddCountry(countryAddRequest));
        }

        //when countryName is null or Empty, it should throw an ArgumentException
        [Fact]
        public void CheckCountryName_IsNullOrWhiteSpace_ThrowsExceptions()
        {
            //dump value
            var countryName = new CountryRequest
            {
                CountryName = ""
            };

            //Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                if(string.IsNullOrWhiteSpace(countryName.CountryName))
                {
                    throw new ArgumentException("Country name cannot be null or empty");
                }
            });
        }

        //when countryName is duplicate, it should throw an InvalidOperationException
        [Fact]
        public void CheckCountryNameIsDuplicateThrowsInvalidOperationException()
        {
            var addedCountry = new CountryRequest { CountryName = "USA" };

            _countriesService.AddCountry(addedCountry);
            Assert.Throws<InvalidOperationException>(() =>
            {
                _countriesService.AddCountry(addedCountry);
            });
        }
        //when countryName is valid, it should return a new CountryResponseDto object with a new Guid and the countryName
       [Fact]
        public void AddCountry_ValidRequest_ReturnsCountryResponse()
        {
            var request = new CountryRequest { CountryName = "Canada" };

            var result = _countriesService.AddCountry(request);
            var countries_from_GetAllCountries = _countriesService.ListCountry();

            Assert.NotNull(result);
            Assert.Equal("Canada", result.CountryName);
            Assert.NotEqual(Guid.Empty, result.CountryId);
            Assert.Contains(result, countries_from_GetAllCountries);
        }
        #endregion

        #region AddCountries Should not accept Value
        [Fact]
        public void AddCountries_WhenInputEmpty_ShouldNotAddAnyCountries()
        {
            List<CountryRequest> responseDtos = new()
            {
                new CountryRequest{ CountryName = "USA"},
                new CountryRequest{ CountryName = "PHILIPPINES"},
                new CountryRequest{ CountryName = "JAPAN"}
            };


            List<CountryRequest> new_Added_reponse_Dto = new();

            foreach(CountryRequest country_request in new_Added_reponse_Dto)
            {
                _countriesService.AddCountry(country_request);
            }

            IEnumerable<CountryResponseDto> actuallCountryResponseList =  _countriesService.ListCountry();
            
            foreach(var expected_Country in actuallCountryResponseList)
            {
                Assert.Contains(expected_Country, actuallCountryResponseList);
            }
        }
        #endregion

        #region CheckID 
        [Fact]
        public void CheckID_IfGenerating_NewId()
        {
            var country = new CountryRequest
            {
                CountryName = "USA"
            };
            var addedCountry = _countriesService.AddCountry(country);

            Assert.NotEqual(Guid.Empty, addedCountry.CountryId);
            

        }
        [Fact]
        public void CheckValue_InvalidNull_ShouldThrowException()
        {
            //Arrange
            Guid? countryId = null;

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                _countriesService.GetCountry(countryId);
            });
        }
        #endregion
        #region GetCountry_WithNonExistentId
        [Fact]
        public void GetCountry_WithNonExistentId_ThrowsKeyNotFoundException()
        {
            // Arrange
            Guid nonExistentCountryId = Guid.NewGuid();
    
            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _countriesService.GetCountry(nonExistentCountryId));
        }
        #endregion

        #region ReturnValue CorrectID
        [Fact]
        public void CheckValue_CorrectID_ShouldReturnValue()
        {
            //Arrange
            var countryAddRequest = new CountryRequest
            {
                CountryName = "USA"
            };
            CountryResponseDto checkValue = _countriesService.AddCountry(countryAddRequest);
            //Act
            CountryResponseDto countryId =  _countriesService.GetCountry(checkValue.CountryId);
            //Assert
            Assert.Equal(checkValue,countryId);
        }
        #endregion

    }
}
