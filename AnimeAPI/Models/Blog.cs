namespace AnimeAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Blog")]
    public partial class Blog
    {
        public int BlogId { get; set; }

        [StringLength(255)]
        public string Title { get; set; }

        [Column(TypeName = "text")]
        public string Content { get; set; }

        [StringLength(255)]
        public string Author { get; set; }

        [StringLength(255)]
        public string CoverImage { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedAt { get; set; }
    }
}
