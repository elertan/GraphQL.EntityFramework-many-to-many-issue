using GraphQL.EntityFramework;
using GraphQL.EntityFramework_many_to_many_issue.Models;
using GraphQL.EntityFramework_many_to_many_issue.Schemas.Graphs;

namespace GraphQL.EntityFramework_many_to_many_issue.Schemas
{
    public class RootQuery : EfObjectGraphType<object>
    {
        public RootQuery(DatabaseContext db, IEfGraphQLService efGraphQlService) : base(efGraphQlService)
        {
            Name = "Query";
            
            AddQueryField<TrackGraph, Track>(
                name: "tracks",
                resolve: ctx => db.Tracks);
            AddQueryField<AlbumGraph, Album>(
                name: "albums",
                resolve: ctx => db.Albums);
        }
    }

}