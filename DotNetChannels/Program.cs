using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ChannelsDemo
{
    internal static class Program
    {
        private const int TotalItems = 1000;
        private const int ChannelCapacity = 100;

        private static async Task Main()
        {
            // Bounded: when the consumer falls behind, the producer waits
            // instead of growing memory without limit.
            var channel = Channel.CreateBounded<string>(new BoundedChannelOptions(ChannelCapacity)
            {
                SingleReader = true,
                SingleWriter = true,
                FullMode = BoundedChannelFullMode.Wait,
            });

            var producer = new Producer(channel.Writer);
            var consumer = new Consumer(channel.Reader);

            // Start both, then wait for both. The producer completes the writer
            // when it is done, which is what lets the consumer's loop end.
            Task producing = producer.StartAsync(TotalItems);
            Task<int> consuming = consumer.ConsumeAsync();

            await producing;
            int consumed = await consuming;

            Console.WriteLine($"Produced {TotalItems}, consumed {consumed}.");
        }
    }
}
