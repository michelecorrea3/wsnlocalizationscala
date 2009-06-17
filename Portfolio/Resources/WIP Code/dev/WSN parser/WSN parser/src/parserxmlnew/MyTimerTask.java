/* Peter De Cauwer & Tim Van Overtveldt, graduate student Artesis, Applied Engineering: Electronics-ICT
 * Master thesis: WSN localization with Senseless
 */

package parserxmlnew;
import java.util.*;

/**
 * Class as TimerTask: a task that is scheduled for one-time execution by our Timer.
 * This class is needed for when the WSN does not send a status message as reply to a request
 * Timer
 */
class MyTimerTask extends TimerTask
{
    sharedata s;

    public MyTimerTask(sharedata s)
    {
        this.s = s;
    }

    public void run()
    {
        s.set_status("error");
        System.out.println("TimerTask executed!");
    }
}
