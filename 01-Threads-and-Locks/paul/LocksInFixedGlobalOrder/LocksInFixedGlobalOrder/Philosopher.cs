using System;
using System.Threading;

namespace LocksInFixedGlobalOrder
{
    public class Philosopher
    {
        private readonly int _id;
        private readonly Chopstick _first;
        private readonly Chopstick _second;
        private readonly Random _random;

        public Philosopher(int id, Chopstick left, Chopstick right)
        {
            _id = id;
            _first = left.Id < right.Id ? left : right;
            _second = left.Id < right.Id ? right : left;
            _random = new Random();
        }

        public void Dine()
        {
            while (true)
            {
                // thinking
                Thread.Sleep(_random.Next(100));

                lock (_first)
                {
                    lock (_second)
                    {
                        // eating
                        _first.Use(_id);
                        _second.Use(_id);
                        Thread.Sleep(_random.Next(100));
                    }
                }
            }
        }
    }
}