using System;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Controllers;
using WakeyWakeyAPI.Repositories;
using WakeyWakeyAPI.Middleware;
using WakeyWakeyAPI.Interceptors;




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
            services.AddScoped<MyDbCommandInterceptor>();

            services.AddSingleton<MyDbCommandInterceptor>();

            services.AddDbContext<wakeyContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 21)));
                var interceptor = services.BuildServiceProvider().GetRequiredService<MyDbCommandInterceptor>();
                options.AddInterceptors(interceptor);
            }); 




            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
                });

                        
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            
            services.AddScoped<IGenericController<Task>, TaskController>();
            services.AddScoped<ITaskController, TaskController>();
            
            services.AddScoped<IUserController, UserController>();
            services.AddScoped<IGenericController<User>, UserController>();


            services.AddScoped<ISubjectController, SubjectController>();
            services.AddScoped<IGenericController<Subject>, SubjectController>();
            
            services.AddScoped<ICourseController, CourseController>();
            services.AddScoped<IGenericController<Course>, CourseController>();
            
            
            services.AddScoped<IReminderController, ReminderController>();
            services.AddScoped<IGenericController<Reminder>, ReminderController>();
            
            services.AddScoped<IEventController, EventController>();
            services.AddScoped<IGenericController<Event>, EventController>();
            
            services.AddScoped<IRecordController, RecordController>();
            services.AddScoped<IGenericController<Record>, RecordController>();


            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IRepository<Models.Task>, TaskRepository>();
            
            
            
            services.AddScoped(typeof(UserRepository), typeof(UserRepository));
            services.AddScoped(typeof(EventRepository), typeof(EventRepository));
            services.AddScoped(typeof(ReminderRepository), typeof(ReminderRepository));
            services.AddScoped(typeof(CourseRepository), typeof(CourseRepository));
            services.AddScoped(typeof(SubjectRepository), typeof(SubjectRepository));
            services.AddScoped(typeof(TaskRepository), typeof(TaskRepository));
            services.AddScoped(typeof(RecordRepository), typeof(RecordRepository));

    
        


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
            app.UseMiddleware<LoggingMiddleware>();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}