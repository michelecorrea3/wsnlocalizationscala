namespace Elab.Rtls.Engines.WsnEngine
{
    /// <summary>
    /// Specific eventlistener for the location event
    /// Inherits from EventListener
    /// </summary>
    public class PositionEventListener : EventListener
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eventSubscription">Data of the event to listen to</param>
        public PositionEventListener(Scala.Core.EventSubscription eventSubscription)
            : base(eventSubscription)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Advises the event listener with a reference of the current controller instance
        /// </summary>
        /// <param name="controller">Reference to the controller</param>
        public override void Advise(Controller controller)
        {
            controller.LocationUpdated += base.OnEventReceived;
        }

        /// <summary>
        /// Unadvises the event listener with a reference of the current controller instance
        /// </summary>
        /// <param name="controller">Reference to the controller</param>
        public override void UnAdvise(Controller controller)
        {
            controller.LocationUpdated -= base.OnEventReceived;
        }

        #endregion Methods
    }
}