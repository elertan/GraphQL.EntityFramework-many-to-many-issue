using System.Linq;
using GraphQL.EntityFramework;
using GraphQL.EntityFramework_many_to_many_issue.Models;

namespace GraphQL.EntityFramework_many_to_many_issue.Schemas.Graphs
{
    public class AlbumGraph : BaseGraphType<Album>
    {
        public AlbumGraph(DatabaseContext db, IEfGraphQLService efGraphQlService) : base(efGraphQlService)
        {
           
            Field(a => a.Name).Description("The name of the track.");

            // TODO: Attempted to use AddQueryField without success, since that introduces
            // a bunch of independent queries (VERY SLOW).
            // Need to find a way to remove the Select part to use a single query
            AddQueryConnectionField<TrackGraph, Track>(
                name: "tracks",
                resolve: ctx =>
                    db.TrackXAlbums
                        .Where(e => e.AlbumId == ctx.Source.Id)
                        .Join(
                            db.Tracks,
                            e => e.TrackId,
                            e => e.Id,
                            (_, track) => track
                        )
            );
        }
    }
}