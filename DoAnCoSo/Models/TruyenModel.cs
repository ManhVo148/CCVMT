using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DoAnCoSo.Models
{
    public partial class TruyenModel : DbContext
    {
        public TruyenModel()
            : base("name=TruyenModel4")
        {
        }

        public virtual DbSet<Bookmark> Bookmarks { get; set; }
        public virtual DbSet<Chapter> Chapters { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Story> Stories { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chapter>()
                .Property(e => e.content)
                .IsUnicode(false);

            modelBuilder.Entity<Genre>()
                .HasMany(e => e.Stories)
                .WithOptional(e => e.Genre)
                .HasForeignKey(e => e.genre_id);

            modelBuilder.Entity<Genre>()
                .HasMany(e => e.Stories1)
                .WithMany(e => e.Genres)
                .Map(m => m.ToTable("StoryGenres").MapLeftKey("genre_id").MapRightKey("story_id"));

            modelBuilder.Entity<Story>()
                .Property(e => e.cover_image_url)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.password)
                .IsUnicode(false);
        }
    }
}
