using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Ships
{
    internal class Ship : IShip
    {
        private readonly ShipType _type;
        private readonly Size _size;

        public Ship(ShipType type, Size size) {
            _type = type;
            _size = size;
        }

        public ShipType Type => _type;

        public Size Size => _size;
    }
}
