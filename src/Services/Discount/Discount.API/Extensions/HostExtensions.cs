using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Threading;

namespace Discount.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigratePosgresDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Migrating posgresql database.");

                    using var connection = new NpgsqlConnection(
                            configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();

                    using var command = new NpgsqlCommand
                    {
                        Connection = connection
                    };

                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE Coupon
                    (
                        ID SERIAL PRIMARY KEY,
                        Productname varchar(24)  NOT NULL,
                        Description text,
                        Amount int
                    )";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon(Productname,Description,Amount) VALUES('IPhone X','IPhone Desc',150)";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon(Productname,Description,Amount) VALUES('Samsung 10','Samsung Desc',100)";
                    command.ExecuteNonQuery();

                    logger.LogInformation("Migrated posgresql database.");
                }
                catch (Exception)
                {
                    logger.LogError("Error Migrate posgresql database.");

                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        Thread.Sleep(2000);
                        MigratePosgresDatabase<TContext>(host, retryForAvailability);
                    }
                }
            }

            return host;
        }
    }
}
