/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 * Java versie onvereenkomend met vmware linux (ubuntos)
 *
 * Versie 5 met goede java versie, en nieuw XML schema
 *
 */

package parserxmlnew;
import java.io.IOException;
import net.tinyos.message.*;

/**
 *
 * @author philippe
 */
public class Main {

    private static MoteIF gateway;
    private static Serialsender Sender;
    //private static Requesthandler Handler;
    /**
     * @param args the command line arguments
     */

    private static void usage() {
        System.out.println("usage: [-comm <source>]");
    }

    private static void startrunning() throws IOException {
        // declare threads which manage the serial connection
        Sender = new Serialsender(gateway);
        //Handler = new Requesthandler(Sender);
        Thread Scouter = new Thread(new Serial(gateway));
        Thread Handler = new Thread(new Requesthandler(Sender));

        Scouter.start();
        Sender.start();
        Handler.start();
        System.out.println("Threads started");
        //Handler.test();

        // manage request for the WSN
        /*while (true) {
            msg = new String();
            msg = Handler.readConnection();
            Handler.handle(msg);
        }*/
    }

    public static void main(String[] args) throws IOException {

        // start
        String source = "";
        if (args.length == 2)
            source = args[1];
        else
        {
            usage();
            System.exit(1);
        }
        gateway = new MoteIF();
        if (gateway == null)
        {
            System.out.println("gateway == null");
            System.exit(1);
        }
        System.out.println("Startrunning()");
        startrunning();
        }
}
