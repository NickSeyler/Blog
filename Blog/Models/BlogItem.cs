using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class BlogItem
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign Key
        /// </summary>
        [Required]
        [Display(Name = "Blog Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string BlogName { get; set; } = "";

        /// <summary>
        /// Description of the Blog
        /// </summary>
        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string Description { get; set; } = "";

        /// <summary>
        /// When the Blog was created.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        /// <summary>
        /// The latest date the Blog was modified.
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

        //List of Posts
        public ICollection<BlogPost> BlogPosts { get; set; } = new HashSet<BlogPost>();
    }
}
