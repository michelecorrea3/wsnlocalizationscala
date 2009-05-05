using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elab.Rtls.Engines.WsnEngine
{
    public class SensorDataEventListener : EventListener
    {
        public SensorDataEventListener(Scala.Core.EventSubscription eventSubscription)
            : base(eventSubscription)
        { }

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
    }
}
