using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StarrySkies.Protobuf;
using Google.Protobuf;

using Confluent.Kafka;
using System.Threading;

namespace Util.Consumers
{
    public class TelemetryConsumer : IDisposable
    {
        public class TelemetryEventArgs : EventArgs
        {
            public Telemetry Message { get; private set; }
            public TelemetryEventArgs(Telemetry msg)
            {
                Message = msg;
            }
        }

        public event EventHandler<TelemetryEventArgs> HandleMessage;

        private IConsumer<Ignore, byte[]> consumer;


        private CancellationTokenSource cts = new CancellationTokenSource();

        private Task backgroundThread;

        private bool stopped = true;

        public void Dispose()
        {
            consumer.Dispose();

            if (!stopped)
            {
                Stop();
            }
        }

        public TelemetryConsumer()
        {
            // Setup Kafka connection
            ConsumerConfig config = new ConsumerConfig
            {
                // unique group id so each consumer gets all messages
                GroupId = Guid.NewGuid().ToString(),
                BootstrapServers = Util.Constants.KAFKA_CONNECTION,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            consumer = new ConsumerBuilder<Ignore, byte[]>(config).Build();
            consumer.Subscribe(Util.Constants.TELEMETRY_TOPIC);
        }

        public void Start()
        {
            // Begin polling for messages until cancelled
            Task.Run(() =>
            {
                stopped = false;

                try
                {
                    while (!cts.Token.IsCancellationRequested)
                    {
                        // Poll for messages
                        var result = consumer.Consume(cts.Token);
                        if (result != null)
                        {
                            // Fire event
                            Telemetry msg = Telemetry.Parser.ParseFrom(result.Message.Value);
                            TelemetryEventArgs args = new TelemetryEventArgs(msg);
                            HandleMessage?.Invoke(this, args);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Graceful shutdown
                }
                finally
                {
                    consumer.Close();
                    stopped = true;
                }
            });
        }

        public void Stop()
        {
            cts.Cancel();

            backgroundThread.Wait(500);
        }
    }
}
