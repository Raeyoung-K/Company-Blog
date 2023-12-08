using Blog.Data;
using Blog.Models.Domain;
using Blog.Models.ViewModels;
using Blog.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
	{
        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        
        [HttpGet]
		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddTagRequest addTagRequest)
		{
			// Mapping AddTagRequest to Tag domain model
			var tag = new Tag
			{
				Name = addTagRequest.Name,
				DisplayName = addTagRequest.DisplayName
			};

			await tagRepository.AddAsync(tag);

			//await blogDbContext.Tags.AddAsync(tag);
			//await blogDbContext.SaveChangesAsync();

			return RedirectToAction("List") ;
		}

		[HttpGet]
		public async Task<IActionResult> List()
		{
			var tags = await tagRepository.GetAllTagsAsync();

			return View(tags);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(Guid id)
		{
			var tag = await tagRepository.GetAsync(id);

			if (tag != null)
			{
				var editTagRequest = new EditTagRequest
				{
					Id = tag.Id,
					Name = tag.Name,
					DisplayName = tag.DisplayName
				};
                return View(editTagRequest);
            }

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
		{
			var tag = new Tag
			{
				Id = editTagRequest.Id,
				Name = editTagRequest.Name,
				DisplayName = editTagRequest.DisplayName
			};

			var updatedTag = await tagRepository.UpdateAsync(tag);

			if (updatedTag != null) 
			{
				return RedirectToAction("List");
			}
			else
			{
                return RedirectToAction("Edit", new { id = editTagRequest.Id });
            }

			
        }


		[HttpPost]
		public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
		{
			var tagToDelete = await tagRepository.DeleteAsync(editTagRequest.Id);

			if (tagToDelete != null)
			{
				return RedirectToAction("List");
			}
			else
			{
                return RedirectToAction("Edit", new { id = editTagRequest.Id });
            }

		
		}
	}
}
