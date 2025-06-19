using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Tests
{
    public class CalculatorTests
    {
        [Fact]
        public void Devide_ValidInput_ReturnsQuotient()
        {
            // Arrange
            var calculator = new AppTests();

            // Act
            int result = calculator.Devide(10, 2);

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void Devide_DenominatorZero_ThrowsDivideByZeroException()
        {
            // Arrange
            var calculator = new AppTests();

            // Act & Assert
            Assert.Throws<DivideByZeroException>(() => calculator.Devide(10, 0));
        }

        [Fact]
        public void GetTasks_ReturnsListOfTasks()
        {
            // Arrange
            var app = new AppTests();

            // Act
            var tasks = app.GetTasks();

            // Assert
            Assert.NotNull(tasks);
            Assert.Equal(3, tasks.Count);
            Assert.Contains("Code", tasks);
            Assert.Contains("Test", tasks);
            Assert.Contains("Deploy", tasks);
        }

        [Theory]
        [InlineData(2, 4)]
        [InlineData(4, 16)]
        [InlineData(8, 64)]
        public void Square_ValidInput_ReturnsSquare(int input, int expected)
        {
            // Arrange
            var app = new AppTests();

            // Act
            int result = app.Square(input);

            // Assert
            Assert.Equal(expected, result);
        }
    }


    #region Snippet1
    internal class AppTests
    {
        //devide method
        public int Devide(int numerator, int denominator)
        {
            if (denominator == 0)
            {
                throw new DivideByZeroException("Denominator cannot be zero.");
            }

            return numerator / denominator;
        }

        public List<string> GetTasks() => ["Code", "Test", "Deploy"];
        public int Square(int n) => n * n;


    }
    #endregion
}
