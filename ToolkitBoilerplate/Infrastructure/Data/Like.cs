using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ToolkitBoilerplate.Infrastructure.Data
{
    public interface ILikeParent
    {
        int LikeCount { get; set; }
        //List<TLike> Likes { get; set; }
    }

    public class Like<TParent> : ChildEntity<TParent>
        where TParent : ApplicationEntity, new()
    {
    }

}
