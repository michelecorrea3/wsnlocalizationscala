/* Philippe Belet, graduate student Artesis, Applied Engineering: Electronics-ICT
 * ICT 7: WSN & DB
 * Project: TelosB WSN module
 */

package parserxmlnew;

import java.util.Date;
import java.text.*;

/**
 *
 * @author philippe
 */
public class xml {
    private int moteID;
    private Date date;
    private double temperature;
    private double humidity;
    private double light;
    private int button;
    private int RSSI;
    private int LQI;
    private int Xcor;
    private int Ycor;
    private int Zcor;
    private int extra1;
    private int extra2;
    private int extra3;
    private String message;

    public xml() {
        moteID = 0;
        date = new Date();
        temperature = 0;
        humidity = 0;
        light = 0;
        button = 0;
        RSSI = 13;
        LQI = 14;
        Xcor = 15;
        Ycor = 16;
        Zcor = 17;
        extra1 = 31;
        extra2 = 32;
        extra3 = 33;
        message = "";
    }

    public int get_moteID() { return moteID; }
    public void set_moteID(int moteID) { this.moteID = moteID; }
    public Date get_date() { return date; }
    public void set_date(Date date) { this.date = date; }
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

    public void parse() {
        // parse xml
        DateFormat dateformat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
        String xmlscheme1  = "<?xml version=\"1.0\" standalone=\"yes\"?>";
        String xmlscheme2  = "<SensorMeasurements xmlns=\"http://tempuri.org/SensorMeasurements.xsd\">";
        String xmlscheme3  = "<Sensor><ID>";
        String xmlscheme4  = Integer.toString(moteID);
        String xmlscheme5  = "</ID><Type>2</Type><Time>";
        String xmlscheme6  = dateformat.format(date);
        String xmlscheme7  = "</Time><Temp>";
        String xmlscheme8  = Double.toString(temperature);
        String xmlscheme9  = "</Temp><Humidity>";
        String xmlscheme10 = Double.toString(humidity);
        String xmlscheme11 = "</Humidity><Light>";
        String xmlscheme12 = Double.toString(light);
        String xmlscheme13 = "</Light><Power></Power><AccX></AccX><AccY></AccY><AccZ></AccZ><TiltX></TiltX><TiltY></TiltY><TiltZ></TiltZ><LED1></LED1><LED2></LED2><LED3></LED3><LED4></LED4><LED5></LED5><LED6></LED6><LED7></LED7><LED8></LED8><Polling></Polling><Button1>";
        String xmlscheme14 = Integer.toString(button);
        String xmlscheme15 = "</Button1><Button2></Button2><RSSI>";
        String xmlscheme16 = Integer.toString(RSSI);
        String xmlscheme17 = "</RSSI><LQI>";
        String xmlscheme18 = Integer.toString(LQI);
        String xmlscheme19 = "</LQI><X>";
        String xmlscheme20 = Integer.toString(Xcor);
        String xmlscheme21 = "</X><Y>";
        String xmlscheme22 = Integer.toString(Ycor);
        String xmlscheme23 = "</Y><Z>";
        String xmlscheme24 = Integer.toString(Zcor);
        String xmlscheme25 ="</Z><extra1>";
        String xmlscheme26 = Integer.toString(extra1);
        String xmlscheme27 = "</extra1><extra2>";
        String xmlscheme28 = Integer.toString(extra2);
        String xmlscheme29 = "</extra2><extra3>";
        String xmlscheme30 = Integer.toString(extra3);
        String xmlscheme31 = "</extra3></Sensor>";
        String xmlscheme32 = "</SensorMeasurements>\r\n";
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
        xmlparsed = xmlparsed.concat(xmlscheme29);
        xmlparsed = xmlparsed.concat(xmlscheme30);
        xmlparsed = xmlparsed.concat(xmlscheme31);
        xmlparsed = xmlparsed.concat(xmlscheme32);
        //de volledige xml boodschap
        message = xmlparsed;
    }
}

