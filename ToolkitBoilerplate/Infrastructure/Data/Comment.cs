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
    public class Comment<TSelf, TParent, TLike> : ApplicationEntity, ILikeParent
        where TSelf : Comment<TSelf, TParent, TLike>, new()
        where TParent : ApplicationEntity, new()
        where TLike : Like<TSelf>, new()
    {
        [DataMember(IsRequired = true)]
        public int ParentId { get; set; }
        public TParent Parent { get; set; }

        [DataMember(IsRequired = true), MinLength(5), MaxLength(1000)]
        public string Body { get; set; }
        
        [DataMember]
        public int? ParentCommentId { get; set; }
        public TSelf ParentComment { get; set; }
        public List<TSelf> ChildComments { get; set; }

        [DataMember, Sieve(CanSort = true)]
        public virtual DateTimeOffset LastActive { get; set; }

        [DataMember]
        public int LikeCount { get; set; }
        public List<TLike> Likes { get; set; }
    }
}
