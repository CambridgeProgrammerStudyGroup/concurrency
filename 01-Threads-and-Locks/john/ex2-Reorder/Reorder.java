/*
	*** Behaviour depends on flags passed to Java !! ***

	- If no flags are passed then it hangs!
	- If -Xint (interpret only) is passed then it runs indefinately...
	- If -Xcomp (compile first) then it works.

	(javac 1.8.0_73, java version "1.8.0_73")

	Even then I'm not confident that the behaviour isn't a symptom of my
	ignorance.

	But in the Numbers.CountPrint method the expected states are:

  		lenX:  0  lenY  0
  		lenX:  0  lenY 40
  		lenX: 40  lenY 40
  		lenX: 40  lenY 80

	However we do get cases where lenX=0 and lenY=80, which I think
	means we're getting reordering.

	Sample output:
	
[ex2-Reorder] java -Xcomp Reorder
----------------------------------------
================================================================================
----------------------------------------
================================================================================
<snip>
----------------------------------------
================================================================================
----------------------------------------
================================================================================
Wooah! x:0 y:80 (Run count: 88)
[ex2-Reorder]

*/

public class Reorder {
  	
  	public static void main(String[] args) throws InterruptedException {
		
		while(! Numbers.done){
			RunOnce();				
		}
	}

	static int count = 40;
	static void RunOnce() throws InterruptedException {
  		Numbers.runCount++;

  		Numbers numbers = new Numbers();		

		Thread countPrint = new Thread(){
			public void run()  {
				numbers.CountAndPrint(count);
			};
		};

		Thread watch = new Thread(){
			public void run() {
				numbers.Watch(count);
			};
		};

		watch.start();
		countPrint.start();
		countPrint.join();
		watch.join();
  	}
}
