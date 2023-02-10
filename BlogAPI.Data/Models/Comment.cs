using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Data.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int AuthorId { get; set; }
        public Person Author { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }

    }

    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(x => x.CommentId);
            builder.Property(x => x.CommentId).ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.HasOne(x => x.Author)
                .WithMany()
                .HasForeignKey(x => x.AuthorId);

            builder.HasOne(x => x.Post)
                .WithMany(y => y.Comments)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
