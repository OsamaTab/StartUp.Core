using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badass.Core.Models
{
    public interface IPostRepository
    {
        List<Post> GetPost(int? filterPostId);
    }
}
