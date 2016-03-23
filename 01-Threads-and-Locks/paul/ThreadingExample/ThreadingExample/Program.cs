using System;
using System.Threading;

namespace ThreadingExample
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var account = new Account(100.00m);

			Thread thread1 = new Thread(() => account.Withdraw(40));
			Thread thread2 = new Thread(() => account.Withdraw(50));

			thread1.Start();
			thread2.Start();

			// wait for program to finish
			thread1.Join();
			thread2.Join();

			// exit()
		}
	}
}
