using Entities;
using Entities.Enums;
using System.ComponentModel.DataAnnotations;


namespace ServiceContracts.Dto.PersonDto
{
    /// <summary>
    /// Act as Main model for adding new Person
    /// </summary>
    public class PersonRequest
    {
        [Required(ErrorMessage = "Person Name is required")]
        public string PersonName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; } = string.Empty;

        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.IsoDateTimeConverter))]
        public DateTime? BirthDay { get; set; }
        public GenderOptions? Gender { get; set; } // you can have type safety, you cannot just input any value aside from genders
        public string? CountryName { get; set; }
        public Guid? CountryID { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        /// <summary>
        /// Convert the Person Main model into MapPersonModel object
        /// </summary>
        /// <returns></returns>
        public PersonModel MapPersonModel()
        {
            return new PersonModel
            {
                PersonName = this.PersonName,
                Email = this.Email,
                BirthDay = this.BirthDay,
                Gender = this.Gender?.ToString(),
                CountryID = this.CountryID,
                CountryName = this.CountryName,
                ReceiveNewsLetters = this.ReceiveNewsLetters
            };
        }
    }
    
}



