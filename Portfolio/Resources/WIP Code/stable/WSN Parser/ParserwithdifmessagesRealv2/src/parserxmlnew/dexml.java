/* Philippe Belet, graduate student Artesis, Applied Engineering: Electronics-ICT
 * ICT 7: WSN & DB
 * Project: TelosB WSN module
 */

package parserxmlnew;

/**
 *
 * @author philippe
 */
public class dexml {

       private String message;      //Received message from the controller
       public int targetid;         //ID of the node that needs to get the message
       public short request;        //the request what needs to be changed at the targetid
       public int parameter;        //the parameter of the request

       public boolean pollingS;     //Request to change Sample period?
       public boolean pollingL;     //Request to change Location period?
       public boolean leds;         //Request to change the Leds?
       public boolean active;       //Request to change the status (active/nonactive)
       public boolean AN;           //Request to make the node a blind- or anchor node
       public boolean power;        //Request to change the power level
       public boolean frequency;    //Request to change the frequency
       public boolean posx;
       public boolean posy;
       public boolean status;

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
    
    public String get_message() { return message; }
    public void set_message(String str) { this.message = str; }
    public int get_targetid() { return targetid; }
    public void set_targetid(int i) { this.targetid = i; }

    /**
     * We disect the received stream to check which data needs to be changed
     */
    
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

        char[] msg = message.toCharArray();

        char[] polS = new char[5];      //used to transform the Sample period to the right format
        polS[0] = 'l';
        polS[1] = 'l';
        polS[2] = 'l';
        polS[3] = 'l';
        polS[4] = 'l';
        char[] polL = new char[5];      //used to transform the location period to the right format
        polL[0] = 'l';
        polL[1] = 'l';
        polL[2] = 'l';
        polL[3] = 'l';
        polL[4] = 'l';
        int length_ = 5;
        int number = 0;
        
        // possible requests:
        //Sample Rate
        if (message.contains("Samplerate") == true)
        {
                //Samplerate
            if ((message.indexOf(tag10)) != (message.indexOf(tag9) + tag9.length())) {
                System.out.println("Er staat iets voor de SampleRate");
                pollingS = true;
                for (int i = 0; i<(message.indexOf(tag10) - message.indexOf(tag9) - tag9.length()); i++) {
                    polS[i] = msg[message.indexOf(tag9)+tag9.length()+i];
                }
                for (int i = 0; i<length_; i++) {
                    if (polS[i] == 'l') {
                        number++;
                        polS[i] = '0';
                    }   
                }
                pollS = Integer.parseInt(String.valueOf(polS));
                for (int i = 0; i<number; i++) {
                    pollS = pollS / 10;
                }
                pollS = pollS * 1000; // convert to milliseconds
            }
            else
                pollingS = false;
        }
        //Location rate
        if(message.contains("Locrate")){

                //LocRAte
            if ((message.indexOf(tag12)) != (message.indexOf(tag11) + tag11.length())) {
                System.out.println("Er staat iets voor de LocRate");
                pollingL = true;
                for (int i = 0; i<(message.indexOf(tag12) - message.indexOf(tag11) - tag11.length()); i++) {
                    polL[i] = msg[message.indexOf(tag11)+tag11.length()+i];
                }
                for (int i = 0; i<length_; i++) {
                    if (polL[i] == 'l') {
                        number++;
                        polL[i] = '0';
                    }
                }
                pollL = Integer.parseInt(String.valueOf(polL));
                for (int i = 0; i<number; i++) {
                    pollL = pollL / 10;
                }
                pollL = pollL * 1000; // convert to milliseconds
            }
            else
                pollingL = false;

        }

        //active
        if(message.contains("active")==true)
        {
            if ((message.indexOf(tag4)) != (message.indexOf(tag3) + tag3.length())) {
                System.out.println("Er staat iets voor de active");
                active = true;
                char act = msg[message.indexOf(tag3) + tag3.length()];
               activer = Integer.parseInt(String.valueOf(act));
                
            }
        }

        //AN
        if(message.contains("AN"))
        {
            if ((message.indexOf(tag6)) != (message.indexOf(tag5) + tag5.length())) {
                System.out.println("Er staat iets voor de AN");
                AN = true;
                char ANN = msg[message.indexOf(tag5) + tag5.length()];
                ANr = Integer.parseInt(String.valueOf(ANN));
            }
        }

        //X
        if(message.contains("X")){
            if ((message.indexOf(tag8)) != (message.indexOf(tag7) + tag7.length())) {
                System.out.println("Er staat iets voor de posx");
                posx = true;
                char XX = msg[message.indexOf(tag7) + tag7.length()];
                Xr = Integer.parseInt(String.valueOf(XX));
            }
        }

        //Y
        if(message.contains("Y"))
        {
            if ((message.indexOf(tag22)) != (message.indexOf(tag21) + tag21.length())) {
                System.out.println("Er staat iets voor de posy");
                posy = true;
                char YY = msg[message.indexOf(tag21) + tag21.length()];
                Yr = Integer.parseInt(String.valueOf(YY));
            }
            else
                posy = false;
        }

        //Leds
        if (message.contains("Leds") == true)
        {
            if ((message.indexOf(tag14)) != (message.indexOf(tag13) + tag13.length())) {
                System.out.println("Er staat iets voor de leds");
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
                System.out.println("Er staat iets voor de power");
                power = true;
                char powerc = msg[message.indexOf(tag15) + tag15.length()];
                int powerc_ = Integer.parseInt(String.valueOf(powerc));
                if (powerc_ == 1)
                    powerr = 1;
                else
                    powerr = 0;
            }
            else
                power = false;
        }

        //Frequency
        if(message.contains("Frequency"))
        {
            if ((message.indexOf(tag18)) != (message.indexOf(tag17) + tag17.length())) {
                System.out.println("Er staat iets voor de frequency");
                frequency = true;
                char freq = msg[message.indexOf(tag17) + tag17.length()];
                int freq_ = Integer.parseInt(String.valueOf(freq));
                if (freq_ == 1)
                    frequencyr = 1;
                else
                    frequencyr = 0;
            }
            else
                frequency = false;
        }

        //Discovery/status
        if(message.contains("Discovery"))
        {
           if ((message.indexOf(tag20)) != (message.indexOf(tag19) + tag19.length())) {
               System.out.println("Er staat iets voor de discovery");
                status = true;
                char disc = msg[message.indexOf(tag21) + tag19.length()];
                discoveryr = Integer.parseInt(String.valueOf(disc));
            }
           else
               status = false;
        }
        
        // determine targetid
        char target = msg[message.indexOf(tag1) + tag1.length()];
        int target_ = Integer.parseInt(String.valueOf(target));
        this.targetid = target_;
    }
}

