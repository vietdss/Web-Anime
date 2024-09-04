namespace AnimeAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ViewCount
    {
        public int Id { get; set; }

        public int? AnimeId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ViewDate { get; set; }

        [Column("ViewCount")]
        public int? ViewCount1 { get; set; }
    }
}
