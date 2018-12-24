using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToolkitBoilerplate.Data;
using ToolkitBoilerplate.Infrastructure.Data;

namespace ToolkitBoilerplate.Infrastructure.Controllers
{
    [Authorize]
    public abstract class VotesController<TVote, TVoteParent> : ApplicationController<TVote>
        where TVote : Vote<TVoteParent>, new()
        where TVoteParent : ApplicationEntity, IVoteParent, new()
    {
        public VotesController(ApplicationDbContext dbContext, SieveProcessor sieveProcessor, IConfiguration config, ILogger<ApplicationController<TVote>> logger) : base(dbContext, sieveProcessor, config, logger)
        {
        }

        [HttpPost("{parentId}")]
        public async Task<IActionResult> Vote(int parentId, bool upVote)
        {
            if (!await CanVote(parentId))
                return Unauthorized();

            var vote = await GetAsNoTracking()
                    .FirstOrDefaultAsync(v => v.ParentId == parentId
                                           && v.UserId == CurrentUserId);

            int voteChange = 0;

            if (vote == null)
            {
                vote = new TVote
                {
                    ParentId = parentId,
                    UpVote = upVote
                };
                vote.Create(CurrentUserId);
                _dbContext.Add(vote);

                if (upVote) voteChange++;
                else voteChange--;
            }
            else if (vote.UpVote != upVote)
            {
                vote.UpVote = upVote;
                _dbContext.Entry(vote)
                    .Property(nameof(vote.UpVote))
                    .IsModified = true;

                if (upVote) voteChange += 2;
                else voteChange -= 2;
            }
            else
            {
                return NoContent();
            }

            int parentVoteCount = await _dbContext.Set<TVoteParent>()
                    .Where(c => c.Id == parentId)
                    .Select(c => c.VoteCount)
                    .FirstOrDefaultAsync();

            var parent = new TVoteParent
            {
                Id = parentId,
                VoteCount = parentVoteCount + voteChange
            };

            _dbContext.Attach(parent)
                .Property(c => c.VoteCount)
                .IsModified = true;

            return await SaveChangesAndReturn(vote);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVote(int id)
        {
            if (!await DoesCurrentUserOwnAnyOfAsync(id))
                return Unauthorized();

            RemoveWithId(id);

            var parentData = await GetAsNoTracking()
                    .Where(v => v.Id == id)
                    .Select(v => new { v.ParentId, v.Parent.VoteCount })
                    .FirstOrDefaultAsync();

            var parent = new TVoteParent
            {
                Id = parentData.ParentId,
                VoteCount = parentData.VoteCount - 1
            };

            _dbContext.Attach(parent)
                .Property(c => c.VoteCount)
                .IsModified = true;

            return await SaveChangesAndReturn();
        }

        protected virtual Task<bool> CanVote(int parentId)
        {
            return Task.FromResult(true);
        }
    }

}
