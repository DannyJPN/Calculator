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
                var result = new DataTable().Compute(Display, null);
                History.Add($"{Display} = {result}");
                Display = result.ToString();
            }
            catch (Exception ex)
            {
                Display = "Error";
                // Log the error with full stack trace
                Console.WriteLine($"Error calculating expression: {ex.Message}\n{ex.StackTrace}");
            }
            return Page();
        }
    }
}
