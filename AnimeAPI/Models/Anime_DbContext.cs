using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace AnimeAPI.Models
{
    public partial class Anime_DbContext : DbContext
    {
        public Anime_DbContext()
            : base("name=Anime_DbContext")
        {
        }

        public virtual DbSet<AnimeCategory> AnimeCategories { get; set; }
        public virtual DbSet<AnimeDetail> AnimeDetails { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Video> Videos { get; set; }
        public virtual DbSet<ViewCount> ViewCounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnimeDetail>()
                .Property(e => e.Image)
                .IsUnicode(false);

            modelBuilder.Entity<AnimeDetail>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<AnimeDetail>()
                .Property(e => e.Scores)
                .HasPrecision(4, 2);

            modelBuilder.Entity<AnimeDetail>()
                .Property(e => e.Rating)
                .HasPrecision(3, 1);

            modelBuilder.Entity<Blog>()
                .Property(e => e.Content)
                .IsUnicode(false);

            modelBuilder.Entity<Review>()
                .Property(e => e.Review_text)
                .IsUnicode(false);
        }
    }
}
