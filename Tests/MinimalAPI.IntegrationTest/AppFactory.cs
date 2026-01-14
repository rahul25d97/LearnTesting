using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MinimalAPI.Models;

namespace MinimalAPI.IntegrationTest
{
    public class AppFactory:WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //builder.ConfigureTestServices(services => {
            //    services.RemoveAll(typeof(DbContextOptions<DatabaseContext>));
            //    services.AddDbContext<DatabaseContext>(options =>
            //    {
            //        options.UseInMemoryDatabase("BiappsDB");
            //    });
            //});
            //

            //base.ConfigureWebHost(builder);

            builder.ConfigureServices(services =>
            {
                // Remove the existing DbContext registration
                services.RemoveAll<DbContextOptions<DatabaseContext>>();
                services.RemoveAll<DatabaseContext>();

                // Add a new DbContext registration for the test database
                services.AddDbContext<DatabaseContext>(options =>
                {
                    // Use an in-memory database for testing
                    //options.UseInMemoryDatabase("InMemoryDbForTesting");
                    // Or, use a dedicated test database (e.g., SQL Server LocalDB)
                     options.UseSqlServer("Data Source=199.97.26.22; Initial Catalog=BiappsDB_Test; Persist Security Info=True; User ID=Dhritee; Password=m3E_0e8;TrustServerCertificate=True;");
                });

                // Build the service provider and apply migrations if needed
                //var sp = services.BuildServiceProvider();
                //using (var scope = sp.CreateScope())
                //{
                //    var scopedServices = scope.ServiceProvider;
                //    var db = scopedServices.GetRequiredService<DatabaseContext>();
                //    db.Database.EnsureCreated(); // Or db.Database.Migrate(); for migrations
                //}
            });

        }

    }
}
