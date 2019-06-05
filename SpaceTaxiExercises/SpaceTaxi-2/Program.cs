using System;
using System.Timers;
using SpaceTaxi_2.States;

namespace SpaceTaxi_2 {
    internal class Program {
        System.Timers.Timer timer;
        public static void Main(string[] args) {
            var game = new Game();
            game.GameLoop();
            }
        }
    }
