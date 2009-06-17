/* Philippe Belet, graduate student Artesis, Applied Engineering: Electronics-ICT
 * ICT 7: WSN & DB
 * Project: TelosB WSN module
 */

package parserxmlnew;

import java.io.*;
import java.util.*;
import java.net.*;
import net.tinyos.message.*;

/**
 *
 * @author philippe
 */
public class Serial implements Runnable, MessageListener
{
    private MoteIF gateway;
    private Socket requestSocket;
    private OutputStreamWriter wr;
    private int port;
    private String host;

    public Serial(MoteIF gateway)
    {
        this.gateway = gateway;
        //this.host = "192.168.1.100";
        this.host = connectiondetails.IP;
        //this.port = 3800;
        //this.port = Integer.getInteger(connectiondetails.Sendport);
        //this.port = Integer.parseInt(connectiondetails.Sendport);
        this.port = connectiondetails.Sendport;
    }
    
    public void run()
    {
        this.gateway.registerListener(new CollMsg(), this);
        //this.gateway.registerListener(new StatMsg(), this);
        this.gateway.registerListener(new LocMsg(), this);
        while (true) {}
    }
    
    private double convertTemperature(int temp)
    {
        //double ap = (((temp/4096*1.2) - 0.986)/0.00355)+273.15;
        double ap = 0.0098 * temp + 234.75;
        return ap;
    }
    
    private double convertHumidity(int hum)
    {
        double ap = -0.0000028*hum*hum + 0.0405*hum-4;
        return ap;
    }
    
    private double convertLight(int light)
    {
        //double ap = 0.625 * 1000000 * 1000 * (light/100000);
        return (double)light;
    }
    
    private int convertButton(byte but)
    {
        return (int)but;
    }
    
    private int convertActive(byte act)
    {
      return (int)act;
    }
    
    private int convertAN(byte AN)
    {
      return (int)AN;
    }
    
    private void createClient() throws UnknownHostException, IOException
    {
	    System.out.println("Created socket on port " + connectiondetails.Sendport);
        this.requestSocket = new Socket(this.host, this.port);
        this.wr = new OutputStreamWriter(this.requestSocket.getOutputStream());
    }
    
    // when serial interface receives a message
    public void messageReceived(int dest_addr, Message message)
    {
        // if it's a datapacket
 
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
            
            // detect corrupt measurements (with dummyvalue humidity is negative)
            if (convertHumidity(CollectedMsg.get_humidityreading()) < 0)
                System.out.println("corrupt message");
            // when not corrupt, forward
            else
            {
            
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
            System.out.println("posx: " + LocationMsg.get_posx());
            System.out.println("Posy: " + LocationMsg.get_posy());
            System.out.println("RSSI: " + LocationMsg.get_RSSI());
            
            
                // fill up xml2
                xml2 xmlParser2 = new xml2();
                xmlParser2.set_moteID(LocationMsg.get_moteid());
                xmlParser2.set_ANode(LocationMsg.get_ANmoteid());
                xmlParser2.set_posx(LocationMsg.get_posx());
                xmlParser2.set_posy(LocationMsg.get_posy());
                xmlParser2.set_RSSI(LocationMsg.get_RSSI());
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


            System.out.println("received status message");
            System.out.println("Mote id: " + StatusMsg.get_moteid());
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
                    System.out.println("status message sent to controller");
                } catch (IOException e) { e.printStackTrace(); }

        }
 }
}

