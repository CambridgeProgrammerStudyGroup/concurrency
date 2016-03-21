using System;

namespace DiningPhilosophersDeadlock
{
    public class Chopstick
    {
        private int _id;

        public Chopstick(int id)
        {
            _id = id;
        }

        public void Use(int philosopherId)
        {
            // just to show that something is going on
            Console.WriteLine("Philosopher {0} is using chopstick {1}", philosopherId, _id);
        }
    }
}
