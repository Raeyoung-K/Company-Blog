using Microsoft.AspNetCore.Mvc;

namespace Blog.Repositories
{
    public interface IImageRepository
    {
        Task<string> UploadAsync(IFormFile file);

    }
}
