using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeerRatings.Models;
using BeerRatings.Repository;
using Microsoft.AspNetCore.Authorization;

namespace BeerRatings.Views
{
    public class BreweriesController : Controller
    {
        private readonly BeerContext _context;
        private readonly ICommon _common;

        public BreweriesController(BeerContext context, ICommon common)
        {
            _context = context;
            _common = common;
        }

        // GET: Breweries
        public async Task<IActionResult> Index()
        {
            return View(await _context.Brewery.ToListAsync());
        }

        // GET: Breweries/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            var brewery = await _common.GetBreweryByIdAsync(id);
            if (brewery == null)
            {
                return NotFound();
            }

            return View(brewery);
        }

        // GET: Breweries/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Breweries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("BreweryName,Location")] Brewery brewery)
        {
            if (!_common.BreweryExists(brewery.Id))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(brewery);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(brewery);
        }

        // GET: Breweries/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            
            var brewery = await _context.Brewery.FindAsync(id);
            
            if (brewery == null)
            {
                return NotFound();
            }

            return View(brewery);
        }


    }
}
