using System;
using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    public class StudentAddRequest
    {
        [Required(ErrorMessage = "Student Name can't be blank")]
        public string? StudentName { get; set; }

        [Required(ErrorMessage = "Student Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email should be a valid email")]
        [DataType(DataType.EmailAddress)]
        public string? StudentEmail { get; set; }

        [Required(ErrorMessage = "Please select a course")]
        public Guid? CourseID { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Date of Birth can't be blank")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address can't be blank")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Please select a gender")]
        public GenderOptions? Gender { get; set; }

        /// <summary>
        /// Converts the current object of StudentAddRequest into a new object of Student type
        /// </summary>
        /// <returns></returns>
        public Student ToStudent()
        {
            return new Student()
            {
                StudentName = StudentName,
                StudentEmail = StudentEmail,
                CourseID= CourseID,
                DateOfBirth = DateOfBirth,
                Address = Address,
                Gender = Gender.ToString()
            };
        }
    }
}
