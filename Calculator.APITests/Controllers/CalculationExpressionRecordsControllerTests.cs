using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator.API.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using Calculator.API.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Calculator.API.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Linq.Expressions;

namespace Calculator.API.Controllers.Tests
{
    [TestClass]
    public class CalculationExpressionRecordsControllerTests
    {
        private Mock<ILogger<CalculationExpressionRecordsController>> _loggerMock;
        private Mock<CalculationExpressionRecordContext> _contextMock;
        private CalculationExpressionRecordsController _controller;

        [TestInitialize]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<CalculationExpressionRecordsController>>();
            _contextMock = new Mock<CalculationExpressionRecordContext>(new DbContextOptions<CalculationExpressionRecordContext>());

            var calculationRecords = new List<CalculationExpressionRecord>().AsQueryable();
            var dbSetMock = CreateDbSetMock(calculationRecords);

            _contextMock.Setup(c => c.CalculationExpressionRecord).Returns(dbSetMock.Object);
            _controller = new CalculationExpressionRecordsController(_loggerMock.Object, _contextMock.Object);
        }

        private Mock<DbSet<T>> CreateDbSetMock<T>(IQueryable<T> elements) where T : class
        {
            var dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elements.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elements.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elements.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(elements.GetEnumerator());

            dbSetMock.As<IAsyncEnumerable<T>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<T>(elements.GetEnumerator()));

   

            return dbSetMock;
        }

        [TestMethod]
        public void CalculationExpressionRecordsControllerTest()
        {
            // Ensure the controller is instantiated correctly
            Assert.IsNotNull(_controller);
        }

        [TestMethod]
        public async Task SaveCalculationRecord_ValidInput_Test()
        {
            // Test case for valid input
            string validExpr = "2+2";
            CalculationExpressionRecord validRecord = new CalculationExpressionRecord(validExpr);
            _contextMock.Setup(c => c.CalculationExpressionRecord.Add(It.IsAny<CalculationExpressionRecord>()));
            _contextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            OkObjectResult? result = await _controller.SaveCalculationRecord(validExpr) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(validRecord.Record, ((CalculationExpressionRecord)result.Value).Record);
        }

        [TestMethod]
        public async Task SaveCalculationRecord_InvalidInput_Test()
        {
            // Test case for invalid input (empty string)
            string invalidExpr = "";
            BadRequestObjectResult? result = await _controller.SaveCalculationRecord(invalidExpr) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Invalid input.", result.Value);
        }

        [TestMethod]
        public async Task SaveCalculationRecord_DatabaseUpdateException_Test()
        {
            // Test case for database update exception
            string exceptionExpr = "3+3";
            _contextMock.Setup(c => c.SaveChangesAsync(default)).ThrowsAsync(new DbUpdateException());

            ObjectResult? result = await _controller.SaveCalculationRecord(exceptionExpr) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(503, result.StatusCode);
            Assert.AreEqual("Database is currently unavailable. Please try again later.", result.Value);
        }

        [TestMethod]
        public void Calculate_ValidExpression_Test()
        {
            // Test case for valid expression
            string validExpr = "2+2";
            OkObjectResult? result = _controller.Calculate(validExpr) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("4", result.Value);
        }

        [TestMethod]
        public void Calculate_InvalidInput_Test()
        {
            // Test case for invalid input (empty string)
            string invalidExpr = "";
            BadRequestObjectResult? badRequestResult = _controller.Calculate(invalidExpr) as BadRequestObjectResult;

            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Invalid input.", badRequestResult.Value);
        }

        [TestMethod]
        public void Calculate_MalformedExpression_Test()
        {
            // Test case for malformed expression
            string malformedExpr = "2+";
            BadRequestObjectResult? malformedResult = _controller.Calculate(malformedExpr) as BadRequestObjectResult;

            Assert.IsNotNull(malformedResult);
            Assert.AreEqual(400, malformedResult.StatusCode);
            Assert.AreEqual("Invalid expression format.", malformedResult.Value);
        }

        [TestMethod]
        public void Calculate_DivideByZero_Test()
        {
            // Test case for divide by zero
            string divideByZeroExpr = "10/0";
            BadRequestObjectResult? divideByZeroResult = _controller.Calculate(divideByZeroExpr) as BadRequestObjectResult;

            Assert.IsNotNull(divideByZeroResult);
            Assert.AreEqual(400, divideByZeroResult.StatusCode);
            Assert.AreEqual("Cannot divide by zero.", divideByZeroResult.Value);
        }



        [TestMethod]
        public async Task LoadLastCalculations_InvalidCount_Test()
        {
            // Test case for invalid count (zero or negative)
            int invalidCount = 0;
            BadRequestObjectResult? result = await _controller.LoadLastCalculations(invalidCount) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Invalid count.", result.Value);
        }

        [TestMethod]
        public async Task LoadLastCalculations_DatabaseUpdateException_Test()
        {
            // Test case for database update exception
            int validCount = 5;
            _contextMock.Setup(c => c.CalculationExpressionRecord).Throws(new DbUpdateException());

            ObjectResult? result = await _controller.LoadLastCalculations(validCount) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(503, result.StatusCode);
            Assert.AreEqual("Database is currently unavailable. Please try again later.", result.Value);
        }

        internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _inner;

            public TestAsyncEnumerator(IEnumerator<T> inner)
            {
                _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            }

            public ValueTask DisposeAsync()
            {
                _inner.Dispose();
                return ValueTask.CompletedTask;
            }

            public ValueTask<bool> MoveNextAsync()
            {
                return new ValueTask<bool>(_inner.MoveNext());
            }

            public T Current => _inner.Current;
        }


    }
}