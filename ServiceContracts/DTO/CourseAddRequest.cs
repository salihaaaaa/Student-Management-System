using System;
using Entities;

namespace ServiceContracts.DTO
{
    public class CourseAddRequest
    {
        public string? CourseName { get; set; }

        public Course ToCourse()
        {
            return new Course()
            {
                CourseName = CourseName
            };
        }
    }
}
