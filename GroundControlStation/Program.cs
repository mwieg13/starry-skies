
using Util.Consumers;

namespace GroundControlStation
{
    internal class Program
    {
        private static volatile bool _keepRunning = true;

        static void Main(string[] args)
        {
            TelemetryConsumer telemetryConsumer = new TelemetryConsumer();

            GroundControlStation gcs = new GroundControlStation();

            telemetryConsumer.HandleMessage += gcs.onTelemetryReceived;


            Console.CancelKeyPress += (sender, e) =>
            {
                Console.WriteLine("GCS received Kill signal -- Initiating graceful shutdown...");
                e.Cancel = true; // Prevent immediate termination
                _keepRunning = false;
            };


            Console.WriteLine("GCS is coming online. Press Ctrl+C or kill docker container to initiate shutdown.");

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
    }
}
