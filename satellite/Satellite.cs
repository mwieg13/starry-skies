using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satellite
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


        // data members:
        // x location
        // y location
        // x velocity
        // y velocity
        // callsign
        // upper bound x
        // upper bound y

        public Satellite(int _upperBoundX, int _upperBoundY)
        {
            upperBoundX = _upperBoundX;
            upperBoundY = _upperBoundY;

            // constructor
            Random rand = new Random();

            xLocation = rand.Next(0, upperBoundX);
            yLocation = rand.Next(0, upperBoundY);

            xVelocity = rand.Next(-1, 2); // -1, 0, or 1
            yVelocity = rand.Next(-1, 2); // -1, 0, or 1

            GenerateCallsign();
        }

        public void Move()
        {
            // TODO - implement collision avoidance

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

            // TODO - send telemetry data
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


    }
}
