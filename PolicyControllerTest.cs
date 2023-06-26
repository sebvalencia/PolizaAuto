using InsuranceApi.Controllers;
using InsuranceApi.Data;
using InsuranceApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Linq;

namespace PolizaAuto.Tests
{
    [TestFixture]
    public class PolicyControllerTests
    {
        private PolicyController _policyController;
        private InsuranceContext _insuranceContext;

        [SetUp]
        public void Setup()
        {
            // Configurar la base de datos en memoria para las pruebas
            var options = new DbContextOptionsBuilder<InsuranceContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Configurar el contexto de base de datos
            _insuranceContext = new InsuranceContext(options);

            // Inicializar algunas pólizas ficticias en la base de datos de prueba
            _insuranceContext.Policies.AddRange(
                new Policy { PolicyNumber = "P001", CustomerName = "John Doe", PolicyStartDate = DateTime.UtcNow.AddDays(-1), PolicyEndDate = DateTime.UtcNow.AddDays(7) },
                new Policy { PolicyNumber = "P002", CustomerName = "Jane Smith", PolicyStartDate = DateTime.UtcNow.AddDays(-5), PolicyEndDate = DateTime.UtcNow.AddDays(-1) },
                new Policy { PolicyNumber = "P003", CustomerName = "Alice Johnson", PolicyStartDate = DateTime.UtcNow.AddDays(-10), PolicyEndDate = DateTime.UtcNow.AddDays(-5) }
            );
            _insuranceContext.SaveChanges();

            // Configurar el controlador de pólizas con el contexto de base de datos
            _policyController = new PolicyController(_insuranceContext);
        }

        [Test]
        public void GetPolicy_ReturnsCorrectPolicy()
        {
            // Arrange
            var policyNumber = "P001";

            // Act
            var result = _policyController.GetPolicy(policyNumber);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okResult = result.Result as OkObjectResult;
            var policy = okResult.Value as Policy;

            Assert.AreEqual(policyNumber, policy.PolicyNumber);
        }

        [Test]
        public void GetPolicy_ReturnsNotFoundForInvalidPolicy()
        {
            // Arrange
            var policyNumber = "P999";

            // Act
            var result = _policyController.GetPolicy(policyNumber);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public void GetPolicies_ReturnsAllPolicies()
        {
            // Act
            var result = _policyController.GetPolicies();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okResult = result.Result as OkObjectResult;
            var policies = okResult.Value as IQueryable<Policy>;

            Assert.AreEqual(3, policies.Count());
        }

        [TearDown]
        public void TearDown()
        {
            // Eliminar la base de datos de prueba después de cada prueba
            _insuranceContext.Database.EnsureDeleted();
            _insuranceContext.Dispose();
        }
    }
}
