using Grpc.Core;
using Grpc.Core.Logging;
using MagicOnion.Client;
using MagicOnion.Service.Definition;
using System;

namespace MagicOnion.Service.Client {
    class Program {
        static void Main(string[] args) {
            GrpcEnvironment.SetLogger(new ConsoleLogger());

            var channel = new Channel("192.168.1.106", 5001, ChannelCredentials.Insecure);
            var service = MagicOnionClient.Create<ICalculateService>(channel);

            var result = service.SumAsync(100, 200).GetAwaiter().GetResult();
            Console.WriteLine($"100+200={result.GetAwaiter().GetResult()}");

            var result1 = service.SumAsync2(200, 500);
            Console.WriteLine($"200+500={result1.GetAwaiter().GetResult()}");

            Console.ReadKey();
        }
    }
}
