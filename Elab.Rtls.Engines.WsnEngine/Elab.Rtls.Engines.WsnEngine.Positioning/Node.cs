namespace Elab.Rtls.Engines.WsnEngine.Positioning
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;

    using DatabaseConnection;

    /// <summary>
    /// Struct that holds information about the anchors, including RSS!
    /// </summary>
    public class AnchorNode
    {
        #region Fields

        /// <summary>
        /// Last 20 RSS samples
        /// </summary>
        public Queue<double> RSS = new Queue<double>(20);
        /// <summary>
        /// Filtered RSS
        /// </summary>
        public double fRSS;
        /// <summary>
        /// Time of last update
        /// </summary>
        public DateTime lastUpdate;
        /// <summary>
        /// WsnId of this node
        /// </summary>
        public string nodeid;
        /// <summary>
        /// X-coordinate
        /// </summary>
        public double posx;
        /// <summary>
        /// Y-coordinate
        /// </summary>
        public double posy;
        /// <summary>
        /// Distance from the connected blind node to this node in meters
        /// </summary>
        public double range;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="wsnid">WsnId of this node</param>
        /// <param name="posx">X-coordinate</param>
        /// <param name="posy">Y-coordinate</param>
        /// <param name="RSS">Current RSS reading</param>
        /// <param name="now">Time of last update</param>
        public AnchorNode(string wsnid, double posx, double posy, double RSS, DateTime now)
        {
            this.nodeid = wsnid;
            this.posx = posx;
            this.posy = posy;
            this.RSS.Enqueue(RSS);
            this.lastUpdate = now;
        }

        #endregion Constructors
    }

    //: Elab.Rtls.Engines.WsnEngine.Positioning.INode
    public class Node
    {
        #region Fields
        /// <summary>
        /// WsnId of this node
        /// </summary>
        public string WsnId;
        /// <summary>
        /// Position of this node
        /// </summary>
        public Point position;
        /// <summary>
        /// Connectionstring to use for the connection to the database (MySQL!)
        /// </summary>
        private MySQLClass MyDb;
        /// <summary>
        /// Connected anchors
        /// </summary>
        private List<AnchorNode> anchorList = new List<AnchorNode>();
        /// <summary>
        ///Connected virtual anchors
        /// Virtual Anchors: Blind nodes with a known position
        /// </summary>
        private List<AnchorNode> virtualAnchorList = new List<AnchorNode>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="WsnId">WsnId of this node</param>
        /// <param name="MyDb">Connectionstring to use for the connection to the database (MySQL!)</param>
        public Node(string WsnId, MySQLClass MyDb)
        {
            this.MyDb = MyDb;
            this.WsnId = WsnId;
            this.position = new Point(0.00, 0.00);
        }

        #endregion Constructors

        #region Delegates

        /// <summary>
        /// Method which filters the RSS
        /// </summary>
        /// <param name="RSS">RSS reading to be filtered</param>
        /// <returns>Filtered RSS</returns>
        public delegate double FilterMethod(Queue<double> RSS);

        /// <summary>
        /// Methods which performs the ranging
        /// </summary>
        /// <param name="fRSS"></param>
        /// <returns></returns>
        public delegate double RangingMethod(double fRSS);

        #endregion Delegates

        #region Properties

        /// <summary>
        /// Connected anchor nodes
        /// </summary>
        public List<AnchorNode> Anchors
        {
            get { return anchorList; }
        }

        /// <summary>
        /// Position of this node
        /// </summary>
        public Point Position
        {
            get
                {
                    return position;
                }
        }

        /// <summary>
        /// Connected virtual anchors
        /// Virtual Anchors: Blind nodes with a known position
        /// </summary>
        public List<AnchorNode> VirtualAnchors
        {
            get { return virtualAnchorList; }
        }

        /// <summary>
        /// WsnID of this node
        /// </summary>
        public string WsnIdProperty
        {
            get { return WsnId; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Allows a node to determine its own position from the database, 
        /// only applicable in case the node is an anchor
        /// </summary>
        public void SetOwnPosition()
        {
            position = GetANPosition(this.WsnId);
        }

        /// <summary>
        /// Updates the list with the connected anchors with the node described in the parameters
        /// </summary>
        /// <param name="AnchorWsnId">WsnID of the anchor ndoe</param>
        /// <param name="RSS">RSS reading from this anchor node</param>
        /// <param name="van">VAN status of the anchor node</param>
        /// <param name="now">Time when this node was added</param>
        public void UpdateAnchors(string AnchorWsnId, double RSS, int van, DateTime now)
        {
            Point ANpos = new Point();
            AnchorNode TempNode;
            double fRSS;

            ANpos = GetANPosition(AnchorWsnId);

            if (anchorList.Exists(AN => AN.nodeid == AnchorWsnId) || virtualAnchorList.Exists(VAN => VAN.nodeid == AnchorWsnId))
            {

                if (van == 1)
                {
                    int index = anchorList.FindIndex(AN => AN.nodeid == AnchorWsnId);
                    try{
                    if (anchorList[index].RSS.Count >= 20)
                    {
                        do
                        {
                            anchorList[index].RSS.Dequeue();
                        } while (anchorList[index].RSS.Count > 20);
                    } }
                    catch{}

                    anchorList[index].RSS.Enqueue(RSS);

                    anchorList[index].lastUpdate = now;
                }
                else
                {
                    int index = virtualAnchorList.FindIndex(AN => AN.nodeid == AnchorWsnId);

                    if (virtualAnchorList[index].RSS.Count >= 20)
                    {
                        do
                        {
                            virtualAnchorList[index].RSS.Dequeue();
                        } while (virtualAnchorList[index].RSS.Count > 20);
                    }

                    virtualAnchorList[index].RSS.Enqueue(RSS);

                    virtualAnchorList[index].lastUpdate = now;
                }
            }
            else
                NewAnchor(AnchorWsnId, RSS, van, now);

            //TODO: enable
            RemoveOutdatedAnchors();
            UpdateAnchorPositions();
        }

        /// <summary>
        /// Retrieves the position of the specified node from the DB  
        /// </summary>
        /// <param name="AN">The WSNid of the Anchor Node</param>
        /// <returns>The position of the Anchor Node</returns>
        private Point GetANPosition(string AnchorWsnId)
        {
            DataSet returnSet;
            string cmd = "call getPositionWsnId('" + AnchorWsnId + "');";
            Point pos = new Point();

            try
            {
                returnSet = MyDb.Query(cmd);

                foreach (DataRow row in returnSet.Tables[0].Rows)
                {
                    if (row["X"] == null || row["Y"] == null)
                        return null;
                    else
                    {
                        pos.x = Convert.ToDouble(row["X"]);
                        pos.y = Convert.ToDouble(row["Y"]);
                    }
                }
            }
            catch (Exception e_mysql)
            {
                Console.WriteLine(e_mysql.Message);
                Console.WriteLine(e_mysql.StackTrace);
            }

            return pos;
        }

        /// <summary>
        /// Adds a new anchor to the list of connected anchors
        /// </summary>
        /// <param name="AnchorWsnId">The WsnID of this anchor</param>
        /// <param name="RSS">RSS reading from this anchor node</param>
        /// <param name="van">VAN status of the anchor node</param>
        /// <param name="now">Time when this node was added</param>
        private void NewAnchor(string AnchorWsnId, double RSS, int van, DateTime now)
        {
            Point ANpos = GetANPosition(AnchorWsnId);

            if (van == 1)
                anchorList.Add(new AnchorNode(AnchorWsnId, ANpos.x, ANpos.y, RSS, now));
            else
                virtualAnchorList.Add(new AnchorNode(AnchorWsnId, ANpos.x, ANpos.y, RSS, now));
        }

        /// <summary>
        /// Removes anchors which have not been updated for two minutes
        /// </summary>
        private void RemoveOutdatedAnchors()
        {
            for (int i = 0; i < Anchors.Count; i++)
            {
                if (Anchors[i].lastUpdate < DateTime.Now.Subtract(new TimeSpan(0, 2, 0)))
                    this.Anchors.RemoveAt(i);
            }
        }

        //TEST
        //public void NewAnchor(string AnchorWsnId, double RSS, double posx, double posy, int van)
        //{
        //    if(van == 1)
        //    anchorList.Add(new AnchorNode(AnchorWsnId, posx, posy, RSS));
        //    else
        //        virtualAnchorList.Add(new AnchorNode(AnchorWsnId, posx, posy, RSS));
        //}

        /// <summary>
        /// Updates the position of the anchors with the newest position from the database
        /// </summary>
        private void UpdateAnchorPositions()
        {
            for (int i = 0; i < Anchors.Count; i++)
            {
                Point newPosition = GetANPosition(Anchors[i].nodeid);

                if (newPosition == null)
                    this.anchorList.RemoveAt(i);
                else
                {
                    Anchors[i].posx = newPosition.x;
                    Anchors[i].posy = newPosition.y;
                }
            }
        }

        #endregion Methods
    }
}