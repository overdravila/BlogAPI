using BlogAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Biz.Services.People
{
    public interface IPeopleService
    {
        Task<Person> GetPersonByUserId(int userId);
        Task<Person> GetPersonByEmail(string email);
    }
}
