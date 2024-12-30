using AutoMapper;
using LeaveManagementSystem.Models.LeaveRequests;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Services.LeaveRequests
{
    public class LeaveRequestsService(IMapper _mapper, UserManager<ApplicationUser> _userManager, IHttpContextAccessor _httpContextAccessor, ApplicationDbContext _context) : ILeaveRequestsService
    {
        public Task CancelLeaveRequest(int leaveRequestId)
        {
            throw new NotImplementedException();
        }

        public async Task CreateLeaveRequest(LeaveRequestCreateVM model)
        {
            // Map data to leave request data model
            var leaveRequest = _mapper.Map<LeaveRequest>(model);

            // get logged in employee id
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            leaveRequest.EmployeeId = user.Id;

            // set LeaveRequestStatusId to pending
            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Pending;

            // save leave request
            //_context.LeaveRequests.Add(leaveRequest); same as the next two lines(_context.Add(leaveRequest); and _context.SaveChanges();)
            _context.Add(leaveRequest);

            // deduct allocation days based on request
            var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;
            var allocationToDeduct = await _context.LeaveAllocations
                .FirstAsync(q => q.EmployeeId == user.Id && q.LeaveTypeId == model.LeaveTypeId);
            allocationToDeduct.Days -= numberOfDays;

            await _context.SaveChangesAsync();
        }

        public Task<EmployeeLeaveRequestListVM> AdminGetAllLeaveRequests()
        {
            throw new NotImplementedException();
        }

        public async Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequests()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            var leaveRequests = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .Where(q => q.EmployeeId == user.Id)
                .ToListAsync();

            var model = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
            {
                StartDate = q.StartDate,
                EndDate = q.EndDate,
                Id = q.Id,
                LeaveType = q.LeaveType.Name,
                LeaveRequestStatus = (LeaveRequestStatusEnum)q.LeaveRequestStatusId,
                NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber
            }).ToList();

            return model;

        }

        public async Task<bool> RequestDatesExceedAllocation(LeaveRequestCreateVM model)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;
            var allocation = await _context.LeaveAllocations
               .FirstAsync(q => q.EmployeeId == user.Id && q.LeaveTypeId == model.LeaveTypeId);

            return allocation.Days < numberOfDays;

        }

        public Task ReviewLeaveRequest(ReviewLeaveRequestVM model)
        {
            throw new NotImplementedException();
        }
    }
}
