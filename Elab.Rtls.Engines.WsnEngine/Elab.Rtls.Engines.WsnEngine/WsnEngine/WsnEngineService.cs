namespace Elab.Rtls.Engines.WsnEngine
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.ServiceModel;
    using Scala.Core;

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, AutomaticSessionShutdown = false, IncludeExceptionDetailInFaults = true)]
    public class WsnEngineService : Scala.Core.IQueryService, IMapService, IEventService, ITagInformationService
    {
        #region Fields

        /// <summary>
        /// The ID's of the events
        /// </summary>
        private List<string> EventIDs;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor
        /// References the instance of the engine
        /// Reroutes the events from the engine to a local eventhandler
        /// </summary>
        public WsnEngineService()
        {
            this.WsnEngine = WsnEngine.Instance;
            this.WsnEngine.EventRaised += this.WsnEngineEventRaised;
            EventIDs = new List<string>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Static instance of the engine
        /// </summary>
        public WsnEngine WsnEngine
        {
            get;
            set;
        }

        /// <summary>
        /// Callback for the events
        /// </summary>
        private IEventSourceCallback Callback
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Not supported
        /// </summary>
        /// <param name="mapObject"></param>
        /// <returns></returns>
        public Map AssociateMiddlewareMapToEngineMaps(MiddlewareToEngineMapLink mapObject)
        {
            throw new FaultException(
                new FaultReason(
                    new FaultReasonText(
                        "Associating middleware map with enginemaps is not supported without a middleware.", CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// Not supported
        /// </summary>
        public void DeleteMap(string mapId)
        {
            throw new FaultException(
                new FaultReason(
                    new FaultReasonText(
                        "Deleting a map is not supported without a middleware.", CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// Not supported
        /// </summary>
        public void DeleteSite(string siteId)
        {
            throw new FaultException(
                new FaultReason(
                    new FaultReasonText(
                        "Sites are not supported without a middleware.", CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// Retreives all existing maps in the database
        /// </summary>
        /// <returns>List containing all the maps</returns>
        public List<Map> GetAllMaps()
        {
            return this.WsnEngine.GetAllMaps();
        }

        /// <summary>
        /// Not supported
        /// </summary>
        public List<Site> GetAllSites()
        {
            throw new FaultException(
                new FaultReason(
                    new FaultReasonText(
                        "Sites are not supported without a middleware.", CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// Retreives all existing tags in the database
        /// Warning these can be inactive
        /// </summary>
        /// <returns></returns>
        public List<Tag> GetAllTags()
        {
            return this.WsnEngine.GetAllTags();
        }

        /// <summary>
        /// Not supported
        /// </summary>
        /// <returns></returns>
        public List<Zone> GetAllZones()
        {
            return this.WsnEngine.GetAllZones();
        }

        /// <summary>
        /// Retreives a specific map based on its id
        /// </summary>
        /// <param name="mapId">Id of the map native to this system</param>
        /// <returns>Map</returns>
        public Map GetMap(string mapId)
        {
            return this.WsnEngine.GetMap(mapId);
        }

        /// <summary>
        /// Retreives a single tag based on its id
        /// </summary>
        /// <param name="tagId">Id of the tag native to this system</param>
        /// <returns>Tag</returns>
        public Tag GetTag(string tagId)
        {
            return this.WsnEngine.GetTag(tagId);
        }

        /// <summary>
        /// Not supported
        /// </summary>
        /// <param name="zoneId"></param>
        /// <returns></returns>
        public Zone GetZone(string zoneId)
        {
            return this.WsnEngine.GetZone(zoneId);
        }

        /// <summary>
        /// Method to the whether the engine is connected or not
        /// </summary>
        /// <param name="word">Any word</param>
        /// <returns>Hello + parameter word</returns>
        public string Ping(string word)
        {
            return this.WsnEngine.Ping(word);
        }

        /// <summary>
        /// Queries the Engine to retrieve tag information
        /// </summary>
        /// <param name="query"><see cref="Core.Query"/></param>
        /// <returns></returns>
        public QueryResponse Query(Query query)
        {
            return this.WsnEngine.Query(query);
        }

        /// <summary>
        /// Not supported
        /// </summary>
        public void SaveMap(Map map)
        {
            throw new FaultException(
                new FaultReason(
                    new FaultReasonText(
                        "Saving of a map is not supported without a middleware.", CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// Not supported
        /// </summary>
        public void SaveSite(Site site)
        {
            throw new FaultException(
               new FaultReason(
                   new FaultReasonText(
                       "Sites are not supported without a middleware.", CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// Subscribes to a specific event defined in eventSubscription
        /// </summary>
        /// <param name="eventSubscription">Data conserning the event to subscribe to</param>
        public void Subscribe(EventSubscription eventSubscription)
        {
            this.Callback = OperationContext.Current.GetCallbackChannel<IEventSourceCallback>();
            this.WsnEngine.Subscribe(eventSubscription);
            this.EventIDs.Add(eventSubscription.EventId);
        }

        /// <summary>
        /// Unsubscribes to a specific event defined in eventSubscription
        /// </summary>
        /// <param name="id">Id of the eventsubscription</param>
        public void Unsubscribe(string id)
        {
            this.WsnEngine.Unsubscribe(id);
            this.EventIDs.Remove(id);
        }

        /// <summary>
        /// Unsubscribes to all subscripted events
        /// </summary>
        public void UnsubscribeAll()
        {
            this.WsnEngine.UnsubscribeAll();
            this.EventIDs.Clear();
        }

        /// <summary>
        /// Occurs when any event is thrown in the controller
        /// </summary>
        /// <param name="sender">Engine (rerouted from the controller)</param>
        /// <param name="eventMessage">Custom data from the event</param>
        private void WsnEngineEventRaised(object sender, EventMessage eventMessage)
        {
            //this.Logger.Trace("Event received from Ekahau4EngineAdapter, triggering callback.");
            if (this.Callback == null)
            {
                //this.Logger.Info("Callback is null!");
            }
            else
            {
                if (EventIDs.Contains(eventMessage.EventSubscriptionId))
                    this.Callback.OnEventRaised(eventMessage);
            }
        }

        #endregion Methods
    }
}