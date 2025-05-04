using System;
using System.ComponentModel.DataAnnotations;
using Entities;

namespace ServiceContracts.Dto.CountryDto
{
    /// <summary>
    /// Dto Class that is used as return type for country methods
    /// used for HTTPGET created new Dto object for countries
    /// </summary>
    public class CountryResponseDto: IEquatable<CountryResponseDto>
    {
        [Key]
        public Guid CountryId { get; set; }
        [Required]
        public string CountryName { get; set; } = string.Empty;

        public bool Equals(CountryResponseDto? other)
        {
            if (other == null)
                return false;

            return CountryId == other.CountryId &&
                   CountryName == other.CountryName;
        }
            
        public override bool Equals(object? obj) => Equals(obj as CountryResponseDto);


        //it returns the hash code of the current object
        public override int GetHashCode()
        {
            return HashCode.Combine(HashCode.Combine(CountryId, CountryName));
        }
    }
    public  static class CountryResponseExtensions
    {
        public static  CountryResponseDto ToCountryResponse(this CountryModel country)
        {
            return new CountryResponseDto
            {
                CountryId = country.CountryId,
                CountryName = country.CountryName
            };
        }
    }
}
