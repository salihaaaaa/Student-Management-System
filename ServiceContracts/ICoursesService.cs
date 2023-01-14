using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Course entity
    /// </summary>
    public interface ICoursesService
    {
        /// <summary>
        /// Adds a course object to the list of courses
        /// </summary>
        /// <param name="courseAddRequest">Course object to add</param>
        /// <returns>Returns the course object after adding it (including newly generated CourseID)</returns>
        CourseResponse AddCourse(CourseAddRequest? courseAddRequest);

        /// <summary>
        /// Returns all courses from the list
        /// </summary>
        /// <returns>All courses from the list</returns>
        List<CourseResponse> GetAllCourse();

        /// <summary>
        /// Returns  a course object based on the given course id
        /// </summary>
        /// <param name="courseID">CourseID to search</param>
        /// <returns>Matching course as CourseResponse object</returns>
        CourseResponse? GetCourseByCourseID(Guid? courseID);
    }
}