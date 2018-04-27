using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TravixTest.DataAccess;
using TravixTest.Logic;
using TravixTest.Logic.Contracts;

namespace TravixTest.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {            
            services.AddDbContext<PostsCommentsContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICommentsRepository, CommentsRepository>(resolver =>
            {
                var options = resolver.GetService<DbContextOptions<PostsCommentsContext>>();

                return new CommentsRepository(options);
            });

            services.AddScoped<IPostsRepository, PostsRepository>(resolver =>
            {
                var options = resolver.GetService<DbContextOptions<PostsCommentsContext>>();

                return new PostsRepository(options);
            });

            services.AddScoped<ICommentsService, CommentsService>(resolver =>
            {
                var commentsRepository = resolver.GetService<ICommentsRepository>();
                var postsRepository = resolver.GetService<IPostsRepository>();

                return new CommentsService(commentsRepository, postsRepository);
            });

            services.AddScoped<IPostsService, PostsService>(resolver =>
            {
                var postsRepository = resolver.GetService<IPostsRepository>();

                return new PostsService(postsRepository);
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
