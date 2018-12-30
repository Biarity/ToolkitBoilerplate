using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToolkitBoilerplate.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostVote> PostVotes { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<PostCommentLike> PostCommentVotes { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SeedUsers(modelBuilder);
        }

        protected virtual void SeedUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser()
                {
                    Id = -1,
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "biarity@outlook.com",
                    NormalizedEmail = "BIARITY@OUTLOOK.COM",
                    EmailConfirmed = true,
                    SecurityStamp = "THIS_IS_SEEDED_0"
                },
                new ApplicationUser()
                {
                    Id = -2,
                    UserName = "system",
                    NormalizedUserName = "SYSTEM",
                    Email = "biarity+system@outlook.com",
                    NormalizedEmail = "BIARITY+SYSTEM@OUTLOOK.COM",
                    EmailConfirmed = true,
                    SecurityStamp = "THIS_IS_SEEDED_1"
                });
        }
    }

}
