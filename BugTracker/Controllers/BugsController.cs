using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTracker.Data;
using BugTracker.Models;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;

namespace BugTracker.Controllers
{
    public class BugsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BugsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bugs
        public async Task<IActionResult> Index(string SortOrder, string SelectedTickets, string SelectedDescription, string SelectedPriority, string SelectedDueDate)

        {
            ViewBag.Tickets = String.IsNullOrEmpty(SortOrder) ? "Tickets_desc" : "";
            ViewBag.Description = SortOrder == "Description" ? "Description_desc" : "Description";
            ViewBag.Priority = SortOrder == "Priority" ? "Priority_desc" : "Priority";
            ViewBag.DueDate = SortOrder == "DueDate" ? "DueDate_desc" : "DueDate";
            var rawData = (from s in _context.Bugs
                           select s).ToList();
            var emp = from s in rawData
                      select s;


            if(!String.IsNullOrEmpty(SelectedTickets))
            {
                emp = emp.Where(s => s.Tickets.Trim().Equals(SelectedTickets.Trim()));
            }
        
            if (!String.IsNullOrEmpty(SelectedPriority))
            {
                emp = emp.Where(s => s.Priority.Trim().Equals(SelectedPriority.Trim()));
            }

            if (!String.IsNullOrEmpty(SelectedDescription))
            {
                emp = emp.Where(s => s.Description.Trim().Equals(SelectedDescription.Trim()));
            }

            if (!String.IsNullOrEmpty(SelectedDueDate))
            {
                emp = emp.Where(s => s.DueDate.ToString().Trim().Equals(SelectedDueDate.Trim()));
            }

            var UniqueTickets = from s in emp
                                group s by s.Tickets into newGroup
                                where newGroup.Key != null
                                orderby newGroup.Key
                                select new { Tickets = newGroup.Key };
            ViewBag.UniqueTickets = UniqueTickets.Select(m => new SelectListItem { Value = m.Tickets, Text = m.Tickets }).ToList();

            var UniqueDescription = from s in emp
                                group s by s.Tickets into newGroup
                                where newGroup.Key != null
                                orderby newGroup.Key
                                select new { Description = newGroup.Key };
            ViewBag.UniqueDescription = UniqueDescription.Select(m => new SelectListItem { Value = m.Description, Text = m.Description }).ToList();

            var UniquePriority = from s in emp
                                group s by s.Priority into newGroup
                                where newGroup.Key != null
                                orderby newGroup.Key
                                select new { Priority = newGroup.Key };
            ViewBag.UniquePriority = UniquePriority.Select(m => new SelectListItem { Value = m.Priority, Text = m.Priority }).ToList();

            var UniqueDueDate = from s in emp
                                 group s by s.DueDate into newGroup
                                 orderby newGroup.Key
                                 select new { DueDate = newGroup.Key };
            ViewBag.UniqueDueDate = UniqueDueDate.Select(m => new SelectListItem { Value = m.DueDate.ToString(), Text = m.DueDate.ToString() }).ToList();
            
            ViewBag.SelectedTickets = SelectedTickets;
            ViewBag.SelectedPriority = SelectedPriority;
            ViewBag.SelectedTickets = SelectedDescription;
            ViewBag.SelectedDueDate = SelectedDueDate;


            switch (SortOrder)
            {
                case "Tickets_desc":
                    emp = emp.OrderByDescending(s => s.Tickets);
                    break;

                case "Tickets":
                    emp = emp.OrderBy(s => s.Tickets);
                    break;

                case "Description_desc":
                    emp = emp.OrderByDescending(s => s.Description);
                    break;

                case "Description":
                    emp = emp.OrderBy(s => s.Description);
                    break;

                case "Priority_desc":
                    emp = emp.OrderByDescending(s => s.Priority);
                    break;

                case "Priority":
                    emp = emp.OrderBy(s => s.Priority);
                    break;

                case "DueDate_desc":
                    emp = emp.OrderByDescending(s => s.DueDate);
                    break;

                case "DueDate":
                    emp = emp.OrderBy(s => s.DueDate);
                    break;

                default:
                    emp = emp.OrderByDescending(s => s.DueDate);
                    break;
            }

            return View(emp.ToList());
        }

        // GET: Bugs/ShowSearchForm
        public IActionResult ShowSearchForm()
        {
            return View();
        }

        // POST: Bugs/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            var search = from b in _context.Bugs
                       select b;

            if (!string.IsNullOrEmpty(SearchPhrase))
            {
                search = search.Where(s => s.Tickets.Contains(SearchPhrase)
                                         || s.Description.Contains(SearchPhrase)
                                         || s.Priority.Contains(SearchPhrase));
            }
              
           
                return View("Index", await search.ToListAsync());
            

        }


        // GET: Bugs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bugs = await _context.Bugs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bugs == null)
            {
                return NotFound();
            }

            return View(bugs);
        }

        // GET: Bugs/Create

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bugs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tickets,Description,Priority,DueDate,TimeCreated,ClosedDate,Resolved")] Bugs bugs)
        {


            if (ModelState.IsValid)
            {
                _context.Add(bugs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bugs);
        }

        // GET: Bugs/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bugs = await _context.Bugs.FindAsync(id);
            if (bugs == null)
            {
                return NotFound();
            }
            return View(bugs);
        }

        // POST: Bugs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tickets,Description,Priority,DueDate,TimeCreated,ClosedDate,Resolved")] Bugs bugs)
        {
            if (id != bugs.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bugs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BugsExists(bugs.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bugs);
        }

        // GET: Bugs/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bugs = await _context.Bugs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bugs == null)
            {
                return NotFound();
            }

            return View(bugs);
        }

        // POST: Bugs/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bugs = await _context.Bugs.FindAsync(id);
            _context.Bugs.Remove(bugs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BugsExists(int id)
        {
            return _context.Bugs.Any(e => e.Id == id);
        }
    }
}
