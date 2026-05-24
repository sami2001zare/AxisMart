using System.Data;

namespace AxisMart.Application.Shared.Data;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}
