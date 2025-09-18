using System;

using StarrySkies.Protobuf;

namespace MyNamespace
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = new Telemetry
            {
                Callsign = "AGENT-007",
                XLocation = 0,
                YLocation = 1,
                XVelocity = 2,
                YVelocity = 3
            };

            Console.WriteLine($"Telemetry: {data.Callsign}, {data.XLocation}");
        }
    }
}