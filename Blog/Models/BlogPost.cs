using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class BlogPost
    {
        public int Id { get; set; }

        //Foreign Key
        public int BlogItemId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string Title { get; set; } = "";

        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string Abstract { get; set; } = "";

        [Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string Body { get; set; } = "";

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        //Navigation Properties
        public BlogItem BlogItem { get; set; } = default!;
    }
}
