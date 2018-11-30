using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ToolkitBoilerplate.Infrastructure.Data
{
    public interface IVoteParent
    {
        int VoteCount { get; set; }
        //List<TVote> Votes { get; set; }
    }

    public class Vote<TParent> : ChildEntity<TParent>
        where TParent : ApplicationEntity, new()
    {
        [DataMember]
        public bool UpVote { get; set; }
    }
}
