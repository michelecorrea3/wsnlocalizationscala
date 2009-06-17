namespace Elab.Rtls.Engines.WsnEngine
{
    /// <summary>
    /// Specific eventlistener for the sensordata events
    /// Inherits from EventListener
    /// </summary>
    public class SensorDataEventListener : EventListener
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eventSubscription">Data of the event to listen to</param>
        public SensorDataEventListener(Scala.Core.EventSubscription eventSubscription)
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
            switch (base.EventSubscription.EventType)
            {
                case "TemperatureChanged":
                    controller.TemperatureChanged += base.OnEventReceived;
                    break;
                case "LightChanged":
                    controller.LightChanged += base.OnEventReceived;
                    break;
                case "HumidityChanged":
                    controller.HumidityChanged += base.OnEventReceived;
                    break;
                case "ButtonPressed":
                    controller.ButtonPressed += base.OnEventReceived;
                    break;
            }
        }

        /// <summary>
        /// Unadvises the event listener with a reference of the current controller instance
        /// </summary>
        /// <param name="controller">Reference to the controller</param>
        public override void UnAdvise(Controller controller)
        {
            switch (base.EventSubscription.EventType)
            {
                case "TemperatureChanged":
                    controller.TemperatureChanged -= base.OnEventReceived;
                    break;
                case "LightChanged":
                    controller.LightChanged -= base.OnEventReceived;
                    break;
                case "HumidityChanged":
                    controller.HumidityChanged -= base.OnEventReceived;
                    break;
                case "ButtonPressed":
                    controller.ButtonPressed -= base.OnEventReceived;
                    break;
            }
        }

        #endregion Methods
    }
}