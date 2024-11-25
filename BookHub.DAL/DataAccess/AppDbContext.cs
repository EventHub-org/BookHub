using Microsoft.EntityFrameworkCore;
using BookHub.DAL.Entities;

namespace BookHub.DAL.DataAccess
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<BookEntity> Books { get; set; }
        public virtual DbSet<ReviewEntity> Reviews { get; set; }
        public virtual DbSet<ReadingProgressEntity> ReadingProgresses { get; set; }
        public virtual DbSet<FriendshipEntity> Friendships { get; set; }
        public virtual DbSet<CollectionEntity> Collections { get; set; }
        public virtual DbSet<AchievementEntity> Achievements { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.HasMany(u => u.Achievments)
                .WithMany(a => a.Users)
                .UsingEntity(j =>
                {
                    j.ToTable("UsersAchievments");
                    j.Property<DateTime>("DateAchieved");
                });
            });

            modelBuilder.Entity<FriendshipEntity>(entity =>
            {
                entity.HasKey(fs => new { fs.User1Id, fs.User2Id });
                entity.HasOne(fs => fs.User1)
                    .WithMany(u => u.Inviters)
                    .HasForeignKey(fs => fs.User1Id)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(fs => fs.User2)
                    .WithMany(u => u.Invitees)
                    .HasForeignKey(fs => fs.User2Id)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BookEntity>()
           .HasMany(b => b.Collections)
           .WithMany(c => c.Books)
           .UsingEntity<Dictionary<string, object>>(
               "BookCollection",
               j => j.HasOne<CollectionEntity>().WithMany().HasForeignKey("CollectionId"),
               j => j.HasOne<BookEntity>().WithMany().HasForeignKey("BookId"));


            modelBuilder.Entity<AchievementEntity>();
        }
    }
}