using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeerRatings.Models;
using BeerRatings.Repository;
using BeerRatings.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace BeerRatings
{
    public class BeersController : Controller
    {
        private readonly BeerContext _context;
        private readonly ICommon _common;

        public BeersController(BeerContext context, ICommon common)
        {
            _context = context;
            _common = common;
        }

        // GET: Beers
        public async Task<IActionResult> Index()
        {
            var beerContext = _context.Beer.Include(b => b.Brewery).Include(b => b.Style);
            return View(await beerContext.ToListAsync());
        }

        // GET: Beers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beer = _common.GetBeerDetailsViewModel(id);
            if (beer == null)
            {
                return NotFound();
            }

            return View(beer);
        }

        // GET: Beers/Create
        [Authorize]
        public IActionResult Create()
        {
            var model = new BeerViewModel
            {
                BreweryList = _common.GetBreweryChoices(),
                StyleList = _common.GetStyleChoices()
            };
            return View(model);
        }

        // POST: Beers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Name,BreweryId,Abv,StyleId,Notes")] BeerViewModel beer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(beer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            beer.BreweryList = _common.GetBreweryChoices();
            beer.StyleList = _common.GetStyleChoices();
            return View(beer);
        }

        // GET: Beers/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beer = _common.GetBeerViewModel(id);
            if (beer == null)
            {
                return NotFound();
            }

            return View(beer);
        }

        // POST: Beers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BreweryId,Abv,StyleId,Notes")] BeerViewModel beer)
        {
            if (id != beer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(beer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_common.BeerExists(beer.Id))
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
            beer.BreweryList = _common.GetBreweryChoices();
            beer.StyleList = _common.GetStyleChoices();
            return View(beer);
        }

        // GET: Beers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beer = await _context.Beer
                .Include(b => b.Brewery)
                .Include(b => b.Style)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (beer == null)
            {
                return NotFound();
            }

            return View(beer);
        }

        // POST: Beers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var beer = await _context.Beer.FindAsync(id);
            _context.Beer.Remove(beer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
