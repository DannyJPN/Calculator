using System;
using System.IO;

namespace Calculator.MathLib
{
    public static class MathLib
    {
        public static readonly char[] Operators = { '+', '-', '*', '/' };

        public static double Calculate(string expression, bool integersOnly = false)
        {
            try
            {
                // Remove whitespaces from the input string
                expression = expression.Replace(" ", string.Empty);

                // Validate the expression format
                if (!IsValidExpression(expression))
                {
                    Console.WriteLine("Invalid expression format.");
                    return double.NaN;
                }

                // Evaluate the expression
                double result = EvaluateExpression(expression);

                // Apply integers only restriction if required
                if (integersOnly)
                {
                    result = Math.Floor(result);
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating expression: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        private static bool IsValidExpression(string expression)
        {
            // Check if the expression is a valid double or in the format "double operator double"
            foreach (char op in Operators)
            {
                string[] parts = expression.Split(new[] { op }, StringSplitOptions.None);
                if (parts.Length == 2)
                {
                    if (double.TryParse(parts[0], out _) && double.TryParse(parts[1], out _))
                    {
                        return true;
                    }
                }
            }

            // Check if the expression is a valid single number
            return double.TryParse(expression, out _);
        }

        private static double EvaluateExpression(string expression)
        {
            try
            {
                // Split the expression into two parts: number1, operator, number2
                foreach (char op in Operators)
                {
                    string[] parts = expression.Split(new[] { op }, StringSplitOptions.None);
                    if (parts.Length == 2)
                    {
                        double number1 = double.Parse(parts[0]);
                        double number2 = double.Parse(parts[1]);
                        switch (op)
                        {
                            case '+':
                                return number1 + number2;
                            case '-':
                                return number1 - number2;
                            case '*':
                                return number1 * number2;
                            case '/':
                                if (number2 == 0)
                                    throw new DivideByZeroException("Division by zero.");
                                return number1 / number2;
                            default:
                                throw new ArgumentException("Invalid operator.");
                        }
                    }
                }

                return double.Parse(expression);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error evaluating expression: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }
    }
}