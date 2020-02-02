using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Class.Web.Models;
using Class.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Class.Web.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment _enviorment;
        private string _connectionString;
        public HomeController(IHostingEnvironment enviornment, IConfiguration configuration)
        {
            _enviorment = enviornment;
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddCandidate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCandidate(Candidate candidate)
        {
            var db = new CandidateRepository(_connectionString);
            db.AddCandidate(candidate);

            return Redirect("/home/pending");
        }
        public IActionResult Pending()
        {
            var db = new CandidateRepository(_connectionString);
            ViewBag.PendingCount = db.GetPendingCount();
            return View(db.GetPending());
        }
        public IActionResult ViewCandidate(int id)
        {
            var db = new CandidateRepository(_connectionString);
            return View(db.GetCandidate(id));

        }

        public IActionResult Confirm(int id)
        {
            var db = new CandidateRepository(_connectionString);
            db.Confirm(id);

            return Json(id);
        }
        public IActionResult Confirmed()
        {
            var db = new CandidateRepository(_connectionString);
            ViewBag.ConfirmedCount = db.GetConfirmedCount();

            return View(db.GetConfirmed());
        }
        public IActionResult Declined()
        {
            var db = new CandidateRepository(_connectionString);
            ViewBag.DeclinedCount = db.GetDeclinedCount();

            return View(db.GetDeclined());
        }
        [HttpPost]
        public IActionResult Decline(int id)
        {
            var db = new CandidateRepository(_connectionString);
            db.Decline(id);

            return Json(new { Id = id });
        }
        [HttpPost]
        public IActionResult GetCounts()
        {
            var db = new CandidateRepository(_connectionString);
            int confirmed= db.GetConfirmedCount();
            int declined = db.GetDeclinedCount();
            int pending = db.GetPendingCount();
            return Json(new { Confirmed = confirmed, Declined = declined, Pending = pending });
        }
    }
}
