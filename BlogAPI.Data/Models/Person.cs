using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Data.Models
{
    public class Person
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PersonTypeId { get; set; }
        public int UserId { get; set; }
        public virtual PersonType PersonType { get; set; } 
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }

    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure (EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(x => x.PersonId);
            builder.Property(x => x.PersonId).ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.HasOne(x => x.ApplicationUser)
                .WithOne()
                .HasForeignKey<Person>(x => x.UserId);

            builder.HasOne(x => x.PersonType)
                .WithMany(y => y.Persons)
                .HasForeignKey(x => x.PersonTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}
