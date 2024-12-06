﻿
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Services.LeaveAllocations
{
    public class LeaveAllocationsService(ApplicationDbContext _context) : ILeaveAllocationsService
    {
        public async Task AllocateLeave(string employeeId)
        {
            // get all the leave types
            var leaveTypes = await _context.LeaveTypes.ToListAsync();

            // get the current period based on the year
            var currentDate = DateTime.Now;
            var year = currentDate.Year;
            var randomDay = new Random().Next(1, 32);
            var randomDateInMay = new DateTime(year, 5, randomDay);

            var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == randomDateInMay.Year);
            var monthsRemaining = period.EndDate.Month - randomDateInMay.Month;

            // foreach leave type, create an allocation entry
            foreach (var leaveType in leaveTypes)
            {
                var accuralRate = decimal.Divide(leaveType.NumberOfDays, 12);
                var leaveAllocation = new LeaveAllocation
                {
                    EmployeeId = employeeId,
                    LeaveTypeId = leaveType.Id,
                    PeriodId = period.Id,
                    Days = (int)Math.Ceiling(accuralRate * monthsRemaining)
                };

                _context.Add(leaveAllocation);
            }

            await _context.SaveChangesAsync(); // it saves in db
        }
    }
}