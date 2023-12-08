using Azure.Identity;
using Blog.Models.Domain;
using Blog.Models.ViewModels;
using Blog.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IPostLikeRepository postLikeRepository;
		private readonly IPostCommentRepository postCommentRepository;
		private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public BlogController(IBlogPostRepository blogPostRepository, IPostLikeRepository postLikeRepository, IPostCommentRepository postCommentRepository, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            this.blogPostRepository = blogPostRepository;
            this.postLikeRepository = postLikeRepository;
			this.postCommentRepository = postCommentRepository;
			this.signInManager = signInManager;
            this.userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var liked = false;
            var post = await blogPostRepository.GetByUrlHandleAsync
                (urlHandle);
            var postDetailViewModel = new PostDetailViewModel();
            
            if (post != null)
            {
                var totalLikes = await postLikeRepository.GetTotalLikes(post.Id);

                if (signInManager.IsSignedIn(User))
                {
                    // Get like for post for this user 
                    var likesForPost = await postLikeRepository.GetLikesForUser(post.Id);

                    var userId = userManager.GetUserId(User);

                    if (userId != null)
                    {
                        var likeFromUser = likesForPost.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                        liked = likeFromUser != null;
                    }
                }

                // fetch comments for the post 

                var postCommentsDomainModel = await postCommentRepository.GetCommentsByPostIdAsync(post.Id);

                var postCommentsForView = new List<PostCommentViewModel>();

                foreach (var postComment in postCommentsDomainModel)
                {
                    postCommentsForView.Add(new PostCommentViewModel
                    {
                        Description = postComment.Description,
                        DateAdded = postComment.DateAdded,
                        Username = (await userManager.FindByIdAsync(postComment.UserId.ToString())).UserName
                    });
                }

                postDetailViewModel = new PostDetailViewModel
                {
                    Id = post.Id,
                    Author = post.Author,
                    Content = post.Content,
                    FeaturedImageUrl = post.FeaturedImageUrl,
                    Heading = post.Heading,
                    PageTitle = post.PageTitle,
                    PublisehdDate = post.PublisehdDate,
                    ShortDescription = post.ShortDescription,
                    Tags = post.Tags,
                    UrlHandle = post.UrlHandle,
                    Visible = post.Visible,
                    TotalLikes = totalLikes,
                    Liked = liked,
                    Comments = postCommentsForView
                };

            }
            
            return View(postDetailViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(PostDetailViewModel postDetailViewModel)
        {

            if (signInManager.IsSignedIn(User))
            {
                var domainModel = new PostComment
                {
                    PostId = postDetailViewModel.Id,
                    Description = postDetailViewModel.CommentDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                    DateAdded = DateTime.Now
                };

                await postCommentRepository.AddAsync(domainModel);

                return RedirectToAction("Index", "Blog", new { urlHandle = postDetailViewModel.UrlHandle });
			}

            return Forbid();    // 403
        }
    }
}
