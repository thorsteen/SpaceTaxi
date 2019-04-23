using System;
using System.Collections.Generic;
using DIKUArcade;
using DIKUArcade.Math;

namespace SpaceTaxi_1 {
    public class Level {
        
        public char[,] map; //an array with the characters representing level tiles
        public string mapName;
        public int platforms; //amount of platforms in the level
        public Dictionary<char, string> keyLegend; //legend of which files characters represent
        public List<Customer> customers;

        public Level(char[,] Map, string MapName, int Platforms, Dictionary<char, string> KeyLegend,
            List<Customer> Customers) {
            map = Map;
            mapName = MapName;
            platforms = Platforms;
            keyLegend = KeyLegend;
            customers = Customers;
        }
    }
}