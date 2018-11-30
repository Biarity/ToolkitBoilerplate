using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sieve.Services;
using ToolkitBoilerplate.Data;
using ToolkitBoilerplate.Infrastructure.Controllers;

namespace ToolkitBoilerplate.Controllers.Posts
{
    public class PostCommentsController : CommentsController<PostComment, Post, PostCommentVote>
    {
        public PostCommentsController(ApplicationDbContext dbContext, SieveProcessor sieveProcessor, IConfiguration config, ILogger<ApplicationController<PostComment>> logger) : base(dbContext, sieveProcessor, config, logger)
        {
        }
    }
}
