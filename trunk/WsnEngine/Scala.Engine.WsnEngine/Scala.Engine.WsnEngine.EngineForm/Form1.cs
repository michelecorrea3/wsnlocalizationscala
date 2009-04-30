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
        public Form1()
        {
            InitializeComponent();
            this.Text = "WsnEngine Panel";

            //start the WsnController
            Controller WsnController = new Controller();
            WsnEngine.Advise(WsnController);

            //start the Wcf Host
            Host WsnHost = new Host();
        }

        public string SelectedAlgorithm
        {
            get
            {
                if (radioButtonCentroidLocalization.Checked)
                    return "CentroidLocalization";
                else if (radioButtonMinMaxSimpleModel.Checked)
                    return "MinMaxSimpleModel";
                else
                {
                    return "Rubbish";
                }
            }
        }
    }
}
