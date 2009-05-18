using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using DatabaseConnection;
using SocketConnection;


namespace Elab.Rtls.Engines.WsnEngine.Positioning
{
    public class Node //: Elab.Rtls.Engines.WsnEngine.Positioning.INode
    {
        private MySQLClass MyDb;
        private string WsnId;
        private List<AnchorNode> anchorList = new List<AnchorNode>();
        private List<AnchorNode> virtualAnchorList = new List<AnchorNode>();
        private Point position;

        public Point Position
        { 
            get
                {
                    return position;
                }
        }

        public List<AnchorNode> Anchors
        {
            get { return anchorList; }
        }

        public List<AnchorNode> VirtualAnchors
        {
            get { return virtualAnchorList; }
        }

        public string WsnIdProperty
        {
            get { return WsnId; }
        }

        public delegate double FilterMethod(Queue<double> RSS);

        public delegate double RangingMethod(double fRSS);

        public Node(string WsnId, MySQLClass MyDb)
        {
            this.MyDb = MyDb;
            this.WsnId = WsnId;
        }

        public Node(string WsnId, MySQLClass MyDb, string AnchorWsnId, double RSS, int van)
        {
            this.MyDb = MyDb;
            this.WsnId = WsnId;
            NewAnchor(AnchorWsnId, RSS, van);
        }

        private void NewAnchor(string AnchorWsnId, double RSS, int van)
        {
            Point ANpos = new Point();

            ANpos = GetANPosition(AnchorWsnId);

            if (van == 1)
                anchorList.Add(new AnchorNode(AnchorWsnId, ANpos.x, ANpos.y, RSS));
            else 
                virtualAnchorList.Add(new AnchorNode(AnchorWsnId, ANpos.x, ANpos.y, RSS));
        }
        
        //TEST
        public void NewAnchor(string AnchorWsnId, double RSS, double posx, double posy)
        {
            anchorList.Add(new AnchorNode(AnchorWsnId, posx, posy, RSS));
        }

        public void AddAnchor(string AnchorWsnId, double RSS, int van)
        {
            Point ANpos = new Point();
            AnchorNode TempNode;
            double fRSS;

            ANpos = GetANPosition(AnchorWsnId);


            if (anchorList.Exists(AN => AN.nodeid == AnchorWsnId) || virtualAnchorList.Exists(VAN => VAN.nodeid == AnchorWsnId))
            {
                if (van == 1)
                {
                    AnchorNode TempANode = anchorList.Find(AN => AN.nodeid == AnchorWsnId);
                    int index = anchorList.IndexOf(TempANode);
                    
                    anchorList[index].RSS.Enqueue(RSS);

                    if (anchorList[index].RSS.Count > 20)
                    {
                        do
                        {
                            anchorList[index].RSS.Dequeue();
                        } while (anchorList[index].RSS.Count > 20);
                    }
                }
                else
                {
                    AnchorNode TempANode = virtualAnchorList.Find(AN => AN.nodeid == AnchorWsnId);
                    int index = virtualAnchorList.IndexOf(TempANode);

                    virtualAnchorList[index].RSS.Enqueue(RSS);

                    if (virtualAnchorList[index].RSS.Count > 20)
                    {
                        do
                        {
                            virtualAnchorList[index].RSS.Dequeue();
                        } while (virtualAnchorList[index].RSS.Count > 20);
                    }
                }
            }
            else
            {
                if (van == 1)
                    anchorList.Add(new AnchorNode(AnchorWsnId, ANpos.x, ANpos.y, RSS));
                else
                    virtualAnchorList.Add(new AnchorNode(AnchorWsnId, ANpos.x, ANpos.y, RSS));                
            }
        }


        //OUTDATED....  
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
                {
                    returnSet = MyDb.Query(cmd);

                    foreach (DataRow row in returnSet.Tables[0].Rows)
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
                Console.WriteLine("Some field is probably missing");
            }

            return pos;
        }

        public void SetOwnPosition()
        {
            position = GetANPosition(this.WsnId);
        }
    }

    /// <summary>
    /// Struct that holds information about the anchors, including RSS!
    /// </summary>
    public class AnchorNode
    {
        public string nodeid;
        public double posx;
        public double posy;
        public double fRSS;
        public Queue<double> RSS = new Queue<double>(20);
        public double range;

        public AnchorNode(string wsnid, double posx, double posy, double RSS)
        {
            this.nodeid = wsnid;
            this.posx = posx;
            this.posy = posy;
            this.RSS.Enqueue(RSS);
        }
    }
}
