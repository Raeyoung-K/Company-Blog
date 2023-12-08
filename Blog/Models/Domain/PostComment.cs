namespace Blog.Models.Domain
{
	public class PostComment
	{
		public Guid Id { get; set; }
		public string Description { get; set; }
		public Guid	PostId { get; set; }
		public Guid UserId { get; set; }
		public DateTime DateAdded { get; set; }
	}
}
