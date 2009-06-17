/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package parserxmlnew;

import java.util.Date;
import java.text.*;

/**
 *
 * @author Possey
 *
 * type 1 sens message
 * type 2 loc message
 * type 3 status
 */
public class xml3 {
    private int moteID;
    private Date date;
    private boolean Active;
    private boolean AN;
    private int X;
    private int Y;
    private int Samplerate;
    private int LocRate;
    private int LEDS;
    private int powerlevel;
    private int Freq;
    private int Conn;
    private String message;

    public xml3() {
        moteID = 1;
        date = new Date();
        Active = true;
        AN = true;
        X = 40;
        Y = 50;
        Samplerate = 125;
        LocRate = 225;
        LEDS = 5;
        powerlevel = 2;
        Freq = 24;
        Conn = 3;
        message = "";
    }
/*
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
 */

    // added
    public void set_date(Date date) { this.date = date; }
    public String get_message() { return message; }
    // added

    public void parse() {
        // parse xml3
        String actief = new Boolean(Active).toString();
        String Anchor = new Boolean(AN).toString();

        DateFormat dateformat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
        String xmlscheme1  = "<?xml version=\"1.0\" standalone=\"yes\"?>";
        String xmlscheme2  = "<StatusMessage xmlns=\"http://tempuri.org/StatusMessage.xsd\">";
        String xmlscheme3  = "<Status><ID>";
        String xmlscheme4  = Integer.toString(moteID);
        String xmlscheme5  = "</ID><Time>";
        String xmlscheme6  = dateformat.format(date);
        String xmlscheme7  = "</Time><Active>";
        String xmlscheme8  = actief;
        String xmlscheme9  = "</Active><AN>";
        String xmlscheme10 = Anchor;
        String xmlscheme11 = "</AN><X>";
        String xmlscheme12 = Integer.toString(X);
        String xmlscheme13 = "</X><Y>";
        String xmlscheme14 = Integer.toString(Y);
        String xmlscheme15 = "</Y><SampleRate>";
        String xmlscheme16 = Integer.toString(Samplerate);
        String xmlscheme17 = "</SampleRate><LocRate>";
        String xmlscheme18 = Integer.toString(LocRate);
        String xmlscheme19 = "</LocRate><Leds>";
        String xmlscheme20 = Integer.toString(LEDS);
        String xmlscheme21 = "</Leds><Power>";
        String xmlscheme22 = Integer.toString(powerlevel);
        String xmlscheme23 = "</Power><Frequency>";
        String xmlscheme24 = Integer.toString(Freq);
        String xmlscheme25 = "</Frequency><Conn>";
        String xmlscheme26 = Integer.toString(Conn);
        String xmlscheme27 = "</Conn></Status>";
        String xmlscheme28 = "</StatusMessage>\r\n";
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
