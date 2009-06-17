/* Philippe Belet, graduate student Artesis, Applied Engineering: Electronics-ICT
 * ICT 7: WSN & DB
 * Project: TelosB WSN module
 */

package parserxmlnew;

import java.util.*;
import net.tinyos.message.*;
/**
 *
 * @author philippe
 */
public class Serialsender extends Thread {
    private LinkedList queue;
    private MessageToSend MsgToSend;
    private MoteIF gateway;
    
    public Serialsender(MoteIF gateway) {
        queue = new LinkedList();
        this.gateway = gateway;
    }
    
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
                    System.out.println(MsgToSend.sMsg.get_targetid());
                    System.out.println(MsgToSend.sMsg.get_request());
                    System.out.println(MsgToSend.sMsg.get_parameter());
                    gateway.send(MsgToSend.sMsg.get_targetid(), MsgToSend.sMsg);
                    Thread.sleep(2000);                 
                } catch (Exception e) { e.printStackTrace(); }
            }
        }
    }
    
    public void add(int targetid, short request, int parameter, String str) {
        BN_DissMsg sMsg = new BN_DissMsg();
        sMsg.set_targetid(targetid);
        sMsg.set_request(request);
        sMsg.set_parameter(parameter);
        queue.add(new MessageToSend(sMsg, str));
    }
}

class MessageToSend {
    public BN_DissMsg sMsg;
    public String str;
    
    public MessageToSend(BN_DissMsg sMsg, String str) {
        this.sMsg = sMsg;
        this.str = str;
    }
}

