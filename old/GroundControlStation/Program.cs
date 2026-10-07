
using Util.Consumers;
using Confluent.Kafka;

namespace GroundControlStation
{
    internal class Program
    {
        private static volatile bool _keepRunning = true;

        static void Main(string[] args)
        {
            Console.WriteLine("GCS is coming online. Press Ctrl+C or kill docker container to initiate shutdown.");

            int kafkaTimeout = 15000;
            Console.WriteLine($"Sleeping for {kafkaTimeout}ms to allow Kafka connection to establish...");
            Thread.Sleep(kafkaTimeout);

            waitForKafka();

            Console.WriteLine("GCS is now online.");

            TelemetryConsumer telemetryConsumer = new TelemetryConsumer();

            GroundControlStation gcs = new GroundControlStation();

            telemetryConsumer.HandleMessage += gcs.onTelemetryReceived;


            Console.CancelKeyPress += (sender, e) =>
            {
                Console.WriteLine("GCS received Kill signal -- Initiating graceful shutdown...");
                e.Cancel = true; // Prevent immediate termination
                _keepRunning = false;
            };


            

            while (_keepRunning)
            {
                
                // if needed, call an update method on the GCS

                // 2 Hz
                Thread.Sleep(500);
            }

            /* Teardown */

            Console.WriteLine("GCS finished terminating.");

            telemetryConsumer.Dispose();
        }

        private static void waitForKafka()
        {
            var config = new AdminClientConfig
            {
                BootstrapServers = Util.Constants.KAFKA_CONNECTION,
                // Optional: a short timeout for quick retries
                SocketTimeoutMs = 1000
            };

            using var adminClient = new AdminClientBuilder(config).Build();

            const int maxRetries = 10;
            int attempt = 0;
            bool connected = false;

            while (attempt < maxRetries && !connected)
            {
                attempt++;
                try
                {
                    Console.WriteLine($"Attempt {attempt}: Checking connection to Kafka broker at '{Util.Constants.KAFKA_CONNECTION}'...");

                    // Attempt to fetch metadata from Kafka
                    var meta = adminClient.GetMetadata(TimeSpan.FromSeconds(1));
                    Console.WriteLine($"Connected to Kafka cluster '{meta.OriginatingBrokerId}'");
                    connected = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Attempt {attempt}: Kafka not ready yet. {ex.Message}");
                    Thread.Sleep(1000); // Wait 1 second before retry
                }
            }

            if (!connected)
            {
                Console.WriteLine("Failed to connect to Kafka after multiple attempts.");
                return;
            }

        }

    }
}
