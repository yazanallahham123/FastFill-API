using AutoMapper;
using FastFill_API.Data;
using FastFill_API.Interfaces;
using FastFill_API.Model;
using FastFill_API.Repositories;
using FastFill_API.Web.Profile;
using FastFill_API.Web.Services;
using FastFill_API.Web.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFill_API
{
    public class Startup
    {
       public static readonly Microsoft.Extensions.Logging.LoggerFactory _myLoggerFactory =
       new LoggerFactory(new[]
       {
                new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
       });

        public static IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile(Directory.GetCurrentDirectory() + "/Properties/launchSettings.json", optional: true,
                    reloadOnChange: true)
                .AddJsonFile(Directory.GetCurrentDirectory() + "/appsettings.json", optional: true,
                    reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy(name: "_myAllowSpecificOrigins", builder =>
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            // for routing
            services.AddControllers();

            // for connection of DB
            services.AddDbContext<FastFillDBContext>(
                options =>
                {
                    options.UseLoggerFactory(_myLoggerFactory);
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FastFill_API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

            // Inject the public repository
            services.AddScoped<IPublicRepository, PublicRepository>();
            // Inject services
            services.AddScoped<UserServices>();
            services.AddScoped<SecurityServices>();

            // Mapper setting
            IMapper mapper =
                new MapperConfiguration(config => { config.AddProfile<AutoMapperProfile>(); }).CreateMapper();
            services.AddSingleton(mapper);



            // for JWT config
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Startup.Configuration.GetSection("Jwt")["SecretKey"].ToString())),
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = (context) =>
                        {
                            // this is a default method
                            // the response statusCode and headers are set here
                            // Call this to skip the default logic and avoid using the default response
                            context.HandleResponse();

                            if (context.AuthenticateFailure != null)
                            {
                                context.Response.StatusCode = 401;

                                // we can write our own custom response content here
                                return context.HttpContext.Response.WriteAsync(
                                    "Token Validation Has Failed. Request Access Denied");
                            }
                            else
                            {

                                // Write to the response in any way you wish
                                context.Response.StatusCode = 401;
                                var body = Encoding.UTF8.GetBytes("You are not authorized!");

                                context.Response.Headers.Append("my-custom-header", "custom-value");
                                context.Response.Body.WriteAsync(body, 0, body.Length);
                                return Task.CompletedTask;
                                // AuthenticateFailure property contains 
                                // the details about why the authentication has failed

                            }
                        }
                    };
                });


            services.AddAuthorization(config =>
            {
                config.AddPolicy(Policies.Admin, Policies.AdminPolicy());
                config.AddPolicy(Policies.Support, Policies.SupportPolicy());
                config.AddPolicy(Policies.User, Policies.UserPolicy());
            });

            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FastFill_API v1"));
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<FastFillDBContext>();
                //context.Database.Migrate();
            }

            app.UseStaticFiles(); // For the wwwroot folder.

            // using Microsoft.Extensions.FileProviders;
            // using System.IO;
            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "Attachments")),
                RequestPath = "/Attachments",
                EnableDirectoryBrowsing = true
            });

            // for JWT config
            app.UseAuthentication();

            app.UseRouting();
            app.UseCors("_myAllowSpecificOrigins");
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            await InitializeDatabase.Run(app);
        }
    }
}
