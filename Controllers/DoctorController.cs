using Day2.Task1.Data;
using Day2.Task1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Day2.Task1.Controllers
{
    public class DoctorController : Controller
    {
       private readonly AppDbContext _context;

        public DoctorController(AppDbContext context)
        {
            _context = context;
        }
        public  IActionResult AllDoctors()
        {
            var doctors = _context.Doctors.Include(x => x.Department).ToList();
            return View(doctors);
        }

        public IActionResult Details(int? id)
        {
            if (id is null) return NotFound();
            var doctor = _context.Doctors.Include(x => x.Department).FirstOrDefault(s => s.DoctorId == id);
            if (doctor is null) return NotFound();
            return View(doctor);
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id is null) return NotFound();
            var doctor = _context.Doctors.FirstOrDefault(s => s.DoctorId == id);
            if (doctor is null) return NotFound();

            _context.Doctors.Remove(doctor);
            _context.SaveChanges();

            return RedirectToAction("AllDoctors");
        }


        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Departments = _context.Departments
             .AsNoTracking()
             .OrderBy(d => d.Name)
             .ToList();

            return View(new Doctor());
        }

        [HttpPost]
        public IActionResult Add(Doctor doc)
        {
            doc.Name = (doc.Name ?? "").Trim();
            doc.Email = (doc.Email ?? "").Trim();

            if (doc.DepartmentId > 0 && !_context.Departments.Any(d => d.DepartmentId == doc.DepartmentId))
            {
                ModelState.AddModelError(nameof(Doctor.DepartmentId), "The selected department does not exist");
            }

            _context.Doctors.Add(doc);
            _context.SaveChanges();
            return RedirectToAction("AllDoctors");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {

            if (id is null) return NotFound();
            var doctor = _context.Doctors.FirstOrDefault(s => s.DoctorId == id);
            if (doctor is null) return NotFound();

            ViewBag.Departments = _context.Departments
                       .AsNoTracking()
                       .OrderBy(d => d.Name)
                       .ToList();


            return View(doctor);

        }


        [HttpPost]
        public IActionResult Edit(Doctor doc)
        {

            doc.Name = (doc.Name ?? "").Trim();
            doc.Email = (doc.Email ?? "").Trim();

           
            _context.Doctors.Update(doc);
            _context.SaveChanges();
            return RedirectToAction(nameof(AllDoctors));
        }


    }
}
