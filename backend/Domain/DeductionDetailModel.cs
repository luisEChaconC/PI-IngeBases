using System;

namespace backend.Domain
{
    public class DeductionDetailModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal AmountDeduced { get; set; }
        public decimal EmployerAmount { get; set; }
        public Guid PaymentDetailsId { get; set; }
        public string DeductionType { get; set; } 
    }
}