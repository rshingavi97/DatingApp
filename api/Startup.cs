using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer; // for JwtBearerDefaults
using Microsoft.IdentityModel.Tokens; // FOR TokenValidationParameters
using api.Data;
using api.Interfaces;
using api.Services;
using System.Text;

namespace api
{
    using api.Middleware;
    using api.Helpers;
    public class Startup
    {
        private readonly IConfiguration _config;
        /* IConfiguration represents appSettings.json file */
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*
            What is the need of using Interface for the creation of service? i.e. why  IServiceCollection is created?
            Creation of Interface is an optional here as without interface, we could create the service as following:       
                          services.AddScoped<TokenService>();
	        There is a benefit of unit testing if we add the interface as we could easily do the unit testing of all methods supported by particular interface.
            that’s why, added the interface below.
            */
            services.AddScoped<ITokenService,TokenService>();
            /*
            There are 3 ways to add the services as following:
            AddSingleton() : once created, it keeps on running as long as application is running which means, such service continously consuming the resources.
            This is not suitable for generation of token as it is one time activity hence not used.
            AddScoped() : this is type of service gets instantiated when HTTP request is received.
            it gets terminated when HTTP request is served or done.
            hence this is very suitable for the issuing of JWT token hence it is used.

            AddTransient() : this type of service lifetime is until the execution of given method.
            It gets terminated once method is finished.
            Hence not useful for us as we are dealing with HTTP request so we need to call multiple methods 
            */
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            //step 16 (start)  Creation of connection string 
            services.AddDbContext<DataContext>(options=>{
                options.UseSqlite(_config.GetConnectionString("DefaultConnection"));
                /* Here, options.UseSqlite() has 3 overloaded methods.
                Out of 3, we need to select the one which passing the connection string.
                Here. DefaultConnection represents the name of database which is defined in the 
                appsettings.Development.json file*/
            });

            //step 16 (end)  Creation of connection string 
            services.AddControllers();
            services.AddCors();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options=>{
                    options.TokenValidationParameters=new TokenValidationParameters{
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"])),
                        /*Our main concern is the authentication of user, above two flags are doing that task.
                        we are additionally adding below two parameters.
                        ValidateIssuer means our Api project which issues the token
                        ValidateAudience , is the Angular client application
                        */
                        ValidateIssuer = false, 
                        ValidateAudience = false,

                    };

                }
            );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {   
            
           /*if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage(); 
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api v1"));
            }*/
            //Above framework supported Exception handling is commented as going to use manually created ExceptionMiddleware.
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(policy=>policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
