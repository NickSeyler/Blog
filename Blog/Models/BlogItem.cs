using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class BlogItem
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Blog Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string BlogName { get; set; } = "";

        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string Description { get; set; } = "";

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        //Image storage
        [Display(Name = "Choose Image")]
        public byte[] ImageData { get; set; } = Array.Empty<byte>();
        public string ImageType { get; set; } = "";

        //List of Posts
        public ICollection<BlogPost> BlogPosts { get; set; } = new HashSet<BlogPost>();
    }
}
