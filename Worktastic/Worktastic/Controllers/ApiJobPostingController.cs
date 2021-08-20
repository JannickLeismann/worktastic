using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Worktastic.Data;
using Worktastic.Filters;
using Worktastic.Models;

namespace Worktastic.Controllers
{
    [Route("api/jobposting")]
    [ApiController]
    [ApiKeyAuthoriziation]
    public class ApiJobPostingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApiJobPostingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var allJobPostings = _context.JobPostings.ToArray();
            return Ok(allJobPostings);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var jobPosting = _context.JobPostings.SingleOrDefault(x => x.Id == id);

            if (jobPosting == null)
                return NotFound();

            return Ok(jobPosting);
        }

        [HttpPost("Create")]
        public IActionResult Create(JobPosting jobPosting)
        {
            if (jobPosting.Id != 0)
                return BadRequest();

            _context.JobPostings.Add(jobPosting);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var jobPosting = _context.JobPostings.SingleOrDefault(x => x.Id == id);

            if (jobPosting == null)
                return NotFound();

            _context.JobPostings.Remove(jobPosting);
            _context.SaveChanges();

            return Ok("Objekt wurde gelöscht.");
        }

        [HttpPut("Update")]
        public IActionResult Update(JobPosting jobPosting)
        {
            if (jobPosting.Id == 0)
                return BadRequest("JobPosting hat keine Id.");

            _context.JobPostings.Update(jobPosting);
            _context.SaveChanges();

            return Ok("Objekt gespeichert");
        }
    }
}
