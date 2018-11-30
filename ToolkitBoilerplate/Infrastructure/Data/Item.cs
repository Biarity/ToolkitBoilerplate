using Sieve.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ToolkitBoilerplate.Infrastructure;

namespace ToolkitBoilerplate.Infrastructure.Data
{
    [DataContract]
    public class Item : ApplicationEntity
    {
        [DataMember(IsRequired = true), MinLength(3), MaxLength(50), RegularExpression(RegexForName)]
        public string Name { get; set; }
        [DataMember, MinLength(20), MaxLength(200)]
        public string Tagline { get; set; }
        [DataMember, MaxLength(4500)]
        public string Description { get; set; }
        [DataMember, Url]
        public string CoverImageUrl { get; set; }
        [DataMember, Sieve(CanFilter = true)]
        public bool IsMature { get; set; }
    }

    public class Item<TSelf, TVote> : Item, IVoteParent
        where TSelf : Item<TSelf, TVote>, new()
        where TVote : Vote<TSelf>, new()
    {
        [DataMember]
        public int VoteCount { get; set; }
        public List<TVote> Votes { get; set; }
    }
}
