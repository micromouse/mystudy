using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DXQ.Study.Mediator.MediatorConsole.Application.Notifications {
    /// <summary>
    /// 我的打招呼事件处理器
    /// </summary>
    public class MyHelloeventHandler : INotificationHandler<HelloNotificationEvent> {
        /// <summary>
        /// 处理打招呼事件
        /// </summary>
        /// <param name="notification">打招呼事件信息</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>任务</returns>
        public Task Handle(HelloNotificationEvent notification, CancellationToken cancellationToken) {
            Console.WriteLine($"我的打招呼,提醒时间:{notification.HelloTime},提醒内容:{notification.HelloContent}");
            return Task.CompletedTask;
        }
    }
}
