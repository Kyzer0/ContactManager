using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.Dto.PersonDto
{
    /// <summary>
    /// Represents the request Dto  model for updating a person.
    /// </summary>
    /// not all models are properties to update
    public class PersonUpdateRequest 
    {
        [Required(ErrorMessage = "Person ID is required")]
        public Guid PersonId { get; set; }
        [Required(ErrorMessage = "Person Name is required")]
        public string PersonName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; } = string.Empty;

        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.IsoDateTimeConverter))]
        public DateTime? BirthDay { get; set; }
        public GenderOptions? Gender { get; set; }
        public string? CountryName { get; set; }
        public Guid? CountryID { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        /// <summary>
        /// Convert the Person Main model into ToPersonUpdateRequest object
        /// </summary>
        /// <returns>Return Person object</returns>
        public PersonModel ToPersonUpdateRequest()
        {
            return new PersonModel
            {
                PersonId = this.PersonId,
                PersonName = this.PersonName,
                Email = this.Email,
                BirthDay = this.BirthDay,
                Gender = Gender.ToString(),
                CountryID = this.CountryID,
                CountryName = this.CountryName,
                ReceiveNewsLetters = this.ReceiveNewsLetters
            };
        }
    }
}
