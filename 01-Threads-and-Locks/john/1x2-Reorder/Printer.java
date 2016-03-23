
class Printer{

	static boolean done = false;
	static int runCount = 0;

	private String x = "";
	private String y = "";

	static void wr(String line){ System.out.println(line); }

  	public void BuildAndPrint(int count) {

  		String temp1 = "";
  		for(int i = 0; i < count; i++){
  			temp1 += "=";
  		}
  		
  		String temp2 = "";
  		for(int i = 0; i < count; i++){
  			temp2 += "-";
  		}

  		// lenX: 0 lenY 0
  		
  		y = temp2;  
  		// lenX: 0  lenY 35
  		
  		x = temp1;
  		// lenX: 35 lenY 35
  		
  		y = y + y;
  		// lenX: 35 lenY 70

  		wr(x);
  		wr(y);
      wr("");
  	}

  	private String reX, reY;
  	private int reXLen, reYLen;
  	public void Watch(int count) {
  		int doubleCount = count + count;
  		String myX = "", myY = "";
  		do{
  			myY = this.y;  // Order here is imporatant to make sure 
  			myX = this.x;  // anomalies are not due to stuff happening  
  							       // between assignments.

  			// this shouldn't be true if happening in written order.
  			if(myX.length() == 0 && myY.length() == doubleCount){
  				wr("Wooah! x:" + myX.length() + " y:" + myY.length()
  					+ " (Run count: " + runCount + ")");
  				done = true;
  			}
  		} 
      while ((!done) && myY.length() < doubleCount && myX.length() < count);
  	}

}