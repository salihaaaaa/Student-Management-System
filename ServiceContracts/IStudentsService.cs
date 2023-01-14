using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IStudentsService
    {
        /// <summary>
        /// Adds a new student into the list of students
        /// </summary>
        /// <param name="studentAddRequest">Student to add</param>
        /// <returns>Returns the same student details, along with newly generated StudentID</returns>
        StudentResponse AddStudent(StudentAddRequest? studentAddRequest);

        /// <summary>
        /// Returns all students
        /// </summary>
        /// <returns>Returns a list of objects of StudentResponse type</returns>
        List<StudentResponse> GetAllStudents();

        /// <summary>
        /// Returns the student object based on the given student id
        /// </summary>
        /// <param name="studentID">Student id to search</param>
        /// <returns>Returns matching person object</returns>
        StudentResponse? GetStudentByStudentID(Guid? studentID);

        /// <summary>
        /// Returns all student objects that matches with the given search field and search string
        /// </summary>
        /// <param name="searchBy">search string to search</param>
        /// <param name=""></param>
        /// <returns>Returns all matching students based on the given search by and search string</returns>
        List<StudentResponse> GetFilteredStudents(string searchBy, string? searchString);

        /// <summary>
        /// Returns sorted list of students
        /// </summary>
        /// <param name="allStudents">Represents list of students to sort</param>
        /// <param name="sortBy">Name of the property(key), based on which students shoul be sorted</param>
        /// <param name="sortOrder">ASC or DESC</param>
        /// <returns>Returns sorted students as StudentResponse list</returns>
        List<StudentResponse> GetSortedStudents(List<StudentResponse> allStudents, string sortBy, SortOrderOptions sortOrder);

        /// <summary>
        /// Updates the specified student details based on the given student ID
        /// </summary>
        /// <param name="studentUpdateRequest">Student details to update, including student id</param>
        /// <returns>Returns the student response object after updation</returns>
        StudentResponse UpdateStudent(StudentUpdateRequest? studentUpdateRequest);

        /// <summary>
        /// Delete student based on the given student id
        /// </summary>
        /// <param name="studentID">student id to delete</param>
        /// <returns>Returns true if deletion is success</returns>
        bool DeleteStudent(Guid? studentID);
    }
}
