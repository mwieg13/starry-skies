using System;
using Confluent.Kafka;

namespace Satellite
{
    internal class Program
    {
        private static volatile bool _keepRunning = true;

        static void Main(string[] args)
        {
            waitForKafka();

            // TODO - make upper bounds configurable via args
            Satellite satellite = new Satellite(100, 100);


            Console.CancelKeyPress += (sender, e) =>
            {
                Console.WriteLine("Satellite received Kill signal -- Initiating graceful shutdown...");
                e.Cancel = true; // Prevent immediate termination
                _keepRunning = false;
            };


            Console.WriteLine("Satellite is deploying. Press Ctrl+C or kill docker container to initiate shutdown.");

            while (_keepRunning)
            {
                satellite.Move();

                // 2 Hz
                Thread.Sleep(500);
            }

            Console.WriteLine("Satellite finished terminating.");
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