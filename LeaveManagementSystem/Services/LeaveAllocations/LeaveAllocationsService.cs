
using AutoMapper;
using LeaveManagementSystem.Models.LeaveAllocations;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Services.LeaveAllocations
{
    public class LeaveAllocationsService(ApplicationDbContext _context, IHttpContextAccessor _httpContextAccessor, UserManager<ApplicationUser> _userManager, IMapper _mapper) : ILeaveAllocationsService
    {
        public async Task AllocateLeave(string employeeId)
        {
            // get all the leave types that this employee doesn't have
            var leaveTypes = await _context.LeaveTypes
                .Where(q => !q.LeaveAllocations.Any(x => x.EmployeeId == employeeId))
                .ToListAsync();

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
                // works, but not best practice
                //var allocationExists = await AllocationExists(employeeId, period.Id, leaveType.Id);

                //if (allocationExists)
                //{
                //    continue;
                //}

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

        public async Task<EmployeeAllocationVM> GetEmployeeAllocations(string? userId)
        {
            var user = string.IsNullOrEmpty(userId) 
                ? await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User) 
                : await _userManager.FindByIdAsync(userId);

            var allocations = await GetAllocations(user.Id);
            var allocationVmList = _mapper.Map<List<LeaveAllocation>, List<LeaveAllocationVM>>(allocations); // convert employee allocation list from the domain objects into the view model objects.
            var leaveTypesCount = await _context.LeaveTypes.CountAsync();

            //var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            var employeeVm = new EmployeeAllocationVM
            {
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                LeaveAllocations = allocationVmList,
                IsCompletedAllocation = leaveTypesCount == allocations.Count
            };

            return employeeVm;
        }

        public async Task<List<EmployeeListVM>> GetEmployees()
        {
            var users = await _userManager.GetUsersInRoleAsync(Roles.Employee);
            var employees = _mapper.Map<List<ApplicationUser>, List<EmployeeListVM>> (users.ToList());

            return employees;
        }

        public async Task<LeaveAllocationEditVM> GetEmployeeAllocation(int allocationId)
        {
            var allocation = await _context.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .FirstOrDefaultAsync(q => q.Id == allocationId);

            var model = _mapper.Map<LeaveAllocationEditVM>(allocation);

            return model;
        }
        public async Task EditAllocation(LeaveAllocationEditVM allocationEditVm)
        {
            //var leaveAllocation = await GetEmployeeAllocation(allocationEditVm.Id) ?? throw new Exception("Leave allocation record does not exist."); // if leaveAllocation == null, then throw exception
            //leaveAllocation.Days = allocationEditVm.Days;
            // option 1 _context.Update(leaveAllocation);
            // option 2 _context.Entry(leaveAllocation).State = EntityState.Modified;
            // await _context.SaveChangesAsync();

            await _context.LeaveAllocations
                .Where(q => q.Id == allocationEditVm.Id)
                .ExecuteUpdateAsync(s => s.SetProperty(e => e.Days, allocationEditVm.Days));
        }

        private async Task<List<LeaveAllocation>> GetAllocations(string? userId)
        {
            //string employeeId = string.Empty;
            //if(string.IsNullOrEmpty(userId))
            //{
            //    employeeId = userId;
            //}
            //else
            //{
            //    var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User); // allows me to peek into the HTTP context associated with the request that is called in this method
            //    employeeId = user.Id;
            //}
            var currentDate = DateTime.Now;
            // following the date of the tutorial to compare the codes
            var year = currentDate.Year;
            var randomDay = new Random().Next(1, 32);
            var randomDateInMay = new DateTime(year, 5, randomDay);

            var leaveAllocations = await _context.LeaveAllocations
                .Include(q => q.LeaveType) // Inner join statement
                                           //.Include(q => q.Employee)
                .Include(q => q.Period)
                .Where(q => q.EmployeeId == userId && q.Period.EndDate.Year == randomDateInMay.Year)
                .ToListAsync();

            return leaveAllocations;
        }

        private async Task<bool> AllocationExists(string userId, int periodId, int LeaveTypeId)
        {
            var exists = await _context.LeaveAllocations.AnyAsync(q =>
                q.EmployeeId == userId
                && q.PeriodId == periodId
                && q.LeaveTypeId == LeaveTypeId
            );

            return exists;
        }
    }
}
