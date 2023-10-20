using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Controllers;
using WakeyWakeyAPI.Repositories;

namespace WakeyWakeyAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WakeyWakeyAPI", Version = "v1" });
            });



            services.AddDbContextPool<wakeyContext>(options => options
                .UseMySql(Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 21)))
            );

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://84.15.187.34")
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });
            
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<UserController>();
            services.AddScoped<EventController>();
            services.AddScoped<ReminderController>();
            services.AddScoped<CourseController>();
            services.AddScoped<SubjectController>();
            services.AddScoped<TaskController>();
            services.AddScoped<RecordController>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WakeyWakeyAPI v1"));
            }

            // app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}