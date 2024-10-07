using Microsoft.EntityFrameworkCore;
using BookHub.DAL.Entities;

namespace BookHub.DAL.DataAccess
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<CollectionEntity> Collections { get; set; }
        public virtual DbSet<AchievmentEntity> Achievments { get; set; }

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

            modelBuilder.Entity<AchievmentEntity>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Name)
                .IsRequired();

            });
        }


    }
}
