using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Worktastic.Data;
using Worktastic.Models;

namespace Worktastic.Controllers
{
    public class JobPostingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobPostingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var jobPostingsFromDb = _context.JobPostings.Where(x => x.OwnerUsername == User.Identity.Name).ToList();
            return View(jobPostingsFromDb);
        }

        public IActionResult CreateEditJobPosting(int id)
        {
            if(id != 0)
            {
                var jobPostingFromDb = _context.JobPostings.SingleOrDefault(x => x.Id == id);

                if(jobPostingFromDb != null)
                {
                    return View(jobPostingFromDb);
                } else
                {
                    return NotFound();
                }
            }

            return View();
        }

        public IActionResult CreateEditJob(JobPosting jobPosting, IFormFile file)
        {
            jobPosting.OwnerUsername = User.Identity.Name;

            if(file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var bytes = ms.ToArray();
                    jobPosting.CompanyImage = bytes;
                }
            }

            if(jobPosting.Id == 0)
            {
                // Add new job if not editing
                _context.JobPostings.Add(jobPosting);
            } else
            {
                var jobFromDb = _context.JobPostings.SingleOrDefault(x => x.Id == jobPosting.Id);

                if(jobFromDb == null)
                {
                    return NotFound();
                }

                jobFromDb.CompanyImage = jobPosting.CompanyImage;
                jobFromDb.CompanyName = jobPosting.CompanyName;
                jobFromDb.ContactMail = jobPosting.ContactMail;
                jobFromDb.ContactPhone = jobPosting.ContactPhone;
                jobFromDb.ContactWebsite = jobPosting.ContactWebsite;
                jobFromDb.Description = jobPosting.Description;
                jobFromDb.JobLocation = jobPosting.JobLocation;
                jobFromDb.JobTitle = jobPosting.JobTitle;
                jobFromDb.Salary = jobPosting.Salary;
                jobFromDb.StartDate = jobPosting.StartDate;
                jobFromDb.OwnerUsername = jobPosting.OwnerUsername;
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteJobPostingById(int id)
        {
            if (id == 0)
                return BadRequest();

            var jobPostingFromDb = _context.JobPostings.SingleOrDefault(x => x.Id == id);

            if (jobPostingFromDb == null)
                return NotFound();

            _context.JobPostings.Remove(jobPostingFromDb);
            _context.SaveChanges();

            return Ok();
        }


    }
}
