using BeerRatings.Models;
using BeerRatings.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BeerRatings.Repository
{

    public interface ICommon
    {
        Beer GetBeerByID(int beerId);
        Brewery GetBreweryByBeer(int beerId);
        IEnumerable<Beer> GetBeersByStyle(int styleId);
        IEnumerable<Beer> GetBeersByBrewery(int breweryId);
        IEnumerable<Ratings> GetRatingsByUser(int userId);
        IEnumerable<SelectListItem> GetBreweryChoices();
        IEnumerable<SelectListItem> GetStyleChoices();
        Task<Brewery> GetBreweryByIdAsync(int? breweryId);
        bool BreweryExists(int id);
        bool BeerExists(int id);
        BeerViewModel GetBeerViewModel(Beer beer);
        BeerViewModel GetBeerViewModel(int? id);
        BeerDetailsViewModel GetBeerDetailsViewModel(int? beerId);
        RatingViewModel GetRatingsViewModel(int beerId);
        Ratings GetRatingFromViewModel(RatingViewModel ratingViewModel);
    }
    public class Common: ICommon
    {
        private readonly BeerContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public Common(BeerContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Beer GetBeerByID(int beerId)
        {
            var beer = _context.Beer.FirstOrDefault(x => x.Id == beerId);
            return beer;
        }

        public BeerViewModel GetBeerViewModel(Beer beer)
        {
            var model = new BeerViewModel()
            {
                Id = beer.Id,
                BreweryId = beer.BreweryId,
                Name = beer.Name,
                StyleId = beer.StyleId,
                Notes = beer.Notes,
                Abv = beer.Abv,
                BreweryList = GetBreweryChoices(),
                StyleList = GetStyleChoices()
            };
            return model;
        }
        public BeerViewModel GetBeerViewModel(int? beerId)
        {
            var beer = _context.Beer.FirstOrDefault(x => x.Id == beerId);
            var model = new BeerViewModel()
            {
                Abv = beer.Abv,
                Id = beer.Id,
                BreweryId = beer.BreweryId,
                Name = beer.Name,
                StyleId = beer.StyleId,
                Notes = beer.Notes,
                BreweryList = GetBreweryChoices(),
                StyleList = GetStyleChoices()
            };
            return model;
        }
        public RatingViewModel GetRatingsViewModel(int beerId)
        {
            var beer = _context.Beer.FirstOrDefault(x => x.Id == beerId);
            var brewery = _context.Brewery.FirstOrDefault(x => x.Id == beer.BreweryId);
            var ratingViewModel = new RatingViewModel()
            {
                Beer= beer,
                BeerId = beer.Id,
                BeerName = beer.Name,
                BreweryId = brewery.Id,
                BreweryName = brewery.BreweryName
            };

            return ratingViewModel;
        }
        public Ratings GetRatingFromViewModel(RatingViewModel ratingViewModel)
        {
            var rating = new Ratings()
            {
                UserId = ratingViewModel.UserId,
                BeerId = ratingViewModel.BeerId,
                Comment = ratingViewModel.Comment,
                Rating = ratingViewModel.Rating
            };

            return rating;
        }
        public BeerDetailsViewModel GetBeerDetailsViewModel(int? beerId)
        {
            if (beerId == null)
                return null;
            var beer = _context.Beer.Include(x=> x.Brewery).Include(x=>x.Style).FirstOrDefault(x => x.Id == beerId);
            if (beer == null)
                return null;
            double averageRating;
            var ratings =  _context.Ratings.Where(x=>x.BeerId == beerId).Select(x=>x.Rating);
            if (ratings == null || ratings.Count() == 0)
                averageRating = 0.0;
            else averageRating = ratings.Average();

            var viewModel = new BeerDetailsViewModel()
            {
                Abv = beer.Abv,
                Id = beer.Id,
                BreweryId = beer.BreweryId,
                Name = beer.Name,
                StyleId = beer.StyleId,
                Notes = beer.Notes,
                AverageRating = averageRating,
                Style = beer.Style,
                Brewery = beer.Brewery
            };

            return  viewModel;
        }

        public async Task<Brewery> GetBreweryByIdAsync(int? breweryId)
        {
            if (breweryId == null)
            {
                return null;
            }

            return await _context.Brewery.FirstOrDefaultAsync(x => x.Id == breweryId);
        }

        public Brewery GetBreweryByBeer(int beerId)
        {
            var brewery = _context.Brewery
                .Where(x => x.Id == _context.Beer.Where(y => y.Id == beerId).Select(s => s.BreweryId).FirstOrDefault())
                .Select(x => x).FirstOrDefault();
            return brewery;
        }

        public IEnumerable<Beer> GetBeersByStyle(int styleId)
        {
            var beers = _context.Beer.Where(x => x.StyleId == styleId).Select(x => x);
            return beers;
        }

        public IEnumerable<Beer> GetBeersByBrewery(int breweryId)
        {
            var beers = _context.Beer.Where(x => x.BreweryId == breweryId).Select(x => x);
            return beers;
        }

        public IEnumerable<Ratings> GetRatingsByUser(int userId)
        {
            var ratings = _context.Ratings.Where(x => x.UserId == userId).Select(x => x);
            return ratings;
        }

        public IEnumerable<SelectListItem> GetBreweryChoices()
        {
            return _context.Brewery.Select(x => new SelectListItem(x.BreweryName, x.Id.ToString()));
        }

        public IEnumerable<SelectListItem> GetStyleChoices()
        {
            return _context.BeerStyle.Select(x => new SelectListItem(x.Style, x.Id.ToString()));
        }


        public bool BreweryExists(int id)
        {
            return _context.Brewery.Any(e => e.Id == id);
        }

        public bool BeerExists(int id)
        {
            return _context.Beer.Any(e => e.Id == id);
        }
    }
}
