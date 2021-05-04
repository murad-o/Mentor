using System.Threading.Tasks;
using AutoMapper;
using Entities.Models;
using MentorCore.DTO.Courses;
using MentorCore.Interfaces.Courses;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;

        public CoursesController(ICourseService courseService, IMapper mapper)
        {
            _courseService = courseService;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<IActionResult> CreateCourse(CreateCourseModel courseModel)
        {
            var course = _mapper.Map<Course>(courseModel);

            await _courseService.CreateCourseAsync(course);
            return StatusCode(201);
        }
    }
}
