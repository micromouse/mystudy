using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundTaskService {
    /// <summary>
    /// 日志服务
    /// </summary>
    public class LoggingService : BackgroundService {
        private readonly ILogger<LoggingService> _logger;

        /// <summary>
        /// 初始化日志服务
        /// </summary>
        /// <param name="logger">日志器</param>
        public LoggingService(ILogger<LoggingService> logger) {
            _logger = logger;
        }

        /// <summary>
        /// 执行日志服务
        /// </summary>
        /// <param name="stoppingToken">CancellationToken</param>
        /// <returns>任务</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                _logger.LogInformation($"正在执行日志服务:{DateTime.Now}");
                await Task.Delay(1000 * 10);
            }
        }
    }
}
