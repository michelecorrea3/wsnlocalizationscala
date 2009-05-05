using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;
using System.IO;
using System.Net.Sockets;
using System.Data;
using System.ComponentModel;
using System.ServiceModel;
using System.Diagnostics;
using System.Xml.Linq;

using SocketConnection;
using DatabaseConnection;

using Elab.Rtls.Engines.WsnEngine.Positioning;

using Elab.Toolkit.Core.Xml;

using Scala.Core;


namespace Elab.Rtls.Engines.WsnEngine
{   
    /// <summary>
    /// The WsnEngine class
    /// Implements the service providing the ANSI RTLS API
    /// TODO: Implement services...
    /// Singleton
    /// 
    /// </summary>
    public sealed class WsnEngine : IQueryable, IEventSource, IMappable, ITagInformationSource 
    {
        private readonly MySQLClass MyDB = new MySQLClass("DRIVER={MySQL ODBC 3.51 Driver};SERVER=localhost;DATABASE=senseless;UID=root;PASSWORD=admin;OPTION=3;");
        private static readonly WsnEngine instance = new WsnEngine();

        static Controller ControllerRef;

        #region Constructor

        static WsnEngine()
        {
            
        }

        private  WsnEngine()
        {
            this.EventListeners = new Dictionary<string, EventListener>();
        }

        #endregion

        public static WsnEngine Instance
        {
            get
            {
                return instance;
            }
        }

        public static void Advise(Controller controller)
        {
            ControllerRef = controller;
        }

        #region IQueryable Members

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

        #endregion

        #region IEventSource Members

        public event EventHandler<EventMessage> EventRaised;

        //contains the eventlisteners
        //automatic property
        private Dictionary<string, EventListener> EventListeners
        {
            get;
            set;
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

        #endregion

        #region IMappable Members

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

        public List<Zone> GetAllZones()
        {
            throw new FaultException("Not implemented");
        }

        public Map GetMap(string mapId)
        {           
            DataSet ReturnSet;
            Map CurrentMap = new Map();
            //query the database for all maps
            //dataset
            //parse them into the list
            string QueryCmd = "select * from map where idMap = " + mapId.Remove(0, 15) + ";";

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

        public Zone GetZone(string zoneId)
        {
            throw new FaultException("Not implemented");
        }

        #endregion

        #region ITagInformationSource Members

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

        public Tag GetTag(string tagId)
        {
            Tag tag = new Tag(tagId, "TelosWsnEngine");
            return tag;
        }

        #endregion

        public string Ping(string word)
        {
            //this.Logger.Trace("Ping method called in Ekahau4EngineAdapter");
            return "Hello " + word;
        }
    }
}
