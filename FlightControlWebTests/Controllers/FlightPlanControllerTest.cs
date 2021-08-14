using FlightControlWeb.Controllers;
using FlightControlWeb.Models;
using FlightControlWeb.Models.DTOs;
using FlightControlWeb.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FlightControlWebTests
{
    [TestClass]
    public class FlightPlanControllerTest
    {

        [TestMethod]
        public async Task Get200OkResultForAnExistingFlightPlan()
        {
            // Arrange : Set up any prerequisites for the test to run.
            Task<FlightPlan> t = Task<FlightPlan>.FromResult(new FlightPlan { Id = "123", Passengers = 100 });
            var mockRepository = new Mock<IFlightPlanRepository>();
            mockRepository.Setup(x => x.GetFlightPlanAsync("123"))
                .Returns(t);
            var controller = new FlightPlanController(mockRepository.Object);

            // Act:Perform the test.
            OkObjectResult okResult = (await controller.CreateFlightPlan("123")) as OkObjectResult;
            FlightPlanDto fp = okResult.Value as FlightPlanDto;

            // Assert : verify that the test successed
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public  async Task GetReturnsCorrectFlightPlanWith()
        {
            // Arrange
            Task<FlightPlan> t = Task<FlightPlan>.FromResult(new FlightPlan { Id = "123",Passengers=100 });
            var mockRepository = new Mock<IFlightPlanRepository>();
            mockRepository.Setup(x => x.GetFlightPlanAsync("123"))
                .Returns(t);
            var controller = new FlightPlanController(mockRepository.Object);

            // Act
            OkObjectResult okObject =  (await controller.CreateFlightPlan("123")) as OkObjectResult;
            FlightPlanDto fp = okObject.Value as FlightPlanDto;

            // Assert
            Assert.IsNotNull(fp);
            Assert.AreEqual(fp.Passengers, 100);
        }

        [TestMethod]
        public async Task GetBadRequestForNotExistFlightPlan()
        {
            // Arrange : Set up any prerequisites for the test to run.
            FlightPlan flightPlan = null;
            Task<FlightPlan> t = Task<FlightPlan>.FromResult(flightPlan);
            var mockRepository = new Mock<IFlightPlanRepository>();
            mockRepository.Setup(x => x.GetFlightPlanAsync("125"))
                .Returns(t);
            var controller = new FlightPlanController(mockRepository.Object);

            // Act:Perform the test.
            var actionResult = (await controller.CreateFlightPlan("125")) as IStatusCodeActionResult;

            // Assert : verify that the test successed
            Assert.AreEqual(404, actionResult.StatusCode);
        }

        [TestMethod]
        public async Task Return201ForSuccessfullFlightPlanCreating()
        {
            FlightPlan fp = new FlightPlan { Id = "123", Passengers = 100 };
            // Arrange
            Task<bool> t = Task<bool>.FromResult(true);
            var mockRepository = new Mock<IFlightPlanRepository>();
            mockRepository.Setup(x => x.CreateFlightPlan(fp))
                .Returns(t);
            var controller = new FlightPlanController(mockRepository.Object);
            // Act
            var actionResult = (await controller.CreateFlightPlan(fp)) as IStatusCodeActionResult;
            CreatedAtRouteResult created = actionResult as CreatedAtRouteResult;
           // FlightPlanDto fp = actionResult. as FlightPlanDto;

            // Assert
            Assert.AreEqual(201, actionResult.StatusCode);
            Assert.AreEqual(fp.Passengers, (created.Value as FlightPlanDto).Passengers);
        }
    }
}
