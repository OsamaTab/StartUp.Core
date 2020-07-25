using Badass.Core.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badass.Core.Models
{
    public class MackPostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;
        public MackPostRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Post> GetPost(int? filterPostId)
        {
            var pastss = _context.Posts.Include(x => x.PostType).Include(x => x.CreatedBy).Include(x => x.UpdatedBy).Where(x => x.PostTypeId == filterPostId);
            return pastss.ToList();
        }

    }
}
