using Microsoft.AspNetCore.Mvc;
using TestMVCApp.Models;

namespace TestMVCApp.Controllers
{
    public class StudentController : Controller
    {
        List<Student> Students;
        public StudentController()
        {
            Students = new List<Student>()
            {
                new Student(1,"Ruslan","Tagizade"),
                new Student(2,"Ramin","Emrahli"),
                new Student(3,"Abbas","Bayramli"),
                new Student(4,"Malik","Aliyev")
            };
        }
        public IActionResult Index()
        {
            ViewBag.Student = "Malik Aliyev";
            ViewData["Stu"] = "Ruslan Tagizade";
            TempData["Teacher"] = "Sadig Aliyev";
            
            return View(Students);
            //return RedirectToAction(nameof(Details));
        }

        public IActionResult Details(int id)
        {
            Student student = Students.FirstOrDefault(x => x.Id == id);
            if(student == null) return NotFound();
            return View(student);
        }
    }
}
