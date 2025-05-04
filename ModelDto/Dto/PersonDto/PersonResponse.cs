using Entities;
using Entities.Enums;
using System.ComponentModel.DataAnnotations;


namespace ServiceContracts.Dto.PersonDto
{

    /// <summary>
    /// Act as Main model for response as new Person
    /// Responsible for HTTPGET and HTTPGEID
    /// </summary>
    public class PersonResponse
    {
        [Key]
        public Guid PersonId { get; set; }

        [Required(ErrorMessage = "Person Name is required")]
        public string PersonName { get; set; } =string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; } = string.Empty;

        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.IsoDateTimeConverter))]
        public DateTime? BirthDay { get; set; }
        public string? Gender { get; set; }
        public string? CountryName { get; set; }
        public Guid? CountryID { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public int? Age { get; set; }

        /// <summary>
        /// it returns the hash code of the current object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>
        /// true or false indicating weather all details are match with specified obj</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if(obj.GetType() != typeof(PersonResponse))
                return false;

            PersonResponse personResponse_compare = (PersonResponse)obj;

            return 
                this.PersonId == personResponse_compare.PersonId &&
                this.PersonName == personResponse_compare.PersonName &&
                this.Email == personResponse_compare.Email &&
                this.BirthDay == personResponse_compare.BirthDay &&
                this.Gender == personResponse_compare.Gender &&
                this.CountryName == personResponse_compare.CountryName &&
                this.Age == personResponse_compare.Age &&
                this.ReceiveNewsLetters == personResponse_compare.ReceiveNewsLetters;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(HashCode.Combine(PersonId, PersonName, Email, BirthDay, CountryName, Age, ReceiveNewsLetters));
        }
        public override string ToString()
        {
            return $"PersonId: {PersonId}, PersonName: {PersonName}, Email: {Email}, BirthDay: {BirthDay},Gender: {Gender}, CountryName: {CountryName}, Age: {Age}, ReceiveNewsLetters: {ReceiveNewsLetters}";
        }
        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest
            {
                PersonId = this.PersonId,
                PersonName = this.PersonName,
                Email = this.Email,
                BirthDay = this.BirthDay,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions),Gender, true),
                CountryName = this.CountryName,
                CountryID = this.CountryID,
                ReceiveNewsLetters = this.ReceiveNewsLetters
            };
        }
    }
        //it compares the current object with another object
    public static class PersonResponseExtensions
    {
        public static PersonResponse ToPersonResponse(this PersonModel person)
        {
            return new PersonResponse
            {
                PersonId = person.PersonId,
                PersonName = person.PersonName,
                Email = person.Email,
                BirthDay = person.BirthDay,
                Gender = person.Gender,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Age = (person.BirthDay != null)
                   ? (int)Math.Round((DateTime.Now - person.BirthDay.Value).TotalDays / 365.25)
                   : null
            };
        }
    }
}
