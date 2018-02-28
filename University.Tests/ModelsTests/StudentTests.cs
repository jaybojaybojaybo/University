using Microsoft.VisualStudio.TestTools.UnitTesting;
using University.Models;
using System;
using System.Collections.Generic;

namespace University.Models.Tests
{
  [TestClass]
  public class StudentTest : IDisposable
 {
    public StudentTest()
    {
      Console.WriteLine("The port number and database name probably need to be changed");
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=university_test;";
    }

    public void Dispose()
    {
      Student.DeleteAll();
      Course.DeleteAll();
    }

    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      int result = Student.GetAll().Count;

      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Save_StudentSavesToDatabase_Student()
    {
      System.DateTime enrollDate = System.DateTime.Parse("02/13/18");
      Student newStudent = new Student("Bob", enrollDate);
      newStudent.Save();

      List<Student> testList = new List<Student>{newStudent};
      List<Student> result = Student.GetAll();

      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void GetAll_AllStudentsAreReturned_StudentList()
    {
      //Arrange
      System.DateTime enrollDate = System.DateTime.Parse("02/13/18");
      Student newStudent = new Student("Bob", enrollDate);
      newStudent.Save();

      Student newStudent2 = new Student("Tom", enrollDate);
      newStudent2.Save();
      //Act
      List<Student> testList = new List<Student> {newStudent, newStudent2};
      List<Student> result = Student.GetAll();

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Delete_DeletesFromDatabase_Students()
    {
      //Arrange
      System.DateTime enrollDate = System.DateTime.Parse("02/13/18");
      Student newStudent = new Student("Bob", enrollDate);
      newStudent.Save();

      Student newStudent2 = new Student("Tom", enrollDate);
      newStudent2.Save();
      List<Student> testList = new List<Student> {newStudent2};

      //Act
      newStudent.Delete();
      List<Student> result = Student.GetAll();

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void GetCourse_GetsCoursesForStudent_()
    {
      //Arrange
      System.DateTime enrollDate = System.DateTime.Parse("02/13/18");
      Student newStudent = new Student("Bob", enrollDate);
      newStudent.Save();
      Course newCourse = new Course("English", "ENGL100");
      newCourse.Save();
      Course newCourse2 = new Course("History", "HIST100");
      newCourse2.Save();
      newStudent.AddCourse(newCourse);
      newStudent.AddCourse(newCourse2);

      //Act
      List<Course> studentsCourses = newStudent.GetCourses(newStudent);
      List<Course> testList = new List<Course> {newCourse, newCourse2};

      //Assert
      Assert.AreEqual(testList.Count, studentsCourses.Count);
      CollectionAssert.AreEqual(testList, studentsCourses);
    }
  }
}
