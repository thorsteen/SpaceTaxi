using System;
using System.Threading;

namespace SpaceTaxi_3.Timer {
    public class TimerIndex {
        private int timer1;
        private int oneMin1;

        public int timer {
            get => timer1;
            private set => timer1 = value;
        }

        public int oneMin {
            get => oneMin1;
            private set => oneMin1 = value;
        }

        public TimerIndex() {
            timer = 0;
            oneMin1 = 0;
        }
        
        /// <summary>
        /// A void that keeps track of time. Counts from 1-59 sec, and then adds one to the minute counter.
        /// Though because of the values being remebered by the class, every new call should be to the time wanted,
        /// and not the time you want to add.
        /// </summary>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        public void PrintSeconds(int minutes, int seconds) {
            while (timer < seconds || oneMin < minutes) {
                timer += 1;
                Thread.Sleep(1000);
                if (timer == 60) {
                    timer = 0;
                    oneMin += 1;
                }

                Console.WriteLine(oneMin + ":" + timer);
            }
        }
    }
}