using System;
using System.Threading;

namespace reorder
{
    class Program
    {
        static void Main(string[] args)
        {
            var counter = new myCounter();

            var thread1 = new Thread(() =>
            {
                while(true)
                    counter.increment();
            });
            var thread2 = new Thread(() =>
            {
                while (true)
                    if (counter.checkme())
                        Console.WriteLine("Yikes! Re-ordering detected!");
            });

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();
        }
    }

    class myCounter
    {
        private int x, y;

        public void increment()
        {
            x++;
            y++;
        }

        public bool checkme()
        {
            return y > x;
        }
    }
}
