using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Domain
{
    public class DayModel
    {
        [Required(ErrorMessage = "Id is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }

        [Range(0, 24, ErrorMessage = "HoursWorked must be between 0 and 24.")]
        public int? HoursWorked { get; set; }

        [MaxLength(300, ErrorMessage = "WorkDescription must be less than 300 characters.")]
        public string? WorkDescription { get; set; }

        [Required(ErrorMessage = "IsApproved is required.")]
        public bool IsApproved { get; set; }

        [Required(ErrorMessage = "TimesheetId is required.")]
        public Guid TimesheetId { get; set; }

        public Guid? SupervisorId { get; set; }

        [Required(ErrorMessage = "IsSubmitted is required.")]
        public bool IsSubmitted { get; set; }

        public DateTime? LastSubmitTimestamp { get; set; }
    }
}