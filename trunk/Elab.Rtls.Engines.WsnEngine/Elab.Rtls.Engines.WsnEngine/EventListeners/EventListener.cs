// <Copyright> Copyright (c) Artesis University College Antwerp </Copyright>
// <Author> Jerry </Author>
// <Date> 2009-04-13 </Date>
//-----------------------------------------------------------------------
namespace Elab.Rtls.Engines.WsnEngine
{
    using System;
    using System.Net;

    using Scala.Core;

    public abstract class EventListener
    {
        #region Constructors

        protected EventListener(EventSubscription eventSubscription)
        {
            this.EventSubscription = eventSubscription;
        }

        #endregion Constructors

        #region Events

        public event EventHandler<EventMessage> EventReceived;

        #endregion Events

        #region Properties

        public EventSubscription EventSubscription
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        public static EventListener Create(EventSubscription eventSubscription)
        {
            switch (eventSubscription.EventType)
            {
                case "ButtonPressed":
                case "HumidityChanged":
                case "LightChanged":
                case "TemperatureChanged":
                    return new SensorDataEventListener(eventSubscription);
                case "LocationUpdated":
                    return new PositionEventListener(eventSubscription);
            }
            throw new ArgumentOutOfRangeException("eventSubscription", "EventType does not exist.");
        }

        public abstract void Advise(Controller controller);

        public abstract void UnAdvise(Controller controller);

        protected void OnEventReceived(object sender, EventMessage eventMessage)
        {
            EventMessage QueriedEventMessage = new EventMessage();
            //QueriedEventMessage.EventSubscriptionId = eventMessage.EventSubscriptionId;
            //QueriedEventMessage.EventType = eventMessage.EventType;
            QueriedEventMessage.EventSubscriptionId = this.EventSubscription.EventId;
            QueriedEventMessage.EventType = this.EventSubscription.EventType;

            TagBlink TempTagBlink;

            if (WsnQueryHelper.TryQuery(this.EventSubscription, eventMessage.TagBlink, out TempTagBlink))
            {
                QueriedEventMessage.TagBlink = TempTagBlink;
                this.EventReceived(sender, QueriedEventMessage);
            }
        }

        #endregion Methods
    }
}