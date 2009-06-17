/* Peter De Cauwer & Tim Van Overtveldt, graduate student Artesis, Applied Engineering: Electronics-ICT
 * Master thesis: WSN localization with Senseless
 */

package parserxmlnew;

import java.io.*;
import java.util.*;
import java.net.*;
import net.tinyos.message.*;

/**
 * Class that listens to the different messages of the WSN on the serial port
 * It also transmits the data of the messages in the appropriate xml format to the controller
 */
public class Serial implements Runnable, MessageListener
{
    private MoteIF gateway;                                     // Class Moteif to send/receive
    private Socket requestSocket;                               // Socket to send to the controller
    private OutputStreamWriter wr;                              // Class OutputStreamWriter for sending messages to the controller
    private int port;                                           // Port to send to
    private String host;                                        // Address to send to
    sharedata statusmsg;                                        // Variable to share data between different threads

    // Function to initialize moteif class, port, ip and the class sharedata
    //public Serial(MoteIF gateway, sharedata s, MyTimerTask mytask)
    public Serial(MoteIF gateway, sharedata s)
    {
        this.gateway = gateway;
        this.host = connectiondetails.IP;
        this.port = connectiondetails.Sendport;
        this.statusmsg = s;
    }

    // Function used to register listeners for different messsages
    public void run()
    {
        this.gateway.registerListener(new CollMsg(), this);
        this.gateway.registerListener(new StatMsg(), this);
        this.gateway.registerListener(new LocMsg(), this);
        while (true) {}
    }

    // Function to convert the unit of temperature
    private double convertTemperature(int temp)
    {
        double ap = 0.0098 * temp + 234.75;
        return ap;
    }

    // Function to convert the unit of humidty
    private double convertHumidity(int hum)
    {
        double ap = -0.0000028*hum*hum + 0.0405*hum-4;
        return ap;
    }

    // Function to convert light intensity
    private double convertLight(int light)
    {
        //double ap = 0.625 * 1000000 * 1000 * (light/100000);
        return (double)light;
    }

    // Function to convert the status of the button to an integer
    private int convertButton(byte but)
    {
        return (int)but;
    }

    // Function to convert the status "active" to an integer
    private int convertActive(byte act)
    {
      return (int)act;
    }

    // Function to convert the status "AN" to an integer
    private int convertAN(byte AN)
    {
      return (int)AN;
    }

    // Function to create socket and associated OutputStreamWriter to send to the controller
    private void createClient() throws UnknownHostException, IOException
    {
	    System.out.println("Created socket on port " + connectiondetails.Sendport);
        this.requestSocket = new Socket(this.host, this.port);
        this.wr = new OutputStreamWriter(this.requestSocket.getOutputStream());
    }
    
    // Function used for when the serial interface receives a message
    public void messageReceived(int dest_addr, Message message)
    {
 
        if (message instanceof CollMsg)
        {
            CollMsg CollectedMsg = (CollMsg)message;      
            // generate timestamp
            Date date = new Date();
            
            // xmlstring
            String xmlString = "";
            
            System.out.println("received sensormeasuremnts message");
            System.out.println("Mote id: " + CollectedMsg.get_moteid());
            System.out.println("battery: " + CollectedMsg.get_battery());
            System.out.println("Light: " + convertLight(CollectedMsg.get_lightreading()));
            System.out.println("Temperature: " + convertTemperature(CollectedMsg.get_tempreading()));
            System.out.println("Humidity: " + convertHumidity(CollectedMsg.get_humidityreading()));
            System.out.println("Button pressed: " + convertButton(CollectedMsg.get_button()));
            
            // fill up xml1
            xml1 xmlParser1 = new xml1();
            xmlParser1.set_moteID(CollectedMsg.get_moteid());
            xmlParser1.set_battery(CollectedMsg.get_battery());
            xmlParser1.set_temperature(convertTemperature(CollectedMsg.get_tempreading()));
            xmlParser1.set_humidity(convertHumidity(CollectedMsg.get_humidityreading()));
            xmlParser1.set_light(convertLight(CollectedMsg.get_lightreading()));
            xmlParser1.set_button(convertButton(CollectedMsg.get_button()));
            xmlParser1.set_date(date);
            xmlParser1.parse();
            xmlString = xmlParser1.get_message();

            // send to controller
            try
            {
                createClient();
                wr.write(xmlString);
                wr.flush();
                System.out.println("Sensor message sent to controller");
            } catch (IOException e) { e.printStackTrace(); }
        }
        else 
        if (message instanceof LocMsg)
        {
            LocMsg LocationMsg = (LocMsg)message;  
            // generate timestamp
            Date date = new Date();
            
            // xmlstring
            String xmlString2 = "";

            System.out.println("received location message");
            System.out.println("Mote id: " + LocationMsg.get_moteid());
            System.out.println("ANmoteid: " + LocationMsg.get_ANmoteid());
            System.out.println("RSSI: " + (LocationMsg.get_RSSI() - 45) );
            System.out.println("HopCount : " + LocationMsg.get_hopCount());
            System.out.println("VAN of source : " + LocationMsg.get_VANs());
            System.out.println("VAN of receiver : " + LocationMsg.get_VANr());
            
            // fill up xml2
            xml2 xmlParser2 = new xml2();
            xmlParser2.set_moteID(LocationMsg.get_moteid());
            xmlParser2.set_ANode(LocationMsg.get_ANmoteid());
            xmlParser2.set_RSSI(LocationMsg.get_RSSI() -45);
            xmlParser2.set_hopcount(LocationMsg.get_hopCount());
            xmlParser2.set_VANr(LocationMsg.get_VANr());
            xmlParser2.set_VANs(LocationMsg.get_VANs());
            xmlParser2.set_date(date);
            xmlParser2.parse();
            xmlString2 = xmlParser2.get_message();

            // send to controller
            try
            {
                createClient();
                wr.write(xmlString2);
                wr.flush();
                System.out.println("location message sent to controller");
            } catch (IOException e) { e.printStackTrace(); }
        }
        else if (message instanceof StatMsg)
        {
            StatMsg StatusMsg = (StatMsg)message;
            // generate timestamp
            Date date = new Date();

            // xmlstring
            String xmlString3 = "";

            System.out.println("received status/reply message");
            System.out.println("Mote id: " + StatusMsg.get_moteid());
            System.out.println("type: " + StatusMsg.get_type());
            System.out.println("active: " + convertActive(StatusMsg.get_active()));
            System.out.println("AN: " + convertAN(StatusMsg.get_AN()));
            System.out.println("Posx: " + StatusMsg.get_posx());
            System.out.println("Posy: " + StatusMsg.get_posy());
            System.out.println("SampleRate: " + StatusMsg.get_sampleRate());
            System.out.println("LocRate: " + StatusMsg.get_locRate());
            System.out.println("LEDS: " + StatusMsg.get_leds());
            System.out.println("Power level of trans.: " + StatusMsg.get_power());
            System.out.println("Frequency: " + StatusMsg.get_frequency());
            System.out.println("Connection: " + StatusMsg.get_conn());

            if(statusmsg.get_waiting() == false)
            {
                // fill up xml3
                xml3 xmlParser3 = new xml3();
                xmlParser3.set_moteID(StatusMsg.get_moteid());
                xmlParser3.set_active(convertActive(StatusMsg.get_active()));
                xmlParser3.set_AN(convertAN(StatusMsg.get_AN()));
                xmlParser3.set_posx(StatusMsg.get_posx());
                xmlParser3.set_posy(StatusMsg.get_posy());
                xmlParser3.set_samplerate(StatusMsg.get_sampleRate());
                xmlParser3.set_locrate(StatusMsg.get_locRate());
                xmlParser3.set_leds((StatusMsg.get_leds()));
                xmlParser3.set_powerlevel(StatusMsg.get_power());
                xmlParser3.set_frequency(StatusMsg.get_frequency());
                xmlParser3.set_connection(StatusMsg.get_conn());
                xmlParser3.set_date(date);
                xmlParser3.parse();
                xmlString3 = xmlParser3.get_message();
                // send to controller
                try
                {
                    createClient();
                    wr.write(xmlString3);
                    wr.flush();
                    System.out.println("status (reboot)");
                } catch (IOException e) { e.printStackTrace(); }

                
            } else if(statusmsg.get_waiting() == true) //Reply on request
            {
                xml4 xmlParser3 = new xml4();
                xmlParser3.set_moteID(StatusMsg.get_moteid());
                xmlParser3.set_active(convertActive(StatusMsg.get_active()));
                xmlParser3.set_AN(convertAN(StatusMsg.get_AN()));
                xmlParser3.set_posx(StatusMsg.get_posx());
                xmlParser3.set_posy(StatusMsg.get_posy());
                xmlParser3.set_samplerate(StatusMsg.get_sampleRate());
                xmlParser3.set_locrate(StatusMsg.get_locRate());
                xmlParser3.set_leds((StatusMsg.get_leds()));
                xmlParser3.set_powerlevel(StatusMsg.get_power());
                xmlParser3.set_frequency(StatusMsg.get_frequency());
                xmlParser3.set_connection(StatusMsg.get_conn());
                xmlParser3.set_date(date);
                xmlParser3.parse();
                xmlString3 = xmlParser3.get_message();

                statusmsg.set_status(xmlString3);
            }
        }
 }
}



