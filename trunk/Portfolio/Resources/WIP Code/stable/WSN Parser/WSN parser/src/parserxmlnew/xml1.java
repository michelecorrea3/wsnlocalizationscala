/* Peter De Cauwer & Tim Van Overtveldt, graduate student Artesis, Applied Engineering: Electronics-ICT
 * Master thesis: WSN localization with Senseless
 */

package parserxmlnew;

import java.util.Date;
import java.text.*;

/**
 * Class for the xml scheme of the sensormeasurements message
 */
public class xml1 {
    private int moteID;             // ID of the node
    private int typeofnode;         // Type of node (0 = telosb & 1 = sunspot)
    private Date date;              // Time the message arrived
    private double temperature;     // Temperature reading
    private double humidity;        // Humidity reading
    private double light;           // Light intensity reading
    private int Battery;            // Voltage of the telos rev. B
    private char LED1;              // Status of Led1
    private char LED2;              // Status of Led2
    private char LED3;              // Status of Led3
    private int Polling;            // Sampling period
    private int button;             // Status of the button
    private String message;         // String for xml message to return

    public xml1() {
        moteID = 0;
        typeofnode = 0;
        date = new Date();
        temperature = 0;
        humidity = 0;
        light = 0;
        Battery = 100;
        LED1 = 'N';
        LED2 = 'N';
        LED3 = 'N';
        Polling = 125;
        button = 0;
        message = "";
    }

    //Function to get/set members of the class
    public int get_moteID() { return moteID; }
    public void set_moteID(int moteID) { this.moteID = moteID; }
    public Date get_date() { return date; }
    public void set_date(Date date) { this.date = date; }
    public int get_battery() { return Battery; }
    public void set_battery(int Battery) { this.Battery = Battery; }
    public double get_temperature() { return temperature; }
    public void set_temperature(double temperature) { this.temperature = temperature; }
    public double get_humidity() { return humidity; }
    public void set_humidity(double humidity) { this.humidity = humidity; }
    public double get_light() { return light; }
    public void set_light(double light) { this.light = light; }
    public int get_button() { return button; }
    public void set_button(int button) { this.button = button; }
    public String get_message() { return message; }
    public void set_message(String message) { this.message = message; }

    //Function to create the XML message
    public void parse() {
        // parse xml1
        DateFormat dateformat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
        String xmlscheme1  = "<?xml version=\"1.0\" standalone=\"yes\"?>";
        String xmlscheme2  = "<SensorMeasurements xmlns=\"http://tempuri.org/SensorMeasurements.xsd\">";
        String xmlscheme3  = "<Sensor><ID>";
        String xmlscheme4  = Integer.toString(moteID);
        String xmlscheme5  = "</ID><Type>";
        String xmlscheme6  = Integer.toString(typeofnode);
        String xmlscheme7  = "</Type><Time>";
        String xmlscheme8  = dateformat.format(date);
        String xmlscheme9  = "</Time><Temp>";
        String xmlscheme10  = Double.toString(temperature);
        String xmlscheme11  = "</Temp><Humidity>";
        String xmlscheme12 = Double.toString(humidity);
        String xmlscheme13 = "</Humidity><Light>";
        String xmlscheme14 = Double.toString(light);
        String xmlscheme15 = "</Light><Power>";
        String xmlscheme16 = Integer.toString(Battery);
        String xmlscheme17 = "</Power><AccX></AccX><AccY></AccY><AccZ></AccZ><TiltX></TiltX><TiltY></TiltY><TiltZ></TiltZ><LED1>";
        String xmlscheme18 = Character.toString(LED1);
        String xmlscheme19 = "</LED1><LED2>";
        String xmlscheme20 = Character.toString(LED2);
        String xmlscheme21 = "</LED2><LED3>";
        String xmlscheme22 = Character.toString(LED3);
        String xmlscheme23 = "</LED3><LED4></LED4><LED5></LED5><LED6></LED6><LED7></LED7><LED8></LED8><Polling>";
        String xmlscheme24 = Integer.toString(Polling);
        String xmlscheme25 = "</Polling><Button1>";
        String xmlscheme26 = Integer.toString(button);
        String xmlscheme27 = "</Button1><Button2></Button2></Sensor>";
        String xmlscheme28 = "</SensorMeasurements>\r\n";
        String xmlparsed = "";

        //concat methode voegt 2 of meerdere arrays samen
        xmlparsed = xmlparsed.concat(xmlscheme1);
        xmlparsed = xmlparsed.concat(xmlscheme2);
        xmlparsed = xmlparsed.concat(xmlscheme3);
        xmlparsed = xmlparsed.concat(xmlscheme4);
        xmlparsed = xmlparsed.concat(xmlscheme5);
        xmlparsed = xmlparsed.concat(xmlscheme6);
        xmlparsed = xmlparsed.concat(xmlscheme7);
        xmlparsed = xmlparsed.concat(xmlscheme8);
        xmlparsed = xmlparsed.concat(xmlscheme9);
        xmlparsed = xmlparsed.concat(xmlscheme10);
        xmlparsed = xmlparsed.concat(xmlscheme11);
        xmlparsed = xmlparsed.concat(xmlscheme12);
        xmlparsed = xmlparsed.concat(xmlscheme13);
        xmlparsed = xmlparsed.concat(xmlscheme14);
        xmlparsed = xmlparsed.concat(xmlscheme15);
        xmlparsed = xmlparsed.concat(xmlscheme16);
        xmlparsed = xmlparsed.concat(xmlscheme17);
        xmlparsed = xmlparsed.concat(xmlscheme18);
        xmlparsed = xmlparsed.concat(xmlscheme19);
        xmlparsed = xmlparsed.concat(xmlscheme20);
        xmlparsed = xmlparsed.concat(xmlscheme21);
        xmlparsed = xmlparsed.concat(xmlscheme22);
        xmlparsed = xmlparsed.concat(xmlscheme23);
        xmlparsed = xmlparsed.concat(xmlscheme24);
        xmlparsed = xmlparsed.concat(xmlscheme25);
        xmlparsed = xmlparsed.concat(xmlscheme26);
        xmlparsed = xmlparsed.concat(xmlscheme27);
        xmlparsed = xmlparsed.concat(xmlscheme28);
        //de volledige xml1 boodschap
        message = xmlparsed;
    }
}

