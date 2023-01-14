using System;
using Entities;

namespace ServiceContracts.DTO
{
    public class CourseResponse
    {
        public Guid CourseID { get; set; }
        public string? CourseName { get; set; }

        //It compares the current object to another object of CourseResponse type and return true,
        //if both values are same; else return false
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != typeof(CourseResponse)) 
                return false;

            CourseResponse course_to_compare = (CourseResponse)obj;
            return CourseID == course_to_compare.CourseID && CourseName == course_to_compare.CourseName;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }

    public static class CourseExtensions
    {
        public static CourseResponse ToCourseResponse(this Course course)
        {
            return new CourseResponse
            {
                CourseID = course.CourseID,
                CourseName = course.CourseName
            };
        }
    }
}
