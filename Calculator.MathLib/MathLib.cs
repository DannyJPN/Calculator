using System;

namespace Calculator.MathLib
{
    public static class MathLib
    {
        public static readonly char[] Operators = { '+', '-', '*', '/' };

        public static double Calculate(string expression)
        {
            try
            {
                // Remove whitespaces from the input string
                expression = expression.Replace(" ", string.Empty);

                // Evaluate the expression
                return EvaluateExpression(expression);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating expression: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        private static double EvaluateExpression(string expression)
        {
            try
            {
                // Split the expression into two parts: number1, operator, number2
                foreach (var op in Operators)
                {
                    var parts = expression.Split(new[] { op }, StringSplitOptions.None);
                    if (parts.Length == 2)
                    {
                        var number1 = double.Parse(parts[0]);
                        var number2 = double.Parse(parts[1]);
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

                throw new ArgumentException("Invalid expression format.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error evaluating expression: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }
    }
}