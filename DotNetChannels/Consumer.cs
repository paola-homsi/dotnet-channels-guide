using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ChannelsDemo
{
    public sealed class Consumer
    {
        private readonly ChannelReader<string> _reader;

        public Consumer(ChannelReader<string> reader) => _reader = reader;

        /// <summary>Reads until the writer completes the channel. Returns the number of items read.</summary>
        public async Task<int> ConsumeAsync(CancellationToken cancellationToken = default)
        {
            int count = 0;
            await foreach (string msg in _reader.ReadAllAsync(cancellationToken))
            {
                Console.WriteLine(msg);
                count++;
            }

            return count;
        }
    }
}
