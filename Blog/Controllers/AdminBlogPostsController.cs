using Blog.Models.Domain;
using Blog.Models.ViewModels;
using Blog.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {

            var tags = await tagRepository.GetAllTagsAsync();

            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.DisplayName, Value = x.Id.ToString() })
            };
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest) 
        {
            // Map view model to domain model
            var blogPostDomainModel = new BlogPost
            {
               Heading = addBlogPostRequest.Heading,
               PageTitle = addBlogPostRequest.PageTitle,
               Content = addBlogPostRequest.Content,
               ShortDescription = addBlogPostRequest.ShortDescription,
               FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
               UrlHandle = addBlogPostRequest.UrlHandle,
               Author  = addBlogPostRequest.Author,
               Visible = addBlogPostRequest.Visible,
               PublisehdDate = addBlogPostRequest.PublisehdDate
           
            };

            // Map tags from selected tags 

            var selectedTags = new List<Tag>();

            foreach (var tag in addBlogPostRequest.SelectedTags)
            {
                var selectedTagIdGuid = Guid.Parse(tag);
                var existingTag = await tagRepository.GetAsync(selectedTagIdGuid);

                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }

            blogPostDomainModel.Tags = selectedTags;

            await blogPostRepository.AddAsync(blogPostDomainModel);

            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {

            // Call Repository and get data

            var blogPosts = await blogPostRepository.GetAllAsync();

            return View(blogPosts);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            // Get the result from the repository 
            var post = await blogPostRepository.GetAync(id);
            var tagsDomainModel = await tagRepository.GetAllTagsAsync();

            if (post != null)
            {
                // map domain model to view model 
                var model = new EditBlogPostRequest
                {
                    Id = post.Id,
                    Heading = post.Heading,
                    PageTitle = post.PageTitle,
                    Content = post.Content,
                    Author = post.Author,
                    Visible = post.Visible,
                    FeaturedImageUrl = post.FeaturedImageUrl,
                    UrlHandle = post.UrlHandle,
                    PublisehdDate = post.PublisehdDate,
                    ShortDescription = post.ShortDescription,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = post.Tags.Select(x => x.Id.ToString()).ToArray()
                };

                // pass data to view 
                return View(model);
            }

          
            // Pass data to view
            return View(null);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            // map view model to domain model 

            var postDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                PageTitle = editBlogPostRequest.PageTitle,
                Heading = editBlogPostRequest.Heading,
                Content = editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                PublisehdDate = editBlogPostRequest.PublisehdDate,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                Visible = editBlogPostRequest.Visible
            };

            // map tags to domain model 

            var selectedTags = new List<Tag>();
            foreach (var selectedTag in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);

                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }

            postDomainModel.Tags = selectedTags;

            // submit data to repository to update
            
            var updatedPost = await blogPostRepository.UpdateAsync(postDomainModel);

            if (updatedPost != null)
            {
                return RedirectToAction("Edit");
            }

            // fail
            return RedirectToAction("Edit");
            // redirect to GET 
        }


        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            // Talk to reposiytory to delete the post 
            var deletedPost = await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);
            
            if (deletedPost != null)
            {
                // Successfully deleted
                return RedirectToAction("List");
            }

            // Fail to delete 
            return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });

        }
    }
}
