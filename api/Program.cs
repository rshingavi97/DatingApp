using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection; //add for CreateScope()
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore; // for MigrateAsync() method
using Microsoft.Extensions.Logging; // for ILogger interface

namespace api
{
    using api.Data; // for DataContext service
    public class Program
    {
        /*public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }*/
        /* For fake data insertion mechanism, we need to change the implementation of above Main method as following*/
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            //Get the DataContext service so that we could call Seed::SeedUsers() to insert the data
            // for service, we need to create the Scope in which particular service is going to call
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
     /* Since Main() is outside the Middleware hence we need to provide the exception handling explicitly here to
     handle the error generated during insertion of Data inside DB.*/
            try{
                //now fetch the service
                var context = services.GetRequiredService<DataContext>();
                await context.Database.MigrateAsync(); //this is nothing but calling the command "dotnet ef database update" manually
    /* MigrateAsync() perform any pending migration for the context to the database. It also create the database if its not existed.
       Above method is called for convenience as we dont need to provide the command "dotnet ef database update"
       Now onwards, we will just restart the applicaiton to perform the migration of any database changes. */
                //now insert the data by calling Seed class method.
                await Seed.SeedUsers(context);
            }
            catch(Exception ex)
            {
                /* for logging the error, fetch the ILogger service for current class i.e. Program*/
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred during migration");
            }
            /* Run() is most important method to start the application.
            Once data is inserted into DB then we need to call this method.
            Since Main() is now ASYNC hence we need to call the ASYNC version of Run method.*/
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
