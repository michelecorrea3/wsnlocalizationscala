using System;
using System.ServiceModel;

using Elab.Rtls.Engines.WsnEngine;

namespace Elab.Rtls.Engines.WsnEngine.Hosts
{
    /// <summary>
    /// 
    /// </summary>
    public class Host
    {
        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //bad design
            //start the WsnController
            Controller WsnController = new Controller();
            WsnEngine.Advise(WsnController);

            //start the WCF Engine
            using (ServiceHost host = HostService(typeof(Elab.Rtls.Engines.WsnEngine.WsnEngineService)))
            {
                Console.WriteLine("Press <ENTER> to quit hosting. . . ");
                Console.ReadLine();
            }

            //TODO: dispose of WsnController?
        }



        ///// <summary>
        ///// Constructor
        ///// </summary>
        ///// <param name="args"></param>
        //public Host()
        //{
        //    //start the WsnController
        //    Controller WsnController = new Controller();

        //    //start the WCF Engine
        //    using (ServiceHost host = HostService(typeof(Elab.Rtls.Engines.WsnEngine.WsnEngine)))
        //    {
        //        Console.WriteLine("Press <ENTER> to quit hosting. . . ");
        //        Console.ReadLine();
        //    }

        //    //TODO: dispose of WsnController?
        //}


        /// <summary>
        /// Hosts a service given by its type
        /// </summary>
        /// <param name="serviceType">The type of service to host.</param>
        /// <returns>A reference to the hosting-instance of the service.</returns>
        private static ServiceHost HostService(Type serviceType)
        {
            ServiceHost host = null;
            try
            {
                Console.Write("Hosting WsnEngineService. . .");
                host = new ServiceHost(serviceType);
                
                host.Open();
                Console.WriteLine(" Ok.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(String.Format("Scala: Unable to host service {0}: {1}", serviceType.Name, ex));
            }

            return host;
        }

        #endregion Methods
    }
}