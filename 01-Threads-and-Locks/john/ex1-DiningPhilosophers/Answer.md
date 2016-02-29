Experimenting
=============

This is all subjective.  To be objective would need to run each configuration
a bunch of times and check statistical significance.

Adjusting
---------

### Absolute time of eat/think 

Unexpectedly absolute eat/think time had a significant effect when
reduced sufficiently.  E.g. reducing from 1000 to 10 resulted approx 150
cycles. (30 -> 350). A wait of 100 made things faster (in absolute time)
but had a less noticable effect. On the number of cycles.

To save time the rest is done with a wait of 10.


### Number of Philosphers

As expected, reducing the number of Philosophers appeared to reduce the
time before a deadlock.  Because a thinking philosopher prevents a
deadlock so the fewer there are the fewer the chances to avoid the
deadlock.  In contrast a 100 philosophers didn't lock even after two
orders of magnitude more cycles than typically needed for 5
philosophers.

### Relative time of think vs eat

As expected, increasing the eat time, but not the think time, (100 vs
10) significanlty increased the likelihood of a lock. Presumably becuase
deadlock requires all the philosophers to be eating at the same time and
that state is now much more common.

In contrast, the reverse (10 eat vs 100 think) resulted in five 1,000+
cycle attempts being abandoned without deadlock.

Debugging
---------

To increase the likelihood of creating a deadlock for debugging purposes
I'd:
  - Increase amount of time in lock relative to out of lock
  - Decrease overall cycle time if possible
  - If number of locks matches number of threads then have as 
  	few threads as possible.
