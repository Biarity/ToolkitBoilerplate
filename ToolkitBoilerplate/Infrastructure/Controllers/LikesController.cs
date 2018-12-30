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
    public abstract class LikesController<TLike, TLikeParent> : ApplicationController<TLike>
        where TLike : Like<TLikeParent>, new()
        where TLikeParent : ApplicationEntity, ILikeParent, new()
    {
        public LikesController(ApplicationDbContext dbContext, SieveProcessor sieveProcessor, IConfiguration config, ILogger<ApplicationController<TLike>> logger) : base(dbContext, sieveProcessor, config, logger)
        {
        }

        [HttpPost("{parentId}")]
        public async Task<IActionResult> Like(int parentId)
        {
            if (!await CanLike(parentId))
                return Unauthorized();

            if (await GetAsNoTracking()
                    .AnyAsync(l => l.ParentId == parentId
                                && l.UserId == CurrentUserId))
                return NoContent();

            var like = new TLike
            {
                ParentId = parentId
            };
            like.Create(CurrentUserId);
            _dbContext.Add(like);

            await IncrementLikeCount(parentId);

            return await SaveChangesAndReturn(like);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLike(int id)
        {
            if (!await DoesCurrentUserOwnAnyOfAsync(id))
                return Unauthorized();

            RemoveWithId(id);

            await DecrementLikeCount(id);

            return await SaveChangesAndReturn();
        }
        
        private async Task IncrementLikeCount(int parentId)
        {
            int parentLikeCount = await _dbContext.Set<TLikeParent>()
                    .Where(p => p.Id == parentId)
                    .Select(p => p.LikeCount)
                    .FirstOrDefaultAsync();

            var parent = new TLikeParent
            {
                Id = parentId,
                LikeCount = parentLikeCount + 1
            };

            _dbContext.Attach(parent)
                .Property(p => p.LikeCount)
                .IsModified = true;
        }


        private async Task DecrementLikeCount(int id)
        {
            var parentData = await GetAsNoTracking()
                    .Where(l => l.Id == id)
                    .Select(l => new { l.ParentId, l.Parent.LikeCount })
                    .FirstOrDefaultAsync();

            var parent = new TLikeParent
            {
                Id = parentData.ParentId,
                LikeCount = parentData.LikeCount - 1
            };

            _dbContext.Attach(parent)
                .Property(p => p.LikeCount)
                .IsModified = true;
        }

        protected virtual Task<bool> CanLike(int parentId)
        {
            return Task.FromResult(true);
        }
    }

}
