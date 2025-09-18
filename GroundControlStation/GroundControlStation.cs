using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StarrySkies.Protobuf;
using Google.Protobuf;

using Confluent.Kafka;
using Util.Consumers;

namespace GroundControlStation
{
    internal class GroundControlStation
    {


        public GroundControlStation()
        {
            




        }

        public void onTelemetryReceived(object? sender, TelemetryConsumer.TelemetryEventArgs e)
        {
            Telemetry msg = e.Message;

            Console.WriteLine($"Received telemetry from {msg.Callsign}: Location=({msg.XLocation}, {msg.YLocation})");
        }
    }
}
