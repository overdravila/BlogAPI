using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Data.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
