using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.EntityFramework;
using GraphQL.EntityFramework_many_to_many_issue.Models;
using GraphQL.EntityFramework_many_to_many_issue.Models.ManyToMany;
using GraphQL.EntityFramework_many_to_many_issue.Schemas;
using GraphQL.Http;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GraphQL.EntityFramework_many_to_many_issue
{
    public class Startup
    {
        static DatabaseContext Db { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new DbContextOptionsBuilder<DatabaseContext>();
            builder.UseInMemoryDatabase("db");

            Db = new DatabaseContext(builder.Options);

            services.AddSingleton(Db);

            GetGraphQLTypes().ToList().ForEach(type => services.AddSingleton(type));


            // GraphQL.EF (Filtering)
            EfGraphQLConventions.RegisterConnectionTypesInContainer(services);
            EfGraphQLConventions.RegisterInContainer(services, Db);

            //  TEMPORARY DATA FILL:
            var tracks = new[]
            {
                new Track
                {
                    Id = 1,
                    Name = "Track 1",
                    DurationMs = 150000
                },
                new Track
                {
                    Id = 2,
                    Name = "Track 2",
                    DurationMs = 120000
                },
                new Track
                {
                    Id = 3,
                    Name = "Track 3",
                    DurationMs = 160000
                },
                new Track
                {
                    Id = 4,
                    Name = "Track 4",
                    DurationMs = 140000
                },
                new Track
                {
                    Id = 5,
                    Name = "Track 5",
                    DurationMs = 100000
                },
            };

            var albums = new[]
            {
                new Album
                {
                    Id = 1,
                    Name = "My Album 1"
                },
                new Album
                {
                    Id = 2,
                    Name = "My Album 2"
                },
            };

            var rng = new Random();

//            var trackXAlbums = Enumerable.Range(0, tracks.Length * 3).Select(i =>
//                new TrackXAlbum
//                {
//                    Id = i + 1,
//                    AlbumId = rng.Next(albums.Length - 1),
//                    TrackId = rng.Next(tracks.Length - 1)
//                }
//            );
            var trackXAlbums = new[]
            {
                new TrackXAlbum
                {
                    Id = 1,
                    AlbumId = 1,
                    TrackId = 1
                },
                new TrackXAlbum
                {
                    Id = 2,
                    AlbumId = 1,
                    TrackId = 2
                },
                new TrackXAlbum
                {
                    Id = 3,
                    AlbumId = 1,
                    TrackId = 3
                },
                new TrackXAlbum
                {
                    Id = 4,
                    AlbumId = 2,
                    TrackId = 4
                },
            };

            Db.Tracks.AddRange(tracks);
            Db.Albums.AddRange(albums);
            Db.SaveChanges();

            Db.TrackXAlbums.AddRange(trackXAlbums);
            Db.SaveChanges();

            // Enable CORS options
            services.AddCors();

            // Create a dependency resolver for GraphQL
            services.AddSingleton<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));

            // Query parser stuff
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<IDocumentWriter, DocumentWriter>();

            services.AddSingleton<ISchema, RootSchema>();

            // Enable access to HttpContext
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add GraphQL service
            services.AddGraphQL(_ =>
                {
                    _.EnableMetrics = true;
                    _.ExposeExceptions = true;
                })
                // Extract user information from the request
                .AddUserContextBuilder(httpContext => new GraphQLUserContext {User = httpContext.User});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Set CORS to allow any connection
            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            // add http for Schema at default url /graphql
            app.UseGraphQL<ISchema>("/graphql");

            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions
            {
                Path = "/ui/playground"
            });
        }

        // ReSharper disable once InconsistentNaming
        static IEnumerable<Type> GetGraphQLTypes()
        {
            return typeof(Startup).Assembly
                .GetTypes()
                .Where(x => !x.IsAbstract &&
                            (typeof(IObjectGraphType).IsAssignableFrom(x) ||
                             typeof(IInputObjectGraphType).IsAssignableFrom(x)));
        }
    }
}