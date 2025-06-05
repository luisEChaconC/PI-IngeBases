using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Domain
{
    public class PendingApprovalWithEmployeeInfo
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

        // Employee information
        public string FirstName { get; set; }
        public string FirstSurname { get; set; }
        public string SecondSurname { get; set; }
        public string Cedula { get; set; }
        
        // Computed property for full name
        public string FullName => $"{FirstName} {FirstSurname} {SecondSurname}".Trim();
    }
} 