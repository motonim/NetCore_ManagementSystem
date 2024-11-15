using LeaveManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            var data = new TestViewModel
            {
                Name = "Jaeri Park",
                DateOfBirth = new DateTime(1990,09,30),
            };
            return View(data);
        }
    }
}
