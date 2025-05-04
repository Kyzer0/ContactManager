using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudTest.Services
{
    public class Calculator
    {
        public static double AddNumber(double a, double b) => a + b;
        public static double SubtractNumber(double a, double b) => a - b;
        public static double MultiplyNumber(double a, double b) => a * b;
        public static double Divide(double a, double b)
        {
            if (b == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero");
            }
            return a / b;
        }
    }
}
