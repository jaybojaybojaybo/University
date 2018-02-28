using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using University;
using University.Models;

namespace University.Models
{
  public class Course
  {
    private int _id;
    private string _name;
    private string _number;

    public Course(string name, string number, int id = 0)
    {
      _name = name;
      _number = number;
      _id = id;
    }

    public string GetName()
    {
      return _name;
    }

    public string GetNumber()
    {
      return _number;
    }

    public int GetId()
    {
      return _id;
    }

    public override bool Equals(System.Object otherCourse)
    {
      if (!(otherCourse is Course))
      {
        return false;
      }
      else
      {
        Course newCourse = (Course) otherCourse;
        bool idEquality = this.GetId() == newCourse.GetId();
        bool descriptionEquality = this.GetName() == newCourse.GetName();
        bool NumberEquality = this.GetNumber() == newCourse.GetNumber();
        // We no longer compare Students' CourseIds in a categoryEquality bool here.
        return (idEquality && descriptionEquality && NumberEquality);
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

      cmd.CommandText = @"INSERT INTO `courses` (`name`, `number`) VALUES (@name, @number);";
      cmd.Parameters.Add(new MySqlParameter("@name", _name));

      cmd.Parameters.Add(new MySqlParameter("@number", _number));

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }

    public static List<Course> GetAll()
    {
      List<Course> allCourses = new List<Course> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM courses;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
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

    public static Course Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM courses WHERE id = (@searchId);";

      cmd.Parameters.Add(new MySqlParameter("@searchId", id));

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int courseId = 0;
      string courseName = "";
      string courseNumber = "";

      while(rdr.Read())
          {
            courseId = rdr.GetInt32(0);
            courseName = rdr.GetString(1);
            courseNumber = rdr.GetString(2);
          }

      Course newCourse = new Course(courseName, courseNumber, courseId);
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return newCourse;
    }

    public void Delete()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM courses WHERE id = @coursesId;";

        MySqlParameter courseIdParameter = new MySqlParameter();
        courseIdParameter.ParameterName = "@coursesId";
        courseIdParameter.Value = this.GetId();
        cmd.Parameters.Add(courseIdParameter);

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
      cmd.CommandText = @"TRUNCATE TABLE courses;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }

    public void AddStudent(Student newStudent)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO courses_students (course_id, student_id) VALUES (@CourseId, @StudentId);";

      cmd.Parameters.Add(new MySqlParameter("@CourseId", _id));
      cmd.Parameters.Add(new MySqlParameter("@StudentId", newStudent.GetId()));

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Student> GetStudents()
    {
      List<Student> allStudents = new List<Student> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT students.* FROM students
        JOIN courses_students ON (courses.id = courses_students.course_id)
        JOIN students ON (courses_students.student_id = students.id)
        WHERE courses.id = @CourseId;";

      MySqlParameter courseIdParameter = new MySqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = _id;
      cmd.Parameters.Add(courseIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Student> students = new List<Student>{};

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
  }
}
