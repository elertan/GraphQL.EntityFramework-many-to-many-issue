using GraphQL.Types;

namespace GraphQL.EntityFramework_many_to_many_issue.Schemas
{
    public class RootSchema : Schema
    {
        public RootSchema(IDependencyResolver resolver)
            : base(resolver)
        {
            Query = resolver.Resolve<RootQuery>();
//            Mutation = resolver.Resolve<StarWarsMutation>();
        }
    }
}