using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data;

namespace Discount.API.Extensions
{
    public static class HostExtensions
    {
        /// <summary>
        /// Faz a vez do SeedData (carrega algumas informações iniciais e complementares no banco de dados antes do arranque da API)
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="host"></param>
        /// <param name="retry"></param>
        /// <returns></returns>
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Migrating PostgreSQL database");
                    using (var conn = CreateConnection(configuration))
                    {
                        //Exclui a tabela se existir:
                        var sql = "DROP TABLE IF EXISTS Coupon";
                        conn.Execute(sql);
                        //Cria uma nova tabela:
                        sql = @"CREATE TABLE Coupon (Id SERIAL PRIMARY KEY, ProductName VARCHAR(24) NOT NULL, 
                                    Description TEXT, Amount Double Precision);";
                        conn.Execute(sql);
                        //Insere alguns dados na tabela criada:
                        sql = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('iPhone X','iPhone X Discount', 150);";
                        conn.Execute(sql);
                        sql = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung A10','Samsung A10 Discount', 125);";
                        conn.Execute(sql);

                        logger.LogInformation("Migrated PostgreeSQL database");
                    }
                }
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the PostgreSQL database");
                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, retryForAvailability);
                    }
                }
                return host;
            }
        }
        private static IDbConnection CreateConnection(IConfiguration requiredConfiguration)
        {
            return new NpgsqlConnection(requiredConfiguration.GetValue<string>("DatabaseSettings:ConnectionString"));
        }
    }
}
