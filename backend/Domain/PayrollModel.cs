using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Domain
{
    public class PayrollModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "CompanyId is required.")]
        public Guid CompanyId { get; set; }

        [Required(ErrorMessage = "PayrollManagerId is required.")]
        public Guid PayrollManagerId { get; set; }

        public PayrollModel()
        {
            Id = Guid.Empty;
            CompanyId = Guid.Empty;
            PayrollManagerId = Guid.Empty;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }

        public PayrollModel(Guid id, DateTime startDate, DateTime endDate, Guid companyId, Guid payrollManagerId)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            CompanyId = companyId;
            PayrollManagerId = payrollManagerId;
        }
    }
}