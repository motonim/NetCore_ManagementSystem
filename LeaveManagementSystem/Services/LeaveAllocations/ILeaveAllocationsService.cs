﻿using LeaveManagementSystem.Models.LeaveAllocations;

namespace LeaveManagementSystem.Services.LeaveAllocations
{
    public interface ILeaveAllocationsService
    {
        Task AllocateLeave(string employeeId);
        Task<EmployeeAllocationVM> GetEmployeeAllocations(string? userId);
        Task<LeaveAllocationEditVM> GetEmployeeAllocation(int allocationId);
        Task<List<EmployeeListVM>> GetEmployees();
        Task EditAllocation(LeaveAllocationEditVM allocationEditVm);
    }
}
