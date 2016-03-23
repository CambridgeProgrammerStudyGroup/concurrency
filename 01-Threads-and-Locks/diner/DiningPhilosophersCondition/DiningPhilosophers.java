package com.dinerismail.DiningPhilosophersCondition;

import java.util.concurrent.locks.ReentrantLock;

public class DiningPhilosophers {

    public static void main(String[] args) throws InterruptedException {
        Philosopher[] philosophers = new Philosopher[5];
        Object table = new Object();

        for (int i = 0; i < 5; ++i)
            philosophers[i] = new Philosopher(table);
        for (int i = 0; i < 5; ++i) {
            philosophers[i].setLeft(philosophers[(i + 4) % 5]);
            philosophers[i].setRight(philosophers[(i + 1) % 5]);
            philosophers[i].start();
        }
        for (int i = 0; i < 5; ++i)
            philosophers[i].join();
    }
}
