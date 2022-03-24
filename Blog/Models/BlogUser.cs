using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    public class BlogUser : IdentityUser
    {
        /// <summary>
        /// The first name of user.
        /// </summary>
        public string FirstName { get; set; } = "";

        /// <summary>
        /// The last name of user.
        /// </summary>
        public string LastName { get; set; } = "";

        /// <summary>
        /// The display name of the user. This is equivalent to the user's email in all lowercase.
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// The full name of the user.
        /// </summary>
        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{LastName}, {FirstName}";
            }
        }
    }
}
