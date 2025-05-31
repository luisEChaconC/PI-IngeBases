using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Domain
{
    public class PayrollModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "CompanyId is required.")]
        public string CompanyId { get; set; }

        [Required(ErrorMessage = "PayrollManagerId is required.")]
        public string PayrollManagerId { get; set; }

        public PayrollModel()
        {
            Id = string.Empty;
            CompanyId = string.Empty;
            PayrollManagerId = string.Empty;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }

        public PayrollModel(string id, DateTime startDate, DateTime endDate, string companyId, string payrollManagerId)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            CompanyId = companyId;
            PayrollManagerId = payrollManagerId;
        }
    }
}