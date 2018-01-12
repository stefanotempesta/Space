using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolidMvcSharePoint.Models
{
    public class BigCourse : CourseTemplate
    {
        public List<Student> RegisteredStudents { get; set; }

        public List<Student> WaitlistedStudents { get; set; }

        public int Capacity { get; set; }

        public bool EnrolStudent(Student student)
        {
            if (RegisteredStudents.Count >= Capacity)
            {
                WaitlistedStudents.Add(student);
                return false;
            }

            if (RegisteredStudents.Contains(student))
            {
                return false;
            }

            RegisteredStudents.Add(student);
            return true;
        }
    }
}