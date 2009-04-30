using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Elab.Rtls.Engines.WsnEngine;
using Elab.Rtls.Engines.WsnEngine.Hosts;

namespace Elab.Rtls.Engines.WsnEngine.EngineForm
{
    public partial class Form1 : Form
    {
        private Controller WsnController;

        public Form1()
        {
            InitializeComponent();
            this.Text = "WsnEngine Panel";

            //start the WsnController
            WsnController = new Controller();
            WsnEngine.Advise(WsnController);

            //start the Wcf Host
            Host WsnHost = new Host();
            
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
    }
}
