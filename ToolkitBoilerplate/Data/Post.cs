using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToolkitBoilerplate.Infrastructure.Data;

namespace ToolkitBoilerplate.Data
{
    public class Post : Item<Post, PostLike> { }
    public class PostLike : Like<Post> { }
    public class PostComment : Comment<PostComment, Post, PostCommentLike> { }
    public class PostCommentLike : Like<PostComment> { }
}
