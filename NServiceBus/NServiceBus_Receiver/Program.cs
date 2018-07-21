using System;
using System.Threading.Tasks;
using NServiceBus;

namespace NServiceBus_Receiver {
    /// <summary>
    /// Program
    /// </summary>
    class Program {
        /// <summary>
        /// 入口
        /// </summary>
        /// <param name="args">参数</param>
        static void Main(string[] args) {
            Console.Title = "NServiceBus.Receiver";

            var endpointConfiguration = new EndpointConfiguration("NServiceBus.Receiver");
            endpointConfiguration.UsePersistence<LearningPersistence>();
            endpointConfiguration.UseTransport<LearningTransport>();

            var endpointInstance = Endpoint.Start(endpointConfiguration).ConfigureAwait(false).GetAwaiter().GetResult();
            Console.WriteLine("press any key to exit");
            Console.ReadKey();
            endpointInstance.Stop().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
