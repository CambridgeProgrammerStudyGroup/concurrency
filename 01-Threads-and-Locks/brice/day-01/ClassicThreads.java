public class ClassicThreads {

	public static void main(String[] args) throws InterruptedException{
		class Reorder {
			int x = 0;
			int y = 0;
			public void write(){
				x++;
				y++;
			}
			public boolean reordered(){
				return y>x;
			}
		}
		Reorder ro = new Reorder();

		class Explorer extends Thread {
			int tick = 0;
			public void run() {
				while(true){
					if(ro.reordered()){
						System.out.println("Reorder identified.");
						System.exit(0);
					}else{
						tick++;
						if(tick % 1000000 == 0){
							System.out.print(".");
						}
					}
				}
			}
		}

		class Changer extends Thread {
			public void run() {
				while(true){
					ro.write();
				}
			}
		}

		Explorer ex = new Explorer();
		Changer ch = new Changer();

		System.out.println("Begin testing");

		ex.start();
		ch.start();
		ex.join();
		ch.join();
	}
}