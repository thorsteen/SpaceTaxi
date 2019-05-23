using System.Collections.Generic;

namespace SpaceTaxi_2 {
    public class Level {
        
        public string[] map; //an array with the characters representing level tiles
        public string mapName;
        public List<char> platforms; //amount of platforms in the level
        public Dictionary<char, string> keyLegend; //legend of which files characters represent
        public List<Customer> customers;

        public Level(string[] Map, string MapName, List<char> Platforms, Dictionary<char, string> KeyLegend,
            List<Customer> Customers) {
            map = Map;
            mapName = MapName;
            platforms = Platforms;
            keyLegend = KeyLegend;
        }
    }
}