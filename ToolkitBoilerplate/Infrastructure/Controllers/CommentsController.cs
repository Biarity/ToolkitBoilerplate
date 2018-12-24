using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using Sieve.Services;
using ToolkitBoilerplate.Data;
using ToolkitBoilerplate.Infrastructure.Data;

namespace ToolkitBoilerplate.Infrastructure.Controllers
{
    [Authorize]
    public abstract class CommentsController<TComment, TCommentParent, TCommentVote> : ApplicationController<TComment>
        where TComment : Comment<TComment, TCommentParent, TCommentVote>, new()
        where TCommentParent : ApplicationEntity, new()
        where TCommentVote : Vote<TComment>, new()
    {
        public CommentsController(ApplicationDbContext dbContext, SieveProcessor sieveProcessor, IConfiguration config, ILogger<ApplicationController<TComment>> logger) : base(dbContext, sieveProcessor, config, logger)
        {
        }

        //private readonly IHubContext<CommentsHub> _commentsHubContext;

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody]TComment comment)
        {
            CreateAndAdd(comment);

            if (!await CanCreateCommentOnParent(comment))
                return Unauthorized();

            if (comment.ParentCommentId != null)
            {
                if (!await CanCreateCommentOnComment(comment))
                    return Unauthorized();
                
                Bump((int)comment.ParentCommentId);
            }

            var initialVote = new TCommentVote {};
            initialVote.Create(CurrentUserId);
            comment.Votes = new List<TCommentVote>();
            comment.Votes.Add(initialVote);
            comment.VoteCount = 1;

            var result = new
            {
                Comment = comment,
                UserVotes = comment.Votes
            };

            // TODO put into method
            //await _commentsHubContext.Clients
            //    .Group(CommentsHub.GetCommentGroupName(parentType, parentId))
            //    .SendAsync(HubMethod.CommentRecieved.ToString(), JsonConvert.SerializeObject(result));

            return await SaveChangesAndReturn(result);
        }

        [AllowAnonymous]
        [HttpGet("Parent/{parentId}")]
        [ResponseCache(Duration = 10, VaryByQueryKeys = new[] { "*" })]
        public async Task<IActionResult> Read(int parentId, [FromQuery]SieveModel sieveModel)
        {
            var source = GetAsNoTracking();

            source = ApplyFilterAndSort(sieveModel, source);
            source = FilterCanAccessComment(source);

            source = source.Where(c => c.ParentId == parentId
                                    && c.ParentCommentId == null);

            var result = await SelectPaginateComments(sieveModel, source, 2)
                                    .ToListAsync();

            return Ok(new { data = result });
        }

        [AllowAnonymous]
        [HttpGet("Comment/{commentId}")]
        [ResponseCache(Duration = 10, VaryByQueryKeys = new[] { "*" })]
        public async Task<IActionResult> ReadOfComment(int commentId, [FromQuery]SieveModel sieveModel)
        {
            var source = GetAsNoTracking();

            source = ApplyFilterAndSort(sieveModel, source);
            source = FilterCanAccessComment(source);

            source = source.Where(c => c.ParentCommentId == commentId);

            var result = await SelectPaginateComments(sieveModel, source, 5)
                                    .ToListAsync();

            return Ok(new { data = result });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]TComment comment)
        {
            if (!await DoesCurrentUserOwnAnyOfAsync(id))
                return Unauthorized();

            var entry = AttachGetEntry(id, comment);

            MarkPropertiesModified(entry, nameof(entry.Entity.Body));

            return await SaveChangesAndReturn();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await DoesCurrentUserOwnAnyOfAsync(id))
                return Unauthorized();

            RemoveWithId(id);

            return await SaveChangesAndReturn();
        }
        
        protected void Bump(int commentId)
        {
            var comment = new TComment { Id = commentId };
            
            _dbContext.Attach(comment)
                .Property(nameof(comment.LastActive))
                .IsModified = true;

            comment.LastActive = DateTimeOffset.Now;
        }

        protected virtual Task<bool> CanCreateCommentOnParent(TComment comment)
        {
            return Task.FromResult(true);
        }

        protected virtual Task<bool> CanCreateCommentOnComment(TComment comment)
        {
            return Task.FromResult(true);
        }

        protected virtual IQueryable<TComment> FilterCanAccessComment(IQueryable<TComment> comments)
        {
            return comments;
        }

        protected virtual IQueryable<object> SelectPaginateComments(SieveModel sieveModel, 
            IQueryable<TComment> comments, int numChildComments)
        {
            var currentUserId = IsUserAuthenticated ? CurrentUserId : -1;
            
            var result = comments
            .Select(c => new
            {
                Comment = c,
                c.User.UserName,
                UserVotes = c.Votes.Where(r => r.UserId == currentUserId)
                    .Select(r => new { r.Id, r.UpVote }),
                ChildComments = numChildComments <= 0 ? null : c.ChildComments
                    .OrderByDescending(cc => cc.Created)
                    .Select(cc => new
                    {
                        Comment = cc,
                        cc.User.UserName,
                        UserVotes = cc.Votes.Where(r => r.UserId == currentUserId)
                            .Select(r => new { r.Id, r.UpVote }),
                    }).Take(numChildComments)
            });
            
            return _sieveProcessor.Apply(sieveModel, result, applyFiltering: false, applySorting: false);
        }
    }

}
