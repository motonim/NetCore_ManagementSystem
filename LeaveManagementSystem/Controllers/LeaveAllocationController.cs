using LeaveManagementSystem.Models.LeaveAllocations;
using LeaveManagementSystem.Services.LeaveAllocations;
using LeaveManagementSystem.Services.LeaveTypes;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Controllers
{
    [Authorize]
    public class LeaveAllocationController(ILeaveAllocationsService _leaveAllocationsService, ILeaveTypesService _leaveTypesService) : Controller
    {
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> Index()
        {
            var employees = await _leaveAllocationsService.GetEmployees();
            return View(employees);
        }

        [Authorize(Roles = Roles.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AllocateLeave(string? id)
        {
            await _leaveAllocationsService.AllocateLeave(id);
            return RedirectToAction(nameof(Details), new { userId = id });
        }

        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> EditAllocation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allocation = await _leaveAllocationsService.GetEmployeeAllocation(id.Value);
            if (allocation == null)
            {
                return NotFound();
            }
            return View(allocation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAllocation(LeaveAllocationEditVM allocation)
        {
            if (await _leaveTypesService.DaysExceedMaximum(allocation.LeaveType.Id, allocation.Days))
            {
                ModelState.AddModelError("Days", "The allocation exceeds the maximum leave type value");
            }

            if(ModelState.IsValid)
            {
                await _leaveAllocationsService.EditAllocation(allocation);
                return RedirectToAction(nameof(Details), new { userId = allocation.Employee.Id });
            }

            // if the ModelState is NOT valid, then we execute the following to avoid allocation being null
            // we track the important data which is days and load all the information from the db again and send this data to the view page.
            var days = allocation.Days;
            allocation = await _leaveAllocationsService.GetEmployeeAllocation(allocation.Id);
            allocation.Days = days;
            return View(allocation);
        }

        public async Task<IActionResult> Details(string? userId)
        {
            var employeeVm = await _leaveAllocationsService.GetEmployeeAllocations(userId);
            return View(employeeVm);
        }
    }
}
