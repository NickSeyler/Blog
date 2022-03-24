using Blog.Enum;

namespace Blog.Models
{
    public class Comment
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign Key
        /// </summary>
        public int BlogPostId { get; set; }


        /// <summary>
        /// The User ID of the Author.
        /// </summary>
        public string? AuthorId { get; set; }

        /// <summary>
        /// The User ID of the Moderator of a comment, if applicable.
        /// </summary>
        public string? ModeratorId { get; set; }

        /// <summary>
        /// The contents of the comment.
        /// </summary>
        public string CommentBody { get; set; } = "";

        /// <summary>
        /// When the comment was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// The latest date the comment was modified.
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Whether or not the comment is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// The latest date the comment was moderated, if applicable.
        /// </summary>
        public DateTime? ModeratedDate { get; set; }

        public ModerationReason ModerationReason { get; set; }

        /// <summary>
        /// The new contents of the comment if it were modified by a moderator.
        /// </summary>
        public string? ModeratedBody { get; set; }

        public virtual BlogPost? BlogPost { get; set; }

        public virtual BlogUser? Author { get; set; }

        public virtual BlogUser? Moderator { get; set; }
    }
}
