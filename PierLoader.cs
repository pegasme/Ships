using System;
using System.Threading;
using System.Threading.Tasks;
using Test.Ships;
using Test.Tunnels;

namespace Test
{
    internal class PierLoader
    {
        const int millisecondsInSecond = 1000;
        const int loadingSizePerSecond = 10;

        private readonly ShipType _type;
        private readonly Tunnel _tunnel;
        private bool isBusy = false;
        public PierLoader(ShipType type, Tunnel tunnel) {
            _type = type;
            _tunnel = tunnel;
        }

        public ShipType Type => _type;

        public async Task Load(Ship ship) {
            if (ship.Type != _type)
                throw new ArgumentException("Pier and ship has different type");

            var loadingTime = (int)ship.Size % loadingSizePerSecond; // loading time in s
            await Task.Delay(loadingTime * millisecondsInSecond); 
        }

        public void Work() {
            Task.Run(async () =>
            {
                while (true)
                {
                    //Console.WriteLine($"Try GetShip {_type}.");
                    var ship = await _tunnel.GetShipAsync(_type);
                    if (ship != null)
                    {
                        await Load(ship);
                        Console.WriteLine($"Load {ship.Type} finished.");
                    }
                }
            });
        }
    }
}
