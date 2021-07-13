using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Worktastic.Data;

namespace Worktastic.Controllers
{
    [Route("api/jobposting")]
    [ApiController]
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
    }
}
