/* Peter De Cauwer & Tim Van Overtveldt, graduate student Artesis, Applied Engineering: Electronics-ICT
 * Master thesis: WSN localization with Senseless
 */

package parserxmlnew;

/**
 * Class used to decode the xml request from the GUI
 */
public class dexml {

       private String message;      // Received message from the controller
       public int targetid;         // ID of the node that needs to get the message
       public short request;        // The request what needs to be changed at the targetid
       public int parameter;        // The parameter of the request

       public boolean pollingS;     // Request to change Sample period
       public boolean pollingL;     // Request to change Location period
       public boolean leds;         // Request to change the Leds (decimal)
       public boolean active;       // Request to change the status (active/nonactive)
       public boolean AN;           // Request to make the node a blind- or anchor node (1 = anchor & 0 = blind)
       public boolean power;        // Request to change the power level
       public boolean frequency;    // Request to change the frequency
       public boolean posx;         // Request to change the X coordinate
       public boolean posy;         // Request to change the Y coordinate
       public boolean status;       // Request to receive the status of the node

       // Value used as request
       public short SAMPLING_PERIOD = 0x22;
       public short LOC_PERIOD = 0x11;
       public short ACTIVE_REQUEST = 0x33;
       public short AN_REQUEST = 0x44;
       public short LED_REQUEST = 0x55;
       public short POWER_REQUEST = 0x77;
       public short FREQUENCY_REQUEST = 0x88;
       public short POSX_REQUEST = 0x99;
       public short POSY_REQUEST = 0xAA;
       public short STATUS_REQUEST = 0xBB;

       // Variables that contain the parameter of the request
       public int pollS;
       public int pollL;
       public int ledr;
       public int activer;
       public int ANr;
       public int Xr;
       public int Yr;
       public int powerr;
       public int frequencyr;
       public int discoveryr;
    
    public dexml() {
        message = "";
        targetid = 0;
        request = 0;
        parameter = 0;

        pollingS = false;
        pollingL = false;
        leds = false;
        active = false;
        AN = false;
        power = false;
        frequency = false;
        posx = false;
        posy = false;
        status = false;

         pollS = 0;
         pollL = 0;
         ledr = 0;
         activer = 0;
         ANr = 0;
         Xr = 0;
         Yr = 0;
         powerr = 0;
         frequencyr = 0;
         discoveryr = 0;
    }

    //Function to get/set members of the class
    public String get_message() { return message; }
    public void set_message(String str) { this.message = str; }
    public int get_targetid() { return targetid; }
    public void set_targetid(int i) { this.targetid = i; }

    
     // Disect the received XML message to check which data needs to be changed
    public void decode() {

        // xmltags to be found
        String tag1 = "<NodeID>";
        String tag2 = "</NodeID>";
        String tag3 = "<active>";
        String tag4 = "</active>";
        String tag5 = "<AN>";
        String tag6 = "</AN>";
        String tag7 = "<X>";
        String tag8 = "</X>";
        String tag9 = "<Samplerate>";
        String tag10 = "</Samplerate>";
        String tag11 = "<LocRate>";
        String tag12 = "</LocRate>";
        String tag13 = "<Leds>";
        String tag14 = "</Leds>";
        String tag15 = "<Power>";
        String tag16 = "</Power>";
        String tag17 = "<Frequency>";
        String tag18 = "</Frequency>";
        String tag19 = "<Discovery>";
        String tag20 = "</Discovery>";
        String tag21 = "<Y>";
        String tag22 = "</Y>";

        // The received XML message transformed to a char array
        char[] msg = message.toCharArray();

        /*
         * Possible requests in the XML message:
         */
        
        //active
        if(message.contains("active")==true)
        {
            if ((message.indexOf(tag4)) != (message.indexOf(tag3) + tag3.length())) {
                active = true;
                char act = msg[message.indexOf(tag3) + tag3.length()];
               activer = Integer.parseInt(String.valueOf(act));
            }
        }

        //AN
        if(message.contains("AN"))
        {
            if ((message.indexOf(tag6)) != (message.indexOf(tag5) + tag5.length())) {
                AN = true;
                char ANN = msg[message.indexOf(tag5) + tag5.length()];
                ANr = Integer.parseInt(String.valueOf(ANN));
            }
        }

        //X
        if(message.contains("X")){
            if ((message.indexOf(tag8)) != (message.indexOf(tag7) + tag7.length())) {
                posx = true;
                int length = message.indexOf(tag8) - message.indexOf(tag7) - tag7.length();
                char[] xcord = new char[length];
                for (int i = 0; i<length; i++)
                {
                    xcord[i] = msg[message.indexOf(tag7)+tag7.length()+i];

                }
                //int Xr_ = Integer.parseInt(String.valueOf(xcord));
                int Xr_ = (int) ( Double.parseDouble(String.valueOf(xcord)) * 100 );
                Xr = Xr_;
            }
            else
                posx = false;
        }

        //Y
        if(message.contains("Y"))
        {
            if ((message.indexOf(tag22)) != (message.indexOf(tag21) + tag21.length())) {
                posy = true;
                int length = message.indexOf(tag22) - message.indexOf(tag21) - tag21.length();
                char[] ycord = new char[length];
                for (int i = 0; i<length; i++)
                {
                    ycord[i] = msg[message.indexOf(tag21)+tag21.length()+i];

                }
                int Yr_ = (int) ( Double.parseDouble(String.valueOf(ycord)) * 100);
                Yr = Yr_;

            }
            else
                posy = false;
        }

        //Sample Rate
        if (message.contains("Samplerate") == true)
        {
                //Samplerate
            if ((message.indexOf(tag10)) != (message.indexOf(tag9) + tag9.length())) {
                pollingS = true;

                int length = message.indexOf(tag10) - message.indexOf(tag9) - tag9.length();
                char[] SSamp = new char[length];
                for (int i = 0; i<length; i++)
                {
                    SSamp[i] = msg[message.indexOf(tag9)+tag9.length()+i];

                }
                int pollS_ = Integer.parseInt(String.valueOf(SSamp));
                pollS = pollS_;
            }
            else
                pollingS = false;
        }
        //Location rate
        if(message.contains("LocRate")){

            if ((message.indexOf(tag12)) != (message.indexOf(tag11) + tag11.length())) {
                pollingL = true;
                int length = message.indexOf(tag12) - message.indexOf(tag11) - tag11.length();
                char[] LSamp = new char[length];
                for (int i = 0; i<length; i++)
                {
                    LSamp[i] = msg[message.indexOf(tag11)+tag11.length()+i];

                }
                int pollL_ = Integer.parseInt(String.valueOf(LSamp));
                pollL = pollL_;
            }
            else
                pollingL = false;

        }

        //Leds
        if (message.contains("Leds") == true)
        {
            if ((message.indexOf(tag14)) != (message.indexOf(tag13) + tag13.length())) {
                leds = true;
                char Led = msg[message.indexOf(tag13) + tag13.length()];
                int Led_ = Integer.parseInt(String.valueOf(Led));
                    ledr = Led_;
            }
            else
                leds = false;
        }

        //Power
        if(message.contains("Power"))
        {
            if ((message.indexOf(tag16)) != (message.indexOf(tag15) + tag15.length())) {
                power = true;

                int length = message.indexOf(tag16) - message.indexOf(tag15) - tag15.length();
                char[] pow = new char[length];
                for (int i = 0; i<length; i++)
                {
                    pow[i] = msg[message.indexOf(tag15)+tag15.length()+i];

                }
                int Powerr_ = Integer.parseInt(String.valueOf(pow));
                powerr = Powerr_;
            }
            else
                power = false;
        }

        //Frequency
        if(message.contains("Frequency"))
        {
            if ((message.indexOf(tag18)) != (message.indexOf(tag17) + tag17.length())) {
                frequency = true;

                int length = message.indexOf(tag18) - message.indexOf(tag17) - tag17.length();
                char[] freq = new char[length];
                for (int i = 0; i<length; i++)
                {
                    freq[i] = msg[message.indexOf(tag17)+tag17.length()+i];

                }
                int freq_ = Integer.parseInt(String.valueOf(freq));
                frequencyr = freq_;
            }
            else
                frequency = false;
        }

        //Discovery/status
        if(message.contains("Discovery"))
        {
           if ((message.indexOf(tag20)) != (message.indexOf(tag19) + tag19.length()))
           {
                status = true;
                char disc = msg[message.indexOf(tag19) + tag19.length()];
                discoveryr = Integer.parseInt(String.valueOf(disc));
            }
           else
               status = false;
        }
        
        // Targetid
             if ((message.indexOf(tag2)) != (message.indexOf(tag1) + tag1.length()))
             {
                int length = message.indexOf(tag2) - message.indexOf(tag1) - tag1.length();
                char[] target = new char[length];
                for (int i = 0; i<length; i++)
                {
                    target[i] = msg[message.indexOf(tag1)+tag1.length()+i];

                }
                int target_ = Integer.parseInt(String.valueOf(target));

        this.targetid = target_;
            }
}
}

