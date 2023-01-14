using System;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace StudentManagementSystemTests
{
    public class CoursesServiceTest
    {
        //private field
        private readonly ICoursesService _coursesService;

        //contructor
        public CoursesServiceTest()
        {
            _coursesService = new CoursesService();
        }

        #region AddCourse
        //When CourseAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public void AddCourse_NullCourse()
        {
            //Arrange
            CourseAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() => 
                
                //Act
                _coursesService.AddCourse(request));
        }

        //When CourseName is null, it should throw ArgumentException
        [Fact]
        public void AddCourse_CourseNameIsNull()
        {
            //Arrange
            CourseAddRequest? request = new CourseAddRequest()
            {
                CourseName = null
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _coursesService.AddCourse(request);
            });
        }

        //When CourseName is duplicate, it should throw ArgumentException
        [Fact]
        public void AddCourse_CourseNameIsDuplicate()
        {
            //Arrange
            CourseAddRequest? request1 = new CourseAddRequest()
            {
                CourseName = "C#"
            };
            CourseAddRequest? request2 = new CourseAddRequest()
            {
                CourseName = "C#"
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _coursesService.AddCourse(request1);
                _coursesService.AddCourse(request2);
            });     
        }

        //When you supply proper CourseName, it should add the course to the existing list of courses
        [Fact]
        public void AddCourse_ProperCourseDetails()
        {
            //Arrange
            CourseAddRequest? request = new CourseAddRequest()
            {
                CourseName = "C#"
            };

            //Act
            CourseResponse response = _coursesService.AddCourse(request);
            List<CourseResponse> courses_from_GetAllCourses = _coursesService.GetAllCourse();

            //Assert
            Assert.True(response.CourseID != Guid.Empty);
            Assert.Contains(response, courses_from_GetAllCourses);
        }
        #endregion

        #region GetAllCourses
        [Fact]
        //The list of courses should be empty by default (before adding new courses)
        public void GetAllCourses_EmptyList()
        {
            //Act
            List<CourseResponse> actual_course_response_list = _coursesService.GetAllCourse();

            //Assert
            Assert.Empty(actual_course_response_list);
        }

        [Fact]
        public void GetAllCourses_AddFewCourses()
        {
            //Arrange
            List<CourseAddRequest> course_request_list = new List<CourseAddRequest>()
            {
                new CourseAddRequest()
                {
                    CourseName = "C#"
                },
                new CourseAddRequest()
                {
                    CourseName = "Java"
                }
            };

            //Act
            List<CourseResponse> course_list_from_add_course = new List<CourseResponse>();

            foreach (CourseAddRequest courseRequest in course_request_list)
            {
                 course_list_from_add_course.Add(_coursesService.AddCourse(courseRequest));
            }

            List<CourseResponse> actual_course_response_list = _coursesService.GetAllCourse();

            //read each element from courses_list_from_add_course
            foreach (CourseResponse expected_course in course_list_from_add_course)
            {
                Assert.Contains(expected_course, actual_course_response_list);
            }
        }
        #endregion

        #region GetCourseByCourseID
        [Fact]
        //If we supply null as CourseID, it should return null as CourseResponse
        public void GetCourseByCourseID_NullCourseID()
        {
            //Arrange
            Guid? courseID = null;

            //Act
            CourseResponse? course_response_from_get_method = _coursesService.GetCourseByCourseID(courseID);

            //Assert
            Assert.Null(course_response_from_get_method);
        }


        [Fact]
        //If we supply a valid course id, it should return the matching details as CourseResponse object
        public void GetCourseByCourseID_ValidCourseID()
        {
            //Arrange
            CourseAddRequest? course_add_request = new CourseAddRequest()
            {
                CourseName = "C#"
            };

            CourseResponse course_response_from_add_request = _coursesService.AddCourse(course_add_request);

            //Act
            CourseResponse? course_response_from_get = _coursesService.GetCourseByCourseID(course_response_from_add_request.CourseID);

            //Assert
            Assert.Equal(course_response_from_add_request, course_response_from_get);
        }
        #endregion
    }
}
