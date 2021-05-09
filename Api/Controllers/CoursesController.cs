using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Entities.Models;
using MentorCore.DTO.Courses;
using MentorCore.Interfaces.Account;
using MentorCore.Interfaces.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CoursesController(ICourseService courseService, IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _courseService = courseService;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CourseModel>> GetCourse(int id)
        {
            var course = await _courseService.GetCourseAsync(id);

            if (course is null)
                return NotFound();

            var courseResponse = _mapper.Map<CourseModel>(course);
            return courseResponse;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseModel>>> GetCourses()
        {
            var courses = await _courseService.GetCoursesAsync();

            if (!courses.Any())
                return NotFound();

            var coursesResponse = courses.Select(c => _mapper.Map<CourseModel>(c));
            return Ok(coursesResponse);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCourse(CreateCourseModel courseModel)
        {
            var course = _mapper.Map<Course>(courseModel);

            try
            {
                await _courseService.CreateCourseAsync(course);
            }
            catch (SecurityTokenException)
            {
                return BadRequest();
            }

            return StatusCode(201);
        }


        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCourse(int id, UpdateCourseModel updateCourseModel)
        {
            try
            {
                var courseToUpdate = await _courseService.GetCourseAsync(id);

                if (courseToUpdate is null)
                    return NotFound();

                var currentUser = await _currentUserService.GetCurrentUser();

                if (!_courseService.IsUserOwner(currentUser, courseToUpdate))
                    return BadRequest();

                await _courseService.UpdateCourseAsync(courseToUpdate, updateCourseModel);

                return NoContent();
            }
            catch (SecurityTokenException)
            {
                return BadRequest();
            }
        }


        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> RemoveCourse(int id)
        {
            try
            {
                var course = await _courseService.GetCourseAsync(id);

                if (course is null)
                    return NotFound();

                var currentUser = await _currentUserService.GetCurrentUser();

                if (!_courseService.IsUserOwner(currentUser, course))
                    return BadRequest();

                await _courseService.RemoveCourseAsync(course);
                return NoContent();
            }
            catch (SecurityTokenException)
            {
                return BadRequest();
            }
        }
    }
}
