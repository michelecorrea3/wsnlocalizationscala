namespace Elab.Rtls.Engines.WsnEngine
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net.Sockets;
    using System.ServiceModel;
    using System.Text;
    using System.Threading;
    using System.Xml;
    using System.Xml.Linq;

    using DatabaseConnection;

    using Elab.Rtls.Engines.WsnEngine.Positioning;
    using Elab.Toolkit.Core.Xml;
    using Elab.Toolkit.Imaging;

    using Scala.Core;

    using SocketConnection;

    /// <summary>
    /// The WsnEngine class
    /// Implements the service providing the ANSI RTLS API
    /// TODO: Implement services...
    /// Singleton
    /// </summary>
    public sealed class WsnEngine : IQueryable, IEventSource, IMappable, ITagInformationSource
    {
        #region Fields

        private static readonly WsnEngine instance = new WsnEngine();

        private readonly MySQLClass MyDB = new MySQLClass("DRIVER={MySQL ODBC 3.51 Driver};SERVER=localhost;DATABASE=senseless;UID=root;PASSWORD=admin;OPTION=3;");

        static Controller ControllerRef;

        #endregion Fields

        #region Constructors

        static WsnEngine()
        {
        }

        private WsnEngine()
        {
            this.EventListeners = new Dictionary<string, EventListener>();
        }

        #endregion Constructors

        #region Events

        public event EventHandler<EventMessage> EventRaised;

        #endregion Events

        #region Properties

        public static WsnEngine Instance
        {
            get
            {
                return instance;
            }
        }

        //contains the eventlisteners
        //automatic property
        private Dictionary<string, EventListener> EventListeners
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        public static void Advise(Controller controller)
        {
            ControllerRef = controller;
        }

        /// <summary>
        /// Determines the accuracy and changes the coordinates to be bounded by the map
        /// </summary>
        /// <param name="x">The X-coordinate</param>
        /// <param name="y">The Y-coordinate</param>
        /// <param name="mapid">The ID of the map</param>
        /// <returns>The accuracy, is 100 when the position is within bounds else 0</returns>
        public static string CheckMapBounds(ref double x, ref double y, string mapid)
        {
            string accuracy;

            Map tempMap = WsnEngine.Instance.GetMap(mapid);
            Image tempImage = tempMap.MapImageBytes.ToImage();

            if (x < 0.0 || x > (tempImage.Width * tempMap.MapScale )|| y < 0 || y > ( tempImage.Height * tempMap.MapScale ))
            {
                accuracy = "0";
                if (x < 0.0)
                    x = 0.0;
                else
                    x = tempImage.Width * tempMap.MapScale;

                if (y < 0.0)
                    y = 0.0;
                else
                    y = tempImage.Height * tempMap.MapScale;
            }
            else
                accuracy = "100";

            return accuracy;
        }

        public List<Map> GetAllMaps()
        {
            //            throw new NotImplementedException();
            List<Map> MapList = new List<Map>();
            DataSet ReturnSet;

            //query the database for all maps
            //dataset
            //parse them into the list
            string QueryCmd = "select * from map;";

            //try
            {
                ReturnSet = MyDB.Query(QueryCmd);

                foreach (DataRow Row in ReturnSet.Tables[0].Rows)
                {
                    Map CurrentMap = new Map();

                    CurrentMap.EngineId = "WsnEngine1";
                    CurrentMap.FloorNumber = Convert.ToInt32(Row["Floor"]);
                    CurrentMap.MapId = CurrentMap.EngineId + "map" + Row["idMap"].ToString();

                    //test
                    var ImageBytes = Row["ImageBlob"];

                    CurrentMap.MapImageBytes = (byte[])Row["ImageBlob"];
                    CurrentMap.MapName = Row["Name"].ToString();
                    CurrentMap.MapScale = Convert.ToDouble(Row["Scale"]);
                    CurrentMap.NativeMapId = Row["idMap"].ToString();
                    //CurrentMap.Site = Row["Site"];

                    MapList.Add(CurrentMap);
                }
            }
            //catch (Exe fex)
            //{
            //    throw fex;
            //}
            return MapList;
        }

        public List<Tag> GetAllTags()
        {
            List<Tag> tagList = new List<Tag>();
            string queryCmd = "SELECT idnode FROM node n where sensortype = 2;";

            DataSet ReturnSet = MyDB.Query(queryCmd);

            foreach (DataRow Row in ReturnSet.Tables[0].Rows)
            {
                tagList.Add(new Tag(Row["idnode"].ToString(), "TelosWsnEngine"));
            }

            return tagList;
        }

        public List<Zone> GetAllZones()
        {
            throw new FaultException("Not implemented");
        }

        public Map GetMap(string mapId)
        {
            DataSet ReturnSet;
            Map CurrentMap = new Map();
            string QueryCmd;
            //query the database for all maps
            //dataset
            //parse them into the list
            if (mapId.Length > 5)
                QueryCmd = "select * from map where idMap = " + mapId.Remove(0, 15) + ";";
            else
                QueryCmd = "select * from map where idMap = " + mapId + ";";

            //try
            {
                ReturnSet = MyDB.Query(QueryCmd);

                foreach (DataRow Row in ReturnSet.Tables[0].Rows)
                {

                    CurrentMap.EngineId = "WsnEngine1";
                    CurrentMap.FloorNumber = Convert.ToInt32(Row["Floor"]);
                    CurrentMap.MapId = CurrentMap.EngineId + "map" + Row["idMap"].ToString();
                    CurrentMap.MapImageBytes = (byte[])Row["ImageBlob"];
                    CurrentMap.MapName = Row["Name"].ToString();
                    CurrentMap.MapScale = Convert.ToDouble(Row["Scale"]);
                    CurrentMap.NativeMapId = Row["idMap"].ToString();
                    //Optioneel
                    //CurrentMap.Site = Row["Site"];

                }
            }

            return CurrentMap;
        }

        public Tag GetTag(string tagId)
        {
            Tag tag = new Tag(tagId, "TelosWsnEngine");
            return tag;
        }

        public Zone GetZone(string zoneId)
        {
            throw new FaultException("Not implemented");
        }

        public string Ping(string word)
        {
            //this.Logger.Trace("Ping method called in Ekahau4EngineAdapter");
            return "Hello " + word;
        }

        /// <summary>
        /// Implements the service
        /// Processes the Query
        /// </summary>
        /// <param name="query">Query</param>
        /// <returns>QueryResult</returns>
        public QueryResponse Query(Query query)
        {
            return WsnQueryHelper.Query(query, MyDB);
        }

        public void Subscribe(EventSubscription eventSubscription)
        {
            if (!this.EventListeners.ContainsKey(eventSubscription.EventId))
            {
                var eventListener = EventListener.Create(eventSubscription);
                eventListener.EventReceived += this.EventListenerEventReceived;
                this.EventListeners.Add(eventSubscription.EventId, eventListener);

                //pass a reference to the event of the controller
                eventListener.Advise(ControllerRef);
            }
        }

        public void Unsubscribe(string id)
        {
            if (this.EventListeners.ContainsKey(id))
            {
                this.EventListeners[id].UnAdvise(ControllerRef);
                this.EventListeners.Remove(id);
            }
        }

        public void UnsubscribeAll()
        {
            foreach (string key in this.EventListeners.Keys)
            {
                this.Unsubscribe(key);
            }
        }

        private void EventListenerEventReceived(object sender, EventMessage eventMessage)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<EventMessage> handler = this.EventRaised;

            // Event will be null if there are no subscribers
            if (handler != null)
            {
                handler(this, eventMessage);
            }
        }

        #endregion Methods
    }
}