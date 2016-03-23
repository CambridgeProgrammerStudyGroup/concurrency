package com.dinerismail.DiningPhilosophersCondition;

import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.ReentrantLock;

import java.util.Random;

class Philosopher extends Thread {

    private boolean eating;
    private Philosopher left;
    private Philosopher right;
    private Object table;
    private Condition condition;
    private int thinkCount;

    public Philosopher(Object table) {
        eating = false;
        this.table = table;
        //condition = table.newCondition();
    }

    public void setLeft(Philosopher left) { this.left = left; }
    public void setRight(Philosopher right) { this.right = right; }

    public void run() {
        try {
            while (true) {
                think();
                eat();
            }
        } catch (InterruptedException e) {}
    }

    private void think() throws InterruptedException {
        synchronized (table) {
            eating = false;
            table.notify();
        }

        ++thinkCount;
        if (thinkCount % 10 == 0) {
            System.out.println("Philosopher " + this + " has thought " + thinkCount + " times");
        }
        Thread.sleep(1000);
    }

    private void eat() throws InterruptedException {
        synchronized (table) {
            while (left.eating || right.eating) {
                table.wait();
            }
            eating = true;
        }

        Thread.sleep(1000);
    }
}
