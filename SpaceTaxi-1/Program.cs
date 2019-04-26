using System;

namespace SpaceTaxi_1 {
    internal class Program {
        public static void Main(string[] args) {
            ///Test.Method(); /// test af file stream reader class skabelon
            var game = new Game();
            Level testLevel = game.levelParser.CreateLevel("the-beach.txt");
            Level testLevel1 = game.levelParser.CreateLevel("short-n-sweet.txt");
            Console.WriteLine(testLevel.map[0]);
            Console.WriteLine(testLevel.mapName);
            Console.WriteLine(testLevel1.mapName);
            ///game.GameLoop();
        }
    }
}