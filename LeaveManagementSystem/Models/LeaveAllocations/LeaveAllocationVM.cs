﻿using LeaveManagementSystem.Models.LeaveTypes;
using LeaveManagementSystem.Models.Period;

namespace LeaveManagementSystem.Models.LeaveAllocations
{
    public class LeaveAllocationVM
    {
        public int Id { get; set; }

        [Display(Name = "Number Of Days")]
        public int Days { get; set; }

        [Display(Name = "Allocation Period")]
        public PeriodVM Period { get; set; } = new PeriodVM(); // to avoid null exception, we instantiate the class

        public LeaveTypeReadOnlyVM? LeaveType { get; set; } = new LeaveTypeReadOnlyVM();  // to avoid null exception, we instantiate the class
    }
}
