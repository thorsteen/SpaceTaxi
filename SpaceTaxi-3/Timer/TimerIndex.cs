using System;
using System.Threading;

namespace SpaceTaxi_3.Timer {
    public class TimerIndex {
        private int timer = 0;
        private int oneMin = 0;

        public void PrintSeconds(int minutes, int seconds) {
            while (timer < seconds || oneMin < minutes) {
                timer += 1;
                Thread.Sleep(1000);
                if (timer == 60) {
                    timer = 0;
                    oneMin += 1;
                }

                OutPut(timer);
                Console.WriteLine(oneMin + ":" + timer);
            }
        }

        public int OutPut(int a) {
            return a;
        }
    }
}