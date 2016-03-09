/***
 * Excerpted from "Seven Concurrency Models in Seven Weeks",
 * published by The Pragmatic Bookshelf.
 * Copyrights apply to this code. It may not be used to create training material, 
 * courses, books, articles, and the like. Contact us if you are in doubt.
 * We make no guarantees that this code is fit for any purpose. 
 * Visit http://www.pragmaticprogrammer.com/titles/pb7con for more book information.
***/

import java.net.URL;

public class HttpDownload {

  public static void main(String[] args) throws Exception {
    //URL from = new URL("http://download.wikimedia.org/enwiki/latest/enwiki-latest-pages-articles.xml.bz2");
  	URL from = new URL("https://storage.googleapis.com/golang/go1.6.linux-amd64.tar.gz");
  	Downloader downloader = new Downloader(from, "download.out");
  	downloader.start();
  	downloader.addListener(new ProgressListener() {
  		public void onProgress(int n) { System.out.print("\r"+n); System.out.flush(); }
  		public void onComplete(boolean success) {}
  	});
  	downloader.join();
  }

}
