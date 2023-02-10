using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Data.Models
{
    public class PostRejectionComment
    {
        public int PostRejectionCommentId { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public int PostId { get; set; }
        public virtual Person Author { get; set; }
        public virtual Post Post { get; set; }
    }
    public class PostRejectionCommentConfiguration : IEntityTypeConfiguration<PostRejectionComment>
    {
        public void Configure(EntityTypeBuilder<PostRejectionComment> builder)
        {
            builder.HasKey(x => x.PostRejectionCommentId);
            builder.Property(x => x.PostRejectionCommentId).ValueGeneratedOnAdd().UseIdentityColumn();

            builder.HasOne(x => x.Author)
                .WithOne()
                .HasForeignKey<PostRejectionComment>(x => x.AuthorId);

            builder.HasOne(x => x.Post)
                .WithMany(y => y.PostRejectionComments)
                .HasForeignKey(x => x.PostRejectionCommentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
