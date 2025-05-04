using BussinessLogicService.Helpers;
using Entities.Enums;
using ModelDto;
using ServiceContracts;
using ServiceContracts.Dto.PersonDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogicService
{
    public class PersonServiceLogic : IPersonService
    {
        private readonly List<Entities.PersonModel> _persons;
        private readonly ICountriesService _countries;
        public PersonServiceLogic(ICountriesService countriesService)
        {
            if (countriesService == null)
            {
                System.Diagnostics.Debug.WriteLine("Countries Service is null! Check instantiation call stack");
                throw new ArgumentNullException("Countries Service is null!");
            }
            _persons = new List<Entities.PersonModel>();
            _countries = countriesService ?? throw new ArgumentNullException("Countries is null here");
        }
        /// <summary>
        /// Converts a PersonUpdateResponse object into a PersonResponseDto object.
        /// Adds the country name to the response by fetching it from the country service.
        /// </summary>
        /// <param name="person">The PersonUpdateResponse object to be converted.</param>
        /// <returns>A PersonResponseDto object containing the person's details and country name.</returns>
        private ServiceContracts.Dto.PersonDto.PersonResponse PersonIntoPersonResponse(Entities.PersonModel person)
        {
            // Convert the PersonUpdateResponse to a PersonResponseDto
            var personResponse = person.ToPersonResponse();

            // Fetch the country name using the country service
            var country = _countries.GetCountry(person.CountryID);
            if (country == null)
                throw new KeyNotFoundException($"Country with ID {person.CountryID} not found.");

            personResponse.CountryName = country.CountryName;

            return personResponse;
        }

        /// <summary>
        /// Adds a new person to the list of persons.
        /// </summary>
        /// <param name="personRequest">The person data to be added.</param>
        /// <returns>A response DTO containing the added person's details.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the personRequest is null or required fields are missing.</exception>
        /// <exception cref="ArgumentException">Thrown when the person name is null or empty.</exception>
        public ServiceContracts.Dto.PersonDto.PersonResponse AddPerson(PersonRequest? personRequest)
        {
            if (personRequest == null)
                throw new ArgumentNullException(nameof(personRequest), "Person request cannot be null.");

            // Validate the input
            ModelValidation.ValidateRequest(personRequest);

            // Map the PersonCreatedDto to a PersonUpdateResponse and assign a new unique ID
            var person = personRequest.MapPersonModel();
            person.PersonId = Guid.NewGuid();

            if (person == null)
                throw new ArgumentNullException(nameof(person));


            if (person.CountryID == null || !_countries.ListCountry().Any(c => c.CountryId == person.CountryID))
                throw new KeyNotFoundException($"Country with ID {person.CountryID} not found");



            // Add the new person to the in-memory list
            _persons.Add(person);

            //getting the countryName from country service
            return PersonIntoPersonResponse(person);
        }

        public ServiceContracts.Dto.PersonDto.PersonResponse GetPersonByID(Guid? personId)
        {
            // Validate the input ID
            ModelValidation.ValidateId(personId);

            var person = _persons.FirstOrDefault(x => x.PersonId == personId); //returns null if not found

            if (person == null)
                throw new KeyNotFoundException($"Person with ID {personId} not found.");

            // Validate if the CountryID exists in the countries service
            if (person.CountryID == null || !_countries.ListCountry().Any(c => c.CountryId == person.CountryID))
                throw new KeyNotFoundException($"Country with ID {person.CountryID} not found.");

            return PersonIntoPersonResponse(person);
        }


        public List<ServiceContracts.Dto.PersonDto.PersonResponse> ListPersons()
        {
            // Convert each PersonUpdateResponse in the list to a PersonResponseDto
            return _persons.Select(person => PersonIntoPersonResponse(person)).ToList();
        }

        public List<ServiceContracts.Dto.PersonDto.PersonResponse> GetFilteredPerson(string searchby, string? searchString)
        {
            var allPerson = ListPersons();

            var holdAllPerson_Data = allPerson;
            if (string.IsNullOrWhiteSpace(searchby) || string.IsNullOrWhiteSpace(searchString))
                return holdAllPerson_Data;

            switch (searchby)
            {
                case nameof(PersonResponse.PersonName):
                    holdAllPerson_Data = allPerson.Where(allPerson => (!string.IsNullOrWhiteSpace(allPerson.PersonName) ?
                    allPerson.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(PersonResponse.Email):
                    holdAllPerson_Data = allPerson.Where(allPerson => (!string.IsNullOrWhiteSpace(allPerson.Email) ?
                    allPerson.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(PersonResponse.BirthDay):
                    holdAllPerson_Data = allPerson.Where(allPerson => (allPerson.BirthDay != null) ?
                    allPerson.BirthDay.Value.ToString("dd MMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(PersonResponse.Gender):
                    holdAllPerson_Data = allPerson.Where(allPerson =>
                        allPerson.Gender.ToString().Equals(searchString, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                    break;
                case nameof(PersonResponse.CountryName):
                    holdAllPerson_Data = allPerson.Where(allPerson => allPerson.CountryName != null &&
                    allPerson.CountryName.StartsWith(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                default: holdAllPerson_Data = allPerson;
                    break;
            }
            return holdAllPerson_Data;
        }

        public List<ServiceContracts.Dto.PersonDto.PersonResponse> GetSortedPerson(List<ServiceContracts.Dto.PersonDto.PersonResponse> allPerson, string sortby, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrWhiteSpace(sortby))
                return allPerson;

            // Sort the list based on the specified property and order
            List<PersonResponse> sortedPerson = sortby switch
            {
                // Sorting based on PersonName
                nameof(PersonResponse.PersonName) when sortOrder == SortOrderOptions.Ascending =>
                    allPerson.OrderBy(x => x.PersonName, StringComparer.CurrentCultureIgnoreCase).ToList(),

                nameof(PersonResponse.PersonName) when sortOrder == SortOrderOptions.Descending =>
                    allPerson.OrderByDescending(x => x.PersonName, StringComparer.CurrentCultureIgnoreCase).ToList(),

                // Sorting based on Email
                nameof(PersonResponse.Email) when sortOrder == SortOrderOptions.Ascending =>
                    allPerson.OrderBy(x => x.Email, StringComparer.CurrentCultureIgnoreCase).ToList(),

                nameof(PersonResponse.Email) when sortOrder == SortOrderOptions.Descending =>
                    allPerson.OrderByDescending(x => x.Email, StringComparer.CurrentCultureIgnoreCase).ToList(),

                // Sorting based on BirthDay
                nameof(PersonResponse.BirthDay) when sortOrder == SortOrderOptions.Ascending =>
                    allPerson.OrderBy(x => x.BirthDay).ToList(),

                nameof(PersonResponse.BirthDay) when sortOrder == SortOrderOptions.Descending =>
                    allPerson.OrderByDescending(x => x.BirthDay).ToList(),

                // Sorting based on Age
                nameof(PersonResponse.Age) when sortOrder == SortOrderOptions.Ascending =>
                    allPerson.OrderBy(x => x.Age).ToList(),

                nameof(PersonResponse.Age) when sortOrder == SortOrderOptions.Descending =>
                    allPerson.OrderByDescending(x => x.Age).ToList(),

                // Sorting based on Gender
                nameof(PersonResponse.Gender) when sortOrder == SortOrderOptions.Ascending =>
                    allPerson.OrderBy(x => x.Gender).ToList(),

                nameof(PersonResponse.Gender) when sortOrder == SortOrderOptions.Descending =>
                    allPerson.OrderByDescending(x => x.Gender).ToList(),

                // Sorting based on CountryName
                nameof(PersonResponse.CountryName) when sortOrder == SortOrderOptions.Ascending =>
                    allPerson.OrderBy(x => x.CountryName, StringComparer.CurrentCultureIgnoreCase).ToList(),

                nameof(PersonResponse.CountryName) when sortOrder == SortOrderOptions.Descending =>
                    allPerson.OrderByDescending(x => x.CountryName, StringComparer.CurrentCultureIgnoreCase).ToList(),

                // Sorting based on ReceiveNewsLetters
                nameof(PersonResponse.ReceiveNewsLetters) when sortOrder == SortOrderOptions.Ascending =>
                    allPerson.OrderBy(x => x.ReceiveNewsLetters).ToList(),

                nameof(PersonResponse.ReceiveNewsLetters) when sortOrder == SortOrderOptions.Descending =>
                    allPerson.OrderByDescending(x => x.ReceiveNewsLetters).ToList(),

                // Default case if no match is found
                _ => allPerson
            };

            return sortedPerson;
        }
    

    }
}
