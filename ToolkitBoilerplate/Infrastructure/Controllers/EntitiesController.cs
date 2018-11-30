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
    public abstract class EntitiesController<TEntity, TEntityParent> : EntitiesController<TEntity>
        where TEntity : ChildEntity<TEntityParent>, new()
        where TEntityParent : ApplicationEntity, new()
    {
        public EntitiesController(ApplicationDbContext dbContext, SieveProcessor sieveProcessor, IConfiguration config, ILogger<ApplicationController<TEntity>> logger) : base(dbContext, sieveProcessor, config, logger)
        {
        }

        [AllowAnonymous]
        [HttpGet("Parent/{parentId}")]
        [ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "*" })]
        public async Task<IActionResult> Read(int parentId, [FromQuery]SieveModel sieveModel)
        {
            var source = GetAsNoTracking();

            source = ApplyFilterAndSort(sieveModel, source);

            source = source
                .Where(r => r.ParentId == parentId
                         && r.Parent.UserId == CurrentUserId);

            source = FilterCanAccessContent(source);

            source = ApplyPagination(sieveModel, source);

            var result = await source.ToListAsync();

            return Ok(new { data = result });
        }
    }

    [Authorize]
    public abstract class EntitiesController<TEntity> : ApplicationController<TEntity>
        where TEntity : ApplicationEntity, new()
    {
        public EntitiesController(ApplicationDbContext dbContext, SieveProcessor sieveProcessor, IConfiguration config, ILogger<ApplicationController<TEntity>> logger) : base(dbContext, sieveProcessor, config, logger)
        {
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody]TEntity content)
        {
            if (!await CanCreateContent(content))
                return Unauthorized();

            CreateAndAdd(content);
            
            return await SaveChangesAndReturn(content);

        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        [ResponseCache(Duration = 200)]
        public async Task<IActionResult> Read(int id)
        {
            var source = GetAsNoTracking();

            source = FilterToId(id, source);
            source = FilterCanAccessContent(source);

            var result = await source.FirstOrDefaultAsync();

            return Ok(result);

        }

        [AllowAnonymous]
        [HttpGet("")]
        [ResponseCache(Duration = 200)]
        public async Task<IActionResult> Read([FromQuery]SieveModel sieveModel)
        {
            var source = GetAsNoTracking();

            source = ApplyFilterAndSort(sieveModel, source);
            source = FilterCanAccessContent(source);

            source = ApplyPagination(sieveModel, source);

            var result = await source.ToListAsync();

            return Ok(new { data = result });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]TEntity item)
        {
            if (!await CanUpdateContent(id, item)
             || !await DoesCurrentUserOwnAnyOfAsync(id))
                return Unauthorized();

            var entry = AttachGetEntry(id, item);

            MarkAllPropertiesModified(entry);

            return await SaveChangesAndReturn();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await CanDeleteContent(id)
             || !await DoesCurrentUserOwnAnyOfAsync(id))
                return Unauthorized();

            RemoveWithId(id);

            return await SaveChangesAndReturn();
        }

        protected virtual IQueryable<TEntity> FilterCanAccessContent(IQueryable<TEntity> entities)
        {
            return entities;
        }

        protected virtual Task<bool> CanCreateContent(TEntity entity)
        {
            return Task.FromResult(true);
        }

        protected virtual Task<bool> CanUpdateContent(int id, TEntity entity)
        {
            return Task.FromResult(true);
        }

        protected virtual Task<bool> CanDeleteContent(int id)
        {
            return Task.FromResult(true);
        }
    }
}
