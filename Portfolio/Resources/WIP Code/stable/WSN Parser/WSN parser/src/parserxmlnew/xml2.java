/* Peter De Cauwer & Tim Van Overtveldt, graduate student Artesis, Applied Engineering: Electronics-ICT
 * Master thesis: WSN localization with Senseless
 */

package parserxmlnew;

import java.util.Date;
import java.text.*;

/**
 * Class for the xml scheme of the location message
 */
public class xml2 {
    private int moteID;         // ID of the node
    private int ANode;          // Acher node (1 = Anchor node & 0 = Blind node)
    private Date date;          // Time the message arrived
    private int RSSI;           // The Received Signal Strength Indication
    private int hopcount;       // The hopcount
    private int VANs;           // The VAN grade of the source
    private int VANr;           // The VAN grade of the receiver
    private int Xcor;           // X coordinate of the blind note
    private int Ycor;           // Y coordinate of the blind note
    private int Zcor;           // Z coordinate of the blind note
    private String message;     // String for xml message to return

    public xml2() {
        moteID = 1;
        ANode = 0;
        date = new Date();
        RSSI = 30;
        hopcount = 0;
        VANs = 0;
        VANr = 0;
        Xcor = 13;
        Ycor = 14;
        Zcor = 15;
        message = "";
    }

    //Function to get/set members of the class
    public int get_moteID() { return moteID; }
    public void set_moteID(int moteID) { this.moteID = moteID; }
    public int get_ANode() { return ANode; }
    public void set_ANode(int ANode) { this.ANode = ANode; }
    public Date get_date() { return date; }
    public void set_date(Date date) { this.date = date; }
    public int get_posx() { return Xcor; }
    public void set_posx(int Xcor) { this.Xcor = Xcor; }
    public int get_posy() { return Ycor; }
    public void set_posy(int Ycor) { this.Ycor = Ycor; }
    public int get_RSSI() { return RSSI; }
    public void set_RSSI(int RSSI) { this.RSSI = RSSI; }
    public int get_hopcount() { return hopcount; }
    public void set_hopcount(int hopcount) { this.hopcount = hopcount; }
    public int get_VANs() { return VANs; }
    public void set_VANs(int VANs) { this.VANs = VANs; }
    public int get_VANr() { return VANr; }
    public void set_VANr(int VANs) { this.VANr = VANs; }
    public String get_message() { return message; }
    public void set_message(String message) { this.message = message; }
    

    //Function to create the XML message
    public void parse() {
        // parse xml2
        DateFormat dateformat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
        String xmlscheme1  = "<?xml version=\"1.0\" standalone=\"yes\"?>";
        String xmlscheme2  = "<LocationMessage xmlns=\"http://tempuri.org/LocationMessage.xsd\">";
        String xmlscheme3  = "<Location><ID>";
        String xmlscheme4  = Integer.toString(moteID);
        String xmlscheme5  = "</ID><ANode>";
        String xmlscheme6  = Integer.toString(ANode);
        String xmlscheme7  = "</ANode><Time>";
        String xmlscheme8  = dateformat.format(date);
        String xmlscheme9  = "</Time><VANs>";
        String xmlscheme10 = Integer.toString(VANs);
        String xmlscheme11 = "</VANs><VANr>";
        String xmlscheme12 = Integer.toString(VANr);
        String xmlscheme13 = "</VANr><RSSI>";
        String xmlscheme14  = Integer.toString(RSSI);
        String xmlscheme15  = "</RSSI><X>";
        String xmlscheme16 = Integer.toString(Xcor);
        String xmlscheme17 = "</X><Y>";
        String xmlscheme18 = Integer.toString(Ycor);
        String xmlscheme19 = "</Y><Z>";
        String xmlscheme20 = Integer.toString(Zcor);
        String xmlscheme21 = "</Z></Location>";
        String xmlscheme22 = "</LocationMessage>\r\n";
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
        //de volledige xml1 boodschap
        message = xmlparsed;
    }
}
