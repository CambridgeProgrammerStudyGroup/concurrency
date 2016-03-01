
class Numbers{

	static boolean done = false;
	static int runCount = 0;

	private String x = "";
	private String y = "";

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

  		// lenX: 0 lenY 0
  		
  		y = temp2;  
  		// lenX: 0 lenY 40
  		
  		x = temp1;
  		// lenX: 40 lenY 40
  		
  		y = y + y;
  		// lenX: 40 lenY 80

  		wr(x);
  		wr(y);
  	}

  	private String reX, reY;
  	private int reXLen, reYLen;
  	public void Watch(int count) {
  		int doubleCount = count + count;
  		String myX = "", myY = "";
  		do{
  			myY = this.y;  	// order imporatant to make sure anomalies
  			myX = this.x;  	// are not due to stuff happening between 
  							// assignments

  			// this shouldn't be true if happening in written order.
  			if(myX.length() == 0 && myY.length() == doubleCount){
  				wr("Wooah! x:" + myX.length() + " y:" + myY.length()
  					+ " (Run count: " + runCount + ")");
  				done = true;
  			}
  		} while ((!done) && myY.length() < doubleCount && myX.length() < count);
  	}

}