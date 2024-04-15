namespace DoAnCoSo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Bookmark
    {
        [Key]
        public int bookmark_id { get; set; }

        public int? user_id { get; set; }

        public int? chapter_id { get; set; }

        public virtual Chapter Chapter { get; set; }

        public virtual User User { get; set; }
    }
}
