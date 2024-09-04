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
    public class ViewCountsController : ApiController
    {
        private Anime_DbContext db = new Anime_DbContext();

        // GET: api/ViewCounts
        public IQueryable<ViewCount> GetViewCounts()
        {
            return db.ViewCounts;
        }

        // GET: api/ViewCounts/5
        [ResponseType(typeof(ViewCount))]
        public IHttpActionResult GetViewCount(int id)
        {
            ViewCount viewCount = db.ViewCounts.Find(id);
            if (viewCount == null)
            {
                return NotFound();
            }

            return Ok(viewCount);
        }

        // GET: api/ViewCounts/Anime/5
        [ResponseType(typeof(IQueryable<ViewCount>))]
        [HttpGet]
        [Route("api/ViewCounts/Anime/{animeId}")]
        public IHttpActionResult GetViewCountsByAnimeId(int animeId)
        {
            var viewCounts = db.ViewCounts.Where(vc => vc.AnimeId == animeId);
            if (!viewCounts.Any())
            {
                return NotFound();
            }

            return Ok(viewCounts);
        }

        // GET: api/ViewCounts/Anime/5/Day
        [ResponseType(typeof(IQueryable<ViewCount>))]
        [HttpGet]
        [Route("api/ViewCounts/Anime/{animeId}/Day/{date}")]
        public IHttpActionResult GetViewCountsByDay(int animeId, DateTime date)
        {
            var viewCounts = db.ViewCounts.Where(vc => vc.AnimeId == animeId && DbFunctions.TruncateTime(vc.ViewDate) == DbFunctions.TruncateTime(date));
            if (!viewCounts.Any())
            {
                return NotFound();
            }

            return Ok(viewCounts);
        }

        // GET: api/ViewCounts/Anime/5/Week
        [ResponseType(typeof(IQueryable<ViewCount>))]
        [HttpGet]
        [Route("api/ViewCounts/Anime/{animeId}/Week/{year}/{week}")]
        public IHttpActionResult GetViewCountsByWeek(int animeId, int year, int week)
        {
            var firstDayOfYear = new DateTime(year, 1, 1);
            var firstWeekStart = firstDayOfYear.AddDays(-(int)firstDayOfYear.DayOfWeek);
            var weekStart = firstWeekStart.AddDays((week - 1) * 7);
            var weekEnd = weekStart.AddDays(7);

            var viewCounts = db.ViewCounts.Where(vc => vc.AnimeId == animeId && vc.ViewDate >= weekStart && vc.ViewDate < weekEnd);
            if (!viewCounts.Any())
            {
                return NotFound();
            }

            return Ok(viewCounts);
        }

        // GET: api/ViewCounts/Anime/5/Month
        [ResponseType(typeof(IQueryable<ViewCount>))]
        [HttpGet]
        [Route("api/ViewCounts/Anime/{animeId}/Month/{year}/{month}")]
        public IHttpActionResult GetViewCountsByMonth(int animeId, int year, int month)
        {
            var viewCounts = db.ViewCounts.Where(vc => vc.AnimeId == animeId && vc.ViewDate.HasValue && vc.ViewDate.Value.Year == year && vc.ViewDate.Value.Month == month);
            if (!viewCounts.Any())
            {
                return NotFound();
            }

            return Ok(viewCounts);
        }

        // GET: api/ViewCounts/Anime/5/Year
        [ResponseType(typeof(IQueryable<ViewCount>))]
        [HttpGet]
        [Route("api/ViewCounts/Anime/{animeId}/Year/{year}")]
        public IHttpActionResult GetViewCountsByYear(int animeId, int year)
        {
            var viewCounts = db.ViewCounts.Where(vc => vc.AnimeId == animeId && vc.ViewDate.HasValue && vc.ViewDate.Value.Year == year);
            if (!viewCounts.Any())
            {
                return NotFound();
            }

            return Ok(viewCounts);
        }

        // PUT: api/ViewCounts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutViewCount(int id, ViewCount viewCount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != viewCount.Id)
            {
                return BadRequest();
            }

            db.Entry(viewCount).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ViewCountExists(id))
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

        // POST: api/ViewCounts
        [ResponseType(typeof(ViewCount))]
        public IHttpActionResult PostViewCount(ViewCount viewCount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ViewCounts.Add(viewCount);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = viewCount.Id }, viewCount);
        }

        // DELETE: api/ViewCounts/5
        [ResponseType(typeof(ViewCount))]
        public IHttpActionResult DeleteViewCount(int id)
        {
            ViewCount viewCount = db.ViewCounts.Find(id);
            if (viewCount == null)
            {
                return NotFound();
            }

            db.ViewCounts.Remove(viewCount);
            db.SaveChanges();

            return Ok(viewCount);
        }
        [HttpPost]
        [Route("api/ViewCounts/IncrementViews/{animeId}")]
        public IHttpActionResult IncrementViewCount(int animeId)
        {
            var today = DateTime.Today;
            var viewCount = db.ViewCounts.FirstOrDefault(vc => vc.AnimeId == animeId && DbFunctions.TruncateTime(vc.ViewDate) == today);

            if (viewCount != null)
            {
                viewCount.ViewCount1 = (viewCount.ViewCount1 ?? 0) + 1;
            }
            else
            {
                viewCount = new ViewCount
                {
                    AnimeId = animeId,
                    ViewDate = today,
                    ViewCount1 = 1
                };
                db.ViewCounts.Add(viewCount);
            }

            db.SaveChanges();

            return Ok(viewCount);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ViewCountExists(int id)
        {
            return db.ViewCounts.Count(e => e.Id == id) > 0;
        }
    }
}
