namespace Elab.Rtls.Engines.WsnEngine
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.ServiceModel;
    using System.Text;

    using Scala.Core;

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, AutomaticSessionShutdown = false, IncludeExceptionDetailInFaults = true)]
    public class WsnEngineService : Scala.Core.IQueryService, IMapService, IEventService, ITagInformationService
    {
        #region Fields

        private List<string> EventIDs;

        #endregion Fields

        #region Constructors

        public WsnEngineService()
        {
            this.WsnEngine = WsnEngine.Instance;
            this.WsnEngine.EventRaised += this.WsnEngineEventRaised;
            EventIDs = new List<string>();
        }

        #endregion Constructors

        #region Properties

        public WsnEngine WsnEngine
        {
            get;
            set;
        }

        private IEventSourceCallback Callback
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        public Map AssociateMiddlewareMapToEngineMaps(MiddlewareToEngineMapLink mapObject)
        {
            throw new FaultException(
                new FaultReason(
                    new FaultReasonText(
                        "Associating middleware map with enginemaps is not supported without a middleware.", CultureInfo.InvariantCulture)));
        }

        public void DeleteMap(string mapId)
        {
            throw new FaultException(
                new FaultReason(
                    new FaultReasonText(
                        "Deleting a map is not supported without a middleware.", CultureInfo.InvariantCulture)));
        }

        public void DeleteSite(string siteId)
        {
            throw new FaultException(
                new FaultReason(
                    new FaultReasonText(
                        "Sites are not supported without a middleware.", CultureInfo.InvariantCulture)));
        }

        public List<Map> GetAllMaps()
        {
            return this.WsnEngine.GetAllMaps();
        }

        public List<Site> GetAllSites()
        {
            throw new FaultException(
                new FaultReason(
                    new FaultReasonText(
                        "Sites are not supported without a middleware.", CultureInfo.InvariantCulture)));
        }

        public List<Tag> GetAllTags()
        {
            return this.WsnEngine.GetAllTags();
        }

        public List<Zone> GetAllZones()
        {
            return this.WsnEngine.GetAllZones();
        }

        public Map GetMap(string mapId)
        {
            return this.WsnEngine.GetMap(mapId);
        }

        public Tag GetTag(string tagId)
        {
            return this.WsnEngine.GetTag(tagId);
        }

        public Zone GetZone(string zoneId)
        {
            return this.WsnEngine.GetZone(zoneId);
        }

        /// <summary>
        /// Sends a trivial word to a service and returns a trivial phrase.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
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

        public void SaveMap(Map map)
        {
            throw new FaultException(
                new FaultReason(
                    new FaultReasonText(
                        "Saving of a map is not supported without a middleware.", CultureInfo.InvariantCulture)));
        }

        public void SaveSite(Site site)
        {
            throw new FaultException(
               new FaultReason(
                   new FaultReasonText(
                       "Sites are not supported without a middleware.", CultureInfo.InvariantCulture)));
        }

        public void Subscribe(EventSubscription eventSubscription)
        {
            this.Callback = OperationContext.Current.GetCallbackChannel<IEventSourceCallback>();
            this.WsnEngine.Subscribe(eventSubscription);
            this.EventIDs.Add(eventSubscription.EventId);
        }

        public void Unsubscribe(string id)
        {
            this.WsnEngine.Unsubscribe(id);
            this.EventIDs.Remove(id);
        }

        public void UnsubscribeAll()
        {
            this.WsnEngine.UnsubscribeAll();
            this.EventIDs.Clear();
        }

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