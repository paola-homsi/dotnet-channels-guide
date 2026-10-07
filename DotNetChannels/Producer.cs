using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ChannelsDemo
{
    public sealed class Producer
    {
        private readonly ChannelWriter<string> _writer;

        public Producer(ChannelWriter<string> writer) => _writer = writer;

        public async Task StartAsync(int count, TimeSpan? delay = null)
        {
            try
            {
                for (int i = 0; i < count; i++)
                {
                    if (delay is { } d)
                    {
                        await Task.Delay(d);
                    }

                    // Waits while a bounded channel is full.
                    await _writer.WriteAsync(i.ToString());
                }

                _writer.Complete();
            }
            catch (Exception ex)
            {
                // Pass the failure on so the consumer does not wait forever.
                _writer.Complete(ex);
                throw;
            }
        }
    }
}
