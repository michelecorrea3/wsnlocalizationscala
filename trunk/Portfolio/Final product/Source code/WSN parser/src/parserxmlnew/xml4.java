/* Tim Van Overtveldt, graduate student Artesis, Applied Engineering: Electronics-ICT
 * Master thesis: WSN localization for SCALA
 */

package parserxmlnew;

import java.util.Date;
import java.text.*;

/**
 * Class for the xml scheme of the reply-on-request message
 */
public class xml4 {
    private int moteID;             // ID of the node
    private Date date;              // Time the message arrived
    private int Active;             // The modus of the node (1 = active & 0 = inactive), if the mode is participating in the localization process
    private int AN;                 // Acher node (1 = Anchor node & 0 = Blind node)
    private double X;                  // X coordinate of the note
    private double Y;                  // Y coordinate of the note
    private long Samplerate;         // Sample rate by which reading are taken from the sensors
    private int LocRate;            // The rate by which anchor nodes transmit beacon messages
    private int LEDS;               // Status of the LEds in decimal
    private int powerlevel;         // The power level of the transmissions
    private int Freq;               // The frequency by which packets are sent
    private int Conn;               // Containts the number of motes the BN is connected to
    private String message;         // String for xml message to return
    
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

    //Function to get/set members of the class
    public int get_moteID() { return moteID; }
    public void set_moteID(int moteID) { this.moteID = moteID; }
    public Date get_date() { return date; }
    public void set_date(Date date) { this.date = date; }
    public int get_active() { return Active; }
    public void set_active(int Active) { this.Active = Active; }
    public int get_AN() { return AN; }
    public void set_AN(int AN) { this.AN = AN; }
    public double get_posx() { return X; }
    public void set_posx(int X) { this.X = X; }
    public double get_posy() { return Y; }
    public void set_posy(int Y) { this.Y = Y; }
    public long get_samplerate() { return Samplerate; }
    public void set_samplerate(long Samplerate) { this.Samplerate = Samplerate; }
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

    //Function to create the XML message
    public void parse() {
        double Xcor = X / 100;
        double Ycor = Y / 100;

        DateFormat dateformat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
        String xmlscheme1  = "<?xml version=\"1.0\" standalone=\"yes\"?>";
        String xmlscheme2  = "<WSNReply xmlns=\"http://tempuri.org/WSNActionReq.xsd\">";
        String xmlscheme3  = "<WSNReply><ID>";
        String xmlscheme4  = Integer.toString(moteID);
        String xmlscheme5  = "</ID><Time>";
        String xmlscheme6  = dateformat.format(date);
        String xmlscheme7  = "</Time><active>";
        String xmlscheme8  = Integer.toString(Active);
        String xmlscheme9  = "</active><AN>";
        String xmlscheme10 = Integer.toString(AN);
        String xmlscheme11 = "</AN><X>";
        String xmlscheme12 = Double.toString(Xcor);
        String xmlscheme13 = "</X><Y>";
        String xmlscheme14 = Double.toString(Ycor);
        String xmlscheme15 = "</Y><Samplerate>";
        String xmlscheme16 = Long.toString(Samplerate);
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
