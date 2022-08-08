using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Test.Ships;

namespace Test.Tunnels
{
    internal class Tunnel
    {
        private const int maxCount = 5;

        private ConcurrentBag<Ship> _currentShips;

        private readonly ShipGenerator _generator;
        private static SemaphoreSlim _pool;

        public Tunnel(ShipGenerator generator) {
            _generator = generator;
            _pool = new SemaphoreSlim(3, 3);
            _currentShips = new ConcurrentBag<Ship>();
        }

        public void Start()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        if (_currentShips.Count < maxCount)
                        {
                            var ship = _generator.GetShip();
                            if (ship != null)
                            {
                                Console.WriteLine($"Tunnel: Ship type = {ship.Type} and size = {ship.Size} ThreadId: {Thread.CurrentThread.ManagedThreadId}.");
                                _currentShips.Add(ship);
                            }
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine("Error", e.Message);
                    }
                }
            });
        }

        public Task<Ship> GetShipAsync(ShipType type) {
            return GetShipFromList(type);
        }

        private async Task<Ship> GetShipFromList(ShipType type) {
            try
            {
                await _pool.WaitAsync();

                if (!_currentShips.TryTake(out var ship))
                    return null;

                if (ship.Type == type) 
                { 
                    return ship; 
                }
                else
                {
                    _currentShips.Add(ship);
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error", e.Message);
                return null;
            }
            finally
            {
                _pool.Release();
            }
        }
    }
}
