using Calculator.API.DTO;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Calculator.API.DatabaseContexts
{
    public class CalculationExpressionRecordContext : DbContext
    {
        public CalculationExpressionRecordContext(DbContextOptions<CalculationExpressionRecordContext> options) : base(options)
        {
        }
        public DbSet<CalculationExpressionRecord> CalculationExpressionRecord { get; set; }
        /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
         {
            string path = Directory.GetCurrentDirectory();
            //@"Server=(localdb)\MSSQLLocalDB;Database=CalculationHistory;Trusted_Connection=True;MultipleActiveResultSets=true"
             optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=CalculationHistory;Trusted_Connection=True;MultipleActiveResultSets=true");
        }*/
    }
}
