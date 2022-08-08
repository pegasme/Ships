using System;
using Test;
using Test.Ships;
using Test.Tunnels;

class MainClass
{
    static void Main()
    {
        var generator = new ShipGenerator(1000, 5);
        generator.Start();

        var tunnel = new Tunnel(generator);
        var oilLoader = new PierLoader(ShipType.Oil, tunnel);
        var stuffLoader = new PierLoader(ShipType.Stuff, tunnel);
        var foodLoader = new PierLoader(ShipType.Food, tunnel);

        tunnel.Start();

        oilLoader.Work();
        stuffLoader.Work();
        foodLoader.Work();

        Console.ReadLine();
    }

}