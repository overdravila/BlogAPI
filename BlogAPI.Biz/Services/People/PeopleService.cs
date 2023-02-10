using BlogAPI.Data;
using BlogAPI.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Biz.Services.People
{
    public class PeopleService : IPeopleService
    {
        private readonly BlogAPIDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public PeopleService(BlogAPIDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _dbContext = context;
            _userManager = userManager;
        }

        public async Task<Person> GetPersonByUserId(int userId)
        {
            return await _dbContext.Person.Where(p => p.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<Person> GetPersonByEmail(string email)
        {
            return await _dbContext.Person
                .Include(x => x.ApplicationUser)
                .Where(x => x.ApplicationUser.Email == email)
                .FirstOrDefaultAsync();
        }
    }
}
