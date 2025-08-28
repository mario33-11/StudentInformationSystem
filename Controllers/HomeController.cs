using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentInformationSystem.Data;
using StudentInformationSystem.Models;

namespace StudentInformationSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get basic statistics for dashboard
            ViewBag.TotalStudents = await _context.Students.CountAsync();
            ViewBag.TotalDepartments = await _context.Students.Select(s => s.Department).Distinct().CountAsync();
            ViewBag.RecentEnrollments = await _context.Students
                .Where(s => s.EnrollmentDate >= DateTime.Now.AddDays(-30))
                .CountAsync();
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
        }
    }
}
