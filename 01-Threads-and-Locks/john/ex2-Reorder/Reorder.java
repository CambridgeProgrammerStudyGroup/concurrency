
public class Reorder {
  	
  	public static void main(String[] args) throws InterruptedException {
		
		while(! Printer.done){
			RunOnce();				
		}
	}

	static int count = 35;
	static void RunOnce() throws InterruptedException {
  		Printer.runCount++;

  		Printer printer = new Printer();		

		Thread buildAndPrint = new Thread(){
			public void run()  {
				printer.BuildAndPrint(count);
			};
		};

		Thread watch = new Thread(){
			public void run() {
				printer.Watch(count);
			};
		};

		watch.start();
		buildAndPrint.start();
		buildAndPrint.join();
		watch.join();
  	}
}
