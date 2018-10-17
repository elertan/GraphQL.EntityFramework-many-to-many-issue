using GraphQL.EntityFramework;
using GraphQL.EntityFramework_many_to_many_issue.Models;

namespace GraphQL.EntityFramework_many_to_many_issue.Schemas.Graphs
{
    public abstract class BaseGraphType<T> : EfObjectGraphType<T> where T : BaseModel
    {
        protected BaseGraphType(IEfGraphQLService efGraphQlService) : base(efGraphQlService)
        {
            Field(t => t.Id).Description("The id of the entity");
        }
    }
}