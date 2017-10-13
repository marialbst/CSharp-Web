namespace SocialNetwork.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;

    public class SocialNetworkDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Friendship> Friendships { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Picture> Pictures { get; set; }

        public DbSet<AlbumPicture> AlbumPictures { get; set; }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Server=.;Database=SocialNetworkDb;Integrated Security=True");

            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Friendship>()
                .HasKey(fs => new { fs.FromUserId, fs.ToUserId });

            builder.Entity<AlbumPicture>()
                .HasKey(ap => new { ap.AlbumId, ap.PictureId });

            builder.Entity<User>()
                .HasMany(u => u.FromFriends)
                .WithOne(f => f.FromUser)
                .HasForeignKey(f => f.FromUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>()
                .HasMany(u => u.ToFriends)
                .WithOne(f => f.ToUser)
                .HasForeignKey(f => f.ToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Album>()
                .HasOne(a => a.Owner)
                .WithMany(o => o.Albums)
                .HasForeignKey(a => a.OwnerId);

            builder.Entity<Album>()
                .HasMany(a => a.Pictures)
                .WithOne(ap => ap.Album)
                .HasForeignKey(ap => ap.AlbumId);

            builder.Entity<Picture>()
                .HasMany(p => p.Albums)
                .WithOne(ap => ap.Picture)
                .HasForeignKey(ap => ap.PictureId);

            builder.Entity<AlbumTag>()
                .HasKey(at => new { at.AlbumId, at.TagId });

            builder.Entity<Tag>()
                .HasMany(t => t.Albums)
                .WithOne(a => a.Tag)
                .HasForeignKey(a => a.TagId);

            builder.Entity<Album>()
                .HasMany(a => a.Tags)
                .WithOne(t => t.Album)
                .HasForeignKey(t => t.AlbumId);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var serviceProvider = this.GetService<IServiceProvider>();
            var items = new Dictionary<object, object>();

            foreach (var entry in this.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                var entity = entry.Entity;
                var context = new ValidationContext(entity,serviceProvider,items);
                var results = new List<ValidationResult>();

                if (!Validator.TryValidateObject(entity,context,results,true))
                {
                    foreach (var result in results)
                    {
                        if (result != ValidationResult.Success)
                        {
                            throw new ValidationException(result.ErrorMessage);
                        }
                    }
                }
            }

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
    }
}
