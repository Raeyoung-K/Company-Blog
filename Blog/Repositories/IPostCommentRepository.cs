using Blog.Models.Domain;

namespace Blog.Repositories
{
	public interface IPostCommentRepository
	{
		Task<PostComment> AddAsync(PostComment postComment);

		Task<IEnumerable<PostComment>> GetCommentsByPostIdAsync(Guid postId);
	}
}
