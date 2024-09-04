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

    public class AnimeCategoriesController : ApiController
    {
        private Anime_DbContext db = new Anime_DbContext();

        // GET: api/AnimeCategories
        public IQueryable<AnimeCategory> GetAnimeCategories()
        {
            return db.AnimeCategories;
        }

        // GET: api/AnimeCategories/5
        [ResponseType(typeof(AnimeCategory))]
        public IHttpActionResult GetAnimeCategory(int id)
        {
            AnimeCategory animeCategory = db.AnimeCategories.Find(id);
            if (animeCategory == null)
            {
                return NotFound();
            }

            return Ok(animeCategory);
        }

        // PUT: api/AnimeCategories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAnimeCategory(int id, AnimeCategory animeCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != animeCategory.AnimeId)
            {
                return BadRequest();
            }

            db.Entry(animeCategory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimeCategoryExists(id))
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

        // POST: api/AnimeCategories
        [ResponseType(typeof(AnimeCategory))]
        public IHttpActionResult PostAnimeCategory(AnimeCategory animeCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AnimeCategories.Add(animeCategory);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AnimeCategoryExists(animeCategory.AnimeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = animeCategory.AnimeId }, animeCategory);
        }

        // DELETE: api/AnimeCategories/5
        [ResponseType(typeof(AnimeCategory))]
        public IHttpActionResult DeleteAnimeCategory(int id)
        {
            AnimeCategory animeCategory = db.AnimeCategories.Find(id);
            if (animeCategory == null)
            {
                return NotFound();
            }

            db.AnimeCategories.Remove(animeCategory);
            db.SaveChanges();

            return Ok(animeCategory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AnimeCategoryExists(int id)
        {
            return db.AnimeCategories.Count(e => e.AnimeId == id) > 0;
        }
    }
}