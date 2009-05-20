namespace Elab.Rtls.Engines.WsnEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class SensorDataEventListener : EventListener
    {
        #region Constructors

        public SensorDataEventListener(Scala.Core.EventSubscription eventSubscription)
            : base(eventSubscription)
        {
        }

        #endregion Constructors

        #region Methods

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