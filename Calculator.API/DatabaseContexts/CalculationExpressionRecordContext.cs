using Calculator.API.DTO;
using Microsoft.EntityFrameworkCore;

namespace Calculator.API.DatabaseContexts
{
    public class CalculationExpressionRecordContext : DbContext
    {
        public CalculationExpressionRecordContext(DbContextOptions<CalculationExpressionRecordContext> options) : base(options)
        {
        }

        public DbSet<CalculationExpressionRecord> CalculationExpressionRecord { get; set; }
    }
}