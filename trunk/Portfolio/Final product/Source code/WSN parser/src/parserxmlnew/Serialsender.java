/* Peter De Cauwer & Tim Van Overtveldt, graduate student Artesis, Applied Engineering: Electronics-ICT
 * Master thesis: WSN localization with Senseless
 */

package parserxmlnew;

import java.util.*;
import net.tinyos.message.*;

/**
 * Class used to send requests with dissemination to the WSN
 */
public class Serialsender extends Thread {
    private LinkedList queue;               // Queue to store all the requests
    private MessageToSend MsgToSend;        // Class MessegeToSend
    private MoteIF gateway;                 // Class MoteIF

    // Function that creates a new Queue and initializes the MOTEIF class
    public Serialsender(MoteIF gateway) {
        queue = new LinkedList();
        this.gateway = gateway;
    }

    // Function that runs continually (thread)
    // Checks if there are requests on the stack to transmit to the WSN
    public void run() {
        while (true) {
            if (queue.isEmpty())
                try {Thread.sleep(20);} catch (Exception e) { e.printStackTrace(); }
            else {
                try {
                    System.out.println("sending dissemination message, actual queue size:");
                    System.out.println(queue.size());
                    MsgToSend = (MessageToSend)queue.remove(0);
                    System.out.println("disseminated message:");
                    System.out.println("Target ID: " + MsgToSend.sMsg.get_targetid());
                    System.out.println("The request in dec: " + MsgToSend.sMsg.get_request());
                    System.out.println("The parameter: " + MsgToSend.sMsg.get_parameter());
                    gateway.send(MsgToSend.sMsg.get_targetid(), MsgToSend.sMsg);
                    Thread.sleep(2000);                 
                } catch (Exception e) { e.printStackTrace(); }
            }
        }
    }

    // Function used to put requests on the queue
    public void add(int targetid, short request, int parameter, String str) {
        DissMsg sMsg = new DissMsg();
        sMsg.set_targetid(targetid);
        sMsg.set_request(request);
        sMsg.set_parameter(parameter);
        queue.add(new MessageToSend(sMsg, str));
    }
}

/**
 * Class used for the structure of the messages
 */
class MessageToSend {
    public DissMsg sMsg;
    public String str;
    
    public MessageToSend(DissMsg sMsg, String str) {
        this.sMsg = sMsg;
        this.str = str;
    }
}

