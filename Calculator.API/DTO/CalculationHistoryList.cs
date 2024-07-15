using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Calculator.API.DTO
{
    public class CalculationHistoryList
    {
        [JsonPropertyName("records")]
        public List<string?> Records { get; set; } = new List<string>();
    }
}