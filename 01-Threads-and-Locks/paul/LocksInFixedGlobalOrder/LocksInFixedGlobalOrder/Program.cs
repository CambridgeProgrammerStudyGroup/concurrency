using System.Collections.Generic;
using System.Threading;

namespace LocksInFixedGlobalOrder
{
    class Program
    {
        static void Main(string[] args)
        {
            var chopsticks = new List<Chopstick>
            {
                new Chopstick(0),
                new Chopstick(1),
                new Chopstick(2),
                new Chopstick(3),
                new Chopstick(4),
            };

            var threads = new List<Thread>
            {
                new Thread(() => new Philosopher(0, chopsticks[0], chopsticks[1]).Dine()),
                new Thread(() => new Philosopher(1, chopsticks[1], chopsticks[2]).Dine()),
                new Thread(() => new Philosopher(2, chopsticks[2], chopsticks[3]).Dine()),
                new Thread(() => new Philosopher(3, chopsticks[3], chopsticks[4]).Dine()),
                new Thread(() => new Philosopher(4, chopsticks[4], chopsticks[0]).Dine()),
            };

            foreach (var thread in threads)
                thread.Start();

            foreach (var thread in threads)
                thread.Join();
        }
    }
}
