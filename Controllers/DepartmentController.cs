using Day2.Task1.Data;
using Day2.Task1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Day2.Task1.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly AppDbContext _context;

        public DepartmentController(AppDbContext context)
        {
            _context = context;

        }
        public IActionResult AllDepartments()
        {
            var departments = _context.Departments.ToList();
            return View(departments);
        }

        public IActionResult Details(int? id)
        {
            if (id is null) return NotFound();
            var department = _context.Departments.Include(d => d.Doctors).FirstOrDefault(s => s.DepartmentId == id);
            if (department is null) return NotFound();
            return View(department);
        }
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id is null) return NotFound();
            var department = _context.Departments.FirstOrDefault(s => s.DepartmentId == id);
            if (department is null) return NotFound();
            _context.Departments.Remove(department);
            _context.SaveChanges();

            return RedirectToAction("AllDepartments");
        }

        [HttpGet]
        public IActionResult Add()
        {

            return View(new Department());
        }

        [HttpPost]
        public IActionResult Add(Department dept)
        {
            dept.Name = (dept.Name ?? "").Trim();
            dept.Description = (dept.Description ?? "").Trim();

            if (!string.IsNullOrEmpty(dept.Name) &&
                _context.Departments.Any(d => d.Name == dept.Name))
            {
                ModelState.AddModelError(nameof(Department.Name), "A department with this name already exists.");
            }

            if (!ModelState.IsValid)
            {
                return View(dept);
            }


            _context.Departments.Add(dept);
            _context.SaveChanges();
            return RedirectToAction(nameof(AllDepartments));
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {

            if (id is null) return NotFound();
            var department = _context.Departments.FirstOrDefault(s => s.DepartmentId == id);
            if (department is null) return NotFound();


            return View(department);
        }


        [HttpPost]
        public IActionResult Edit(Department dept)
        {
          

            dept.Name = (dept.Name ?? "").Trim();
            dept.Description = (dept.Description ?? "").Trim();

            if (!string.IsNullOrEmpty(dept.Name) &&
                _context.Departments.Any(d => d.Name == dept.Name && d.DepartmentId != dept.DepartmentId))
            {
                ModelState.AddModelError(nameof(Department.Name), "A department with this name already exists.");
            }

            if (!ModelState.IsValid)
            {
                return View(dept);
            }

            _context.Departments.Update(dept);
            _context.SaveChanges();
            return RedirectToAction(nameof(AllDepartments));
        }
    }
}
