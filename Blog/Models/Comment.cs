namespace Blog.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int BlogPostId { get; set; }

        public string? AuthorId { get; set; }

        public string CommentBody { get; set; } = "";

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }

        public virtual BlogPost? BlogPost { get; set; }

        public virtual BlogUser? Author { get; set; }
    }
}
