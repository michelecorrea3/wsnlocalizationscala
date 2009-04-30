using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;

using Elab.Rtls.Engines.WsnEngine;
//using Elab.Rtls.Engines.WsnEngine.Hosts;

namespace Elab.Rtls.Engines.WsnEngine.EngineForm
{
    public partial class Form1 : Form
    {
        private Controller WsnController;
        private ServiceHost host;

        public Form1()
        {
            InitializeComponent();
            this.Text = "WsnEngine Panel";

            //start the WsnController
            WsnController = new Controller();
            WsnEngine.Advise(WsnController);

            //start the Wcf Host
            //Host WsnHost = new Host();
            host = HostService(typeof(Elab.Rtls.Engines.WsnEngine.WsnEngineService));
        }

        private void radioButtonCentroidLocalization_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCentroidLocalization.Checked)
                WsnController.SelectedAlgorithm = "CentroidLocalization";
        }

        private void radioButtonMinMaxSimpleModel_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMinMaxSimpleModel.Checked)
                WsnController.SelectedAlgorithm = "MinMax";
        }

        #region Methods





        /// <summary>
        /// Constructor
        /// </summary>
        ///<param name="args"></param>
        static void Host()
        {
            //start the WsnController
            //Controller WsnController = new Controller();

            //start the WCF Engine
            using (ServiceHost host = HostService(typeof(Elab.Rtls.Engines.WsnEngine.WsnEngineService)))
            {
                Console.WriteLine("Press <ENTER> to quit hosting. . . ");
                Console.ReadLine();
            }

            //TODO: dispose of WsnController?
        }


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
                //Console.Error.WriteLine(String.Format("Scala: Unable to host service {0}: {1}", serviceType.Name, ex));
                MessageBox.Show((String.Format("Scala: Unable to host service {0}: {1}", serviceType.Name, ex)));
            }

            return host;
        }

        #endregion Methods
    }
}
