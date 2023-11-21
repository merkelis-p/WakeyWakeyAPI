using System;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WakeyWakeyAPI", Version = "v1" });
            });

            
            services.AddControllers();

            services.AddDbContextPool<wakeyContext>(options => options
                .UseMySql(Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 21)))
            );
            

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
                });


                        
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(GenericController<,>), typeof(GenericController<,>));
            services.AddScoped(typeof(UserRepository), typeof(UserRepository));
            services.AddScoped(typeof(UserController), typeof(UserController));
            services.AddScoped(typeof(EventRepository), typeof(EventRepository));
            services.AddScoped(typeof(EventController), typeof(EventController));
            services.AddScoped(typeof(ReminderRepository), typeof(ReminderRepository));
            services.AddScoped(typeof(ReminderController), typeof(ReminderController));
            services.AddScoped(typeof(CourseRepository), typeof(CourseRepository));
            services.AddScoped(typeof(CourseController), typeof(CourseController));
            services.AddScoped(typeof(SubjectRepository), typeof(SubjectRepository));
            services.AddScoped(typeof(SubjectController), typeof(SubjectController));
            services.AddScoped(typeof(TaskRepository), typeof(TaskRepository));
            services.AddScoped(typeof(TaskController), typeof(TaskController));
            services.AddScoped(typeof(RecordRepository), typeof(RecordRepository));
            services.AddScoped(typeof(RecordController), typeof(RecordController));

    
            //services.AddScoped<UserController>();
            // services.AddScoped<EventController>();
            // services.AddScoped<ReminderController>();
            // services.AddScoped<CourseController>();
            // services.AddScoped<SubjectController>();
            // services.AddScoped<TaskController>();
            // services.AddScoped<RecordController>();


            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://84.15.187.34")
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });

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