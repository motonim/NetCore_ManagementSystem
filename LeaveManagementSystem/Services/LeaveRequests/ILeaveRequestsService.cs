using LeaveManagementSystem.Models.LeaveRequests;

namespace LeaveManagementSystem.Services.LeaveRequests
{
    public interface ILeaveRequestsService
    {
        Task CreateLeaveRequest(LeaveRequestCreateVM model);
        Task<EmployeeLeaveRequestListVM> GetEmployeeLeaveRequests();
        Task<LeaveRequestListVM> GetAllLeaveRequests();
        Task CancelLeaveRequest(int leaveRequestId);
        Task ReviewLeaveRequest(ReviewLeaveRequestVM model);
    }
}