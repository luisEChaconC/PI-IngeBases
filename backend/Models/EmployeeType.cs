using System.Collections.Generic;
using System;

namespace backend.Models
{
    public class EmployeeType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        // Navigation: Many benefits can apply to one employee type
        public List<Benefit> Benefits { get; set; }
    }
}
