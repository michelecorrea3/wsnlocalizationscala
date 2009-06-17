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
public class xml2 {
    private int moteID;
    private int ANode; //0 = telosb & 1 = sunspot
    private Date date;
    private int RSSI;
    private int Xcor;
    private int Ycor;
    private int Zcor;
    private String message;

    public xml2() {
        moteID = 1;
        ANode = 0;
        date = new Date();
        RSSI = 30;
        Xcor = 13;
        Ycor = 14;
        Zcor = 15;
        message = "";
    }

    
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
    public String get_message() { return message; }
    public void set_message(String message) { this.message = message; }
    


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
        String xmlscheme9  = "</Time><RSSI>";
        String xmlscheme10  = Integer.toString(RSSI);
        String xmlscheme11  = "</RSSI><X>";
        String xmlscheme12 = Integer.toString(Xcor);
        String xmlscheme13 = "</X><Y>";
        String xmlscheme14 = Integer.toString(Ycor);
        String xmlscheme15 = "</Y><Z>";
        String xmlscheme16 = Integer.toString(Zcor);
        String xmlscheme17 = "</Z></Location>";
        String xmlscheme18 = "</LocationMessage>\r\n";
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
        //de volledige xml1 boodschap
        message = xmlparsed;
    }
}
