using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Data.Models
{
    public class PostStatus
    {
        public int PostStatusId { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }

    public class PostStatusConfiguration : IEntityTypeConfiguration<PostStatus>
    {
        public void Configure(EntityTypeBuilder<PostStatus> builder)
        {
            builder.HasKey(x => x.PostStatusId);
            builder.Property(x => x.PostStatusId).ValueGeneratedOnAdd().UseIdentityColumn();

            builder.HasData(
                new PostStatus
                {
                    PostStatusId = 1,
                    Description = "Approved"
                },
                new PostStatus
                {
                    PostStatusId = 2,
                    Description = "Pending Approval"
                },
                new PostStatus
                {
                    PostStatusId = 3,
                    Description = "Rejected"
                },
                new PostStatus
                {
                    PostStatusId = 4,
                    Description = "Unsubmitted"
                }
            );
        }
    }
}
