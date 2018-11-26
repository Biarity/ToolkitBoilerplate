using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Models;
using Sieve.Services;
using Swashbuckle.AspNetCore.Swagger;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using ToolkitBoilerplate.Data;
using ToolkitBoilerplate.Infrastructure;
using ToolkitBoilerplate.Services;

namespace ToolkitBoilerplate
{
    public class Startup
    {
        private string ClientRootPath { get => "ClientApp/dist"; }
        private bool UseLocalDb { get => false; }
        private string ProxySpaAddress { get => "http://localhost:8080"; }
        private string AuthEmailTokenProviderName { get => "Email"; }
        private string SwaggerEndpointName { get => "Swagger :)"; }
        private string SpaSourcePath { get => "ClientApp"; }
        
        private string LocalDbDevConnectionConfigKey { get => "MSSQLLocalDB"; }
        private string NpgsqlConnectionConfigKey { get => "Postgres"; }
        private string DistributedRedisConnectionConfigKey { get => "Redis"; }
        private string SignalRRedisConnectionConfigKey { get => "Redis"; }
        private string SieveConfigSectionConfigKey { get => "Sieve"; }
        private string JwtAudienceConfigKey { get => "JwtAuth:Audience"; }
        private string JwtAuthorityConfigKey { get => "JwtAuth:AuthorityUrl"; }
        private string DistributedRedisNameKey { get => "DistributedRedisName"; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Config = configuration;
            Env = env;
        }

        public IConfiguration Config { get; }
        public IHostingEnvironment Env { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Directory to serve SPA files from
            // FOR PRODUCTION, RUN `npm run build` FIRST
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = ClientRootPath;
            });

            // SWASHBUCKLE
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = SwaggerEndpointName, Version = "v1" });
            });

            // DATABASE
            services.AddDbContext<ApplicationDbContext>(opts =>
            {
                if (Env.IsDevelopment())
                {
                    opts.EnableSensitiveDataLogging(true);
                    if (UseLocalDb)
                        opts.UseSqlServer(Config.GetConnectionString(LocalDbDevConnectionConfigKey));
                    else
                        opts.UseNpgsql(Config.GetConnectionString(NpgsqlConnectionConfigKey));
                }
                else
                {
                    opts.UseNpgsql(Config.GetConnectionString(NpgsqlConnectionConfigKey));
                }
            });

            // CACHE
            services.AddMemoryCache(); // Not distributed, 1 per server, unused mostly

            if (Env.IsDevelopment())
            {
                services.AddDistributedMemoryCache(); // Not distributed, same as AddMemoryCache, good as a placeholder
            }
            else
            {
                services.AddDistributedRedisCache(options =>
                {
                    options.InstanceName = Config[DistributedRedisNameKey];
                    options.Configuration = Config.GetConnectionString(DistributedRedisConnectionConfigKey); // StackExchange.Redis format
                });
            }


            // MISC
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // SIEVE
            services.Configure<SieveOptions>(Config.GetSection(SieveConfigSectionConfigKey));
            services.AddScoped<SieveProcessor>();
            services.AddScoped<ISieveCustomSortMethods, SieveCustomSortMethods>();
            services.AddScoped<ISieveCustomFilterMethods, SieveCustomFilterMethods>();

            // EMAIL SENDER
            if (Env.IsDevelopment())
                services.AddTransient<IEmailSender, DevEmailSender>();
            else
                services.AddScoped<IEmailSender, SmtpMessageSender>();

            // RECAPTCHA
            services.AddScoped<ValidateRecaptchaAttribute>();

            // COOKIE POLICY
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // IDENTITY
            services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddUserStore<UserStore<ApplicationUser, IdentityRole<int>, ApplicationDbContext, int>>()
                .AddRoleStore<RoleStore<IdentityRole<int>, ApplicationDbContext, int>>()
                .AddTokenProvider<EmailTokenProvider<ApplicationUser>>(AuthEmailTokenProviderName)
                .AddSignInManager();

            // COOKIE OPTIONS
            // Order matters for this one, should be after IDENTITY
            services.ConfigureApplicationCookie(options =>
            {
                // Return a 401 with [Authorized]
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.Clear();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
            });

            // AUTH TOKEN VALIDATION
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opts =>
            {
                opts.Audience = Config[JwtAudienceConfigKey];
                opts.Authority = Config[JwtAuthorityConfigKey];
                opts.IncludeErrorDetails = true;

                if (Env.IsDevelopment())
                    opts.RequireHttpsMetadata = false;
            });

            // RESPONSE CACHING
            services.AddResponseCaching();

            // SIGNALR
            if (Env.IsDevelopment())
            {
                services.AddSignalR(); // Note: SignalR stores group/connection info in-memory (literally a dictionary)
                                       // and as such each connection is tied to a specific server. There is a possibility
                                       // of using distributed Redis instead but requires more work.
                                       // This also means it doesn't matter if you simply store group info yourself in a
                                       // dictionary (MemoryCache), no gain from using a distributed cache (Redis) for this
            }
            else
            {
                services.AddSignalR()
                    .AddRedis(Config.GetConnectionString(SignalRRedisConnectionConfigKey)); // TODO: does this clash?
            }
        }

        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            app.AddUserDetailCookie();

            if (env.IsDevelopment())
            {
                // SWASHBUCKLE
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", SwaggerEndpointName);
                });

                //app.UseSetDevelopmentUser();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCookiePolicy();

            app.UseResponseCaching();

            if (Env.IsProduction())
                app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseSieveExceptionHandler();

            // STILL NEED TO DO THIS IN CHILD CLASS
            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<CommentsHub>("/CommentsHub");
            //});

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = SpaSourcePath;

                // FOR DEVELOPMENT, RUN `npm run serve` FIRST
                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer(ProxySpaAddress);
                }
            });
        }

    }
}
