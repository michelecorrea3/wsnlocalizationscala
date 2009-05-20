namespace Elab.Rtls.Engines.WsnEngine.EngineForm
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    static class Program
    {
        #region Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        #endregion Methods
    }
}