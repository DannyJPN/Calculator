namespace Calculator.API.DTO
{
    public class CalculationExpressionRecord
    {
        public int ID { get; set; }
        public string? Record { get; set; }

        public CalculationExpressionRecord()
        { }

        public CalculationExpressionRecord(string? record)
        {
            Record = record;
        }
    }
}
