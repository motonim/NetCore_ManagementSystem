
using AutoMapper;
using LeaveManagementSystem.Models.LeaveAllocations;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Services.LeaveAllocations
{
    public class LeaveAllocationsService(ApplicationDbContext _context, IHttpContextAccessor _httpContextAccessor, UserManager<ApplicationUser> _userManager, IMapper _mapper) : ILeaveAllocationsService
    {
        public async Task AllocateLeave(string employeeId)
        {
            // get all the leave types
            var leaveTypes = await _context.LeaveTypes.ToListAsync();

            // get the current period based on the year
            var currentDate = DateTime.Now;
            // following the date of the tutorial to compare the codes
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


        public async Task<List<LeaveAllocation>> GetAllocations()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User); // allows me to peek into the HTTP context associated with the request that is called in this method
            var currentDate = DateTime.Now;
            // following the date of the tutorial to compare the codes
            var year = currentDate.Year;
            var randomDay = new Random().Next(1, 32);
            var randomDateInMay = new DateTime(year, 5, randomDay);

            var leaveAllocations = await _context.LeaveAllocations
                .Include(q => q.LeaveType) // Inner join statement
                .Include(q => q.Employee)
                .Include(q => q.Period)
                .Where(q => q.EmployeeId == user.Id && q.Period.EndDate.Year == randomDateInMay.Year)
                .ToListAsync();

            return leaveAllocations;

        }

        public async Task<EmployeeAllocationVM> GetEmployeeAllocations()
        {
            var allocations = await GetAllocations();
            var allocationVmList = _mapper.Map<List<LeaveAllocation>, List<LeaveAllocationVM>>(allocations); // convert employee allocation list from the domain objects into the view model objects.

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            var employeeVm = new EmployeeAllocationVM
            {
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                LeaveAllocations = allocationVmList
            };

            return employeeVm;
        }
    }
}
