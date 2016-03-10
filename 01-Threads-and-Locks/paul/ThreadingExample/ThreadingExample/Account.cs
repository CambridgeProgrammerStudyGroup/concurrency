using System;

namespace ThreadingExample
{
	public class Account
	{
		decimal balance;
		public Account(decimal _balance)
		{
			balance = _balance;
		}

		readonly Object thisLock = new Object();

		public void Withdraw(decimal amount)
		{
			lock (thisLock)
			{
				if (amount > balance)
				{
					throw new Exception("Insufficient funds");
				}
				balance -= amount;
				Console.WriteLine("{0} withdrawn. new balance is {1}", amount, balance);
			}
		}
	}
}

