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
 * type 3 status message (if node is rebooted)
 * type 4 Discovery message (reply to request)
 */
public class xml4 {
    private int moteID;
    private Date date;
    private int Active;
    private int AN;
    private int X;
    private int Y;
    private int Samplerate;
    private int LocRate;
    private int LEDS;
    private int powerlevel;
    private int Freq;
    private int Conn;
    private String message;

    public xml4() {
        moteID = 1;
        date = new Date();
        Active = 1;
        AN = 1;
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

    public int get_moteID() { return moteID; }
    public void set_moteID(int moteID) { this.moteID = moteID; }
    public Date get_date() { return date; }
    public void set_date(Date date) { this.date = date; }
    public int get_active() { return Active; }
    public void set_active(int Active) { this.Active = Active; }
    public int get_AN() { return AN; }
    public void set_AN(int AN) { this.AN = AN; }
    public int get_posx() { return X; }
    public void set_posx(int X) { this.X = X; }
    public int get_posy() { return Y; }
    public void set_posy(int Y) { this.Y = Y; }
    public int get_samplerate() { return Samplerate; }
    public void set_samplerate(int Samplerate) { this.Samplerate = Samplerate; }
    public int get_locrate() { return LocRate; }
    public void set_locrate(int LocRate) { this.LocRate = LocRate; }
    public int get_leds() { return LEDS; }
    public void set_leds(int LEDS) { this.LEDS = LEDS; }
    public int get_powerlevel() { return powerlevel; }
    public void set_powerlevel(int powerlevel) { this.powerlevel = powerlevel; }
    public int get_frequency() { return Freq; }
    public void set_frequency(int Freq) { this.Freq = Freq; }
    public int get_connection() { return Conn; }
    public void set_connection(int Conn) { this.Conn = Conn; }
    public String get_message() { return message; }
    public void set_message(String message) { this.message = message; }

    public void parse() {
        // parse xml3

        //DateFormat dateformat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
        String xmlscheme1  = "<?xml version=\"1.0\" standalone=\"yes\"?>";
        String xmlscheme2  = "<WSNReply xmlns=\"http://tempuri.org/WSNActionReq.xsd\">";
        String xmlscheme3  = "<WSNReply><NodeID>";
        String xmlscheme4  = Integer.toString(moteID);
        String xmlscheme5  = "</NodeID><active>";
        String xmlscheme8  = Integer.toString(Active);
        String xmlscheme9  = "</active><AN>";
        String xmlscheme10 = Integer.toString(AN);
        String xmlscheme11 = "</AN><X>";
        String xmlscheme12 = Integer.toString(X);
        String xmlscheme13 = "</X><Y>";
        String xmlscheme14 = Integer.toString(Y);
        String xmlscheme15 = "</Y><Samplerate>";
        String xmlscheme16 = Integer.toString(Samplerate);
        String xmlscheme17 = "</Samplerate><LocRate>";
        String xmlscheme18 = Integer.toString(LocRate);
        String xmlscheme19 = "</LocRate><Leds>";
        String xmlscheme20 = Integer.toString(LEDS);
        String xmlscheme21 = "</Leds><Power>";
        String xmlscheme22 = Integer.toString(powerlevel);
        String xmlscheme23 = "</Power><Frequency>";
        String xmlscheme24 = Integer.toString(Freq);
        String xmlscheme25 = "</Frequency><Conn>";
        String xmlscheme26 = Integer.toString(Conn);
        String xmlscheme27 = "</Conn></WSNReply>";
        String xmlscheme28 = "</WSNReply>\r\n";
        String xmlparsed = "";

        //concat methode voegt 2 of meerdere arrays samen
        xmlparsed = xmlparsed.concat(xmlscheme1);
        xmlparsed = xmlparsed.concat(xmlscheme2);
        xmlparsed = xmlparsed.concat(xmlscheme3);
        xmlparsed = xmlparsed.concat(xmlscheme4);
        xmlparsed = xmlparsed.concat(xmlscheme5);
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
