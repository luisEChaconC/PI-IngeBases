namespace backend.Models
{
    public class Benefit
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } // On - Off
        public string Type { get; set; } // API-Percentage-Ammount
        public string LinkAPI { get; set; }
        public int FixedPercentage { get; set; }
        public int FixedAmount { get; set; }
        public int RequiredMonthsWorked { get; set; } // Months?
        public List<string> EligibleEmployeeTypes { get; set; }
    }

}
