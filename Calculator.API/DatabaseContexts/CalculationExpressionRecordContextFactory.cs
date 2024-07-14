using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Calculator.API.DatabaseContexts
{
    public class CalculationExpressionRecordContextFactory : IDesignTimeDbContextFactory<CalculationExpressionRecordContext>
    {
        public CalculationExpressionRecordContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CalculationExpressionRecordContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=CalculationHistory;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new CalculationExpressionRecordContext(optionsBuilder.Options);
        }
    }
}
