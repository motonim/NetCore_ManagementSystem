using AutoMapper;
using Azure.Core;
using LeaveManagementSystem.Data;
using LeaveManagementSystem.Models.LeaveAllocations;
using LeaveManagementSystem.Models.LeaveRequests;
using LeaveManagementSystem.Services.LeaveAllocations;
using LeaveManagementSystem.Services.Users;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Services.LeaveRequests
{
    public class LeaveRequestsService(IMapper _mapper, IUserService _userService, ApplicationDbContext _context, ILeaveAllocationsService _leaveAllocationService) : ILeaveRequestsService
    {
        public async Task CancelLeaveRequest(int leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);
            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Canceled;

            // restore allocation days based on request
            //var currentDate = DateTime.Now;
            //// following the date of the tutorial to compare the codes
            //var year = currentDate.Year;
            //var randomDay = new Random().Next(1, 32);
            //var randomDateInMay = new DateTime(year, 5, randomDay);

            //var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == randomDateInMay.Year);
            //var numberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber;
            //var allocation = await _context.LeaveAllocations
            //    .FirstAsync(q => q.EmployeeId == leaveRequest.EmployeeId 
            //    && q.LeaveTypeId == leaveRequest.LeaveTypeId
            //    && q.PeriodId == period.Id);
            //allocation.Days += numberOfDays;

            await UpdateAllocationDays(leaveRequest, false);
            await _context.SaveChangesAsync();

        }

        public async Task CreateLeaveRequest(LeaveRequestCreateVM model)
        {
            // Map data to leave request data model
            var leaveRequest = _mapper.Map<LeaveRequest>(model);

            // get logged in employee id
            var user = await _userService.GetLoggedInUser();
            leaveRequest.EmployeeId = user.Id;

            // set LeaveRequestStatusId to pending
            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Pending;

            // save leave request
            //_context.LeaveRequests.Add(leaveRequest); same as the next two lines(_context.Add(leaveRequest); and _context.SaveChanges();)
            _context.Add(leaveRequest);

            // deduct allocation days based on request
            //var currentDate = DateTime.Now;
            //// following the date of the tutorial to compare the codes
            //var year = currentDate.Year;
            //var randomDay = new Random().Next(1, 32);
            //var randomDateInMay = new DateTime(year, 5, randomDay);

            //var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == randomDateInMay.Year);

            //var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;
            //var allocationToDeduct = await _context.LeaveAllocations
            //    .FirstAsync(q => q.EmployeeId == user.Id 
            //    && q.LeaveTypeId == model.LeaveTypeId
            //    && q.PeriodId == period.Id);
            //allocationToDeduct.Days -= numberOfDays;

            await UpdateAllocationDays(leaveRequest, true);
            await _context.SaveChangesAsync();
        }

        public async Task<EmployeeLeaveRequestListVM> AdminGetAllLeaveRequests()
        {
            var leaveRequests = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .ToListAsync();

            var LeaveRequestsModels = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
            {
                StartDate = q.StartDate,
                EndDate = q.EndDate,
                Id = q.Id,
                LeaveType = q.LeaveType.Name,
                LeaveRequestStatus = (LeaveRequestStatusEnum)q.LeaveRequestStatusId,
                NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber
            }).ToList();

            var model = new EmployeeLeaveRequestListVM
            {
                ApprovedRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Approved),
                PendingRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Pending),
                DeclinedRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Declined),
                TotalRequests = leaveRequests.Count,
                LeaveRequests = LeaveRequestsModels
            };

            return model;
        }

        public async Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequests()
        {
            var user = await _userService.GetLoggedInUser();
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
            var currentDate = DateTime.Now;
            // following the date of the tutorial to compare the codes
            var year = currentDate.Year;
            var randomDay = new Random().Next(1, 32);
            var randomDateInMay = new DateTime(year, 5, randomDay);

            var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == randomDateInMay.Year);

            var user = await _userService.GetLoggedInUser();
            var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;
            var allocation = await _context.LeaveAllocations
               .FirstAsync(q => q.EmployeeId == user.Id 
               && q.LeaveTypeId == model.LeaveTypeId
               && q.PeriodId == period.Id);

            return allocation.Days < numberOfDays;

        }

        public async Task ReviewLeaveRequest(int leaveRequestId, bool approved)
        {
            var user = await _userService.GetLoggedInUser();
            var test = leaveRequestId;
            var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);

            leaveRequest.LeaveRequestStatusId = approved
                ? (int)LeaveRequestStatusEnum.Approved
                : (int)LeaveRequestStatusEnum.Declined;

            leaveRequest.ReviewerId = user.Id;

            if(!approved)
            {
                //var currentDate = DateTime.Now;
                //// following the date of the tutorial to compare the codes
                //var year = currentDate.Year;
                //var randomDay = new Random().Next(1, 32);
                //var randomDateInMay = new DateTime(year, 5, randomDay);

                //var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == randomDateInMay.Year);
                //var allocation = await _context.LeaveAllocations
                //   .FirstAsync(q => q.EmployeeId == leaveRequest.EmployeeId 
                //   && q.LeaveTypeId == leaveRequest.LeaveTypeId
                //   && q.PeriodId == period.Id);
                //var numberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber;
                //allocation.Days += numberOfDays;
                await UpdateAllocationDays(leaveRequest, false);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<ReviewLeaveRequestVM> GetLeaveRequestForReview(int id)
        {
            var leaveRequest = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .FirstAsync(q => q.Id == id);

            var user = await _userService.GetUserById(leaveRequest.EmployeeId);

            var model = new ReviewLeaveRequestVM
            {
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                NumberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber,
                LeaveRequestStatus = (LeaveRequestStatusEnum)leaveRequest.LeaveRequestStatusId,
                Id = leaveRequest.Id,
                LeaveType = leaveRequest.LeaveType.Name,
                RequestComments = leaveRequest.RequestComments,
                Employee = new EmployeeListVM
                {
                    Id = leaveRequest.EmployeeId,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                }
            };

            return model;
            
        }

        private async Task UpdateAllocationDays(LeaveRequest leaveRequest, bool deductDays)
        {
            var allocation = await _leaveAllocationService.GetCurrentAllocation(leaveRequest.LeaveTypeId, leaveRequest.EmployeeId);
            var numberOfDays = CalculateDays(leaveRequest.StartDate, leaveRequest.EndDate);

            if (deductDays)
            {
                allocation.Days -= numberOfDays;
            }
            else
            {
                allocation.Days += numberOfDays;
            }
            _context.Entry(allocation).State = EntityState.Modified;
        }

        private int CalculateDays(DateOnly start, DateOnly end)
        {
            return end.DayNumber - start.DayNumber;
        }
    }
}
