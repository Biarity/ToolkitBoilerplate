using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sieve.Services;
using ToolkitBoilerplate.Data;
using ToolkitBoilerplate.Infrastructure.Controllers;

namespace ToolkitBoilerplate.Controllers
{
    public class PostsController : EntitiesController<Post>
    {
        public PostsController(ApplicationDbContext dbContext, SieveProcessor sieveProcessor, IConfiguration config, ILogger<ApplicationController<Post>> logger) : base(dbContext, sieveProcessor, config, logger)
        {
        }
    }

    public class PostVotesController : VotesController<PostVote, Post>
    {
        public PostVotesController(ApplicationDbContext dbContext, SieveProcessor sieveProcessor, IConfiguration config, ILogger<ApplicationController<PostVote>> logger) : base(dbContext, sieveProcessor, config, logger)
        {
        }
    }

    public class PostCommentsController : CommentsController<PostComment, Post, PostCommentVote>
    {
        public PostCommentsController(ApplicationDbContext dbContext, SieveProcessor sieveProcessor, IConfiguration config, ILogger<ApplicationController<PostComment>> logger) : base(dbContext, sieveProcessor, config, logger)
        {
        }
    }

    public class PostCommentVotesController : VotesController<PostCommentVote, PostComment>
    {
        public PostCommentVotesController(ApplicationDbContext dbContext, SieveProcessor sieveProcessor, IConfiguration config, ILogger<ApplicationController<PostCommentVote>> logger) : base(dbContext, sieveProcessor, config, logger)
        {
        }
    }
}
