using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using University;
using University.Models;

namespace University.Models
{
  public class Student
  {
    private int _id;
    private string _name;
    private System.DateTime _enrollDate;

    public Student(string name, System.DateTime enrollDate, int id = 0)
    {
      _name = name;
      _enrollDate = enrollDate;
      _id = id;
    }

    public string GetName()
    {
      return _name;
    }

    public System.DateTime GetEnrollDate()
    {
      return _enrollDate;
    }

    public int GetId()
    {
      return _id;
    }

    public override bool Equals(System.Object otherStudent)
    {
      if (!(otherStudent is Student))
      {
        return false;
      }
      else
      {
        Student newStudent = (Student) otherStudent;
        bool idEquality = this.GetId() == newStudent.GetId();
        bool descriptionEquality = this.GetName() == newStudent.GetName();
        bool enrollDateEquality = this.GetEnrollDate() == newStudent.GetEnrollDate();
        // We no longer compare Students' categoryIds in a categoryEquality bool here.
        return (idEquality && descriptionEquality && enrollDateEquality);
       }
    }
    public override int GetHashCode()
    {
         return this.GetId().GetHashCode();
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"INSERT INTO `students` (`name`, `enrollDate`) VALUES (@name, @enrollDate);";
      Console.WriteLine(_name);
      cmd.Parameters.Add(new MySqlParameter("@name", _name));

      cmd.Parameters.Add(new MySqlParameter("@enrollDate", _enrollDate));

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }

    public static List<Student> GetAll()
    {
      List<Student> allStudents = new List<Student> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM students;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int studentId = rdr.GetInt32(0);
        string studentName = rdr.GetString(1);
        System.DateTime studentEnrollDate = rdr.GetDateTime(2);
        Student newStudent = new Student(studentName, studentEnrollDate, studentId);
        allStudents.Add(newStudent);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allStudents;
    }

    public static Student Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM students WHERE id = (@searchId);";

      cmd.Parameters.Add(new MySqlParameter("@searchId", id));

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int studentId = 0;
      string studentName = "";
      System.DateTime studentEnrollDate = System.DateTime.Today;

      while(rdr.Read())
          {
            studentId = rdr.GetInt32(0);
            studentName = rdr.GetString(1);
            studentEnrollDate = rdr.GetDateTime(2);
            // We no longer read the itemCategoryId here, either.
          }

      Student newStudent = new Student(studentName, studentEnrollDate, studentId);
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return newStudent;
    }

    public void Delete()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM students WHERE id = @studentsId;";

        MySqlParameter studentIdParameter = new MySqlParameter();
        studentIdParameter.ParameterName = "@studentsId";
        studentIdParameter.Value = this.GetId();
        cmd.Parameters.Add(studentIdParameter);

        cmd.ExecuteNonQuery();
        if (conn != null)
        {
          conn.Close();
        }
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"TRUNCATE TABLE students;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }

    public void AddCourse(Course newCourse)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO courses_students (student_id, course_id) VALUES (@CourseId, @StudentId);";

      cmd.Parameters.Add(new MySqlParameter("@StudentId", _id));
      cmd.Parameters.Add(new MySqlParameter("@CourseId", newCourse.GetId()));

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }

    public List<Course> GetCourses(Student student)
    {
      List<Course> allCourses = new List<Course> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT courses.* FROM courses JOIN courses_students ON (courses.id = courses_students.course_id) JOIN students ON (courses_students.student_id = students.id) WHERE student_id = @StudentId";

      MySqlParameter courseIdParameter = new MySqlParameter();
      courseIdParameter.ParameterName = "@StudentId";
      courseIdParameter.Value = student.GetId();
      cmd.Parameters.Add(courseIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Course> students = new List<Course>{};

      while(rdr.Read())
      {
          int courseId = rdr.GetInt32(0);
          string courseName = rdr.GetString(1);
          string courseNumber = rdr.GetString(2);
          Course newCourse = new Course(courseName, courseNumber, courseId);
          allCourses.Add(newCourse);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return allCourses;
    }
  }
}
