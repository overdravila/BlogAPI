using BlogAPI.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Data
{
    public class BlogAPIDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<PersonType> PersonType { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<PostStatus> PostStatus { get; set; }
        public virtual DbSet<PostRejectionComment> PostRejectionComment { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }

        public virtual RefreshToken RefreshToken { get; set; }

        public BlogAPIDbContext()
        {
        }

        public BlogAPIDbContext(DbContextOptions<BlogAPIDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var appSettingsLocation = @Directory.GetCurrentDirectory() + "/../BlogAPI/appsettings.json";
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(appSettingsLocation)
                    .Build();

                var connectionString = configuration.GetConnectionString("SqlServerConnection");
                if (connectionString != null) optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            new PersonConfiguration().Configure(builder.Entity<Person>());
            new PersonTypeConfiguration().Configure(builder.Entity<PersonType>());
            new PostConfiguration().Configure(builder.Entity<Post>());
            new PostStatusConfiguration().Configure(builder.Entity<PostStatus>());
            new PostRejectionCommentConfiguration().Configure(builder.Entity<PostRejectionComment>());
            new CommentConfiguration().Configure(builder.Entity<Comment>());
            new RefreshTokenConfiguration().Configure(builder.Entity<RefreshToken>());
        }

    }
}
