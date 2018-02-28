using Microsoft.VisualStudio.TestTools.UnitTesting;
using University.Models;
using System;
using System.Collections.Generic;

namespace University.Models.Tests
{
  [TestClass]
  public class CourseTest : IDisposable
 {
   public CourseTest()
   {
     Console.WriteLine("The port number and database name probably need to be changed");
     DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=university_test;";
   }

   public void Dispose()
   {
     Course.DeleteAll();
     Student.DeleteAll();
   }

   [TestMethod]
   public void GetAll_DatabaseEmptyAtFirst_0()
   {
     int result = Course.GetAll().Count;

     Assert.AreEqual(0, result);
   }

   [TestMethod]
   public void Save_CourseSavesToDatabase_Course()
   {
    Course newCourse = new Course("History of History Class", "HIST100");
    newCourse.Save();

    List<Course> testList = new List<Course>{newCourse};
    List<Course> result = Course.GetAll();

    CollectionAssert.AreEqual(testList, result);
   }

   [TestMethod]
   public void GetAll_AllCoursesAreReturned_CourseList()
   {
    //Arrange
    Course newCourse = new Course("History of History Class", "HIST100");
    newCourse.Save();

    Course newCourse2 = new Course("History of History Class", "HIST100");
    newCourse2.Save();
    //Act
    List<Course> testList = new List<Course> {newCourse, newCourse2};
    List<Course> result = Course.GetAll();

    //Assert
    CollectionAssert.AreEqual(testList, result);
   }

   [TestMethod]
   public void Delete_DeletesFromDatabase_Courses()
   {
    //Arrange
    Course newCourse = new Course("History of History Class", "HIST100");
    newCourse.Save();

    Course newCourse2 = new Course("History of History Class", "HIST100");
    newCourse2.Save();
    List<Course> testList = new List<Course> {newCourse2};

    //Act
    newCourse.Delete();
    List<Course> result = Course.GetAll();

    //Assert
    CollectionAssert.AreEqual(testList, result);
   }

 }
}
