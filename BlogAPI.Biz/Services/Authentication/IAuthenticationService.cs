using BlogAPI.Biz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Biz.Services.Authentication
{
    public interface IAuthenticationService
    {
       Task<AuthenticationResult> RegisterAsync(UserPerson userPerson);
       Task<AuthenticationResult> LoginAsync(string email, string password);
    }
}
