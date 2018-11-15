using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class TestCollection
    {
        public List<Student> Students { get; set; } = new List<Student>();

        public void Test()
        {
            Student s1 = new Student() {Name="Jay",Age=11 };
            Students.Add(s1);

            s1.Age += 10;

            Console.WriteLine(Students.FirstOrDefault().Age.ToString());

        }

    }

    public class Student
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
