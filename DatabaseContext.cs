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
    }
}