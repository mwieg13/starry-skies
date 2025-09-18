using System;

namespace Satellite
{
    internal class Program
    {
        private static volatile bool _keepRunning = true;

        static void Main(string[] args)
        {
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
    }
}