using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreAPI.Models;

namespace CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ContosouniversityContext _context;

        public CoursesController(ContosouniversityContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourse()
        {
            return await _context.Course.ToListAsync();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // GET: api/Courses/5/Departments/1
        [HttpGet("{courseId:int}/Departments/{departmentId:int}")]
        public async Task<ActionResult<IEnumerable<VwCourseStudents>>> GetCourseStudents(int courseId, int departmentId)
        {
            var vwCourseStudents = await _context.VwCourseStudents
                .FromSqlInterpolated($"SELECT * FROM dbo.vwCourseStudents")
                .Where(v => v.CourseId == courseId && v.DepartmentId == departmentId)
                .ToListAsync();

            if (vwCourseStudents == null)
            {
                return NotFound();
            }

            return vwCourseStudents;
        }

        // GET: api/Courses/1/Counts
        [HttpGet("{CourseId:int}/Counts")]
        public async Task<ActionResult<IEnumerable<VwCourseStudentCount>>> GetCourseStudentCount(int courseId)
        {
            var vwCourseStudentCount = await _context.VwCourseStudentCount
                .FromSqlInterpolated($"SELECT * FROM dbo.vwCourseStudentCount")
                .Where(v => v.CourseId == courseId)
                .ToListAsync();

            if (vwCourseStudentCount == null)
            {
                return NotFound();
            }

            return vwCourseStudentCount;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            _context.Course.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Course>> DeleteCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Course.Remove(course);
            await _context.SaveChangesAsync();

            return course;
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseId == id);
        }
    }
}
