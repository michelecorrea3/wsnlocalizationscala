/* Peter De Cauwer & Tim Van Overtveldt, graduate student Artesis, Applied Engineering: Electronics-ICT
 * Master thesis: WSN localization with Senseless
 */

package parserxmlnew;

import java.io.*;
import java.util.*;
import java.net.*;

/**
 * Class for handling requests received by the controller
 */
public class Requesthandler implements Runnable {
    private Serialsender sender;            // Class Serialsender
    private ServerSocket listenSocket;      // Server socket
    private Socket incoming;                // Socket to listen
    private int port;                       // Port to listen on
    private String msg;                     // String of the message received by the controller
    private String xmlreply;                // String of the xml message to send as reply on the request
    private BufferedWriter bw;              // Class BufferedWriter to send messages to the controller
    public static MyTimerTask MyTask;       // TimerTask
    Timer timer;                            // Create a timer
    sharedata s;                            // class sharedata
    sharedata d;                            // class sharedata

    // Function to initialize the class Serialsender and the port
    public Requesthandler(Serialsender sender, sharedata s) {
        this.sender = sender;
        this.port = connectiondetails.Listenport;
        this.s = s;
    }

    // Function to create Server socket
    public void createListener() throws IOException {
        this.listenSocket = new ServerSocket(this.port);
        System.out.println("Created socket on port " + connectiondetails.Listenport);
    }

    // Function that continually runs (needed to implement runnable)
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
            } catch (IOException ex) { ex.printStackTrace(); }
        }
    }
    
    // Function that creates a socket that listen to messages from the controller
    // Also sends a reply to the controller
    public String readConnection() throws IOException {
    
        String xmlString;
        incoming = listenSocket.accept();
        InputStream in = incoming.getInputStream();
        BufferedReader bf = new BufferedReader(new InputStreamReader(in));
        String message = bf.readLine();
        System.out.println(message);

        timer = new Timer();
        MyTask = new MyTimerTask(s);
        System.out.println("Timer started and task sheduled");
        timer.schedule(MyTask, 17000);
        // When a request is received, we wait for a status message from the WSN
        s.set_waiting(true);

        xmlString = handle(message);
        s.set_status("banana");
        timer.cancel();
        OutputStream out = incoming.getOutputStream();
        // Write text to a character-output stream, buffering characters so as to provide for the efficient writing of single characters, arrays, and strings.
        bw = new BufferedWriter(new OutputStreamWriter(out)); 

        if(xmlString.length() > 10)
        {
            bw.write(xmlString);
            bw.flush();
        }
        
        bf.close();
        bw.close();
        in.close();
        out.close();
        incoming.close();
        
        return message;
    }

    // Function that analyses the incoming xml and that put the requests on the queue
    public String handle(String msg) throws IOException {

        // decode xml
        dexml dx = new dexml();
        dx.set_message(msg);
        dx.decode();

        if (dx.active == true)
            sender.add(dx.get_targetid(), dx.ACTIVE_REQUEST, dx.activer, "");
        if (dx.AN == true)
            sender.add(dx.get_targetid(), dx.AN_REQUEST, dx.ANr, "");
        if (dx.posx == true)
            sender.add(dx.get_targetid(), dx.POSX_REQUEST, dx.Xr, "");
        if (dx.posy == true)
            sender.add(dx.get_targetid(), dx.POSY_REQUEST, dx.Yr, "");
        if (dx.pollingS == true)
            sender.add(dx.get_targetid(), dx.SAMPLING_PERIOD, dx.pollS, "");
        if (dx.pollingL == true)
            sender.add(dx.get_targetid(), dx.LOC_PERIOD, dx.pollL, "");
        if (dx.leds == true)
            sender.add(dx.get_targetid(), dx.LED_REQUEST, dx.ledr, "");
        if (dx.power == true)
            sender.add(dx.get_targetid(), dx.POWER_REQUEST, dx.powerr, "");
        if (dx.frequency == true)
            sender.add(dx.get_targetid(), dx.FREQUENCY_REQUEST, dx.frequencyr, "");
        if (dx.status == true)
            sender.add(dx.get_targetid(), dx.STATUS_REQUEST, dx.discoveryr, "");

        for(int i = 0; (i < 5) && s.get_waiting(); i++)
        {
            sender.add(dx.get_targetid(), dx.STATUS_REQUEST, dx.discoveryr, "");
        }

            System.out.println("Request op queue geplaatst");
            String xml = Sendreply();
            return xml;

    }

    // Function that waits on the xml message from the other thread
    public String Sendreply() throws IOException
    {
        boolean status = true;

        while(status)
        {
            if(this.s.get_status().length() < 20)
                if(this.s.get_status() == "error")
                {
                    xmlreply = "error";
                    status = false;
                    System.out.println("Status is error!");
                }
                else
                    status = true;
            else
            {
                xmlreply = this.s.get_status();
                //System.out.println("xml string: " + xmlreply);
                status = false;
            }
        }

        s.set_waiting(false);

        return xmlreply;

    }
}

