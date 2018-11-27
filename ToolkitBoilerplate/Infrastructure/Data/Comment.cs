using Sieve.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ToolkitBoilerplate.Infrastructure;

namespace ToolkitBoilerplate.Infrastructure.Data
{
    [DataContract]
    public class Comment<TParent> : ApplicationEntity
        where TParent : ApplicationEntity
    {
        [DataMember]
        public int ParentId { get; set; }
        public TParent Parent { get; set; }

        [DataMember(IsRequired = true), MinLength(5), MaxLength(1000)]
        public string Body { get; set; }
        
        [DataMember]
        public int? ParentCommentId { get; set; }
        public Comment<TParent> ParentComment { get; set; }
        public List<Comment<TParent>> ChildComments { get; set; }

        [DataMember, Sieve(CanSort = true)]
        public virtual DateTimeOffset LastActive { get; set; }
    }

    public class Comment<TParent, TReaction> : Comment<TParent>
        where TParent : ApplicationEntity
    {
        [DataMember]
        public int VoteCount { get; set; }

        public List<TReaction> Reactions { get; set; }
    }
}
