using Entities.Enums;
using ServiceContracts.Dto.CountryDto;
using ServiceContracts.Dto.PersonDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IPersonService
    {
        /// <summary>
        /// Add Person
        /// </summary>
        /// <param name="personRequest"></param>
        /// <returns></returns>
        PersonResponse AddPerson(PersonRequest personRequest);

        /// <summary>
        /// Get Person By Id
        /// </summary>
        /// <param name="personId"></param>
        /// <returns> Get person by ID</returns>
        /// 
        PersonResponse GetPersonByID(Guid? personId);
        /// <summary>
        /// List of Persons
        /// </summary>
        /// <returns>Return all list of people </returns>
        List<PersonResponse> ListPersons();

        /// <summary>
        /// Get Person by filter
        /// </summary>
        /// <param name="searchby"></param>
        /// <param name="searchString"></param>
        /// <returns>
        /// Return all matching person based on searchby and seachString
        /// </returns>
        List<PersonResponse> GetFilteredPerson(string searchby, string? searchString);

        /// <summary>
        /// Get Person by sort or descending
        /// </summary>
        /// <param name="allPerson"></param>
        /// <param name="sortby"></param>
        /// <param name="sortOrder"></param>
        /// <returns> Return sorting or descsending order of list</returns>
        List<PersonResponse> GetSortedPerson(List<PersonResponse> allPerson,string sortby, SortOrderOptions sortOrder);
    }
}
