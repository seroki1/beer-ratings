using BeerRatings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
    public class Common: ICommon
    {
        private readonly BeerContext _context;
        public Common(BeerContext context)
        {
            _context = context;
        }

        public Beer GetBeerByID(int beerId)
        {
            var beer = _context.Beer.FirstOrDefault(x => x.Id == beerId);
            return beer;
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
    }
}
