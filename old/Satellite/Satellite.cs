using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StarrySkies.Protobuf;
using Google.Protobuf;

using Confluent.Kafka;

namespace Satellite
{
    internal class Satellite
    {
        public int xLocation { get; set; }
        public int yLocation { get; set; }
        public int xVelocity { get; set; }
        public int yVelocity { get; set; }
        public string callsign { get; set; } = "";
        public int upperBoundX { get; set; }
        public int upperBoundY { get; set; }


        private IProducer<Null, byte[]> producer;


        public Satellite(int _upperBoundX, int _upperBoundY)
        {
            Random rand = new Random();

            // Setup Kafka connection
            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = Util.Constants.KAFKA_CONNECTION
            };

            producer = new ProducerBuilder<Null, byte[]>(config).Build();

            // TODO - setup Kafka consumer (once I start reading in msgs from other satellites)

            // Setup everything else
            upperBoundX = _upperBoundX;
            upperBoundY = _upperBoundY;

            xLocation = rand.Next(0, upperBoundX);
            yLocation = rand.Next(0, upperBoundY);

            xVelocity = rand.Next(-1, 2); // -1, 0, or 1
            yVelocity = rand.Next(-1, 2); // -1, 0, or 1

            GenerateCallsign();
        }

        public void Move()
        {
            // TODO - implement collision avoidance (will require reading in other proto msgs)

            int nextX = xLocation + xVelocity;
            int nextY = yLocation + yVelocity;

            if (nextX < 0 || nextX >= upperBoundX)
            {
                xVelocity = -xVelocity;
            }

            if (nextY < 0 || nextY >= upperBoundY)
            {
                yVelocity = -yVelocity;
            }

            xLocation += xVelocity;
            yLocation += yVelocity;

            SendTelemetry();
        }

        private void GenerateCallsign()
        {
            Random rand = new Random();

            // i.e. YC-21
            callsign += 'A' + rand.Next(0, 26);
            callsign += 'A' + rand.Next(0, 26);
            callsign += rand.Next(0, 9).ToString();
            callsign += rand.Next(0, 9).ToString();
        }

        private void SendTelemetry()
        {
            var msg = new Telemetry
            {
                Callsign = callsign,
                XLocation = xLocation,
                YLocation = yLocation,
                XVelocity = xVelocity,
                YVelocity = yVelocity
            };

            producer.ProduceAsync(
                Util.Constants.TELEMETRY_TOPIC,
                new Message<Null, byte[]> { Value = msg.ToByteArray() }
            ).Wait();
        }
    }
}
