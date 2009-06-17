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
    private String message;
    private int targetid;
    private short request;
    private int parameter;
    
    public dexml() {
        message = "";
        targetid = 0;
        request = 0;
        parameter = 0;
    }
    
    public String get_message() { return message; }
    public void set_message(String str) { this.message = str; }
    public int get_targetid() { return targetid; }
    public void set_targetid(int i) { this.targetid = i; }
    public short get_request() { return request; }
    public void set_request(short s) { this.request = s; }
    public int get_parameter() { return parameter; }
    public void set_parameter(int i) { this.parameter = i; }
    
    public void decode() {
        // xmltags to be found
        String tag1 = "<polling>";
        String tag2 = "</polling>";
        String tag3 = "<LED1>";
        String tag4 = "</LED1>";
        String tag5 = "<LED2>";
        String tag6 = "</LED2>";
        String tag7 = "<LED3>";
        String tag8 = "</LED3>";
        String tag9 = "<NodeID>";
        //String tag10 = "</NodeID>";
        String tag11 = "<LED1 />";
        
        boolean polling = false;
        boolean leds1 = false;
        boolean leds2 = false;
        boolean leds3 = false;
        char[] msg = message.toCharArray();
        char[] pol = new char[5];
        pol[0] = 'l';
        pol[1] = 'l';
        pol[2] = 'l';
        pol[3] = 'l';
        pol[4] = 'l';
        int length_ = 5;
        int number = 0;
        int poll = 0;
        int ledr = 0;
        
        // possible requests
        short CHANGE_SAMPLING_PERIOD = 0xAA;
        short LED_REQUEST = 0x55;
        //short SLEEP_REQUEST = 0x66; // unused
        
        // determine request
        // when message contains empty led field => polling request
        if (message.contains(tag11)) {
            // when polling is not empty => polling request
            if ((message.indexOf(tag2)) != (message.indexOf(tag1) + tag1.length())) {
                polling = true;
                for (int i = 0; i<(message.indexOf(tag2) - message.indexOf(tag1) - tag1.length()); i++) {
                    pol[i] = msg[message.indexOf(tag1)+tag1.length()+i];
                }
                for (int i = 0; i<length_; i++) {
                    if (pol[i] == 'l') {
                        number++;
                        pol[i] = '0';
                    }   
                }
                poll = Integer.parseInt(String.valueOf(pol));
                for (int i = 0; i<number; i++) {
                    poll = poll / 10;
                }
                poll = poll * 1000; // convert to milliseconds
            }
        }
        else {
            // when leds are not empty => led request
            // led1
            if ((message.indexOf(tag4)) != (message.indexOf(tag3) + tag3.length())) {
                char led = msg[message.indexOf(tag3) + tag3.length()];
                int led_ = Integer.parseInt(String.valueOf(led));
                if (led_ == 1)
                    leds1 = true;
                else
                    leds1 = false;
            }
            
            // led2
            if ((message.indexOf(tag6)) != (message.indexOf(tag5) + tag5.length())) {
                char led = msg[message.indexOf(tag5) + tag5.length()];
                int led_ = Integer.parseInt(String.valueOf(led));
                if (led_ == 1)
                    leds2 = true;
                else
                    leds2 = false;
            }
        
            // led3
            if ((message.indexOf(tag8)) != (message.indexOf(tag7) + tag7.length())) {
                char led = msg[message.indexOf(tag7) + tag7.length()];
                int led_ = Integer.parseInt(String.valueOf(led));
                if (led_ == 1)
                    leds3 = true;
                else
                    leds3 = false;
            }
        }
        
        // determine targetid
        char target = msg[message.indexOf(tag9) + tag9.length()];
        int target_ = Integer.parseInt(String.valueOf(target));
        
        // determine ledvalue
        if (!leds1 && !leds2 && !leds3)
            ledr = 0;
        else if (leds1 && !leds2 && !leds3)
            ledr = 1;
        else if (!leds1 && leds2 && !leds3)
            ledr = 2;
        else if (leds1 && leds2 && !leds3)
            ledr = 3;
        else if (!leds1 && !leds2 && leds3)
            ledr = 4;
        else if (leds1 && !leds2 && leds3)
            ledr = 5;
        else if (!leds1 && leds2 && leds3)
            ledr = 6;
        else
            ledr = 7;
        
        // polling request
        if (polling == true) {
            this.targetid = target_;
            this.request = CHANGE_SAMPLING_PERIOD;
            this.parameter = poll;
        }
        else {
            this.targetid = target_;
            this.request = LED_REQUEST;
            this.parameter = ledr;
        }            
    }
}

