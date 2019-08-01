using Microsoft.Extensions.Hosting;
using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundTaskService {
    /// <summary>
    /// 服务生命周期
    /// </summary>
    public class ServiceBaseLifetime : ServiceBase, IHostLifetime {
        private readonly IApplicationLifetime _applicationLifetime;
        private readonly TaskCompletionSource<object> _delayStart;
        /// <summary>
        /// 初始化服务生命周期
        /// </summary>
        /// <param name="applicationLifetime">Allows consumers to perform cleanup during a graceful shutdown.</param>
        public ServiceBaseLifetime(IApplicationLifetime applicationLifetime) {
            _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
            _delayStart = new TaskCompletionSource<object>();
        }

        /// <summary>
        /// Called at the start of StartAsync(CancellationToken) which will wait until it's complete before continuing. 
        /// This can be used to delay startup until signaled by an external event.
        /// </summary>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>任务</returns>
        public Task WaitForStartAsync(CancellationToken cancellationToken) {
            cancellationToken.Register(() => _delayStart.TrySetCanceled());
            _applicationLifetime.ApplicationStopping.Register(Stop);

            // Otherwise this would block and prevent IHost.StartAsync from finishing.
            new Thread(Run).Start();
            return _delayStart.Task;
        }

        /// <summary>
        /// 允许服务,设置异常
        /// </summary>
        private void Run() {
            try {
                Run(this);
                _delayStart.TrySetException(new InvalidOperationException("Stopped without starting"));
            } catch (Exception ex) {
                _delayStart.TrySetException(ex);
            }
        }

        /// <summary>
        /// Attempts to gracefully stop the program.
        /// </summary>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>任务</returns>
        public Task StopAsync(CancellationToken cancellationToken) {
            Stop();
            return _delayStart.Task;
        }

        /// <summary>
        /// Called by base.Run when the service is ready to start.
        /// </summary>
        /// <param name="args">参数</param>
        protected override void OnStart(string[] args) {
            _delayStart.TrySetResult(null);
            base.OnStart(args);
        }

        /// <summary>
        /// Called by base.Stop. This may be called multiple times by service Stop, ApplicationStopping, and StopAsync.
        /// That's OK because StopApplication uses a CancellationTokenSource and prevents any recursion.
        /// </summary>
        protected override void OnStop() {
            _applicationLifetime.StopApplication();
            base.OnStop();
        }
    }
}
