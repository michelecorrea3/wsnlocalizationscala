/* Peter De Cauwer & Tim Van Overtveldt, graduate student Artesis, Applied Engineering: Electronics-ICT
 * Master thesis: WSN localization with Senseless
 */

package parserxmlnew;

//import java.util.Date;

/**
 * Class to share data between the threads
 */
public class sharedata {
    private String Status;              // String that contains the xml message that needs to be send to the controller
    public boolean Waitingforreply;     // A bool that is used by the thread requesthandler to wait for the xml message of the thread serial

    public sharedata()
    {
        Waitingforreply = false;
        Status = "banana";
    }

    //Function to get/set members of the class
    synchronized String get_status() { return Status; }
    synchronized void set_status(String status) { this.Status = status; }
    synchronized boolean get_waiting() { return Waitingforreply; }
    synchronized void set_waiting(boolean wait) { this.Waitingforreply = wait; }


}
