﻿using System.Security;
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
    }
}
