using System.Collections.Generic;
using GraphQL.EntityFramework_many_to_many_issue.Models.ManyToMany;

namespace GraphQL.EntityFramework_many_to_many_issue.Models
{
    public class Track : BaseModel
    {
        public string Name { get; set; }
        public int DurationMs { get; set; }
        
        public ICollection<TrackXAlbum> Albums { get; set; }
    }
}