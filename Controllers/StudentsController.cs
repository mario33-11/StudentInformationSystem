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
}


