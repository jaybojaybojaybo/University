using Microsoft.AspNetCore.Mvc;
using University.Models;
using System;
using System.Collections.Generic;

namespace University.Controllers
{
  public class CoursesController : Controller
  {
    [HttpGet("/courses/create")]
    public ActionResult CreateCourse()
    {
      return View();
    }

    [HttpPost("/courses/create")]
    public ActionResult Create()
    {
      Course newCourse = new Course(Request.Form["courseName"], Request.Form["courseNumber"]);
      newCourse.Save();

      List<Student> allStudents = Student.GetAll();
      return View("~/Views/Home/Index.cshtml", allStudents);
    }

    [HttpGet("/courses/delete")]
    public ActionResult DeletePage()
    {
      return View();
    }

    [HttpPost("/courses/delete")]
    public ActionResult DeleteAll()
    {
      Course.DeleteAll();

      List<Student> allStudents = Student.GetAll();
      return View("~/Views/Home/Index.cshtml", allStudents);
    }
  }
}
