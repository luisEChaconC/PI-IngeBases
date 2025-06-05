using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Domain
{
    public class PendingApprovalSummary
    {
        [Required(ErrorMessage = "EmployeeId is required.")]
        public Guid EmployeeId { get; set; }

        [Required(ErrorMessage = "PendingDaysCount is required.")]
        public int PendingDaysCount { get; set; }

        public DateTime? LatestSubmissionTimestamp { get; set; }

        [Required(ErrorMessage = "EarliestDayDate is required.")]
        public DateTime EarliestDayDate { get; set; }

        [Required(ErrorMessage = "LatestDayDate is required.")]
        public DateTime LatestDayDate { get; set; }
    }
} 