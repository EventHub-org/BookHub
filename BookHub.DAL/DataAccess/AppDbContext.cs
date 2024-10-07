using Microsoft.EntityFrameworkCore;
using BookHub.DAL.Entities;

namespace BookHub.DAL.DataAccess
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<CollectionEntity> Collections { get; set; }
        public virtual DbSet<AchievementEntity> Achievements { get; set; }
        public virtual DbSet<ReviewEntity> Reviews { get; set; }
        public virtual DbSet<BookEntity> Books { get; set; }
        public virtual DbSet<ReadingProgressEntity> ReadingProgresses { get; set; }
        public virtual DbSet<FriendshipEntity> Friendships { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.HasKey(c => c.UserId);
                entity.Property(c => c.Name)
                .IsRequired();

                entity.HasMany(u => u.Achievments)
                .WithMany(a => a.Users)
                .UsingEntity(j =>
                {
                    j.ToTable("UsersAchievments");
                    j.Property<DateTime>("DateAchieved");
                });
            });

            modelBuilder.Entity<CollectionEntity>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name)
                .IsRequired();


                entity.HasOne(c => c.User)
                .WithMany( u => u.Collections)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AchievementEntity>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Name)
                .IsRequired();

            });
        }


    }
}
