using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Pastebook.Data.Data;
using Pastebook.Data.Repositories;
using Pastebook.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pastebook.Web
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

            services.AddControllers();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pastebook", Version = "v1" });
            });
            string connectionString = Configuration.GetValue<string>("ConnectionString");
            services.AddDbContext<PastebookContext>(options => options.UseSqlServer(connectionString));
            
            services.AddScoped<IUserAccountRepository, UserAccountRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IAlbumRepository, AlbumRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IFriendRepository, FriendRepository>();

            services.AddScoped<IUserAccountService, UserAccountService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IFriendService, FriendService>();

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pastebook.Web v1"));
            }
            app.UseCors(options => 
            options.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=UserAccount}/{action=Index}/{id?}");
            });
        }
    }
}
