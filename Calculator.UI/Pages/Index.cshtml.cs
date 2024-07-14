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
                string? result = new DataTable().Compute(Display, null).ToString();
                History.Add($"{Display} = {result}");
                Display = result == null ? "" : result;
            }
            catch (Exception ex)
            {
                Display = "Error";
                // Log the error with full stack trace
                _logger.LogError($"Error calculating expression: {ex.Message}\n{ex.StackTrace}");
            }
            return Page();
        }
    }
}