using GraphQL.EntityFramework_many_to_many_issue.Models;
using GraphQL.EntityFramework_many_to_many_issue.Models.ManyToMany;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.EntityFramework_many_to_many_issue
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            
        }

        public DbSet<Track> Tracks { get; set; }
        public DbSet<Album> Albums { get; set; }
        
        // Many To Many
        public DbSet<TrackXAlbum> TrackXAlbums { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Create many-to-many relationship
            modelBuilder.Entity<TrackXAlbum>().HasKey(e => new { e.TrackId, e.AlbumId });

            modelBuilder.Entity<TrackXAlbum>()
                .HasOne(e => e.Track)
                .WithMany(e => e.Albums)
                .HasForeignKey(e => e.TrackId);

            modelBuilder.Entity<TrackXAlbum>()
                .HasOne(e => e.Album)
                .WithMany(e => e.Tracks)
                .HasForeignKey(e => e.AlbumId);
        }
    }
}