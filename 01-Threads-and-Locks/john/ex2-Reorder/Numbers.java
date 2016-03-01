import java.util.*;

class Numbers{

	private String x = "";
	private String y = "";

	private ArrayList<String> watchdata = new ArrayList<String>();

	static void wr(String line){ System.out.println(line); }

  	public void CountAndPrint(int count) {

  		String temp1 = "";
  		for(int i = 0; i < count; i++){
  			temp1 += "-";
  		}
  		
  		String temp2 = "";
  		for(int i = 0; i < count; i++){
  			temp2 += "=";
  		}

  		y = temp2;
  		x = temp1;

  		y = y + y;

  		wr(x);
  		wr(y);
  	}

  	private String reX, reY;
  	private int reXLen, reYLen;
  	public void Watch(int count) {
  		int doubleCount = count + count;
  		String myX = "", myY = "";
  		while(y.length() < doubleCount){
  			myY = this.y;
  			myX = this.x;
  			if(myX.length() != 0 || myY.length() != 0){
	  			watchdata.add(String.valueOf(myX.length()) + ":" + String.valueOf(myY.length()));
  				// while(true){
  				// 	wr("woh! x:" + myX.length() + " y:" + myY.length());
  				// }
  			}
  		}
			myY = this.y;
			myX = this.x;
  		watchdata.add(String.valueOf(myX.length()) + ":" + String.valueOf(myY.length()));
  		for(Iterator<String> itr = watchdata.iterator(); itr.hasNext();){
  			wr(itr.next());
  		}
  	}

  	static void RunOnce() throws InterruptedException {
  		
  		Numbers numbers = new Numbers();		

		Thread countPrint = new Thread(){
			public void run()  {
				numbers.CountAndPrint(40);
			};
		};

		Thread watch = new Thread(){
			public void run() {
				numbers.Watch(40);
			};
		};

		watch.start();
		countPrint.start();
		countPrint.join();
		watch.join();
		wr("Done.");
  	}

}