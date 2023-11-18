using Blog.Models.Domain;
using System.Collections;

namespace Blog.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllTagsAsync();

        Task<Tag> GetAsync(Guid id);

        Task<Tag> AddAsync(Tag tag);

        Task<Tag?> UpdateAsync(Tag tag);

        Task<Tag?> DeleteAsync(Guid id);

    }
}
