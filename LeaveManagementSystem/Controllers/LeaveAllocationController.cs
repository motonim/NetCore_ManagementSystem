using LeaveManagementSystem.Services.LeaveAllocations;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Controllers
{
    [Authorize]
    public class LeaveAllocationController(ILeaveAllocationsService _leaveAllocationsService) : Controller
    {
        public async Task<IActionResult> Details()
        {
            var employeeVm = await _leaveAllocationsService.GetEmployeeAllocations();
            return View(employeeVm);
        }
    }
}
