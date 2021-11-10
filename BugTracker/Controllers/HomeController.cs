using BugTracker.Data;
using BugTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;


        private readonly ApplicationDbContext _context;


        /*public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        */

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        //Pie Chart Data
        public IActionResult GetData()
        {
            int immediate = _context.Bugs.Where(x => x.Priority == "Immediate").Count();
            int urgent = _context.Bugs.Where(x => x.Priority == "Urgent").Count();
            int medium = _context.Bugs.Where(x => x.Priority == "Medium").Count();
            int low = _context.Bugs.Where(x => x.Priority == "Low").Count();
            int verylow = _context.Bugs.Where(x => x.Priority == "Very low").Count();
            int closed = _context.Bugs.Where(x => x.Priority == "Closed").Count();
            Ratio obj = new Ratio();
            obj.Immediate = immediate;
            obj.Urgent = urgent;
            obj.Medium = medium;
            obj.Low = low;
            obj.Verylow = verylow;
            obj.Closed = closed;

            return Json(obj);
        }

        public class Ratio
        {
            public int Immediate { get; set; }  
            public int Urgent { get; set; }
            public int Medium { get; set; }
            public int Low { get; set; }
            public int Verylow { get; set; }
            public int Closed { get; set; }
        }





    }
}
