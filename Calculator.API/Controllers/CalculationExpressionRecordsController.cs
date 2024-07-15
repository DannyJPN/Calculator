using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Calculator.API.DTO;
using Calculator.API.DatabaseContexts;
using Calculator.MathLib;

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
        public async Task<IActionResult> SaveCalculationRecord([FromQuery] string expr)
        {
            if (string.IsNullOrEmpty(expr))
            {
                _logger.LogWarning("Invalid input received for creating calculation record.");
                return BadRequest("Invalid input.");
            }

            try
            {
                CalculationExpressionRecord record = new CalculationExpressionRecord(expr);
                _context.CalculationExpressionRecord.Add(record);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Calculation record created successfully");
                return Ok(record);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error while saving calculation record.");
                return StatusCode(503, "Database is currently unavailable. Please try again later.");
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
            if (string.IsNullOrEmpty(expr))
            {
                _logger.LogWarning("Invalid input received for calculation.");
                return BadRequest("Invalid input.");
            }

            try
            {
                double result = MathLib.MathLib.Calculate(expr);
                if (double.IsNaN(result))
                {
                    _logger.LogWarning("Invalid expression format: {Expression}", expr);
                    return BadRequest("Invalid expression format.");
                }
                _logger.LogInformation("Calculation successful: {Expression} = {Result}", expr, result);
                return Ok(result.ToString());
            }
            catch (DivideByZeroException ex)
            {
                _logger.LogError(ex, "Divide by zero error in expression: {Expression}", expr);
                return BadRequest("Cannot divide by zero.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating expression.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("/LoadLastCalculations")]
        public async Task<IActionResult> LoadLastCalculations([FromQuery] int count)
        {
            if (count <= 0)
            {
                _logger.LogWarning("Invalid count received for loading last calculations.");
                return BadRequest("Invalid count.");
            }

            try
            {
                List<string?> records = await _context.CalculationExpressionRecord
                    .OrderByDescending(r => r.ID)
                    .Take(count)
                    .Select(r => r.Record)
                    .ToListAsync();

                CalculationHistoryList result = new CalculationHistoryList { Records = records };
                _logger.LogInformation("Loaded last {Count} calculation records successfully", count);
                return Ok(result);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error while loading last calculation records.");
                return StatusCode(503, "Database is currently unavailable. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading last calculation records.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}