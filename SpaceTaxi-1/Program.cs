using System;

namespace SpaceTaxi_1 {
    internal class Program {
        public static void Main(string[] args) {
            ///Test.Method(); /// test af file stream reader class skabelon
            var game = new Game();
            string[] abb = game.levelParser.TextInFile("the-beach.txt");
            string[] ab = game.levelParser.TextInFile("short-n-sweet.txt");

            for (int i = 0; i < abb.Length; i++) {
                Console.WriteLine(abb[i]);

            }
            
            for (int i = 0; i < ab.Length; i++) {
                Console.WriteLine(ab[i]);

            }

            ///game.GameLoop();
        }
    }
}