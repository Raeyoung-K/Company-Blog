using Blog.Models.Domain;

namespace Blog.Repositories
{
    public interface IPostLikeRepository
    {
        Task<int> GetTotalLikes(Guid postId);

        Task<PostLike> AddLikeForPost(PostLike postLike);

        Task<IEnumerable<PostLike>> GetLikesForUser(Guid postId);
    }
}
