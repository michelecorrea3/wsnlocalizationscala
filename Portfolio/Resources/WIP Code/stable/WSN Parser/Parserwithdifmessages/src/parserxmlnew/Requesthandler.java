/* Philippe Belet, graduate student Artesis, Applied Engineering: Electronics-ICT
 * ICT 7: WSN & DB
 * Project: TelosB WSN module
 */

package parserxmlnew;

import java.io.*;
import java.net.*;

/**
 *
 * @author philippe
 */
public class Requesthandler implements Runnable {
    private Serialsender sender;
    private ServerSocket listenSocket;
    private int port;
    private String msg;
    
    public Requesthandler(Serialsender sender) {
        this.sender = sender;
        //this.port = 3801;
        //this.port = Integer.getInteger(connectiondetails.Listenport);
        //this.port = Integer.parseInt(connectiondetails.Listenport);
        this.port = connectiondetails.Listenport;
    }
    
    public void createListener() throws IOException {
        //System.out.println("Creating socket on port " + connectiondetails.Listenport);
        this.listenSocket = new ServerSocket(this.port);
        System.out.println("Created socket on port " + connectiondetails.Listenport);
    }
    
    public void run() {
        //test();

	try {
            createListener();
        } catch (IOException ex) { ex.printStackTrace(); }
        
        while (true) {
            msg = new String();
            try {
                System.out.println("Listening on " + connectiondetails.Listenport);
                msg = readConnection();
                System.out.println("Message received over " + connectiondetails.Listenport);
            } catch (IOException ex) { ex.printStackTrace(); }
            handle(msg);
        }
    }
    
    // get message from the controller
    public String readConnection() throws IOException {
        String xmlString = "<?xml version=\"1.0\" standalone=\"yes\"?><Replies xmlns=\"http://tempuri.org/Reply.xsd\"><Reply><INT>1</INT></Reply></Replies>\r\n";
        Socket incoming = listenSocket.accept();
        InputStream in = incoming.getInputStream();
        BufferedReader bf = new BufferedReader(new InputStreamReader(in));
        String message = bf.readLine();
        OutputStream out = incoming.getOutputStream();
        BufferedWriter bw = new BufferedWriter(new OutputStreamWriter(out));
        bw.write(xmlString);
        bw.flush();
        
        bf.close();
        bw.close();
        in.close();
        out.close();
        incoming.close();
        
        return message;
    }
    
    public void handle(String msg) {      
        // decode xml
        dexml dx = new dexml();
        dx.set_message(msg);
        dx.decode();
        
        // queue up to serial connection
        sender.add(dx.get_targetid(), dx.get_request(), dx.get_parameter(), "");
    }
    
    public void test() {
	String xmltest = "<?xml version=\"1.0\" standalone=\"yes\"?><WSNReq xmlns=\"http://tempuri.org/WSNActionReq.xsd\"><RequestAction><NodeID>1</NodeID><LED1>1</LED1><LED2>0</LED2><LED3>0</LED3><LED4></LED4><LED5></LED5><polling>30</polling></RequestAction></WSNReq>";
        dexml dx = new dexml();
	dx.set_message(xmltest);
	dx.decode();
	sender.add(dx.get_targetid(), dx.get_request(), dx.get_parameter(), "");
	
	//sender.add(1, (short)0x55, 4, "");
        //sender.add(0xffff, (short)0xAA, 20000,"");
    }
    
}

