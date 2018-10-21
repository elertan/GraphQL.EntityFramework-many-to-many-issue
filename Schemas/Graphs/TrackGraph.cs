using System.Linq;
using GraphQL.EntityFramework;
using GraphQL.EntityFramework_many_to_many_issue.Models;

namespace GraphQL.EntityFramework_many_to_many_issue.Schemas.Graphs
{
    public class TrackGraph : BaseGraphType<Track>
    {
        public TrackGraph(DatabaseContext db, IEfGraphQLService efGraphQlService) : base(efGraphQlService)
        {
            Name = "Track";

            Field(t => t.Name).Description("The name of the track.");
            Field(t => t.DurationMs).Description("The duration of the track in milliseconds");

            AddNavigationField<AlbumGraph, Album>(
                "albums",
                resolve: ctx => ctx.Source.Albums.Select(e => e.Album)
            );
        }
    }
}