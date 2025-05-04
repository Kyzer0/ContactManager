using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.Dto.CountryDto
{
    /// <summary>
    /// Dto Class for adding a new country
    /// Used for HTTPOST, PUT. Add Country Object to list for Countries
    /// </summary>
    public class CountryRequest
    {
        /// <summary>
        /// This MapCountryDto means Create or add New Country
        /// </summary>
        /// 
        [Required]
        public string CountryName { get; set; } = string.Empty;

        public CountryModel ToCountry()
        {
            return new CountryModel
            {
                CountryName = CountryName.Trim()
            };
        }


    }
}
