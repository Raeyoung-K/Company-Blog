﻿using Blog.Models.Domain;

namespace Blog.Models.ViewModels
{
    public class PostDetailViewModel
    {
        public Guid Id { get; set; }

        public string Heading { get; set; }

        public string PageTitle { get; set; }

        public string Content { get; set; }

        public string ShortDescription { get; set; }

        public string FeaturedImageUrl { get; set; }

        public string UrlHandle { get; set; }

        public DateTime PublisehdDate { get; set; }

        public string Author { get; set; }

        public bool Visible { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public int TotalLikes { get; set; }

        public bool Liked { get; set; }

		public string CommentDescription { get; set; }

		public IEnumerable<PostCommentViewModel> Comments { get; set; }
    }
}
