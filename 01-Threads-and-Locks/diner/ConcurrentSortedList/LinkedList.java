package com.dinerismail.ConcurrentSortedList;

import java.util.Random;
import java.util.Timer;

class LinkedList {

    public static void main(String[] args) throws InterruptedException {
        final ConcurrentSortedListSingleLock list = new ConcurrentSortedListSingleLock();
        final Random random = new Random();

        class TestThread extends Thread {
            public void run() {
                for (int i = 0; i < 12500; ++i)
                    list.insert(random.nextInt());
            }
        }

        class CountingThread extends Thread {
            public void run() {
                while (!interrupted()) {
                    System.out.print("\r" + list.size());
                    System.out.flush();
                }
            }
        }

        Thread t1 = new TestThread();
        Thread t2 = new TestThread();
        Thread t3 = new TestThread();
        Thread t4 = new TestThread();
        Thread county = new CountingThread();

        long start = System.currentTimeMillis();
        t1.start(); t2.start(); t3.start(); t4.start();
        county.start();
        t1.join(); t2.join(); t3.join(); t4.join();
        county.interrupt();
        long ellapsedTime = System.currentTimeMillis() - start;

        System.out.println("\r" + list.size());

        System.out.println("Time ellapsed: " + ellapsedTime);

        if (list.size() != 50000)
            System.out.println("*** Wrong size!");

        if (!list.isSorted())
            System.out.println("*** Not sorted!");
    }
}
