using Sieve.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ToolkitBoilerplate.Data;

namespace ToolkitBoilerplate.Infrastructure.Data
{
    [DataContract]
    public abstract class ApplicationEntity
    {
        [DataMember]
        public virtual int Id { get; set; }

        [DataMember, Sieve(CanSort = true)]
        public virtual DateTimeOffset Created { get; set; }

        [DataMember, Sieve(CanSort = true)]
        public virtual DateTimeOffset Updated { get; set; }

        [DataMember, Sieve(CanFilter = true)]
        public virtual int UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual bool IsDeleted { get; set; }

        [NotMapped]
        public const string RegexForName = "[A-Za-z0-9_ ]+";

        public virtual void Create(int userId = 0)
        {
            Id = 0;
            Created = DateTimeOffset.UtcNow;
            Updated = DateTimeOffset.UtcNow;
            UserId = userId;
            IsDeleted = false;
        }

        public virtual void Update(int id, int userId = 0)
        {
            Id = id;
            Updated = DateTimeOffset.UtcNow;
            UserId = userId;
        }
    }
}
