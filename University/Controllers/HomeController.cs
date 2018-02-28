using Microsoft.AspNetCore.Mvc;
using University.Models;
using System;
using System.Collections.Generic;

namespace University.Controllers
{
  public class HomeController : Controller
  {

    [HttpGet("/")]
    public ActionResult Index()
    {
      Dictionary<string, object> model = new Dictionary<string, object> {};
      List<Student> allStudents = Student.GetAll();
      model.Add("allStudents", allStudents);
      List<Course> allCourses = Course.GetAll();
      return View(allStudents);
    }
  }
}
