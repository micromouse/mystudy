using Autofac;
using DXQ.Study.Mediator.MediatorConsole.Infrastructer;
using MediatR;
using System;

namespace DXQ.Study.Mediator.MediatorConsole {
    /// <summary>
    /// 应用程序入口
    /// </summary>
    class Program {
        /// <summary>
        /// 入口
        /// </summary>
        /// <param name="args">参数</param>
        static void Main(string[] args) {
            var container = new ContainerBuilder();
            container.RegisterModule(new MediatorModule());
            container.RegisterType<MessageSender>();
            var icontainer = container.Build();

            icontainer.Resolve<MessageSender>().Send("this is a test message");
            Console.Read();
        }
    }
}
