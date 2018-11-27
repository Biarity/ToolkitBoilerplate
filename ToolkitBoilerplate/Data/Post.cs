using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToolkitBoilerplate.Infrastructure.Data;

namespace ToolkitBoilerplate.Data
{
    public class Post : Content<PostComment, PostReaction> { }
    public class PostReaction : Reaction<Post, VoteReactionType> { }
    public class PostComment : Comment<Post, PostCommentReaction> { }
    public class PostCommentReaction : Reaction<PostComment, VoteReactionType> { }
}
