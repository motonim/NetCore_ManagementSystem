namespace LeaveManagementSystem.Data
{
    public class LeaveAllocation : BaseEntity
    {
        /* foreign key to LeaveType */
        public LeaveType? LeaveType { get; set; }
        public int LeaveTypeId { get; set; }

        /* foreign key to Employee */
        public ApplicationUser Employee { get; set; }
        public string EmployeeId { get; set; }

        /* foreign key to Period */
        public Period Period { get; set; }
        public int PeriodId { get; set; }

        public int Days { get; set; }
    }
}
