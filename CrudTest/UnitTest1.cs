using CrudTest.Services;

namespace CrudTest
{
    public class UnitTest1
    {
        /*[Fact]*/
        public void Test1()
        {
            //Arrange 
            double a = 15; double b = 30;

            //Act
            double add = Calculator.AddNumber(a, b);
            double subtract = Calculator.SubtractNumber(a, b);
            double Multiply = Calculator.MultiplyNumber(a, b);
            double Divide = Calculator.Divide(a, b);

            //Assert
            Assert.Equal(45, add);
            Assert.Equal(-15, subtract);
            Assert.Equal(450, Multiply);
            Assert.Equal(0.5, Divide);
        }
    }
}
