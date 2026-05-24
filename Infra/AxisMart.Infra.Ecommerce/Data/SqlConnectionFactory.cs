using AxisMart.Application.Shared.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AxisMart.Infra.Ecommerce.Data;

internal sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));
        }
        _connectionString = connectionString;
        Console.WriteLine($"SqlConnectionFactory initialized with connection string: {_connectionString}");
    }

    public IDbConnection CreateConnection()
    {
        try
        {
            var connection = new SqlConnection(_connectionString);
            Console.WriteLine("Attempting to open SQL Server connection...");

            connection.Open();
            Console.WriteLine($"Connection opened successfully. ServerVersion: {connection.ServerVersion}");

            return connection;
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Failed to open connection: {ex.Message} (Error Number: {ex.Number})");
            throw;
        }
    }
}
