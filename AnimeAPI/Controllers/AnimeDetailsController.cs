using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using AnimeAPI.Models;

namespace AnimeAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AnimeDetailsController : ApiController
    {
        private Anime_DbContext db = new Anime_DbContext();

        // GET: api/AnimeDetails
        public IQueryable GetAnimeDetails()
        {
            var animeDetails = db.AnimeDetails.ToList();
            var animeCategories = db.AnimeCategories.ToList();
            var categories = db.Categories.ToList();
            var viewCounts = db.ViewCounts.ToList(); 

            var result = from ad in animeDetails
                         select new
                         {
                             ad.Id,
                             ad.Title,
                             ad.Image,
                             ad.JapaneseTitle,
                             ad.Description,
                             ad.Type,
                             ad.Studios,
                             ad.DateAired,
                             ad.Status,
                             ad.Genre,
                             ad.Scores,
                             ad.Rating,
                             ad.Duration,
                             ad.Quality,
                             ad.Votes,
                             ad.Ep,
                             Categories = (from ac in animeCategories
                                           join c in categories on ac.CategoryId equals c.CategoryId
                                           where ac.AnimeId == ad.Id
                                           select c.CategoryName).ToList(),
                             ViewCount = (from vc in viewCounts
                                          where vc.AnimeId == ad.Id
                                          select vc.ViewCount1).FirstOrDefault() // Retrieve the view count for the anime
                         };

            return result.AsQueryable();
        }

        // GET: api/AnimeDetails/5
        [ResponseType(typeof(AnimeDetail))]
        public IHttpActionResult GetAnimeDetail(int id)
        {
            AnimeDetail animeDetail = db.AnimeDetails.Find(id);
            if (animeDetail == null)
            {
                return NotFound();
            }

            return Ok(animeDetail);
        }

        // PUT: api/AnimeDetails/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAnimeDetail(int id, AnimeDetail animeDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != animeDetail.Id)
            {
                return BadRequest();
            }

            db.Entry(animeDetail).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimeDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/AnimeDetails
        [ResponseType(typeof(AnimeDetail))]
        public IHttpActionResult PostAnimeDetail(AnimeDetail animeDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AnimeDetails.Add(animeDetail);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = animeDetail.Id }, animeDetail);
        }

        // DELETE: api/AnimeDetails/5
        [ResponseType(typeof(AnimeDetail))]
        public IHttpActionResult DeleteAnimeDetail(int id)
        {
            AnimeDetail animeDetail = db.AnimeDetails.Find(id);
            if (animeDetail == null)
            {
                return NotFound();
            }

            db.AnimeDetails.Remove(animeDetail);
            db.SaveChanges();

            return Ok(animeDetail);
        }

        // GET: api/AnimeDetails/Category/{categoryName}
        [HttpGet]
        [Route("api/AnimeDetails/Category/{categoryName}")]
        [ResponseType(typeof(IEnumerable<AnimeDetail>))]
        public IHttpActionResult GetAnimeDetailsByCategory(string categoryName)
        {
            var category = db.Categories.FirstOrDefault(c => c.CategoryName == categoryName);

            if (category == null)
            {
                return NotFound();
            }

            var animeIds = db.AnimeCategories
                .Where(ac => ac.CategoryId == category.CategoryId)
                .Select(ac => ac.AnimeId)
                .ToList();

            var animeDetails = db.AnimeDetails
                .Where(ad => animeIds.Contains(ad.Id))
                .ToList();

            return Ok(animeDetails);
        }

        // GET: api/AnimeDetails/SortedByViews/All
        [HttpGet]
        [Route("api/AnimeDetails/SortedByViews/All")]
        [ResponseType(typeof(IEnumerable<AnimeDetail>))]
        public IHttpActionResult GetAnimeDetailsSortedByViewsAll()
        {
            var viewCounts = db.ViewCounts
                .GroupBy(vc => vc.AnimeId)
                .Select(g => new { AnimeId = g.Key, TotalViews = g.Sum(vc => vc.ViewCount1) })
                .OrderByDescending(x => x.TotalViews)
                .ToList();

            var animeDetails = viewCounts
                .Join(db.AnimeDetails, vc => vc.AnimeId, ad => ad.Id, (vc, ad) => ad)
                .ToList();

            var result = from ad in animeDetails
                         join vc in viewCounts on ad.Id equals vc.AnimeId
                         select new
                         {
                             ad.Id,
                             ad.Title,
                             ad.Image,
                             ad.JapaneseTitle,
                             ad.Description,
                             ad.Type,
                             ad.Studios,
                             ad.DateAired,
                             ad.Status,
                             ad.Genre,
                             ad.Scores,
                             ad.Rating,
                             ad.Duration,
                             ad.Quality,
                             ad.Votes,
                             ad.Ep,
                             ViewCount = vc.TotalViews // Include the TotalViews from viewCounts
                         };

            return Ok(result);
        }

        // GET: api/AnimeDetails/SortedByViews/Day/{date}
        [HttpGet]
        [Route("api/AnimeDetails/SortedByViews/Day/{date}")]
        [ResponseType(typeof(IEnumerable<AnimeDetail>))]
        public IHttpActionResult GetAnimeDetailsSortedByViewsDay(DateTime date)
        {
            var viewCounts = db.ViewCounts
                .Where(vc => DbFunctions.TruncateTime(vc.ViewDate) == DbFunctions.TruncateTime(date))
                .GroupBy(vc => vc.AnimeId)
                .Select(g => new { AnimeId = g.Key, TotalViews = g.Sum(vc => vc.ViewCount1) })
                .OrderByDescending(x => x.TotalViews)
                .ToList();

            var animeDetails = viewCounts
                .Join(db.AnimeDetails, vc => vc.AnimeId, ad => ad.Id, (vc, ad) => ad)
                .ToList();

            var result = from ad in animeDetails
                         join vc in viewCounts on ad.Id equals vc.AnimeId
                         select new
                         {
                             ad.Id,
                             ad.Title,
                             ad.Image,
                             ad.JapaneseTitle,
                             ad.Description,
                             ad.Type,
                             ad.Studios,
                             ad.DateAired,
                             ad.Status,
                             ad.Genre,
                             ad.Scores,
                             ad.Rating,
                             ad.Duration,
                             ad.Quality,
                             ad.Votes,
                             ad.Ep,
                             ViewCount = vc.TotalViews // Include the TotalViews from viewCounts
                         };

            return Ok(result);
        }

        // GET: api/AnimeDetails/SortedByViews/Month/{year}/{month}
        [HttpGet]
        [Route("api/AnimeDetails/SortedByViews/Month/{year}/{month}")]
        [ResponseType(typeof(IEnumerable<AnimeDetail>))]
        public IHttpActionResult GetAnimeDetailsSortedByViewsMonth(int year, int month)
        {
            var viewCounts = db.ViewCounts
                .Where(vc => vc.ViewDate.HasValue && vc.ViewDate.Value.Year == year && vc.ViewDate.Value.Month == month)
                .GroupBy(vc => vc.AnimeId)
                .Select(g => new { AnimeId = g.Key, TotalViews = g.Sum(vc => vc.ViewCount1) })
                .OrderByDescending(x => x.TotalViews)
                .ToList();

            var animeDetails = viewCounts
                .Join(db.AnimeDetails, vc => vc.AnimeId, ad => ad.Id, (vc, ad) => ad)
                .ToList();

            var result = from ad in animeDetails
                         join vc in viewCounts on ad.Id equals vc.AnimeId
                         select new
                         {
                             ad.Id,
                             ad.Title,
                             ad.Image,
                             ad.JapaneseTitle,
                             ad.Description,
                             ad.Type,
                             ad.Studios,
                             ad.DateAired,
                             ad.Status,
                             ad.Genre,
                             ad.Scores,
                             ad.Rating,
                             ad.Duration,
                             ad.Quality,
                             ad.Votes,
                             ad.Ep,
                             ViewCount = vc.TotalViews // Include the TotalViews from viewCounts
                         };

            return Ok(result);
        }

        // GET: api/AnimeDetails/SortedByViews/Year/{year}
        [HttpGet]
        [Route("api/AnimeDetails/SortedByViews/Year/{year}")]
        [ResponseType(typeof(IEnumerable<object>))] // Changed to object to include view count
        public IHttpActionResult GetAnimeDetailsSortedByViewsYear(int year)
        {
            var viewCounts = db.ViewCounts
                .Where(vc => vc.ViewDate.HasValue && vc.ViewDate.Value.Year == year)
                .GroupBy(vc => vc.AnimeId)
                .Select(g => new
                {
                    AnimeId = g.Key,
                    TotalViews = g.Sum(vc => vc.ViewCount1)
                })
                .OrderByDescending(x => x.TotalViews)
                .ToList();

            var animeIds = viewCounts.Select(vc => vc.AnimeId).ToList();
            var animeDetails = db.AnimeDetails
                .Where(ad => animeIds.Contains(ad.Id))
                .ToList();

            var result = from ad in animeDetails
                         join vc in viewCounts on ad.Id equals vc.AnimeId
                         select new
                         {
                             ad.Id,
                             ad.Title,
                             ad.Image,
                             ad.JapaneseTitle,
                             ad.Description,
                             ad.Type,
                             ad.Studios,
                             ad.DateAired,
                             ad.Status,
                             ad.Genre,
                             ad.Scores,
                             ad.Rating,
                             ad.Duration,
                             ad.Quality,
                             ad.Votes,
                             ad.Ep,
                             ViewCount = vc.TotalViews // Include the TotalViews from viewCounts
                         };

            return Ok(result);
        }

        // GET: api/AnimeDetails/AnimeCategories/{animeId}
        [HttpGet]
        [Route("api/AnimeDetails/AnimeCategories/{animeId}")]
        [ResponseType(typeof(IEnumerable<Category>))]
        public IHttpActionResult GetCategoriesByAnimeId(int animeId)
        {
            var anime = db.AnimeDetails.Find(animeId);
            if (anime == null)
            {
                return NotFound();
            }

            var categoryIds = db.AnimeCategories
                .Where(ac => ac.AnimeId == animeId)
                .Select(ac => ac.CategoryId)
                .ToList();

            var categories = db.Categories
                .Where(c => categoryIds.Contains(c.CategoryId))
                .ToList();

            return Ok(categories);
        }
        // GET: api/AnimeDetails/Search/{title}
        [HttpGet]
        [Route("api/AnimeDetails/Search/{title}")]
        [ResponseType(typeof(IEnumerable<AnimeDetail>))]
        public IHttpActionResult GetAnimeDetailsByTitle(string title)
        {
            var animeDetails = db.AnimeDetails
                .Where(ad => ad.Title.Contains(title))
                .ToList();

            if (animeDetails.Count == 0)
            {
                return NotFound();
            }

            return Ok(animeDetails);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AnimeDetailExists(int id)
        {
            return db.AnimeDetails.Count(e => e.Id == id) > 0;
        }
    }
}
