using System;
using System.Collections.Generic;

namespace DatabaseFirst.Models
{
    public partial class User
    {
        public User()
        {
            this.Comments = new HashSet<Comment>();
            this.Posts = new HashSet<Post>();
        }

        public int UserId { get; set; }
        public string Name { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
