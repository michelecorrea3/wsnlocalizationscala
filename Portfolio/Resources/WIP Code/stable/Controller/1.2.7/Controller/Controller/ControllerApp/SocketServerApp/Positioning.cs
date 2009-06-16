using System;
using System.Collections.Generic;
using System.Text;
using DatabaseConnection;
using System.Data;
using SocketConnection;


namespace Positioning
{
    /// <summary>
    /// Abstract base class for all positioning algorithms
    /// Presents two abstract methods, processMessage and CalculatePosition
    /// </summary>
    public abstract class Positioning
    {
        private MySQLClass MySQLConn;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MySQLConn"></param>
        public Positioning(MySQLClass MySQLConn)
        {
            this.MySQLConn = MySQLConn;
        }

        /// <summary>
        /// Processes the data and returns the position of the node
        /// </summary>
        /// <param name="wsnid">WSN ID of the anchor node</param>
        /// <param name="RSSI">RSSI measurement from a message of the anchor node</param>
        public abstract Point ProcessMessage(int wsnid, int RSSI);


        /// <summary>
        /// Calculates the position of the Anchor Node
        /// </summary>
        /// <returns></returns>
        public abstract Point CalculatePosition();

        /// <summary>
        /// Retrieves the position of the specified node from the DB
        /// </summary>
        /// <param name="AN">The WSNid of the Anchor Node</param>
        /// <returns>The position of the Anchor Node</returns>
        protected Point GetANPosition(int AN)
        {
            DataSet returnSet;
            string cmd = "call getStatusWSNID('" + AN.ToString() + "');";
            Point pos = new Point();

            try
            {
                //Update the MySQL-database (if it is available)
                //if (MySQLAllowedConn)
                {
                    returnSet = MySQLConn.Query(cmd);

                    foreach (DataRow row in returnSet.Tables[0].Rows)
                    {
                        pos.x = Convert.ToInt32(row["X"]);
                        pos.y = Convert.ToInt32(row["Y"]);
                    }
                }
            }
            catch (Exception e_mysql)
            {
                Console.WriteLine(e_mysql.Message);
                Console.WriteLine(e_mysql.StackTrace);
                Console.WriteLine("Some field is probably missing");
                SocketServer.LogError(e_mysql, "LogServer.txt");
            }

            return pos;
        }
    }

    ///// <summary>
    ///// Abstract base class for all range based positioning algorithms
    ///// </summary>
    //public abstract class RangeBasedPositioning : Positioning
    //{
    //    public List<RangeAnchors> anchorlist = new List<RangeAnchors>();
    //    private int currentAN;
    //    private double fRSS = 0;

    //    /// <summary>
    //    /// Constructor
    //    /// </summary>
    //    /// <param name="MySQLConn"></param>
    //    public RangeBasedPositioning(MySQLClass MySQLConn)
    //        : base(MySQLConn)
    //    { }

    //    /// <summary>
    //    /// Simple FIR filter, with weight 90%
    //    /// </summary>
    //    /// <param name="RSS"></param>
    //    /// <returns></returns>
    //    private virtual double FilterRSS(int RSS)
    //    {
    //        double fRSS = 0;

    //        if (fRSS == 0)
    //            fRSS = (double)RSS;
    //        else
    //            fRSS = 0.9 * fRSS + 0.1 * (double)RSS;

    //        return fRSS;
    //    }
        
    //    /// <summary>
    //    /// Translates the RSS to distance
    //    /// </summary>
    //    /// <param name="fRSS"></param>
    //    /// <returns></returns>
    //    public abstract double Ranging(double fRSS);
       

    //    /// <summary>
    //    /// Processes the data and returns the position of the node
    //    /// </summary>
    //    /// <param name="wsnid">WSN ID of the anchor node</param>
    //    /// <param name="RSSI">RSSI measurement from a message of the anchor node</param>
    //    public override Point ProcessMessage(int wsnid, int RSSI)
    //    {
    //        currentAN = wsnid;
    //        Point pos = new Point(), ANpos = new Point();

    //        //filter the RSS
    //        fRSS = FilterRSS(RSSI);

    //        //retreive the position of the AN
    //        ANpos = GetANPosition(wsnid);

    //        if (!anchorlist.Exists(HasNodeID))
    //            anchorlist.Add(new RangeAnchors(wsnid, ANpos.x, ANpos.y, fRSS));
    //        else
    //        {
    //            anchorlist.RemoveAll(HasNodeID);
    //            anchorlist.Add(new RangeAnchors(wsnid, ANpos.x, ANpos.y, fRSS));
    //        }

    //        pos = CalculatePosition();
    //        return pos;
    //    }

    //    private bool HasNodeID(RangeAnchors AN)
    //    {
    //        if (AN.nodeid == currentAN)
    //            return true;
    //        else
    //            return false;
    //    }
    //}

    /// <summary>
    /// Abstract base class for all connectivity-based positioning algorithms
    /// </summary>
    public abstract class ConnectivityBasedPositioning : Positioning
    {
        //List containing all the anchors and their position
        public List<ConnectivityAnchors> anchorlist = new List<ConnectivityAnchors>();
        int currentAN;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MySQLConn"></param>
        public ConnectivityBasedPositioning(MySQLClass MySQLConn)
            : base(MySQLConn)
        { }

        /// <summary>
        /// Processes the data and returns the position of the node
        /// </summary>
        /// <param name="wsnid">WSN ID of the anchor node</param>
        /// <param name="RSSI">RSSI measurement from a message of the anchor node</param>
        public override Point ProcessMessage(int wsnid, int RSSI)
        {
            currentAN = wsnid;
            Point pos = new Point(), ANpos = new Point();

            ANpos = GetANPosition(wsnid);
            
            if (!anchorlist.Exists(HasNodeID))
            {
                anchorlist.Add(new ConnectivityAnchors(wsnid, ANpos.x, ANpos.y));
            }
            else
            {
                anchorlist.RemoveAll(HasNodeID);
                anchorlist.Add(new ConnectivityAnchors(wsnid, ANpos.x, ANpos.y));
            }

            pos = CalculatePosition();
            return pos;
        }


        /// <summary>
        /// Delegate predicate for list.Exists
        /// </summary>
        /// <param name="AN"></param>
        /// <returns></returns>
        private bool HasNodeID(ConnectivityAnchors AN)
        {
            if (AN.nodeid == currentAN)
                return true;
            else
                return false;
        }
    }

    /// <summary>
    /// Centroid Localization algorithm, the position is the centroid of the anchor nodes position
    /// </summary>
    public class CentroidLocalization : ConnectivityBasedPositioning
    {
        /// <summary>
        /// ID of the BN
        /// </summary>
        public string WSNid;

        public CentroidLocalization(string WSNid, MySQLClass MySQLConn) 
            : base (MySQLConn)
        {
            this.WSNid = WSNid;
        }

        public override Point CalculatePosition()
        {
            Point position = new Point();
            int tempx = 0, tempy = 0, count = 0;

            foreach (ConnectivityAnchors AN in anchorlist)
            {
                tempx += AN.posx;
                tempy += AN.posy;
                count++;
            }

            position.x = tempx / count;
            position.y = tempy / count;

            return position;
        }
    }

    ///// <summary>
    ///// Min-Max Localization algorithm with simple radio propagation model
    ///// Model parameters:
    ///// </summary>
    //public class MinMaxSimpleModel : RangeBasedPositioning
    //{
    //    /// <summary>
    //    /// ID of the BN
    //    /// </summary>
    //    public string WSNid;

    //    private BoundingBox BnBox = new BoundingBox(0);

    //    public MinMaxSimpleModel(string WSNid, MySQLClass MySQLConn) 
    //        : base (MySQLConn)
    //    {
    //        this.WSNid = WSNid;
    //    }

    //    public override double Ranging(double fRSS)
    //    {
    //        double distance;
            
    //        return distance;
    //    }
        
    //    public override Point CalculatePosition()
    //    {
    //        Point position = new Point();
    //        double distance;

    //        if (anchorlist.Count >= 3)
    //        {
    //            foreach (RangeAnchors AN in anchorlist)
    //            {
    //                //perform the ranging
    //                distance = Ranging(AN.RSS);

    //                Point center = new Point(AN.posx, AN.posy);
    //                BoundingBox AnBox = new BoundingBox(center, distance);

    //                BnBox.Xmin = Math.Max(BnBox.Xmin, AnBox.Xmin);
    //                BnBox.Xmax = Math.Min(BnBox.Xmax, AnBox.Xmax);
    //                BnBox.Ymin = Math.Max(BnBox.Ymin, AnBox.Ymin);
    //                BnBox.Ymax = Math.Min(BnBox.Ymax, AnBox.Ymax);
    //            }
    //        }
    //        else
    //            throw Exception;

    //        position.x = (BnBox.Xmin + BnBox.Xmax) / 2;
    //        position.y = (BnBox.Ymin + BnBox.Ymax) / 2;

    //        return position;
    //    }
    //}

    /// <summary>
    /// Struct that holds information about the anchors, excluding RSS!
    /// </summary>
    public struct ConnectivityAnchors
    {
        public int nodeid;
        public int posx;
        public int posy;

        public ConnectivityAnchors(int wsnid, int posx, int posy)
        {
            this.nodeid = wsnid;
            this.posx = posx;
            this.posy = posy;
        }
    }

    /// <summary>
    /// Struct that holds information about the anchors, including RSS!
    /// </summary>
    public struct RangeAnchors
    {
        public int nodeid;
        public int posx;
        public int posy;
        public double RSS;

        public RangeAnchors(int wsnid, int posx, int posy, double RSS)
        {
            this.nodeid = wsnid;
            this.posx = posx;
            this.posy = posy;
            this.RSS = RSS;
        }
    }


    /// <summary>
    /// Simple struct to hold a 2D point
    /// </summary>
    public struct Point
    {
        /// <summary>
        /// X & Y coordinates
        /// </summary>
        public int x, y;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    
    /// <summary>
    /// CHANGE: struct for the boundingbox in min max
    /// </summary>
    public struct BoundingBox
    {
        public double Xmin, Xmax, Ymin, Ymax;

        public BoundingBox(double number)
        {
            this.Xmin = number;
            this.Xmax = number;
            this.Ymin = number;
            this.Ymax = number;
        }

        public BoundingBox(Point center, double deviation)
        {
            this.Xmin = center.x - deviation;
            this.Xmax = center.x + deviation;
            this.Ymin = center.y - deviation;
            this.Ymax = center.y + deviation;
        }
    }
}

