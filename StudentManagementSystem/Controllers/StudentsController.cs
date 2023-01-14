using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace StudentManagementSystem.Controllers
{
    [Route("[controller]")]
    public class StudentsController : Controller
    {
        //private
        private readonly IStudentsService _studentsService;
        private readonly ICoursesService _coursesService;

        //constructor
        public StudentsController(IStudentsService studentsService, ICoursesService coursesService)
        {
            _studentsService = studentsService;
            _coursesService = coursesService;
        }

        [Route("[action]")]
        [Route("/")]
        public IActionResult Index(string searchBy, string? searchString, string sortBy = nameof(StudentResponse.StudentName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            //search
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(StudentResponse.StudentName), "Student Name" },
                { nameof(StudentResponse.StudentEmail), "Email" },
                { nameof(StudentResponse.CourseID), "Course" },
                { nameof(StudentResponse.DateOfBirth), "Date of Birth" },
                { nameof(StudentResponse.Address), "Address" },
                { nameof(StudentResponse.Gender), "Gender" }
            };

            List<StudentResponse> students = _studentsService.GetFilteredStudents(searchBy, searchString);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            //sort
            List<StudentResponse> sortedStudents = _studentsService.GetSortedStudents(students, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();

            return View(sortedStudents);
        }

        //Executes when the user click on "Create Student" hyperlink (while opening the create view)
        [Route("[action]")]
        [HttpGet]
        public IActionResult Create()
        {
           List<CourseResponse> courses = _coursesService.GetAllCourse();

           ViewBag.Courses = courses.Select(temp => 
           new SelectListItem()
           {
               Text = temp.CourseName,
               Value = temp.CourseID.ToString()
           });

           return View();
        }

         
        [Route("[action]")]
        [HttpPost]
        public IActionResult Create(StudentAddRequest studentAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CourseResponse> courses = _coursesService.GetAllCourse();
                ViewBag.Courses = courses.Select(temp =>
                   new SelectListItem()
                   {
                       Text = temp.CourseName,
                       Value = temp.CourseID.ToString()
                   });

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).SelectMany(e => e.ErrorMessage).ToList();
                return View();
            }

            StudentResponse studentResponse = _studentsService.AddStudent(studentAddRequest);

            return RedirectToAction("Index", "Students");
        }

        //Executes when the user click on "Edit" hyperlink (while opening the edit view)
        [Route("[action]/{studentID}")]
        [HttpGet]
        public IActionResult Edit(Guid studentID)
        {
            StudentResponse? studentResponse = _studentsService.GetStudentByStudentID(studentID);
            if (studentResponse == null)
                return RedirectToAction("Index");

            StudentUpdateRequest studentUpdateRequest = studentResponse.ToStudentUpdateRequest();

            List<CourseResponse> courses = _coursesService.GetAllCourse();

            ViewBag.Courses = courses.Select(temp =>
            new SelectListItem()
            {
                Text = temp.CourseName,
                Value = temp.CourseID.ToString()
            });

            return View(studentUpdateRequest);
        }

        [Route("[action]/{studentID}")]
        [HttpPost]
        public IActionResult Edit(StudentUpdateRequest studentUpdateRequest)
        {
            StudentResponse? studentResponse = _studentsService.GetStudentByStudentID(studentUpdateRequest.StudentID);
            if (studentResponse == null)
                return RedirectToAction("Index");

            if (ModelState.IsValid)
            {
                StudentResponse updatedStudent = _studentsService.UpdateStudent(studentUpdateRequest);
                return RedirectToAction("Index");
            }
            else
            {
                List<CourseResponse> courses = _coursesService.GetAllCourse();
                ViewBag.Courses = courses.Select(temp =>
                   new SelectListItem()
                   {
                       Text = temp.CourseName,
                       Value = temp.CourseID.ToString()
                   });

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).SelectMany(e => e.ErrorMessage).ToList();
                return View(studentResponse.ToStudentUpdateRequest());
            }
        }

        //Executes when the user click on "Delete" hyperlink (while opening the delete view)
        [Route("[action]/{studentID}")]
        [HttpGet]
        public IActionResult Delete(Guid? studentID)
        {
            StudentResponse? studentResponse = _studentsService.GetStudentByStudentID(studentID);
            if (studentResponse == null)
                RedirectToAction("Index");

            return View(studentResponse);
        }

        [Route("[action]/{studentID}")]
        [HttpPost]
        public IActionResult Delete(StudentUpdateRequest studentUpdateRequest)
        {
            StudentResponse? studentResponse = _studentsService.GetStudentByStudentID(studentUpdateRequest.StudentID);
            if (studentResponse == null)
                return RedirectToAction("Index");

            _studentsService.DeleteStudent(studentUpdateRequest.StudentID);
            return RedirectToAction("Index");
        }

        [Route("[action]")]
        public IActionResult StudentsPDF()
        {
            //Get list of students
            List<StudentResponse> students = _studentsService.GetAllStudents();

            //Return view as pdf
            return new ViewAsPdf("StudentsPDF", students, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20, Right = 20, Left = 20, Bottom = 20
                },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }
}
