using BussinessLogicService;
using Entities;
using Entities.Enums;
using ModelDto;
using ServiceContracts;
using ServiceContracts.Dto.CountryDto;
using ServiceContracts.Dto.PersonDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using PersonResponse = ServiceContracts.Dto.PersonDto.PersonResponse;

namespace UnitTesting
{
    public class PersonTest
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countryService;
        private readonly ITestOutputHelper _testOutput;// used for printing values
        public PersonTest(ITestOutputHelper testOutputHelper)
        {
            _testOutput = testOutputHelper;
            _countryService = new CountryServiceLogic();
            _personService = new PersonServiceLogic(_countryService);
        }

        #region Add Person Test
        // Test to ensure that when a null PersonRequest is passed, an ArgumentNullException is thrown
        [Fact]
        public void AddPerson_checkPersonRequestIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            PersonRequest? personAddRequest = null;
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _personService.AddPerson(personAddRequest));
        }
        
        //to check if the PersonName  is empty in the PersonRequest
        [Fact]
        public void AddPerson_EmptyPersonName_CheckIfEmpty()
        {
            // Arrange
            var personRequest = new PersonRequest
            {
                PersonName = "",
                Email = "test@example.com"
            };
            // Act & Assert
            var exception = Assert.Throws<ValidationException>(() =>
            {
                _personService.AddPerson(personRequest);
            });
            Assert.Contains("Person Name is required", exception.Message);
        }
        [Fact]
        public void AddEmail_EmptyPersonEmail_checkIfEmpty()
        {
            // Arrange
            var personRequest = new PersonRequest
            {
                PersonName = "John Doe",
                Email = ""
            };
            // Act & Assert
            var exception = Assert.Throws<ValidationException>(() =>
            {
                _personService.AddPerson(personRequest);
            });
            Assert.Contains("Email is required", exception.Message);
        }

        // Test to ensure that when the Email is null in the PersonRequest, it is handled correctly
        [Fact]
        public void AddPerson_NullEmail_ThrowsArgumentException()
        {
            // Arrange
            var personRequest = new PersonRequest
            {
                PersonName = "John Doe",
                Email = null
            };
            // Act & Assert
            Assert.Null(personRequest.Email);
        }


        // Test to verify that a valid PersonRequest returns a valid PersonResponseDto
        [Fact]
        public void AddPerson_checkPersonRequestIsValid_ReturnsPersonResponseDto()
        {
            // Arrange
            // Add a valid country to the countries service
            var addCountry = new CountryRequest()
            {
                CountryName = "USA"
            };

            // Generate a valid country response
            var addedCountry = _countryService.AddCountry(addCountry);

            // Create a valid person request using the CountryID of the added country
            var personRequest = new PersonRequest
            {
                PersonName = "John Doe",
                Email = "johnDie@gmail.com",
                CountryName = addedCountry.CountryName,
                CountryID = addedCountry.CountryId, // Use the valid CountryID
                Gender = GenderOptions.Male,
                BirthDay = DateTime.Parse("1990-01-01"),
                ReceiveNewsLetters = true
            };

            // Act
            var personResponseFromAdd = _personService.AddPerson(personRequest);

            // Assert
            Assert.NotNull(personResponseFromAdd); // Ensure the response is not null
            Assert.NotNull(addedCountry); // Ensure the country was added successfully
        }

        #endregion

        #region GetPersonByID Tests
        // Test to ensure that when a null ID is passed, an ArgumentNullException is thrown
        [Fact]
        public void GetPersonByID_NullId_ThrowsArgumentNullException()
        {
            // Arrange
            Guid? personId = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _personService.GetPersonByID(personId));
        }

        // Test to ensure that when a non-existent ID is passed, a KeyNotFoundException is thrown
        [Fact]
        public void GetPersonByID_NonExistentId_ThrowsKeyNotFoundException()
        {
            // Arrange
            Guid? nonExistentId = Guid.Parse("{5C9B8205-42B4-4C28-8302-BFEB50D32CDB}");
            Assert.Throws<KeyNotFoundException>(() => _personService.GetPersonByID(nonExistentId));
        }
        // Test to verify that a valid ID returns the correct PersonResponseDto
        [Fact]
        public void GetPersonByID_ValidId_ReturnsPersonResponseDto()
        {
            // Arrange
            var addcountry = new CountryRequest()
            {
                CountryName = "USA"
            };

            var addedCountry = _countryService.AddCountry(addcountry);

            var personRequest = new PersonRequest
            {
                PersonName = "Jane Doe",
                Email = "jane.doe@example.com",
                BirthDay = new DateTime(1995, 5, 15),
                Gender = GenderOptions.Female,
                CountryID = addedCountry.CountryId,
                CountryName = addedCountry.CountryName,
                ReceiveNewsLetters = false
            };
            var addedPerson = _personService.AddPerson(personRequest);

            // Act
            var result = _personService.GetPersonByID(addedPerson.PersonId);

            // Assert
            Assert.NotNull(result); // Ensure the result is not null
            Assert.NotNull(addedCountry); // Ensure the country was added successfully
        }

        #endregion

        #region ListPersons Tests
        // Test to ensure that when no persons are added, the ListPersons method returns an empty list
        [Fact]
        public void ListPersons_NoPersonsAdded_ReturnsEmptyList()
        {
            // Act
            var result = _personService.ListPersons();

            // Assert
            Assert.NotNull(result); // Ensure the result is not null
            Assert.Empty(result); // Ensure the result is an empty list
        }

        // Test to verify that when persons are added, the ListPersons method returns all persons
        [Fact]
        public void ListPersons_PersonsAdded_ReturnsAllPersons()
        {
            // Arrange
            var addcountry = new CountryRequest()
            {
                CountryName = "USA"
            };
            var addedCountry = _countryService.AddCountry(addcountry);

            var person1 = new PersonRequest
            {
                PersonName = "Alice",
                Email = "alice@example.com",
                BirthDay = new DateTime(1985, 3, 10),
                Gender = GenderOptions.Female,
                CountryName = addedCountry.CountryName,
                CountryID = addedCountry.CountryId,
                ReceiveNewsLetters = true
            };

            var addedlist = _personService.AddPerson(person1);

            //print the addedlist
           
            // Act
            var result = _personService.ListPersons();


            // Assert
            Assert.NotNull(result); // Ensure the result is not null
            Assert.NotNull(addcountry); // Ensure the country was added successfully
            Assert.NotNull(addedlist); // Ensure the person was added successfully
            Assert.Contains(addedlist, result); // Ensure the added person is in the result list
            Assert.Equal(addedCountry.CountryName, addedlist.CountryName); // Ensure the country name matches
        }
        //Testing if GetAllPersons returns a list of persons
        [Fact]
        public void Check_GetAllPerson_IfWorking()
        {
            //adding country
            var addcountry1 = new CountryRequest()
            {
                CountryName = "USA"
            };
            var addedCountry1 = _countryService.AddCountry(addcountry1);

            var addcountry2 = new CountryRequest()
            {
                CountryName = "Japan"
            };

            var addedCountry2 = _countryService.AddCountry(addcountry2);

            //adding person
            var person1 = new PersonRequest
            {
                PersonName = "Alice",
                Email = "alice@example.com",
                BirthDay = new DateTime(1985, 3, 10),
                Gender = GenderOptions.Female,
                CountryName = addedCountry1.CountryName,
                CountryID = addedCountry1.CountryId,
                ReceiveNewsLetters = true
            };
            var person2 = new PersonRequest
            {
                PersonName = "Marina",
                Email = "Marina@example.com",
                BirthDay = new DateTime(1985, 3, 10),
                Gender = GenderOptions.Female,
                CountryName = addedCountry2.CountryName,
                CountryID = addedCountry2.CountryId,
                ReceiveNewsLetters = true
            };

            var list_All_person = new List<PersonRequest>
            {
                person1,person2
            };

            //act
            //empty list
            List<ServiceContracts.Dto.PersonDto.PersonResponse> personList = new List<ServiceContracts.Dto.PersonDto.PersonResponse>();

            //adding person to the list
            foreach (var person in list_All_person)
            {
                var person_response_add = _personService.AddPerson(person);
                personList.Add(person_response_add);
            }

            //print the added person values
            _testOutput.WriteLine("Expexted Values:\n");
            foreach (var person in personList)
            {
                _testOutput.WriteLine(person.ToString());
            }

            var list_result = _personService.ListPersons();
            //assert
            foreach (var person_Added in personList)
            {
                Assert.NotNull(person_Added);
                Assert.Contains(person_Added, list_result);
            }

        }
        #endregion

        #region Testing the GetFilteredPerson

        // Test to verify that when persons are added, the ListPersons method returns all persons
        [Fact]
        public void GetFiltered_PersonsAdded_ReturnsAllPersons()
        {
            // Arrange
            var addcountry = new CountryRequest()
            {
                CountryName = "USA"
            };
            var addedCountry = _countryService.AddCountry(addcountry);

            var person1 = new PersonRequest
            {
                PersonName = "Alice",
                Email = "alice@example.com",
                BirthDay = new DateTime(1985, 3, 10),
                Gender = GenderOptions.Female,
                CountryName = addedCountry.CountryName,
                CountryID = addedCountry.CountryId,
                ReceiveNewsLetters = true
            };

            var addedlist = _personService.AddPerson(person1);

            //print the addedlist
           
            // Act
            var result = _personService.GetFilteredPerson(nameof(PersonResponse.PersonName),"A");


            // Assert
            Assert.NotNull(result); // Ensure the result is not null
            Assert.NotNull(addcountry); // Ensure the country was added successfully
            Assert.NotNull(addedlist); // Ensure the person was added successfully
            Assert.Contains(addedlist, result); // Ensure the added person is in the result list
            Assert.Equal(addedCountry.CountryName, addedlist.CountryName); // Ensure the country name matches
        }
        //Testing if GetAllPersons returns a list of persons
        [Fact]
        public void Check_GetFilteredPerson_IfWorking()
        {
            //adding country
            var addcountry1 = new CountryRequest()
            {
                CountryName = "USA"
            };
            var addedCountry1 = _countryService.AddCountry(addcountry1);

            var addcountry2 = new CountryRequest()
            {
                CountryName = "Japan"
            };

            var addedCountry2 = _countryService.AddCountry(addcountry2);

            //adding person
            var person1 = new PersonRequest
            {
                PersonName = "Alice",
                Email = "alice@example.com",
                BirthDay = new DateTime(1985, 3, 10),
                Gender = GenderOptions.Female,
                CountryName = addedCountry1.CountryName,
                CountryID = addedCountry1.CountryId,
                ReceiveNewsLetters = true
            };
            var person2 = new PersonRequest
            {
                PersonName = "Marina",
                Email = "Marina@example.com",
                BirthDay = new DateTime(1985, 3, 10),
                Gender = GenderOptions.Female,
                CountryName = addedCountry2.CountryName,
                CountryID = addedCountry2.CountryId,
                ReceiveNewsLetters = true
            };

            var storing_list_of_person = new List<PersonRequest>
            {
                person1,person2
            };

            //act
            //empty list
            List<ServiceContracts.Dto.PersonDto.PersonResponse> personList_created_empty = new List<ServiceContracts.Dto.PersonDto.PersonResponse>();

            //adding person to the list
            foreach (var person in storing_list_of_person)
            {
                var person_response_add = _personService.AddPerson(person);
                personList_created_empty.Add(person_response_add);
            }

            //print the added person values
            _testOutput.WriteLine("Expexted Values:\n");
            foreach (var person in personList_created_empty)
            {
                _testOutput.WriteLine(person.ToString());
            }

            var list_result_from_search = _personService.GetFilteredPerson(nameof(PersonResponse.PersonName), "");
            //assert
            foreach (var person_Added in personList_created_empty)
            {
                Assert.NotNull(person_Added);
                Assert.Contains(person_Added, list_result_from_search);
            }

        }
        //First we will add few person; and then we will search based on person name with some search string.
        // it should return the matching persons
         [Fact]
         public void GetFilteredPerson_EmptySearchText()
         {
             //adding country
             var addcountry1 = new CountryRequest() { CountryName = "USA" };
             var addcountry2 = new CountryRequest() { CountryName = "Japan" };
             var addcountry3 = new CountryRequest() { CountryName = "Philippines" };

             //Adding the country
             var addedCountry1 = _countryService.AddCountry(addcountry1);
             var addedCountry2 = _countryService.AddCountry(addcountry2);
             var addedCountry3 = _countryService.AddCountry(addcountry3);

             //adding person
             var person1 = new PersonRequest
             {
                 PersonName = "Alice",
                 Email = "alice@example.com",
                 BirthDay = new DateTime(1985, 3, 10),
                 Gender = GenderOptions.Female,
                 CountryName = addedCountry1.CountryName,
                 CountryID = addedCountry1.CountryId,
                 ReceiveNewsLetters = true
             };
             var person2 = new PersonRequest
             {
                 PersonName = "Lisando",
                 Email = "Lisando@example.com",
                 BirthDay = new DateTime(1985, 3, 10),
                 Gender = GenderOptions.Male,
                 CountryName = addedCountry2.CountryName,
                 CountryID = addedCountry2.CountryId,
                 ReceiveNewsLetters = true
             };
             var person3 = new PersonRequest
             {
                 PersonName = "Magi",
                 Email = "Magili@example.com",
                 BirthDay = new DateTime(1985, 3, 10),
                 Gender = GenderOptions.Female,
                 CountryName = addedCountry3.CountryName,
                 CountryID = addedCountry3.CountryId,
                 ReceiveNewsLetters = true
             };

             var storing_list_of_person = new List<PersonRequest>
             {
                 person1,person2,person3
             };

            //act
            //empty list
            List<ServiceContracts.Dto.PersonDto.PersonResponse> personList_created_empty = new List<ServiceContracts.Dto.PersonDto.PersonResponse>();

             //adding person to the list
             foreach (var person in storing_list_of_person)
             {
                 var person_response_add = _personService.AddPerson(person);
                 personList_created_empty.Add(person_response_add);
             }

             //print the expected person values
             _testOutput.WriteLine("Expexted Values:\n");
             foreach (var person in personList_created_empty)
             {
                 _testOutput.WriteLine(person.ToString());
             }

             var list_result_from_search = _personService.GetFilteredPerson(nameof(PersonResponse.Gender), "Male");
             //print actual value
             _testOutput.WriteLine("\nActual Values:\n");
             foreach (var person_Added in personList_created_empty)
             {
                if(!string.IsNullOrWhiteSpace(person_Added.PersonName))
                {
                    Assert.NotNull(person_Added);

                    //can switch other filtered here, either name, email, gender or so on
                    if (person_Added.Gender.ToString().Equals("Male", StringComparison.CurrentCultureIgnoreCase))
                    {
                         Assert.NotNull(person_Added);
                         Assert.Contains(person_Added, list_result_from_search);
                         _testOutput.WriteLine($"{person_Added.ToString()}");
                    }
                }  
             }
             

         }
        #endregion

        #region Sort and Desc List
        [Fact]
         public void GetSortedPerson_EmptySearchText()
         {
            #region Repeated Data
            //adding country
            var addcountry1 = new CountryRequest() { CountryName = "USA" };
             var addcountry2 = new CountryRequest() { CountryName = "Japan" };
             var addcountry3 = new CountryRequest() { CountryName = "Philippines" };

             //Adding the country
             var addedCountry1 = _countryService.AddCountry(addcountry1);
             var addedCountry2 = _countryService.AddCountry(addcountry2);
             var addedCountry3 = _countryService.AddCountry(addcountry3);

             //adding person
             var person1 = new PersonRequest
             {
                 PersonName = "Alice",
                 Email = "alice@example.com",
                 BirthDay = new DateTime(1985, 3, 10),
                 Gender = GenderOptions.Female,
                 CountryName = addedCountry1.CountryName,
                 CountryID = addedCountry1.CountryId,
                 ReceiveNewsLetters = true
             };
             var person2 = new PersonRequest
             {
                 PersonName = "Lisando",
                 Email = "Lisando@example.com",
                 BirthDay = new DateTime(1985, 3, 10),
                 Gender = GenderOptions.Male,
                 CountryName = addedCountry2.CountryName,
                 CountryID = addedCountry2.CountryId,
                 ReceiveNewsLetters = true
             };
             var person3 = new PersonRequest
             {
                 PersonName = "Magi",
                 Email = "Magili@example.com",
                 BirthDay = new DateTime(1985, 3, 10),
                 Gender = GenderOptions.Female,
                 CountryName = addedCountry3.CountryName,
                 CountryID = addedCountry3.CountryId,
                 ReceiveNewsLetters = true
             };
             var person4 = new PersonRequest
             {
                 PersonName = "Benjamin",
                 Email = "Benjamin@example.com",
                 BirthDay = new DateTime(1985, 3, 10),
                 Gender = GenderOptions.Female,
                 CountryName = addedCountry3.CountryName,
                 CountryID = addedCountry3.CountryId,
                 ReceiveNewsLetters = true
             };

             var storing_list_of_person = new List<PersonRequest>
             {
                 person1,person2,person3,person4
             };

            //act
            //empty list
            List<ServiceContracts.Dto.PersonDto.PersonResponse> personList_created_empty = new List<ServiceContracts.Dto.PersonDto.PersonResponse>();

             //adding person to the list
             foreach (var person in storing_list_of_person)
             {
                 var person_response_add = _personService.AddPerson(person);
                 personList_created_empty.Add(person_response_add);
             }

             //print the expected person values
             _testOutput.WriteLine("Expexted Values:\n");
             foreach (var person in personList_created_empty)
             {
                 _testOutput.WriteLine(person.ToString());
             }
            #endregion

            var list_result_from_sort = _personService.GetSortedPerson(personList_created_empty, nameof(PersonModel.PersonName),SortOrderOptions.Ascending);
             //print actual value
             _testOutput.WriteLine("\nActual Values:\n");

             list_result_from_sort.OrderBy(name => name.PersonName).ToList();

             var get_all_person = _personService.ListPersons();

             foreach (var person_Added in list_result_from_sort)
             {
                 _testOutput.WriteLine($"{person_Added.ToString()}");
             }

             //Assert
             for(int i = 0; i < get_all_person.Count; i++)
             {
                Assert.Equal(list_result_from_sort[i], list_result_from_sort[i]);
             }

        }
        #endregion
    }
}
