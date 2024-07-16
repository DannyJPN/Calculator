using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace Calculator.UI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        [BindProperty]
        public string Display { get; set; } = string.Empty;

        public List<string> History { get; set; } = new List<string>();

        public IActionResult OnPostCalculate()
        {
            try
            {
                // Validate the expression format
                if (!IsValidExpression(Display))
                {
                    Display = "Invalid expression format.";
                    _logger.LogWarning("Invalid expression format: {Expression}", Display);
                    return Page();
                }

                string? result = new DataTable().Compute(Display, null).ToString();
                if (result == null)
                {
                    Display = "Error";
                    _logger.LogError("Calculation result is null for expression: {Expression}", Display);
                }
                else
                {
                    History.Add($"{Display} = {result}");
                    Display = result;
                }
            }
            catch (DivideByZeroException ex)
            {
                Display = "Cannot divide by zero.";
                _logger.LogError(ex, "Divide by zero error in expression: {Expression}", Display);
            }
            catch (Exception ex)
            {
                Display = "Error";
                _logger.LogError(ex, "Error calculating expression: {Expression}", Display);
            }
            return Page();
        }

        private bool IsValidExpression(string expression)
        {
            expression = expression.Replace(" ", "");
            char[] operators = { '+', '-', '*', '/' };
            foreach (char op in operators)
            {
                string[] parts = expression.Split(op);
                if (parts.Length == 2)
                {
                    if (double.TryParse(parts[0], out double _) && double.TryParse(parts[1], out double _))
                    {
                        return true;
                    }
                }
            }
            return double.TryParse(expression, out double _);
        }
    }
}