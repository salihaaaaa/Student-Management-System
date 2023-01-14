using System;

namespace Entities
{
    public class Student
    {
        public Guid StudentID { get; set; }
        public string? StudentName { get; set; }
        public string? StudentEmail { get; set; }
        public Guid? CourseID { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }

    }
}
