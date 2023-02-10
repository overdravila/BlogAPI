using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Data.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateOfPublishing { get; set; }
        public int PostStatusId { get; set; }
        public int AuthorId { get; set; }
        public virtual Person Author { get; set; }
        public virtual PostStatus PostStatus { get; set; }
        public virtual ICollection<PostRejectionComment> PostRejectionComments { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

    }

    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(x => x.PostId);
            builder.Property(x => x.PostId).ValueGeneratedOnAdd().UseIdentityColumn();

            builder.HasOne(x => x.PostStatus)
                .WithOne()
                .HasForeignKey<Post>(x => x.PostStatusId);

            builder.HasOne(x => x.Author)
                .WithMany()
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.PostStatus)
                .WithMany(x => x.Posts)
                .HasForeignKey(x => x.PostStatusId);
        }
    }
}
