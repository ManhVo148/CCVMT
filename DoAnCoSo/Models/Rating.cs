namespace DoAnCoSo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Rating
    {
        [Key]
        public int rating_id { get; set; }

        public int? user_id { get; set; }

        public int? story_id { get; set; }

        [Column("rating")]
        public int? rating1 { get; set; }

        public virtual Story Story { get; set; }

        public virtual User User { get; set; }
    }
}
