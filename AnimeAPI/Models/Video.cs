namespace AnimeAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Video
    {
        public int VideoId { get; set; }

        public int? Episode { get; set; }

        [StringLength(255)]
        public string VideoPath { get; set; }

        public DateTime? UploadedAt { get; set; }

        public int? AnimeId { get; set; }
    }
}
