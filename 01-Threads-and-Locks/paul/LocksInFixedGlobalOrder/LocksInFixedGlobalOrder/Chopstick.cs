using System;

namespace LocksInFixedGlobalOrder
{
    public class Chopstick
    {
        public int Id { get; }

        public Chopstick(int id)
        {
            Id = id;
        }

        public void Use(int philosopherId)
        {
            // just to show that something is going on
            Console.WriteLine("Philosopher {0} is using chopstick {1}", philosopherId, Id);
        }
    }
}
