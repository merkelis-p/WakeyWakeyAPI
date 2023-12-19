using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using Microsoft.Extensions.Logging;

namespace WakeyWakeyAPI.Interceptors
{
    public class MyDbCommandInterceptor : DbCommandInterceptor
    {
        private readonly ILogger<MyDbCommandInterceptor> _logger;

        public MyDbCommandInterceptor(ILogger<MyDbCommandInterceptor> logger)
        {
            _logger = logger;
        }

        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command, 
            CommandEventData eventData, 
            InterceptionResult<DbDataReader> result)
        {
            _logger.LogInformation("Executing command: {CommandText}", command.CommandText);

            // Optionally log parameters
            foreach (DbParameter param in command.Parameters)
            {
                _logger.LogInformation("Param {Name}: {Value}", param.ParameterName, param.Value);
            }

            return base.ReaderExecuting(command, eventData, result);
        }
    }
}
