namespace GraphQL.EntityFramework_many_to_many_issue.Models.ManyToMany
{
    public class TrackXAlbum : BaseModel
    {
        public int TrackId { get; set; }
        public Track Track { get; set; }

        public int AlbumId { get; set; }
        public Album Album { get; set; }
    }
}