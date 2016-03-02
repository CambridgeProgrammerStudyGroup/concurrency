/***
 * Excerpted from "Seven Concurrency Models in Seven Weeks",
 * published by The Pragmatic Bookshelf.
 * Copyrights apply to this code. It may not be used to create training material, 
 * courses, books, articles, and the like. Contact us if you are in doubt.
 * We make no guarantees that this code is fit for any purpose. 
 * Visit http://www.pragmaticprogrammer.com/titles/pb7con for more book information.
***/

public class DiningPhilosophers {

  public static void main(String[] args) throws InterruptedException {

  	int count = 5;
  	int thinkPeriod = 10;
  	int eatPeriod = 10;

    Philosopher[] philosophers = new Philosopher[count];
    Chopstick[] chopsticks = new Chopstick[count];
    
    for (int i = 0; i < count; ++i)
      chopsticks[i] = new Chopstick(i);
    for (int i = 0; i < count; ++i) {
      philosophers[i] = 
      	new Philosopher(chopsticks[i], chopsticks[(i + 1) % count], thinkPeriod, eatPeriod);
      philosophers[i].start();
    }
    for (int i = 0; i < count; ++i)
      philosophers[i].join();
  }

}
