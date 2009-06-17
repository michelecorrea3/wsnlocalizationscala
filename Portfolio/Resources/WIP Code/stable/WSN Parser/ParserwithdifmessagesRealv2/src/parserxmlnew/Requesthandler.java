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
    private int length;
    
    public Requesthandler(Serialsender sender) {
        this.sender = sender;
        this.port = connectiondetails.Listenport;
    }
    
    public void createListener() throws IOException {
        //System.out.println("Creating socket on port " + connectiondetails.Listenport);
        this.listenSocket = new ServerSocket(this.port);
        System.out.println("Created socket on port " + connectiondetails.Listenport);
    }
    
    public void run()
    {
        try
        {
           createListener();
        } catch (IOException ex) { ex.printStackTrace(); }
        
        while (true) {
            msg = new String();
            try {
                System.out.println("Listening on " + connectiondetails.Listenport);
                msg = readConnection();
                System.out.println(msg);
                System.out.println("Message received over " + connectiondetails.Listenport);
            } catch (IOException ex) { ex.printStackTrace(); }
            handle(msg); //start the deXML
        }
    }
    
    // get message from the controller
    public String readConnection() throws IOException {
        //String xmlString = "<?xml version=\"1.0\" standalone=\"yes\"?><Replies xmlns=\"http://tempuri.org/Reply.xsd\"><Reply><INT>1</INT></Reply></Replies>\r\n";

        Socket incoming = listenSocket.accept();
        InputStream in = incoming.getInputStream();
        BufferedReader bf = new BufferedReader(new InputStreamReader(in));
        String message = bf.readLine();
        //OutputStream out = incoming.getOutputStream();
        //BufferedWriter bw = new BufferedWriter(new OutputStreamWriter(out)); //Write text to a character-output stream, buffering characters so as to provide for the efficient writing of single characters, arrays, and strings.
        //bw.write(xmlString);
        //bw.flush();
        
        bf.close();
        //bw.close();
        in.close();
        //out.close();
        incoming.close();
        
        return message;
    }
    
    public void handle(String msg) {

        //length = Serial.NumberOfNodes();

        //String xmlString1 = "<?xml version=\"1.0\" standalone=\"yes\"?><DiscoveryReply xmlns=\"http://tempuri.org/WSNActionReq.xsd\"><Nodeid>1</Nodeid></DiscoveryReply>\r\n";

        //String xmlString2 = "</Nodeid><Nodeid>";
   

        // decode xml
        dexml dx = new dexml();
        dx.set_message(msg);
        dx.decode();

        if (dx.AN == true)
            sender.add(dx.get_targetid(), dx.AN_REQUEST, dx.ANr, "");
        if (dx.active == true)
            sender.add(dx.get_targetid(), dx.ACTIVE_REQUEST, dx.activer, "");
        if (dx.frequency == true)
            sender.add(dx.get_targetid(), dx.FREQUENCY_REQUEST, dx.frequencyr, "");
        if (dx.leds == true)
            sender.add(dx.get_targetid(), dx.LED_REQUEST, dx.ledr, "");
        if (dx.pollingL == true)
            sender.add(dx.get_targetid(), dx.SAMPLING_PERIOD, dx.pollL, "");
        if (dx.pollingS == true)
            sender.add(dx.get_targetid(), dx.LOC_PERIOD, dx.pollS, "");
        if (dx.posx == true)
            sender.add(dx.get_targetid(), dx.POSX_REQUEST, dx.Xr, "");
        if (dx.posy == true)
            sender.add(dx.get_targetid(), dx.POSY_REQUEST, dx.Yr, "");
//        if (dx.status == true)
  //          bw.write(xmlString);
        if (dx.power == true)
            sender.add(dx.get_targetid(), dx.POWER_REQUEST, dx.powerr, "");

        // queue up to serial connection (message for the serial send to the root)
        //sender.add(dx.get_targetid(), dx.get_request(), dx.get_parameter(), "");
    }
    
}

