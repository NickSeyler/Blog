using Blog.Enum;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Display(Name = "Blog Id")]
        //Foreign Key
        public int BlogItemId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string Title { get; set; } = "";

        public string Slug { get; set; } = "";

        [Display(Name = "Mark for Deletion")]
        public bool IsDeleted { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string Abstract { get; set; } = "";

        [Display(Name = "Post State")]
        public BlogPostState BlogPostState { get; set; }

        [Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string Body { get; set; } = "";

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }


        //Image storage
        [Display(Name = "Choose Image")]
        public byte[] ImageData { get; set; } = Array.Empty<byte>();
        public string ImageType { get; set; } = "";


        [Display(Name = "Blog")]
        //Navigation Properties
        public BlogItem? BlogItem { get; set; }


        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
    }
}
