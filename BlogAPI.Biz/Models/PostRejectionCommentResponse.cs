using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Biz.Models
{
    public class PostRejectionCommentResponse
    {
        public int PostRejectionCommentId { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string PostTitle { get; set; }
    }
}
