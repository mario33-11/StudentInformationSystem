using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentInformationSystem.Data;
using StudentInformationSystem.Models;

namespace StudentInformationSystem.Controllers;

public class StudentsController : Controller
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: /Students
    public async Task<IActionResult> Index()
    {
        // Seed sample data if database is empty
        if (!await _context.Students.AnyAsync())
        {
            await SeedSampleData();
        }
        
        var students = await _context.Students.AsNoTracking().ToListAsync();
        return View(students);
    }

    // GET: /Students/Search
    [HttpGet]
    public IActionResult Search()
    {
        return View();
    }

    // POST: /Students/Search
    [HttpPost]
    public async Task<IActionResult> Search(int? studentId, string? name)
    {
        IQueryable<Student> query = _context.Students.AsQueryable();

        if (studentId.HasValue)
        {
            query = query.Where(s => s.StudentID == studentId.Value);
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(s => s.Name.Contains(name));
        }

        var results = await query.AsNoTracking().ToListAsync();
        return View(results);
    }

    // GET: /Students/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Students/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Email,Phone,Department,EnrollmentDate")] Student student)
    {
        if (!ModelState.IsValid) return View(student);
        _context.Add(student);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: /Students/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var student = await _context.Students.FindAsync(id);
        if (student == null) return NotFound();
        return View(student);
    }

    // POST: /Students/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("StudentID,Name,Email,Phone,Department,EnrollmentDate")] Student student)
    {
        if (id != student.StudentID) return NotFound();
        if (!ModelState.IsValid) return View(student);
        _context.Update(student);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: /Students/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentID == id);
        if (student == null) return NotFound();
        return View(student);
    }

    // POST: /Students/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student != null)
        {
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task SeedSampleData()
    {
        var sampleStudents = new List<Student>
        {
            new Student { Name = "Ahmed Mohamed", Email = "ahmed.mohamed@university.edu", Phone = "+20-123-456-789", Department = "Computer Science", EnrollmentDate = DateTime.Now.AddDays(-120) },
            new Student { Name = "Fatima Ali", Email = "fatima.ali@university.edu", Phone = "+20-987-654-321", Department = "Engineering", EnrollmentDate = DateTime.Now.AddDays(-90) },
            new Student { Name = "Omar Hassan", Email = "omar.hassan@university.edu", Phone = "+20-555-123-456", Department = "Business", EnrollmentDate = DateTime.Now.AddDays(-60) },
            new Student { Name = "Aisha Mahmoud", Email = "aisha.mahmoud@university.edu", Phone = "+20-777-888-999", Department = "Computer Science", EnrollmentDate = DateTime.Now.AddDays(-30) },
            new Student { Name = "Youssef Ibrahim", Email = "youssef.ibrahim@university.edu", Phone = "+20-111-222-333", Department = "Engineering", EnrollmentDate = DateTime.Now.AddDays(-15) }
        };

        await _context.Students.AddRangeAsync(sampleStudents);
        await _context.SaveChangesAsync();
    }
}


