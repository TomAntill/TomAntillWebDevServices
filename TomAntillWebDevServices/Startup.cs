using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TomAntillWebDevServices.BLL;
using TomAntillWebDevServices.BLL.Contracts;
using TomAntillWebDevServices.Data;
using TomAntillWebDevServices.Data.Auth.DataModels;
using TomAntillWebDevServices.Data.Auth.Stores;
using TomAntillWebDevServices.Data.Contracts;
using TomAntillWebDevServices.Models.Commands;
using TomAntillWebDevServices.Models.ViewModels;
using TomAntillWebDevServices.Services;
using TomAntillWebDevServices.Services.Contracts;

namespace TomAntillWebDevServices
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

            services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=TomAntillWebDev.db"));

            services.AddIdentity<User, UserRole>()
                .AddUserStore<UserStore>()
                .AddRoleStore<RoleStore>()
                .AddDefaultTokenProviders();

            services.AddScoped<IBaseDAL<MediaVm, MediaUpdateCommand, MediaAddCommand>, MediaDAL>();
            services.AddScoped<IEmailService, EmailService>();
            //services.AddScoped<IBaseBLL<MediaVm, MediaUpdateCommand, MediaAddCommand>, PictureBLL>();
            services.AddScoped<IMediaBLL, MediaBLL>();

            services.AddScoped<IEmailBLL, EmailBLL>();

            services.AddScoped<IMediaService>(serviceProvider =>
            {
                var mediaBLL = serviceProvider.GetRequiredService<IMediaBLL>();
                return new AzureBlobService(mediaBLL
                    , new BlobServiceClient(Configuration["AzureBlobStorageConnectionString"])
                    , new AppDbContext()
                    );
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TomAntillWebDevServices", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
            policy =>
            {
                policy.WithOrigins(
                        "http://localhost:3000", 
                        "http://192.168.0.11:3000",
                        "https://www.tidyelectrics.com",
                        "http://192.168.0.11:3000/services",
                        "https://www.leahslt.co.uk",
                        "https://tomantillwebdev.uk",
                        "https://www.coatescarpentry.co.uk",
                        "http://127.0.0.1:8080"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });

                options.AddPolicy("AllowAnyOrigin",
                    policy =>
                    {
                        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    });

                
                
            });

            // Add authentication services
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login"; // Customize the login URL
                    options.AccessDeniedPath = "/Auth/AccessDenied"; // Customize the access denied URL
                });

            
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TomAntillWebDevServices v1"));
            }

            //app.UseCors("AllowAnyOrigin");
            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();


            //app.UseExceptionHandlingMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
