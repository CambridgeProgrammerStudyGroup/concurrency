using System;
using System.Threading;

namespace Concurrency
{
	class Reorder
	{
		public static void Main (string[] args)
		{
			int x = 0;
			int y = 0;

			var t1 = new Thread (() => 
			{
				while (true) 
				{
					x++;
					y++;
				}
			});

			var t2 = new Thread (() => 
			{
				while (true) 
				{
					if (x < y)
					{
						Console.WriteLine("Oh, dear!");
					}
				}
			});

			t1.Start ();
			t2.Start ();

			t1.Join ();
			t2.Join ();
		}
	}
}
