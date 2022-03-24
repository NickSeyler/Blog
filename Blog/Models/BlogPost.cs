using Blog.Enum;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class BlogPost
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign Key
        /// </summary>
        [Display(Name = "Blog Id")]
        public int BlogItemId { get; set; }

        /// <summary>
        /// Title of the Blog Post
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string Title { get; set; } = "";

        /// <summary>
        /// Unique string based on the title that is used in the URL
        /// </summary>
        public string Slug { get; set; } = "";

        /// <summary>
        /// Whether or not the Blog Post is Deleted
        /// </summary>
        [Display(Name = "Mark for Deletion")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// A short description of the contents of the Blog Post
        /// </summary>
        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string Abstract { get; set; } = "";

        /// <summary>
        /// TODO
        /// </summary>
        [Display(Name = "Post State")]
        public BlogPostState BlogPostState { get; set; }

        /// <summary>
        /// The full contents of the Blog Post, including the HMTL.
        /// </summary>
        [Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string Body { get; set; } = "";

        /// <summary>
        /// When the Blog Post was created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// The latest date the Blog Post was modified.
        /// </summary>
        public DateTime? Updated { get; set; }


        /// <summary>
        /// The image data.
        /// </summary>
        [Display(Name = "Choose Image")]
        public byte[] ImageData { get; set; } = Array.Empty<byte>();

        /// <summary>
        /// The extension of the image.
        /// </summary>
        public string ImageType { get; set; } = "";


        [Display(Name = "Blog")]
        //Navigation Properties
        public BlogItem? BlogItem { get; set; }


        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
    }
}
