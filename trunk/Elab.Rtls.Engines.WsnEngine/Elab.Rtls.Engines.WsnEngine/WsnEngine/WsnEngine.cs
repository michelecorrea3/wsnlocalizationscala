namespace Elab.Rtls.Engines.WsnEngine
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.ServiceModel;
    using DatabaseConnection;
    using Elab.Toolkit.Imaging;
    using Scala.Core;

    /// <summary>
    /// The WsnEngine class
    /// Implements the service providing the ANSI RTLS API, thus forming the interface with Scala
    /// Singleton
    /// </summary>
    public sealed class WsnEngine : IQueryable, IEventSource, IMappable, ITagInformationSource
    {
        #region Fields

        /// <summary>
        /// Static instance of this class
        /// </summary>
        private static readonly WsnEngine instance = new WsnEngine();

        /// <summary>
        /// Connectionstring to use for the connection to the database (MySQL!)
        /// </summary>
        private readonly MySQLClass MyDB = new MySQLClass("DRIVER={MySQL ODBC 3.51 Driver};SERVER=localhost;DATABASE=senseless;UID=root;PASSWORD=admin;OPTION=3;");

        /// <summary>
        /// Reference to the controller
        /// </summary>
        static Controller ControllerRef;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor
        /// Initializes the list with the events to listen to
        /// </summary>
        private WsnEngine()
        {
            this.EventListeners = new Dictionary<string, EventListener>();
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Event that rethrows the events coming from the controller
        /// </summary>
        public event EventHandler<EventMessage> EventRaised;

        #endregion Events

        #region Properties

        /// <summary>
        /// Static instance of this class
        /// </summary>
        public static WsnEngine Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// contains the eventlisteners
        /// which are filters for the event which you want to listen to
        /// automatic property
        /// </summary>
        private Dictionary<string, EventListener> EventListeners
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Advises the engine with a reference of the current controller instance
        /// </summary>
        /// <param name="controller"></param>
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

        /// <summary>
        /// Retreives all existing maps in the database
        /// </summary>
        /// <returns>List containing all the maps</returns>
        public List<Map> GetAllMaps()
        {
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

        /// <summary>
        /// Retreives all existing tags in the database
        /// Warning these can be inactive
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Not supported in this engine
        /// </summary>
        /// <returns></returns>
        public List<Zone> GetAllZones()
        {
            throw new FaultException("Not implemented");
        }

        /// <summary>
        /// Retreives a specific map based on its id
        /// </summary>
        /// <param name="mapId">Id of the map native to this system</param>
        /// <returns>Map</returns>
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

        /// <summary>
        /// Retreives a single tag based on its id
        /// </summary>
        /// <param name="tagId">Id of the tag native to this system</param>
        /// <returns>Tag</returns>
        public Tag GetTag(string tagId)
        {
            Tag tag = new Tag(tagId, "TelosWsnEngine");
            return tag;
        }

        /// <summary>
        /// Not supported in this engine
        /// </summary>
        /// <returns></returns>
        public Zone GetZone(string zoneId)
        {
            throw new FaultException("Not implemented");
        }

        /// <summary>
        /// Method to the whether the engine is connected or not
        /// </summary>
        /// <param name="word">Any word</param>
        /// <returns>Hello + parameter word</returns>
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

        /// <summary>
        /// Subscribes to a specific event defined in eventSubscription
        /// </summary>
        /// <param name="eventSubscription">Data conserning the event to subscribe to</param>
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

        /// <summary>
        /// Unsubscribes to a specific event defined in eventSubscription
        /// </summary>
        /// <param name="id">Id of the eventsubscription</param>
        public void Unsubscribe(string id)
        {
            if (this.EventListeners.ContainsKey(id))
            {
                this.EventListeners[id].UnAdvise(ControllerRef);
                this.EventListeners.Remove(id);
            }
        }

        /// <summary>
        /// Unsubscribes to all subscripted events
        /// </summary>
        public void UnsubscribeAll()
        {
            foreach (string key in this.EventListeners.Keys)
            {
                this.Unsubscribe(key);
            }
        }

        /// <summary>
        /// Occurs when any event is thrown in the controller
        /// </summary>
        /// <param name="sender">Controller</param>
        /// <param name="eventMessage">Custom data from the event</param>
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