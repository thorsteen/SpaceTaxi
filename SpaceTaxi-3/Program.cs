using System;
using SpaceTaxi_3.Timer;

namespace SpaceTaxi_3 {
    internal class Program {
        public static void Main(string[] args) {
            var game = new Game();
            game.GameLoop();
        }
    }
}