## Threads and Locks - Day 2 Find

### Fairness
A Fair lock (generally) is granted in the order that it is requested.

This prevents a lock request being blocked while later requests are
granted, this might help achieve necessary responsiveness  and or even
prevent indefinite blocking.  It may make code easier to reason about as
intuitively I would expect this behaviour.

However, it comes at the cost of performance, locking can consume
considerably more resources.

### ReentrantReadWriteLock

Provides a mechanism so that lock contention is avoided if only readonly
access is required to a resource. I.e. Multiple Read locks can be held
at the same time. In contrast the Write lock, like a regular reentrant
lock can only be held once and only after when all the Read locks have
been released.

### Spurious Wakeup

When a thread has await-ed a condition (aka is woken up) there is no
guarantee that the condition is satisfied. There may be numerous threads
awaiting a condition and the fist one to 'wake' (or indeed any other
thread) could have made the condition invalid by the time other threads
are woken.

This is why the normal practice for the 'await condition' to be in a
loop that can only be exited when the condition is satisfied.  (The loop
is itself in a block that is holding the lock so the state cannot change
between checking the state and exiting the loop.)

	get lock
	while conditions not good
		wait for condition signal
	do my stuff
	release lock

### AtomicIntegerFieldUpdate

This appears to share the AtomicInteger functionality across all
instances of a class. Instead of
	
	AtomicInteger x = new AtomicInteger()
	...
	x.GetAndAdd(12)

we can have:

	static AtomicIntegerFieldUpdater<ThisClassName> x_updater = 
		AtomicIntegerFieldUpdater
		.newUpdater(ThisClassName.class, "x");
	...
    volatile int x;    
    ...
    x_updater.getAndAdd(this, 12);
	
This may have performance/resource advantages because only single
_static_ updater is needed to update the field regardless of how many
instances of the class there may be. I.e. you can create 1,000 instances
of the class without creating 1,000 AtomicInteger classes.
