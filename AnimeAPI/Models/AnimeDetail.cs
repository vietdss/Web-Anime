namespace AnimeAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AnimeDetail")]
    public partial class AnimeDetail
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Title { get; set; }

        [StringLength(250)]
        public string Image { get; set; }

        [StringLength(255)]
        public string JapaneseTitle { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(255)]
        public string Studios { get; set; }

        [StringLength(50)]
        public string DateAired { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(255)]
        public string Genre { get; set; }

        public decimal? Scores { get; set; }

        public decimal? Rating { get; set; }

        [StringLength(50)]
        public string Duration { get; set; }

        [StringLength(50)]
        public string Quality { get; set; }

        public int? Views { get; set; }

        public int? Votes { get; set; }

        public int? Ep { get; set; }
    }
}
