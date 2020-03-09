using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Practice.Core.Applications.WebApplication.Controllers;
using Practice.Core.DataAccess.Interfaces;
using Practice.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Practice.Core.Test
{
    [TestClass]
    public class PracticeCoreUnitTestClass
    {
        [TestMethod]        
        public void HomeControllerTest()
        {
            var homeController = new HomeController();
            var actionResult = homeController.Index();
            string viewBagMessage = homeController.ViewBag?.Message ?? string.Empty;

            Assert.IsNotNull(viewBagMessage);
            Assert.AreNotEqual(string.Empty, viewBagMessage);
            Assert.IsNotNull(actionResult);            
        }

        [TestMethod]
        public void EmployeesControllerTest()
        {
            var moqReporsitory = new MockRepository(MockBehavior.Default);

            List<Employees> mockEmployees = new List<Employees> { 
                new Employees { ID = 1, FirstName = "Test 1", MiddleName = "Test 2", LastName = "Test 3", Salary = 1000 },
                new Employees { ID = 2, FirstName = "Test 2", MiddleName = "Test 2", LastName = "Test 3", Salary = 1000 },
                new Employees { ID = 3, FirstName = "Test 3", MiddleName = "Test 2", LastName = "Test 3", Salary = 1000 },
                new Employees { ID = 4, FirstName = "Test 4", MiddleName = "Test 2", LastName = "Test 3", Salary = 1000 }
            };

            var employeesRepository = moqReporsitory.Create<IEmployeesRepository>();

            //This line means - Whenerver "GetEmployees" function get called then return "mockEmployees" list
            employeesRepository.Setup(repository => repository.GetEmployees()).Returns(mockEmployees);

            var employeesCOntroller = new EmployeesController(employeesRepository.Object);

            var actionResult = employeesCOntroller.GetEmployees() as OkObjectResult;

            Assert.AreEqual((actionResult.Value as List<Employees>).Count(), mockEmployees.Count);

            // This lines verifies that "GetEmployees" function should get invoked or called atleast once.
            // This is called behavioural testing
            employeesRepository.Verify(repository => repository.GetEmployees(), times: Times.Exactly(1));
        }
    }
}
