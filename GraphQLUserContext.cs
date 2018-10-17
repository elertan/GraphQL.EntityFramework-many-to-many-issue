using System.Security.Claims;

namespace GraphQL.EntityFramework_many_to_many_issue
{
    public class GraphQLUserContext {
        public ClaimsPrincipal User { get; set; }
    }
}