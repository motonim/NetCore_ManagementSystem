namespace LeaveManagementSystem.Controllers;

[Authorize]
public class LeaveRequestsController : Controller
{
    // Employee View requests
    public async Task<IActionResult> Index()
    {
        return View();
    }

    // Employee create requests
    public async Task<IActionResult> Create()
    {
        return View();
    }

    // Employee create requests
    [HttpPost]
    public async Task<IActionResult> Create(int leaverequestId /*Use VM*/)
    {
        return View();
    }

    // Employee cancel requests
    [HttpPost]
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