using System;
using System.Threading;

namespace DiningPhilosophersDeadlock
{
    public class Philosopher
    {
        private readonly int _id;
        private readonly Chopstick _left;
        private readonly Chopstick _right;
        private readonly Random _random;

        public Philosopher(int id, Chopstick left, Chopstick right)
        {
            _id = id;
            _left = left;
            _right = right;
            _random = new Random();
        }

        public void Dine()
        {
            while (true)
            {
                // thinking
                Thread.Sleep(_random.Next(100));

                lock (_left)
                {
                    lock (_right)
                    {
                        // eating
                        _left.Use(_id);
                        _right.Use(_id);
                        Thread.Sleep(_random.Next(100));
                    }
                }
            }
        }
    }
}