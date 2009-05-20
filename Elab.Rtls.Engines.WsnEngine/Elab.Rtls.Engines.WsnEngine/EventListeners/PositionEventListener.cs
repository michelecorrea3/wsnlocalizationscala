namespace Elab.Rtls.Engines.WsnEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class PositionEventListener : EventListener
    {
        #region Constructors

        public PositionEventListener(Scala.Core.EventSubscription eventSubscription)
            : base(eventSubscription)
        {
        }

        #endregion Constructors

        #region Methods

        public override void Advise(Controller controller)
        {
            controller.LocationUpdated += base.OnEventReceived;
        }

        public override void UnAdvise(Controller controller)
        {
            controller.LocationUpdated -= base.OnEventReceived;
        }

        #endregion Methods
    }
}