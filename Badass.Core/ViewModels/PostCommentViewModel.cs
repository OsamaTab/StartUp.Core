using Badass.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badass.Core.ViewModels
{
    public class PostCommentViewModel : Post
    {
        public IEnumerable<Comment> Comments { get; set; }
        public string CommentBody { get; set; }
    }
}
