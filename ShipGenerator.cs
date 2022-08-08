using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Test.Ships;

namespace Test
{
    internal class ShipGenerator
    {
        private ConcurrentBag<Ship> Ships = new ConcurrentBag<Ship>();
        private readonly int _maxCount;
        private readonly int _maxThreads;

        public ShipGenerator(int maxCount, int maxThreads) {
            _maxCount = maxCount;
            _maxThreads = maxThreads;
        }

        public void Start()
        {
            int i = 0;
            Task.Run(async () =>
            {
                while (i < _maxCount)
                {
                    i++; //@TODO should be thread safe!!!!!
                    var result = await GenerateAsync();
                    Ships.Add(result);
                }
            });
        }

        public Ship? GetShip() {
            Ship ship;
            var result = Ships.TryTake(out ship);
            if (result)
                return ship;
            return null;
        }

        public bool IsEmpty() => Ships.Count == 0;

        private async Task<Ship> GenerateAsync() {
            var size = GenerateSize();
            var type = GenerateType();

            var newShip = new Ship(type, size);
            await Task.Delay(150); //let's imagine that we create ship very long time.
            Console.WriteLine($"Created: Ship type = {type} and size = {size} ({(int)size}) ThreadId: {Thread.CurrentThread.ManagedThreadId}.");
            return newShip;
        }

        private ShipType GenerateType() {
            Random rnd = new Random();
            var values = Enum.GetValues(typeof(ShipType));
            return (ShipType)values.GetValue(rnd.Next(values.Length));
        }

        private Size GenerateSize()
        {
            Random rnd = new Random();
            var values = Enum.GetValues(typeof(Size));
            return (Size)values.GetValue(rnd.Next(values.Length));
        }
    }
}
