using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ToolkitBoilerplate.Infrastructure;

namespace ToolkitBoilerplate.Infrastructure.Data
{
    [DataContract]
    public class Reaction<TParent, TReactionType> : ApplicationEntity
        where TReactionType : Enum
    {
        [DataMember]
        public int ParentId { get; set; }
        public TParent Parent { get; set; }

        [DataMember]
        public TReactionType Type { get; set; }
    }

    public enum VoteReactionType
    {
        Up,
        Down
    }

}
