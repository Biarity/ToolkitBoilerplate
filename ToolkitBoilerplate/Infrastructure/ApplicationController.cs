using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ToolkitBoilerplate.Infrastructure;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using ToolkitBoilerplate.Data;

namespace ToolkitBoilerplate.Infrastructure
{
    [ApiController]
    public abstract class ApplicationController<TEntity> : ControllerBase
        where TEntity : ApplicationEntity, new()
    {
        protected ApplicationDbContext _dbContext;
        protected SieveProcessor _sieveProcessor;
        protected IConfiguration _config;
        private readonly ILogger _logger;

        public ApplicationController(
            ApplicationDbContext dbContext,
            SieveProcessor sieveProcessor,
            IConfiguration config, 
            ILogger<ApplicationController<TEntity>> logger)
        {
            _dbContext = dbContext;
            _sieveProcessor = sieveProcessor;
            _config = config;
            _logger = logger;
        }

        #region HELEPRS

        protected virtual void CreateAndAdd(TEntity entity)
        {
            entity.Create(CurrentUserId);
            _dbContext.Add(entity);
        }

        protected virtual async Task<IActionResult> SaveChangesAndReturn(object result = null)
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is DbException)
            {
                return BadRequest();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status410Gone);
            }
            catch (DbUpdateException)
            {
                throw; // 500
            }

            if (result == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(result);
            }
        }

        protected virtual bool IsUserAuthenticated
        {
            get
            {
                return User.Identity.IsAuthenticated;
            }
        }

        protected virtual int CurrentUserId
        {
            get
            {
                return User.GetUserId();
            }
        }

        protected virtual string GetQueryString(string key)
        {
            var query = Request.Path.ToString();
            return query.Length < 1 ? null : query;
        }

        protected virtual IQueryable<TEntity> GetAsNoTracking()
        {
            return _dbContext.Set<TEntity>().AsNoTracking();
        }

        protected virtual async Task<bool> DoesCurrentUserOwnAnyOfAsync(int Id)
        {
            return await GetAsNoTracking()
                    .AnyAsync(e => e.Id == Id && e.UserId == CurrentUserId);
        }
        
        protected virtual void FilterToUserOwned(ref IQueryable<TEntity> entities)
        {
            entities = entities.Where(e => e.UserId == CurrentUserId);
        }

        protected virtual IQueryable<TEntity> FilterToId(int? id, IQueryable<TEntity> source)
        {
            return id == null ? source : source.Where(e => e.Id == id);
        }

        protected virtual IQueryable<TEntity> FilterDeletedFlag(IQueryable<TEntity> source)
        {
            return source.Where(e => !e.IsDeleted);
        }

        protected virtual IQueryable<TEntity> ApplyPagination(SieveModel sieveModel, IQueryable<TEntity> source)
        {
            return _sieveProcessor.Apply(sieveModel, source,
                applyFiltering: false, applySorting: false);
        }

        protected virtual IQueryable<TEntity> ApplyFilterAndSort(SieveModel sieveModel, IQueryable<TEntity> source)
        {
            return _sieveProcessor.Apply(sieveModel, source,
                applyPagination: false);
        }

        protected virtual EntityEntry<TEntity> AttachGetEntry(int id, TEntity entity)
        {
            entity.Update(id, User.GetUserId());

            var entry = _dbContext.Attach(entity);

            entry.Property(e => e.Updated).IsModified = true;

            return entry;
        }

        protected virtual void MarkPropertiesModified(EntityEntry<TEntity> entityEntry, params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
                entityEntry.Property(propertyName).IsModified = true;
        }

        protected virtual void MarkAllPropertiesModified(EntityEntry<TEntity> entityEntry)
        {
            foreach (var property in entityEntry.Properties)
                property.IsModified = true;
        }

        protected virtual void MarkPropertiesNotModified(EntityEntry<TEntity> entityEntry, params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
                entityEntry.Property(propertyName).IsModified = false;
        }

        protected virtual async Task<bool> IsDeletedFlag(int id)
        {
            return await _dbContext.Set<TEntity>().AnyAsync(e => e.Id == id
                                                            && e.IsDeleted);
        }

        protected virtual void SetDeletedFlag(TEntity entity)
        {
            var set = _dbContext.Set<TEntity>();
            entity.IsDeleted = true;
            _dbContext.Set<TEntity>().Attach(entity).Property(e => e.IsDeleted).IsModified = true;
        }

        protected virtual void RemoveWithId(int id)
        {
            _dbContext.Remove(new TEntity() { Id = id });
        }

        #endregion HELPERS
    }

}
