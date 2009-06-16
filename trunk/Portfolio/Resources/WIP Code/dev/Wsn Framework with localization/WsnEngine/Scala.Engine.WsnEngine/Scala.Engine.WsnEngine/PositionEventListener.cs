using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elab.Rtls.Engines.WsnEngine
{
    public class PositionEventListener : EventListener
    {
        public PositionEventListener(Scala.Core.EventSubscription eventSubscription)
            : base(eventSubscription)
        { }

        public override void Advise(Controller controller)
        {
            controller.NewSensorDataEvent += base.OnEventReceived;
        }

        public override void UnAdvise(Controller controller)
        {
            controller.NewSensorDataEvent -= base.OnEventReceived;
        }
    }
}
