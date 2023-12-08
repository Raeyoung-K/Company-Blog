using Blog.Data;
using Blog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repositories
{
    public class PostLikeRepository : IPostLikeRepository
    {
        private readonly BlogDbContext blogDbContext;

        public PostLikeRepository(BlogDbContext blogDbContext)
        {
            this.blogDbContext = blogDbContext;
        }

		public async Task<PostLike> AddLikeForPost(PostLike postLike)
        {
            await blogDbContext.PostLike.AddAsync(postLike);
            await blogDbContext.SaveChangesAsync();
            return postLike;
        }

        public async Task<IEnumerable<PostLike>> GetLikesForUser(Guid postId)
        {
            return await blogDbContext.PostLike.Where(x => x.PostId == postId).ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid postId)
        {
            return await blogDbContext.PostLike.CountAsync(x => x.PostId == postId);
        }
    }
}
