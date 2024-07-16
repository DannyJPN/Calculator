using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator.MathLib;
using System;

namespace Calculator.MathLib.Tests
{
    [TestClass]
    public class MathLibTests
    {
        [TestMethod]
        public void Calculate_Addition_ReturnsCorrectResult()
        {
            double result = MathLib.Calculate("2+3");
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void Calculate_Subtraction_ReturnsCorrectResult()
        {
            double result = MathLib.Calculate("5-3");
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void Calculate_Multiplication_ReturnsCorrectResult()
        {
            double result = MathLib.Calculate("4*2");
            Assert.AreEqual(8, result);
        }

        [TestMethod]
        public void Calculate_Division_ReturnsCorrectResult()
        {
            double result = MathLib.Calculate("10/2");
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void Calculate_DivisionByZero_ThrowsDivideByZeroException()
        {
            Assert.ThrowsException<DivideByZeroException>(() => MathLib.Calculate("10/0"));
        }

        [TestMethod]
        public void Calculate_InvalidExpression_ReturnsNaN()
        {
            double result = MathLib.Calculate("2+");
            Assert.IsTrue(double.IsNaN(result));
        }

        [TestMethod]
        public void Calculate_IntegersOnly_ReturnsIntegerResult()
        {
            double result = MathLib.Calculate("5/2", true);
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void Calculate_ValidExpressionWithSpaces_ReturnsCorrectResult()
        {
            double result = MathLib.Calculate(" 3 + 4 ");
            Assert.AreEqual(7, result);
        }
    }
}