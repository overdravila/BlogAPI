using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Data.Models
{
    public class PersonType
    {
        public int PersonTypeId { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Person> Persons { get; set; }


    }

    public class PersonTypeConfiguration : IEntityTypeConfiguration<PersonType>
    {
        public void Configure(EntityTypeBuilder<PersonType> builder)
        {
            builder.HasKey(x => x.PersonTypeId);
            builder.Property(x => x.PersonTypeId).ValueGeneratedOnAdd().UseIdentityColumn();

            builder.HasData(new PersonType
            {
                PersonTypeId = 1,
                Description = "Writer"
            });

            builder.HasData(new PersonType
            {
                PersonTypeId = 2,
                Description = "Editor"
            });

            builder.HasData(new PersonType
            {
                PersonTypeId = 3,
                Description = "Public"
            });
        }
    }
}
