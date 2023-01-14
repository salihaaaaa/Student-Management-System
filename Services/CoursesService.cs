using ServiceContracts;
using ServiceContracts.DTO;
using Entities;
using System.ComponentModel.DataAnnotations;

namespace Services
{
    public class CoursesService : ICoursesService
    {
        //private field
        private readonly List<Course> _courses;

        //constructor
        public CoursesService(bool initialize = true)
        {
            _courses = new List<Course>();

            if (initialize)
            {
                _courses.AddRange(new List<Course>()
                {
                    new Course()
                    {
                        CourseID = Guid.Parse("9275F118-508E-4B22-A202-462EFB3D0637"),
                        CourseName = "Bachelor in Accounting"
                    },
                    new Course()
                    {
                        CourseID = Guid.Parse("4C3124FB-FC2F-43C7-9C6C-DD6471489C2D"),
                        CourseName = "Bachelor in Computer Science"
                    },
                    new Course()
                    {
                        CourseID = Guid.Parse("3A806107-9C01-435D-8976-5A1715C7B434"),
                        CourseName = "Bachelor in Electrical Engineering"
                    }
                });  
            }
        }

        public CourseResponse AddCourse(CourseAddRequest? courseAddRequest)
        {
            //Validation: courseAddRequest can't be null
            if (courseAddRequest == null)
            {
                throw new ArgumentNullException(nameof(courseAddRequest));
            }

            //Validation: CourseName can't be null
            if (courseAddRequest.CourseName == null)
            {
                throw new ArgumentException(nameof(courseAddRequest.CourseName));
            }

            //ValidationAttribute: CourseName can't be duplicate
            if (_courses.Where(temp => temp.CourseName == courseAddRequest.CourseName).Count() > 0)
            {
                throw new ArgumentException("Course name already exist");
            }

            //Convert object from CourseAddRequest to Course type
            Course course = courseAddRequest.ToCourse();

            //generated CourseID
            course.CourseID = Guid.NewGuid();

            //Add course object into _courses
            _courses.Add(course);

            return course.ToCourseResponse();
        }

        public List<CourseResponse> GetAllCourse()
        {
            return _courses.Select(course => course.ToCourseResponse()).ToList();
        }

        public CourseResponse? GetCourseByCourseID(Guid? courseID)
        {
            if (courseID == null)
                return null;

            Course? course_response_from_list = _courses.FirstOrDefault(temp => temp.CourseID == courseID);

            if (course_response_from_list == null)
                return null;

            return course_response_from_list.ToCourseResponse();
        }
    }
}