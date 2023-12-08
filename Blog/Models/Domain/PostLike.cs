namespace Blog.Models.Domain
{
    public class PostLike
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
    }
}
