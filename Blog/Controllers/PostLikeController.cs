using Blog.Models.Domain;
using Blog.Models.ViewModels;
using Blog.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PostLikeController : ControllerBase
	{
		private readonly IPostLikeRepository postLikeRepository;

		public PostLikeController(IPostLikeRepository postLikeRepository)
        {
			this.postLikeRepository = postLikeRepository;
		}


        [HttpPost]
		[Route("Add")]
		public async Task<IActionResult> AddLike([FromBody] AddLikeRequest addLikeRequest)
		{

			var domainModel = new PostLike
			{
				PostId = addLikeRequest.PostId,
				UserId = addLikeRequest.UserId
			};

			await postLikeRepository.AddLikeForPost(domainModel);

			return Ok();
		}

		[HttpGet]
		[Route("{postId:Guid}/totalLikes")]
		public async Task<IActionResult> GetTotalLikesForPost([FromRoute] Guid postId)
		{
			var totalLikes = await postLikeRepository.GetTotalLikes(postId);
			
			return Ok(totalLikes);
		}
	}
}
