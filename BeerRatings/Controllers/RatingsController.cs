using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeerRatings.Models;
using Microsoft.AspNetCore.Identity;
using BeerRatings.Repository;
using BeerRatings.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace BeerRatings.Views
{
    [Authorize]
    public class RatingsController : Controller
    {
        private readonly BeerContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICommon _common;

        public RatingsController(BeerContext context, UserManager<ApplicationUser> user, ICommon common)
        {
            _context = context;
            _common = common;
            _userManager = user;
        }

        // GET: Ratings
        public async Task<IActionResult> Index()
        {
            var beerContext = _context.Ratings.Include(r => r.Beer).Include(r => r.User);
            return View(await beerContext.ToListAsync());
        }

        // GET: Ratings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ratings = await _context.Ratings
                .Include(r => r.Beer)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ratings == null)
            {
                return NotFound();
            }

            return View(ratings);
        }

        // GET: Ratings/Create
        [HttpGet("Ratings/Create/{beerId}")]
        public IActionResult Create(int beerId)
        {
            var model = _common.GetRatingsViewModel(beerId);

            return View(model);
        }

        // POST: Ratings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BeerId,Comment,Rating,UserId")] RatingViewModel ratings)
        {
            var _user = await _userManager.GetUserAsync(HttpContext.User);
            if (ModelState.IsValid)
            {
                ratings.UserId = _context.User.Where(x => x.Email == _user.Email).Select(x => x.Id).FirstOrDefault();
                var rating = _common.GetRatingFromViewModel(ratings);
                _context.Add(rating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(ratings);
        }

        // GET: Ratings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ratings = await _context.Ratings.FindAsync(id);
            if (ratings == null)
            {
                return NotFound();
            }

            return View(ratings);
        }

        // POST: Ratings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BeerId,Comment,Rating,UserId")] Ratings ratings)
        {
            if (id != ratings.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ratings);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RatingsExists(ratings.Id))
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

            return View(ratings);
        }

        // GET: Ratings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ratings = await _context.Ratings
                .Include(r => r.Beer)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ratings == null)
            {
                return NotFound();
            }

            return View(ratings);
        }

        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ratings = await _context.Ratings.FindAsync(id);
            _context.Ratings.Remove(ratings);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RatingsExists(int id)
        {
            return _context.Ratings.Any(e => e.Id == id);
        }
    }
}
