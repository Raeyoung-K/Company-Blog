using Blog.Data;
using Blog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BlogDbContext blogDbContext;

        public TagRepository(BlogDbContext blogDbContext)
        {
            this.blogDbContext = blogDbContext;
        }

        public async Task<Tag> AddAsync(Tag tag)
        {
            await blogDbContext.Tags.AddAsync(tag);
            await blogDbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var tagToDelete = await blogDbContext.Tags.FindAsync(id);
            
            if (tagToDelete != null)
            {
                blogDbContext.Tags.Remove(tagToDelete);
                await blogDbContext.SaveChangesAsync();
                return tagToDelete;
            }

            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await blogDbContext.Tags.ToListAsync();

        }

        public Task<Tag?> GetAsync(Guid id)
        {
            return blogDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var tagInDb = await blogDbContext.Tags.FindAsync(tag.Id);

            if (tagInDb != null)
            {
                tagInDb.Name = tag.Name;
                tagInDb.DisplayName = tag.DisplayName;

                await blogDbContext.SaveChangesAsync();

                return tagInDb;
            }

            return null;
        }
    }
}
