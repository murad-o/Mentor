using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Controllers.Common;
using MentorCore.DTO.Courses;
using MentorCore.Interfaces.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class CoursesController : BaseController
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }


        /// <summary>
        /// Get course by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseModel))]
        public async Task<ActionResult> GetCourse(int id)
        {
            var course = await _courseService.GetCourseAsync(id);
            return Ok(course);
        }


        /// <summary>
        /// Get all courses
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CourseModel>))]
        public async Task<ActionResult> GetCourses()
        {
            var courses = await _courseService.GetCoursesAsync();
            return Ok(courses);
        }


        /// <summary>
        /// Create course
        /// </summary>
        /// <param name="courseModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCourse(CreateCourseModel courseModel)
        {
            await _courseService.CreateCourseAsync(courseModel);
            return StatusCode(201);
        }


        /// <summary>
        /// Update course
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateCourseModel"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateCourse(int id, UpdateCourseModel updateCourseModel)
        {
            await _courseService.UpdateCourseAsync(id, updateCourseModel);
            return NoContent();
        }


        /// <summary>
        /// Remove course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveCourse(int id)
        {
            await _courseService.RemoveCourseAsync(id);
            return NoContent();
        }
    }
}
