using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Services.Helpers;

namespace Services
{
    public class StudentsService : IStudentsService
    {
        //private field
        private readonly List<Student> _students;
        private readonly ICoursesService _coursesService;

        //constructor
        public StudentsService(bool initialize = true)
        {
            _students = new List<Student>();
            _coursesService = new CoursesService();

            if (initialize)
            {
                _students.AddRange(new List<Student>()
                {
                    new Student()
                    {
                        StudentID = Guid.Parse("AE2319CD-AB46-4018-BE6F-78A7EC368FD7"),
                        StudentName = "Cleveland",
                        StudentEmail = "cdipietro0@edublogs.org",
                        CourseID = Guid.Parse("9275F118-508E-4B22-A202-462EFB3D0637"),
                        DateOfBirth = DateTime.Parse("1993-07-25"),
                        Address = "3 Bluestem Avenue",
                        Gender = "Male"
                    },
                    new Student()
                    {
                        StudentID = Guid.Parse("F168D5B0-2B4F-4E27-9FBE-6EEC5E2EA1B5"),
                        StudentName = "Isabel",
                        StudentEmail = "ikimmins9 @elpais.com",
                        CourseID = Guid.Parse("4C3124FB-FC2F-43C7-9C6C-DD6471489C2D"),
                        DateOfBirth = DateTime.Parse("1994-06-12"),
                        Address = "5 Lien Plaza",
                        Gender = "Female"
                    },
                    new Student()
                    {
                        StudentID = Guid.Parse("DF1203BD-7695-4300-A91F-6F1555DE050E"),
                        StudentName = "Ailee",
                        StudentEmail = "amccullough7@sun.com",
                        CourseID = Guid.Parse("3A806107-9C01-435D-8976-5A1715C7B434"),
                        DateOfBirth = DateTime.Parse("1996-05-21"),
                        Address = "1991 Mitchell Road",
                        Gender = "Female"
                    },
                    new Student()
                    {
                        StudentID = Guid.Parse("5AA4E8A5-1794-4283-B8FB-D079CC169933"),
                        StudentName = "Onofredo",
                        StudentEmail = "ogofton3@hhs.gov",
                        CourseID = Guid.Parse("4C3124FB-FC2F-43C7-9C6C-DD6471489C2D"),
                        DateOfBirth = DateTime.Parse("1991-07-29"),
                        Address = "4751 Northport Court",
                        Gender = "Male"
                    },
                    new Student()
                    {
                        StudentID = Guid.Parse("48DB5EF8-3B50-412A-930B-9CC344B45EC5"),
                        StudentName = "Jorge",
                        StudentEmail = "jbourley4@instagram.com",
                        CourseID = Guid.Parse("3A806107-9C01-435D-8976-5A1715C7B434"),
                        DateOfBirth = DateTime.Parse("1994-08-17"),
                        Address = "13522 Blue Bill Park Road",
                        Gender = "Male"
                    },
                });
            }
        }

        private StudentResponse ConvertStudentToStudentResponse(Student student)
        {
            StudentResponse studentResponse = student.ToStudentResponse();
            studentResponse.Course = _coursesService.GetCourseByCourseID(student.CourseID)?.CourseName;
            return studentResponse;
        }

        public StudentResponse AddStudent(StudentAddRequest? studentAddRequest)
        {
            //Check if studentAddRequest is null
            if (studentAddRequest == null)
            {
                throw new ArgumentNullException(nameof(studentAddRequest));
            }

            //Model validation
            ValidationHelper.ModelValidation(studentAddRequest);

            //convert studentAddRequest to Student type
            Student student = studentAddRequest.ToStudent();

            //generate StudentID
            student.StudentID = Guid.NewGuid();

            //add student object to students list
            _students.Add(student);

            //convert the Student object into StudentResponse type
            return ConvertStudentToStudentResponse(student);
        }

        public List<StudentResponse> GetAllStudents()
        {
            return _students.Select(temp => ConvertStudentToStudentResponse(temp)).ToList();
        }

        public StudentResponse? GetStudentByStudentID(Guid? studentID)
        {
            if (studentID == null)
                return null;

            Student? student = _students.FirstOrDefault(temp => temp.StudentID == studentID);
            if (student == null)
                return null;

            return student.ToStudentResponse();
        }

        public List<StudentResponse> GetFilteredStudents(string searchBy, string? searchString)
        {
            List<StudentResponse> allStudents = GetAllStudents();
            List<StudentResponse> matchingStudents = allStudents;

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
                return matchingStudents;

            switch (searchBy)
            {
                case nameof(StudentResponse.StudentName):
                    matchingStudents = allStudents.Where(temp => 
                    (!string.IsNullOrEmpty(temp.StudentName)? 
                    temp.StudentName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(StudentResponse.StudentEmail):
                    matchingStudents = allStudents.Where(temp =>
                    (!string.IsNullOrEmpty(temp.StudentEmail) ?
                    temp.StudentEmail.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(StudentResponse.CourseID):
                    matchingStudents = allStudents.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Course) ?
                    temp.Course.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(StudentResponse.DateOfBirth):
                    matchingStudents = allStudents.Where(temp =>
                    (temp.DateOfBirth != null) ?
                    temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(StudentResponse.Address):
                    matchingStudents = allStudents.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Address) ?
                    temp.Address.Equals(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(StudentResponse.Gender):
                    matchingStudents = allStudents.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Gender) ?
                    temp.Gender.Equals(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                default:
                    matchingStudents = allStudents;
                    break;
            }
            return matchingStudents;
        }

        public List<StudentResponse> GetSortedStudents(List<StudentResponse> allStudents, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
                return allStudents;

            List<StudentResponse> sortedStudents = (sortBy, sortOrder)
            switch
            {
                (nameof(StudentResponse.StudentName), SortOrderOptions.ASC) =>
                allStudents.OrderBy(temp => temp.StudentName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(StudentResponse.StudentName), SortOrderOptions.DESC) =>
                allStudents.OrderByDescending(temp => temp.StudentName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(StudentResponse.StudentEmail), SortOrderOptions.ASC) =>
                allStudents.OrderBy(temp => temp.StudentEmail, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(StudentResponse.StudentEmail), SortOrderOptions.DESC) =>
                allStudents.OrderByDescending(temp => temp.StudentEmail, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(StudentResponse.Course), SortOrderOptions.ASC) =>
                allStudents.OrderBy(temp => temp.Course, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(StudentResponse.Course), SortOrderOptions.DESC) =>
                allStudents.OrderByDescending(temp => temp.Course, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(StudentResponse.DateOfBirth), SortOrderOptions.ASC) =>
                allStudents.OrderBy(temp => temp.DateOfBirth).ToList(),

                (nameof(StudentResponse.DateOfBirth), SortOrderOptions.DESC) =>
                allStudents.OrderByDescending(temp => temp.DateOfBirth).ToList(),

                (nameof(StudentResponse.Address), SortOrderOptions.ASC) =>
                allStudents.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(StudentResponse.Address), SortOrderOptions.DESC) =>
                allStudents.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(StudentResponse.Gender), SortOrderOptions.ASC) =>
                allStudents.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(StudentResponse.Gender), SortOrderOptions.DESC) =>
                allStudents.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(StudentResponse.Age), SortOrderOptions.ASC) =>
                allStudents.OrderBy(temp => temp.Age).ToList(),

                (nameof(StudentResponse.Age), SortOrderOptions.DESC) =>
                allStudents.OrderByDescending(temp => temp.Age).ToList(),

                _ => allStudents
            } ;
            return sortedStudents;
        }

        public StudentResponse UpdateStudent(StudentUpdateRequest? studentUpdateRequest)
        {
            if (studentUpdateRequest == null)
                throw new ArgumentNullException(nameof(studentUpdateRequest));

            //Model validation
            ValidationHelper.ModelValidation(studentUpdateRequest);

            //Get matching student object to update
            Student? matchingStudent = _students.FirstOrDefault(temp => temp.StudentID == studentUpdateRequest.StudentID);

            if (matchingStudent == null)
                throw new ArgumentException("Given student id doesn't exist");

            //update all details
            matchingStudent.StudentName = studentUpdateRequest.StudentName;
            matchingStudent.StudentEmail = studentUpdateRequest.StudentEmail;
            matchingStudent.CourseID = studentUpdateRequest.CourseID;
            matchingStudent.DateOfBirth = studentUpdateRequest.DateOfBirth;
            matchingStudent.Address = studentUpdateRequest.Address;
            matchingStudent.Gender = studentUpdateRequest.Gender.ToString();
            
            return matchingStudent.ToStudentResponse();
        }

        public bool DeleteStudent(Guid? studentID)
        {
            if (studentID == null)
                throw new ArgumentNullException(nameof(studentID));

            Student? student = _students.FirstOrDefault(temp => temp.StudentID == studentID);
            if (student == null)
                return false;

            _students.RemoveAll(temp => temp.StudentID == studentID);

            return true;
        }
    }
}
