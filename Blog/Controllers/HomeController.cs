using Blog.Models;
using Blog.Models.ViewModels;
using Blog.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Blog.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository blogPostRepository;
		private readonly ITagRepository tagRepository;

		public HomeController(ILogger<HomeController> logger, IBlogPostRepository blogPostRepository, ITagRepository tagRepository)
		{
			_logger = logger;
            this.blogPostRepository = blogPostRepository;
			this.tagRepository = tagRepository;
		}

		public async Task<IActionResult> Index()
		{
			// get all blogs
			var posts = await blogPostRepository.GetAllAsync();

			// get all tags 
			var tags = await tagRepository.GetAllTagsAsync();

			var model = new HomeViewModel
			{
				BlogPosts = posts,
				Tags = tags
			};

			return View(model);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}