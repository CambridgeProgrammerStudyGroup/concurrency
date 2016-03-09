using System;
using System.Threading;

public class LambdaThreads {

	static void Main(String[] args){
		
		int x = 0;
		int y = 0;

		var ex = new Thread (() => {
			while (true) {
				if (y > x) {
					Console.WriteLine ("Reorder identified.");
					System.Environment.Exit(0);
				}
			}
		});

		var ch = new Thread (() => {
			while (true) {
				x++;
				y++;
			}
		});

		Console.WriteLine("Begin testing");

		ex.Start();
		ch.Start();
		ex.Join();
		ch.Join();
	}
}


