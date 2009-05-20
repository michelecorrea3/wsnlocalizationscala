namespace GUI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public struct Changes
    {
        #region Fields

        public string AN;
        public string Active;
        public string X;
        public string Y;
        public string conn;
        public string frequency;
        public string leds;
        public string locrate;
        public string power;
        public string samplerate;

        #endregion Fields
    }

    public class Grafiek
    {
        #region Fields

        public int NRmeetwaarden;
        public string TypeMeting;

        #endregion Fields

        #region Constructors

        public Grafiek(int NRmeetwaarden, string TypeMeting)
        {
            this.NRmeetwaarden = NRmeetwaarden;
            this.TypeMeting = TypeMeting;
        }

        #endregion Constructors
    }

    public class Metingen
    {
        #region Fields

        public string RSSI;
        public string humidity;
        public string light;
        public string position;
        public string power;
        public string temperature;

        #endregion Fields

        #region Constructors

        public Metingen()
        {
            this.temperature = "temperature";
            this.light = "light";
            this.humidity = "humidity";
            this.power = "power";
            this.RSSI = "RSSI";
            this.position = "Position";
        }

        #endregion Constructors
    }

    public class SensorNames
    {
        #region Fields

        public string Sensorname;
        public string id;

        private List<SensorNames> list = new List<SensorNames>();

        #endregion Fields

        #region Constructors

        public SensorNames(string Sensorname, string ID)
        {
            this.Sensorname = Sensorname;
            this.id = ID;
        }

        #endregion Constructors
    }

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