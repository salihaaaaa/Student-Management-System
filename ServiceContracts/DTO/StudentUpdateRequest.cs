using Entities;
using ServiceContracts.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Represents the DTO class that contains the student details to update
    /// </summary>
    public class StudentUpdateRequest
    {
        [Required(ErrorMessage = "Student ID can't be blank")]
        public Guid StudentID { get; set; }

        [Required(ErrorMessage = "Student Name can't be blank")]
        public string? StudentName { get; set; }

        [Required(ErrorMessage = "Student Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email should be a valid email")]
        public string? StudentEmail { get; set; }

        public Guid? CourseID { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public GenderOptions? Gender { get; set; }

        /// <summary>
        /// Converts the current object of StudentUpdateRequest into a new object of Student type
        /// </summary>
        /// <returnsReturns student object></returns>
        public Student ToStudent()
        { 
            return new Student()
            {
                StudentID = StudentID,
                StudentName = StudentName,
                StudentEmail = StudentEmail,
                CourseID = CourseID,
                DateOfBirth = DateOfBirth,
                Address = Address,
                Gender = Gender.ToString()
            };
        }
    }
}
