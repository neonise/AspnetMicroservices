using Dapper;
using Discount.Grpc.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly NpgsqlConnection connection;

        public DiscountRepository(IConfiguration configuration)
        {
            connection = new NpgsqlConnection(
                configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            var affected = await connection.ExecuteAsync(
                "INSERT INTO Coupon(ProductName,Description,Amount)  VALUES(@ProductName,@Description,@Amount)",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            return affected > 0;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            var affected = await connection.ExecuteAsync(
                "Delete FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName});

            return affected > 0;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
                "SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

            if (coupon == null)
                return new Coupon
                {
                    ProductName = productName,
                    Amount = 0,
                    Description = "No Discount Desc"
                };

            return coupon;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            var affected = await connection.ExecuteAsync(
                "Update Coupon set ProductName=@ProductName,Description=@Description,Amount=@Amount WHERE Id = @Id",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

            return affected > 0;
        }
    }
}
