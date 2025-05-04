using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Entities.Enums;
using Newtonsoft.Json;
namespace Entities
{
    /// <summary>
    /// Person domain model
    /// </summary>
    public class PersonModel
    {
        public Guid PersonId { get; set; }
        public string PersonName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.IsoDateTimeConverter))]
        public DateTime? BirthDay { get; set; }
        public string? Gender { get; set; } //storing database as string
        public string? CountryName { get; set; }    
        public Guid? CountryID { get; set; }
        public bool ReceiveNewsLetters { get; set; }
    }
}
