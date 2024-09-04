namespace AnimeAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Review
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        [StringLength(50)]
        public string Time_posted { get; set; }

        [Column(TypeName = "text")]
        public string Review_text { get; set; }

        public int? AnimeId { get; set; }
    }
}
