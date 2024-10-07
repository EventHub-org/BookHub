using Microsoft.EntityFrameworkCore;
using BookHub.DAL.Entities;
using Microsoft.Extensions.Configuration;
using System;

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
        public virtual DbSet<AchievmentEntity> Achievments { get; set; }
        public DbSet<FriendshipEntity> FriendshipEntity { get; set; }

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
                entity.HasKey(fs => new { fs.FriendId, fs.UserId });

                entity.HasOne(fs => fs.User)
                    .WithMany(u => u.Inviters)
                    .HasForeignKey(fs => fs.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(fs => fs.Friend)
                    .WithMany(u => u.Invitees)
                    .HasForeignKey(fs => fs.FriendId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<AchievmentEntity>();
        }
    }
}
