namespace LeaveManagementSystem.Controllers;
using LeaveManagementSystem.Models.LeaveRequests;
using LeaveManagementSystem.Services.LeaveRequests;
using LeaveManagementSystem.Services.LeaveTypes;
using Microsoft.AspNetCore.Mvc.Rendering;

[Authorize]
public class LeaveRequestsController(ILeaveTypesService _leaveTypesService, ILeaveRequestsService _leaveRequestsService) : Controller
{
    // Employee View requests
    public async Task<IActionResult> Index()
    {
        return View();
    }

    // Employee create requests
    public async Task<IActionResult> Create()
    {
        var leaveTypes = await _leaveTypesService.GetAllAsync();
        var leaveTypesList = new SelectList(leaveTypes, "Id", "Name");
        var model = new LeaveRequestCreateVM
        {
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            LeaveTypes = leaveTypesList
        };
        return View(model);
    }

    // Employee create requests
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LeaveRequestCreateVM model)
    {
        if (ModelState.IsValid)
        {
            await _leaveRequestsService.CreateLeaveRequest(model);
        }

        var leaveTypes = await _leaveTypesService.GetAllAsync();
        model.LeaveTypes = new SelectList(leaveTypes, "Id", "Name");
        return View(model);
    }

    // Employee cancel requests
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int leaverequestId /*Use VM*/)
    {
        return View();
    }

    // Admin/Supervisor review requests
    public async Task<IActionResult> ListRequests()
    {
        return View();
    }

    // Employee cancel requests
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Review(int leaverequestId)
    {
        return View();
    }

    // Employee cancel requests
    [HttpPost]
    public async Task<IActionResult> Review(/*Use VM*/)
    {
        return View();
    }
}