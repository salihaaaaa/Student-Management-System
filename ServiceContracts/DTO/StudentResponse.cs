using System;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Represents DTO class that is used as return type of most methods of Persons Service
    /// </summary>
    public class StudentResponse
    {
        public Guid StudentID { get; set; }
        public string? StudentName { get; set; }
        public string? StudentEmail { get; set; }
        public Guid? CourseID { get; set; }
        public String? Course { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public double? Age { get; set; }

        /// <summary>
        /// Compares the current object data with the parameter object
        /// </summary>
        /// <param name="obj">StudentResponse object to compare</param>
        /// <returns>True or false, indicating whether all person details are matched with specified parameter object</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != typeof(StudentResponse))
                return false;

            StudentResponse student= (StudentResponse)obj;
            return StudentID == student.StudentID && StudentName == student.StudentName && 
                StudentEmail == student.StudentEmail && CourseID == student.CourseID && 
                DateOfBirth == student.DateOfBirth && Address == student.Address 
                && Gender == student.Gender && Age == student.Age;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Student ID: {StudentID}, Student Name: {StudentName}, Student Email: {StudentEmail}, Course ID: {CourseID}, Date of Birth: {DateOfBirth?.ToString("dd MMMM yyyy")}, Address: {Address}, Gender: {Gender}, Age: {Age}";
        }

        public StudentUpdateRequest ToStudentUpdateRequest()
        {
            return new StudentUpdateRequest()
            {
                StudentID = StudentID,
                StudentName = StudentName,
                StudentEmail = StudentEmail,
                CourseID = CourseID,
                DateOfBirth = DateOfBirth,
                Address = Address,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true),
            };
        }
    }

    public static class StudentExtensions
    {
        public static StudentResponse ToStudentResponse(this Student student)
        {
            return new StudentResponse()
            {
                StudentID = student.StudentID,
                StudentName = student.StudentName,
                StudentEmail = student.StudentEmail,
                CourseID = student.CourseID,
                DateOfBirth = student.DateOfBirth,
                Address = student.Address,
                Gender = student.Gender,
                Age = (student.DateOfBirth != null)? Math.Round((DateTime.Now - student.DateOfBirth.Value).TotalDays / 365.25) : null
            };
        }
    }
}
