public class LambdaThreads {
	static int x = 0;
	static int y = 0;
	public static void main(String[] args) throws InterruptedException{
		
		Thread ex = new Thread( ()-> {
			int tick = 0;
			while(true){
				if(y > x){
					System.out.println("Reorder identified.");
					System.exit(0);
				}else{
					tick++;
					if(tick % 1000000 == 0){
						System.out.print(".");
					}
				}
			}
		});

		Thread ch = new  Thread( ()->{
			while(true){
				x++;
				y++;
			}
		});

		System.out.println("Begin testing");

		ex.start();
		ch.start();
		ex.join();
		ch.join();
	}
}