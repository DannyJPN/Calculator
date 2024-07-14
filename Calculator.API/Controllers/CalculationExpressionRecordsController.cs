using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Calculator.API.DTO;
using Calculator.API.DatabaseContexts;

namespace Calculator.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculationExpressionRecordsController : ControllerBase
    {
        private readonly ILogger<CalculationExpressionRecordsController> _logger;
        private readonly CalculationExpressionRecordContext _context;

        public CalculationExpressionRecordsController(ILogger<CalculationExpressionRecordsController> logger, CalculationExpressionRecordContext context)
        {
            _logger = logger;
            _context = context;
        }


        [HttpPost("/Save")]
        public async Task<IActionResult> SaveCalculationRecord([FromBody] CalculationExpressionRecord record)
        {
            if (record == null || string.IsNullOrEmpty(record.Record))
            {
                _logger.LogWarning("Invalid input received for creating calculation record.");
                return BadRequest("Invalid input.");
            }

            try
            {
                _context.CalculationExpressionRecord.Add(record);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Calculation record created successfully with ID: {ID}", record.ID);
                return Ok(record);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving calculation record.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("/Calculate")]
        public IActionResult Calculate([FromQuery] string expr)
        {
            // This method is currently blank as per the task instructions.
            _logger.LogInformation("Received request to calculate expression: {Expression}", expr);
            return Ok("Calculation endpoint is not yet implemented.");
        }
    }
}