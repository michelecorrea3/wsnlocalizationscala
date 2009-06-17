// <Copyright> Copyright (c) Artesis University College Antwerp </Copyright>
// <Author> Jerry </Author>
// <Date> 2009-04-13 </Date>
//-----------------------------------------------------------------------
namespace Elab.Rtls.Engines.WsnEngine
{
    using System;
    using Scala.Core;

    /// <summary>
    /// Abstract base class
    /// Listens to a specific event
    /// </summary>
    public abstract class EventListener
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eventSubscription">Data of the event to listen to</param>
        protected EventListener(EventSubscription eventSubscription)
        {
            this.EventSubscription = eventSubscription;
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Event that rethrows the events coming from the controller
        /// </summary>
        public event EventHandler<EventMessage> EventReceived;

        #endregion Events

        #region Properties

        /// <summary>
        /// Subscription data for the event
        /// </summary>
        public EventSubscription EventSubscription
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Creates an instance of the correct child eventlistener
        /// </summary>
        /// <param name="eventSubscription"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Advises the event listener with a reference of the current controller instance
        /// </summary>
        /// <param name="controller">Reference to the controller</param>
        public abstract void Advise(Controller controller);

        /// <summary>
        /// Unadvises the event listener with a reference of the current controller instance
        /// </summary>
        /// <param name="controller">Reference to the controller</param>
        public abstract void UnAdvise(Controller controller);

        /// <summary>
        /// Occurs when any event is thrown in the controller
        /// Filters all the events and only passes the events which have a corresponding eventsubscription
        /// </summary>
        /// <param name="sender">Controller</param>
        /// <param name="eventMessage">Custom data from the event</param>
        protected void OnEventReceived(object sender, EventMessage eventMessage)
        {
            EventMessage QueriedEventMessage = new EventMessage();
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