using Microsoft.AspNetCore.Mvc;
using University.Models;
using System;
using System.Collections.Generic;

namespace University.Controllers
{
  public class StudentsController : Controller
  {
    [HttpGet("/students/create")]
    public ActionResult CreateStudent()
    {
      return View();
    }

    [HttpPost("/students/create")]
    public ActionResult Create()
    {
      string newEnrollDate = Request.Form["studentEnrollDate"];
      System.DateTime parsedEnrollDate = System.DateTime.Parse(newEnrollDate);
      string studentName = Request.Form["studentName"];
      Student newStudent = new Student(studentName, parsedEnrollDate);
      newStudent.Save();

      List<Student> allStudents = Student.GetAll();
      return View("~/Views/Home/Index.cshtml", allStudents);
    }

    [HttpGet("/students/delete")]
    public ActionResult DeletePage()
    {
      return View();
    }

    [HttpPost("/students/delete")]
    public ActionResult DeleteAll()
    {
      Student.DeleteAll();

      List<Student> allStudents = Student.GetAll();
      return View("~/Views/Home/Index.cshtml", allStudents);
    }
  }
}
