using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

using Scala.Core;

namespace Elab.Rtls.Engines.WsnEngine
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, AutomaticSessionShutdown = false, IncludeExceptionDetailInFaults = true)]
    public class WsnEngineService : Scala.Core.IQueryService, IMapService, IEventService, ITagInformationService
    {
        public WsnEngineService()
        {
            this.WsnEngine = WsnEngine.Instance;
            this.WsnEngine.EventRaised += this.WsnEngineEventRaised;
            EventIDs = new List<string>();
        }
        
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

        private List<string> EventIDs;

        /// <summary>
        /// Queries the Engine to retrieve tag information
        /// </summary>
        /// <param name="query"><see cref="Core.Query"/></param>
        /// <returns></returns>
        public QueryResponse Query(Query query)
        {
            return this.WsnEngine.Query(query);
        }

        public void Subscribe(EventSubscription eventSubscription)
        {
            //this.Logger.Trace("Subscribe method called on Ekahau4EngineAdapterService.");
            this.Callback = OperationContext.Current.GetCallbackChannel<IEventSourceCallback>();
            this.WsnEngine.Subscribe(eventSubscription);
            this.EventIDs.Add(eventSubscription.EventId);
        }

        public void Unsubscribe(string id)
        {
            //this.Logger.Trace("Unsubscribe method called on Ekahau4EngineAdapterService.");
            this.WsnEngine.Unsubscribe(id);
            this.EventIDs.Remove(id);
        }

        /// <summary>
        /// Sends a trivial word to a service and returns a trivial phrase.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public string Ping(string word)
        {
            //this.Logger.Trace("Ping method called in Ekahau4EngineAdapterService");
            return this.WsnEngine.Ping(word);
        }

        public List<Map> GetAllMaps()
        {
            return this.WsnEngine.GetAllMaps();
        }

        public List<Zone> GetAllZones()
        {
            return this.WsnEngine.GetAllZones();
        }

        public Map GetMap(string mapId)
        {
            return this.WsnEngine.GetMap(mapId);
            
        }

        public Zone GetZone(string zoneId)
        {
            return this.WsnEngine.GetZone(zoneId);
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

        #region ITagInformationService Members

        public List<Tag> GetAllTags()
        {
            return this.WsnEngine.GetAllTags();
        }

        public Tag GetTag(string tagId)
        {
            return this.WsnEngine.GetTag(tagId);
        }

        #endregion

        #region IMapService Members

        public Map AssociateMiddlewareMapToEngineMaps(Map mapObject, params string[] engineMapIds)
        {
            throw new FaultException("This is an engine, not the middleware");
        }

        public List<Site> GetAllSites()
        {
            throw new FaultException("This is an engine, not the middleware");
        }

        public void SaveSite(Site site)
        {
            throw new FaultException("This is an engine, not the middleware");
        }

        #endregion
    }
}
