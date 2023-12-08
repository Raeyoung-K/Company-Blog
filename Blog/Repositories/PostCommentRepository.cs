using Blog.Data;
using Blog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repositories
{
	public class PostCommentRepository : IPostCommentRepository
	{
		private readonly BlogDbContext blogDbContext;

		public PostCommentRepository(BlogDbContext blogDbContext)
        {
			this.blogDbContext = blogDbContext;
		}

        public async Task<PostComment> AddAsync(PostComment postComment)
		{
			await blogDbContext.PostComment.AddAsync(postComment);
			await blogDbContext.SaveChangesAsync();
			return postComment;
		}

		public async Task<IEnumerable<PostComment>> GetCommentsByPostIdAsync(Guid postId)
		{
			return await blogDbContext.PostComment.Where(x => x.PostId == postId).ToListAsync();
		}
	}
}
