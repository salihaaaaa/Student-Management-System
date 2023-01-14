using System;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit.Abstractions;

namespace StudentManagementSystemTests
{
    public class StudentsServiceTests
    {
        //private field
        private readonly IStudentsService _studentService;
        private readonly ICoursesService _coursesService;
        private readonly ITestOutputHelper _testOutputHelper;

        //constructor
        public StudentsServiceTests(ITestOutputHelper testOutputHelper)
        {
            _studentService = new StudentsService();
            _coursesService = new CoursesService(false);
            _testOutputHelper = testOutputHelper;
        }

        #region AddStudent
        //When we supply null as StudentAddRequest, it should throw ArgumentNullException
        [Fact]
        public void AddStudent_NullStudent()
        {
            //Arrange
            StudentAddRequest? studentAddRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                _studentService.AddStudent(studentAddRequest);
            });
        }

        //When we supply null as StudentName, it should throw ArgumentException
        [Fact]
        public void AddStudent_StudentNameIsNull()
        {
            //Arrange
            StudentAddRequest? studentAddRequest = new StudentAddRequest()
            {
                StudentName = null
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _studentService.AddStudent(studentAddRequest);
            });
        }

        //When we supply proper Student details, it should insert the student into the students list;
        //and it should return an object of PersonResponse, which includes with the newly generated studentID
        [Fact]
        public void AddStudent_ProperStudentDetails()
        {
            //Arrange
            StudentAddRequest? studentAddRequest = new StudentAddRequest()
            {
                StudentName = "Anna",
                StudentEmail = "Anna@example.com",
                CourseID = Guid.NewGuid(),
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "Address anna",
                Gender = GenderOptions.Female
            };

            //Act
            StudentResponse student_response_from_add = _studentService.AddStudent(studentAddRequest);
            List<StudentResponse> students_list = _studentService.GetAllStudents();

            //Assert
            Assert.True(student_response_from_add.StudentID != Guid.Empty);
            Assert.Contains(student_response_from_add, students_list);
        }

        #endregion

        #region GetStudentByStudentID
        //If we supply null as StudentID, it should return null as StudentResponse
        [Fact]
        public void GetStudentByStudentID_NullStudentID()
        {
            //Arrange
            Guid? studentID = null;

            //Act
            StudentResponse? student_response_from_get = _studentService.GetStudentByStudentID(studentID);

            //Assert
            Assert.Null(studentID);
        }

        //If we supply valid studentID, it should return the valid person details as StudentResponse object
        [Fact]
        public void GetStudentByStudentID_WithStudentID()
        {
            CourseAddRequest course_request = new CourseAddRequest()
            {
                CourseName = "C#"
            };
            CourseResponse course_response = _coursesService.AddCourse(course_request);

            StudentAddRequest? student_request= new StudentAddRequest()
            {
                StudentName = "Anna",
                StudentEmail = "Anna@example.com",
                CourseID = Guid.NewGuid(),
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "Address anna",
                Gender = GenderOptions.Female
            };
            StudentResponse student_response_from_add = _studentService.AddStudent(student_request);

            StudentResponse? student_response_from_get = _studentService.GetStudentByStudentID(student_response_from_add.StudentID);

            //Assert
            Assert.Equal(student_response_from_add, student_response_from_get);
        }
        #endregion

        #region GetAllStudents
        //GetAllStudents() should return empty list by default
        [Fact]
        public void GetAllStudents_EmptyList()
        {
            //Act
            List<StudentResponse> student_from_get = _studentService.GetAllStudents();

            //Assert
            Assert.Empty(student_from_get);
        }

        //First, add few students. Then when call GetAllStudents(), it should return the same students that were added
        [Fact]
        public void GetAllStudents_AddFewStudents()
        {
            //Arrange
            CourseAddRequest course_request_1 = new CourseAddRequest()
            {
                CourseName = "C#"
            };
            CourseAddRequest course_request_2 = new CourseAddRequest()
            {
                CourseName = "Java"
            };

            CourseResponse course_response_1 = _coursesService.AddCourse(course_request_1);
            CourseResponse course_response_2 = _coursesService.AddCourse(course_request_2);

            StudentAddRequest? student_request_1 = new StudentAddRequest()
            {
                StudentName = "Anna",
                StudentEmail = "Anna@example.com",
                CourseID = course_response_1.CourseID,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "Address anna",
                Gender = GenderOptions.Female
            };
            StudentAddRequest? student_request_2 = new StudentAddRequest()
            {
                StudentName = "Bob",
                StudentEmail = "Bob@example.com",
                CourseID = course_response_1.CourseID,
                DateOfBirth = DateTime.Parse("2003-11-01"),
                Address = "Address bob",
                Gender = GenderOptions.Male
            };
            StudentAddRequest? student_request_3 = new StudentAddRequest()
            {
                StudentName = "Cindy",
                StudentEmail = "Cindy@example.com",
                CourseID = course_response_2.CourseID,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "Address cindy",
                Gender = GenderOptions.Female
            };

            List<StudentAddRequest> student_requests = new List<StudentAddRequest>()
            {
                student_request_1,
                student_request_2,
                student_request_3
            };

            List<StudentResponse> student_response_list_from_add = new List<StudentResponse>();

            foreach (StudentAddRequest student_request in student_requests)
            {
                StudentResponse student_response = _studentService.AddStudent(student_request);
                student_response_list_from_add.Add(student_response);
            }

            //print student_response_list_from_add
            _testOutputHelper.WriteLine("Expected: ");
            foreach (StudentResponse student_response_from_add in student_response_list_from_add)
            {
                _testOutputHelper.WriteLine(student_response_from_add.ToString());
            }

            //Act
            List<StudentResponse> students_response_list_from_get = _studentService.GetAllStudents();

            //print student_response_list_from_get
            _testOutputHelper.WriteLine("Actual: ");
            foreach (StudentResponse student_response_from_get in students_response_list_from_get)
            {
                _testOutputHelper.WriteLine(student_response_from_get.ToString());
            }

            //Assert
            foreach (StudentResponse student_response_from_add in student_response_list_from_add)
            {
                Assert.Contains(student_response_from_add, students_response_list_from_get);
            }
        }
        #endregion

        #region GetFilteredStudents
        //If the search text is empty and search by is "StudentName", it should return all students
        [Fact]
        public void GetFilteredPersons_EmptySearchText()
        {
            //Arrange
            CourseAddRequest course_request_1 = new CourseAddRequest()
            {
                CourseName = "C#"
            };
            CourseAddRequest course_request_2 = new CourseAddRequest()
            {
                CourseName = "Java"
            };

            CourseResponse course_response_1 = _coursesService.AddCourse(course_request_1);
            CourseResponse course_response_2 = _coursesService.AddCourse(course_request_2);

            StudentAddRequest? student_request_1 = new StudentAddRequest()
            {
                StudentName = "Anna",
                StudentEmail = "Anna@example.com",
                CourseID = course_response_1.CourseID,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "Address anna",
                Gender = GenderOptions.Female
            };
            StudentAddRequest? student_request_2 = new StudentAddRequest()
            {
                StudentName = "Bob",
                StudentEmail = "Bob@example.com",
                CourseID = course_response_1.CourseID,
                DateOfBirth = DateTime.Parse("2003-11-01"),
                Address = "Address bob",
                Gender = GenderOptions.Male
            };
            StudentAddRequest? student_request_3 = new StudentAddRequest()
            {
                StudentName = "Cindy",
                StudentEmail = "Cindy@example.com",
                CourseID = course_response_2.CourseID,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "Address cindy",
                Gender = GenderOptions.Female
            };

            List<StudentAddRequest> student_requests = new List<StudentAddRequest>()
            {
                student_request_1,
                student_request_2,
                student_request_3
            };

            List<StudentResponse> student_response_list_from_add = new List<StudentResponse>();

            foreach (StudentAddRequest student_request in student_requests)
            {
                StudentResponse student_response = _studentService.AddStudent(student_request);
                student_response_list_from_add.Add(student_response);
            }

            //print student_response_list_from_add
            _testOutputHelper.WriteLine("Expected: ");
            foreach (StudentResponse student_response_from_add in student_response_list_from_add)
            {
                _testOutputHelper.WriteLine(student_response_from_add.ToString());
            }

            //Act
            List<StudentResponse> students_response_list_from_search = _studentService.GetFilteredStudents(nameof(Student.StudentName), "");

            //print student_response_list_from_get
            _testOutputHelper.WriteLine("Actual: ");
            foreach (StudentResponse student_response_from_get in students_response_list_from_search)
            {
                _testOutputHelper.WriteLine(student_response_from_get.ToString());
            }

            //Assert
            foreach (StudentResponse student_response_from_add in student_response_list_from_add)
            {
                Assert.Contains(student_response_from_add, students_response_list_from_search);
            }
        }

        // First, add few students. Then we will search based on student name with some search string. It should return the matching students
        [Fact]
        public void GetFilteredPersons_SearchByStudentName()
        {
            //Arrange
            CourseAddRequest course_request_1 = new CourseAddRequest()
            {
                CourseName = "C#"
            };
            CourseAddRequest course_request_2 = new CourseAddRequest()
            {
                CourseName = "Java"
            };

            CourseResponse course_response_1 = _coursesService.AddCourse(course_request_1);
            CourseResponse course_response_2 = _coursesService.AddCourse(course_request_2);

            StudentAddRequest? student_request_1 = new StudentAddRequest()
            {
                StudentName = "Anna",
                StudentEmail = "Anna@example.com",
                CourseID = course_response_1.CourseID,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "Address anna",
                Gender = GenderOptions.Female
            };
            StudentAddRequest? student_request_2 = new StudentAddRequest()
            {
                StudentName = "Bob",
                StudentEmail = "Bob@example.com",
                CourseID = course_response_1.CourseID,
                DateOfBirth = DateTime.Parse("2003-11-01"),
                Address = "Address bob",
                Gender = GenderOptions.Male
            };
            StudentAddRequest? student_request_3 = new StudentAddRequest()
            {
                StudentName = "Cindy",
                StudentEmail = "Cindy@example.com",
                CourseID = course_response_2.CourseID,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "Address cindy",
                Gender = GenderOptions.Female
            };

            List<StudentAddRequest> student_requests = new List<StudentAddRequest>()
            {
                student_request_1,
                student_request_2,
                student_request_3
            };

            List<StudentResponse> student_response_list_from_add = new List<StudentResponse>();

            foreach (StudentAddRequest student_request in student_requests)
            {
                StudentResponse student_response = _studentService.AddStudent(student_request);
                student_response_list_from_add.Add(student_response);
            }

            //print student_response_list_from_add
            _testOutputHelper.WriteLine("Expected: ");
            foreach (StudentResponse student_response_from_add in student_response_list_from_add)
            {
                if (student_response_from_add.StudentName != null)
                {
                    if (student_response_from_add.StudentName.Contains("cin", StringComparison.OrdinalIgnoreCase))
                    {
                        _testOutputHelper.WriteLine(student_response_from_add.ToString());
                    }
                }  
            }

            //Act
            List<StudentResponse> students_response_list_from_search = _studentService.GetFilteredStudents(nameof(Student.StudentName), "cin");

            //print student_response_list_from_get
            _testOutputHelper.WriteLine("Actual: ");
            foreach (StudentResponse student_response_from_get in students_response_list_from_search)
            {
                _testOutputHelper.WriteLine(student_response_from_get.ToString());
            }

            //Assert
            foreach (StudentResponse student_response_from_add in student_response_list_from_add)
            {
                if (student_response_from_add.StudentName != null)
                {
                    if (student_response_from_add.StudentName.Contains("cin", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(student_response_from_add, students_response_list_from_search);
                    }
                }
            }
        }
        #endregion

        #region GetSortedStudents
        //When we sort based on StudentName in DESC, it should return students list in descending on StudentName
        [Fact]
        public void GetSortedStudents()
        {
            //Arrange
            CourseAddRequest course_request_1 = new CourseAddRequest()
            {
                CourseName = "C#"
            };
            CourseAddRequest course_request_2 = new CourseAddRequest()
            {
                CourseName = "Java"
            };

            CourseResponse course_response_1 = _coursesService.AddCourse(course_request_1);
            CourseResponse course_response_2 = _coursesService.AddCourse(course_request_2);

            StudentAddRequest? student_request_1 = new StudentAddRequest()
            {
                StudentName = "Anna",
                StudentEmail = "Anna@example.com",
                CourseID = course_response_1.CourseID,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "Address anna",
                Gender = GenderOptions.Female
            };
            StudentAddRequest? student_request_2 = new StudentAddRequest()
            {
                StudentName = "Bob",
                StudentEmail = "Bob@example.com",
                CourseID = course_response_1.CourseID,
                DateOfBirth = DateTime.Parse("2003-11-01"),
                Address = "Address bob",
                Gender = GenderOptions.Male
            };
            StudentAddRequest? student_request_3 = new StudentAddRequest()
            {
                StudentName = "Cindy",
                StudentEmail = "Cindy@example.com",
                CourseID = course_response_2.CourseID,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "Address cindy",
                Gender = GenderOptions.Female
            };

            List<StudentAddRequest> student_requests = new List<StudentAddRequest>()
            {
                student_request_1,
                student_request_2,
                student_request_3
            };

            List<StudentResponse> student_response_list_from_add = new List<StudentResponse>();

            foreach (StudentAddRequest student_request in student_requests)
            {
                StudentResponse student_response = _studentService.AddStudent(student_request);
                student_response_list_from_add.Add(student_response);
            }

            student_response_list_from_add = student_response_list_from_add.OrderByDescending(temp => temp.StudentName).ToList();

            //print student_response_list_from_add
            _testOutputHelper.WriteLine("Expected: ");
            foreach (StudentResponse student_response_from_add in student_response_list_from_add)
            {
                _testOutputHelper.WriteLine(student_response_from_add.ToString());  
            }

            List<StudentResponse> allStudents = _studentService.GetAllStudents();

            //Act
            List<StudentResponse> students_response_list_from_sort = _studentService.GetSortedStudents(allStudents, nameof(Student.StudentName), SortOrderOptions.DESC);

            //print student_response_list_from_get
            _testOutputHelper.WriteLine("Actual: ");
            foreach (StudentResponse student_response_from_get in students_response_list_from_sort)
            {
                _testOutputHelper.WriteLine(student_response_from_get.ToString());
            }

            //Assert
            for (int i = 0; i < student_response_list_from_add.Count; i++)
            {
                Assert.Equal(student_response_list_from_add[i], students_response_list_from_sort[i]);
            }
        }
        #endregion

        #region UpdateStudent
        //When we supply null as StudentUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public void UpdateStudent_NullStudent()
        {
            //Arrange
            StudentUpdateRequest? student_update_request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _studentService.UpdateStudent(student_update_request);
            });
        }

        //When we supply invalid Student ID, it should throw ArgumentException
        [Fact]
        public void UpdateStudent_InvalidStudentID()
        {
            //Arrange
            StudentUpdateRequest? student_update_request = new StudentUpdateRequest()
            {
                StudentID = Guid.NewGuid()
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _studentService.UpdateStudent(student_update_request);
            });
        }

        //When StudentName is null, it should throw ArgumentException
        [Fact]
        public void UpdateStudent_StudentNameIsNull()
        {
            //Arrange
            CourseAddRequest course_request = new CourseAddRequest()
            {
                CourseName = "C#"
            };

            CourseResponse course_response = _coursesService.AddCourse(course_request);

            StudentAddRequest? student_add_request = new StudentAddRequest()
            {
                StudentName = "Anna",
                StudentEmail = "Anna@example.com",
                CourseID = course_response.CourseID,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "Address anna",
                Gender = GenderOptions.Female
            };

            StudentResponse student_response_from_add = _studentService.AddStudent(student_add_request);

            StudentUpdateRequest student_update_request = student_response_from_add.ToStudentUpdateRequest();
            student_update_request.StudentName = null;

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _studentService.UpdateStudent(student_update_request);
            });
        }

        //First, add new student and try to update the student name and email
        [Fact]
        public void UpdateStudent_ProperFullDetailsUpdation()
        {
            //Arrange
            CourseAddRequest course_request = new CourseAddRequest()
            {
                CourseName = "C#"
            };

            CourseResponse course_response = _coursesService.AddCourse(course_request);

            StudentAddRequest? student_add_request = new StudentAddRequest()
            {
                StudentName = "Anna",
                StudentEmail = "Anna@example.com",
                CourseID = course_response.CourseID,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "Address anna",
                Gender = GenderOptions.Female
            };

            StudentResponse student_response_from_add = _studentService.AddStudent(student_add_request);

            StudentUpdateRequest student_update_request = student_response_from_add.ToStudentUpdateRequest();
            student_update_request.StudentName = "Mike";
            student_update_request.StudentEmail = "Mike@example.com";

            //Act
            StudentResponse student_response_from_update = _studentService.UpdateStudent(student_update_request);
            StudentResponse? student_response_from_get = _studentService.GetStudentByStudentID(student_response_from_update.StudentID);
            
            //Assert
            Assert.Equal(student_response_from_get, student_response_from_update);
        }
        #endregion

        #region DeleteStudent
        //If you supply an valid StudentID, it should return false
        [Fact]
        public void DeleteStudent_ValidStudentID()
        {
            //Arrange
            CourseAddRequest course_add_request = new CourseAddRequest()
            {
                CourseName = "C#"
            };
            CourseResponse course_response = _coursesService.AddCourse(course_add_request);

            StudentAddRequest student_add_request = new StudentAddRequest()
            {
                StudentName = "Anna",
                StudentEmail = "Anna@example.com",
                CourseID = Guid.NewGuid(),
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "Address anna",
                Gender = GenderOptions.Female
            };

            StudentResponse student_response_from_add = _studentService.AddStudent(student_add_request);

            //Act
            bool isDeleted = _studentService.DeleteStudent(student_response_from_add.StudentID);

            //Assert
            Assert.True(isDeleted);
        }

        //If you supply an invalid StudentID, it should return false
        [Fact]
        public void DeleteStudent_InvalidStudentID()
        {
            //Act
            bool isDeleted = _studentService.DeleteStudent(Guid.NewGuid());

            //Assert
            Assert.False(isDeleted);
        }
        #endregion
    }
}
