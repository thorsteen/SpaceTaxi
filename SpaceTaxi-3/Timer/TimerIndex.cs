using System;
using System.Threading;

namespace SpaceTaxi_3.Timer {
    public class TimerIndex {
        public int Timer { get; private set; }

        public int OneMin { get; private set; }

        public TimerIndex() {
            Timer = 0;
            OneMin = 0;
        }
        
        /// <summary>
        /// A void that keeps track of time. Counts from 1-59 sec, and then adds one to the minute counter.
        /// Though because of the values being remebered by the class, every new call should be to the time wanted,
        /// and not the time you want to add.
        /// </summary>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        public void PrintSeconds(int minutes, int seconds) {
            while (Timer < seconds || OneMin < minutes) {
                Timer += 1;
                Thread.Sleep(1000);
                if (Timer == 60) {
                    Timer = 0;
                    OneMin += 1;
                }

                Console.WriteLine(OneMin + ":" + Timer);
            }
        }
    }
}