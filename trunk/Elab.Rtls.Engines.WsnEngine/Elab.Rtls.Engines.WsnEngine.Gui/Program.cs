using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml.Schema;
using System.IO;
using System.Collections;

namespace GUI
{
    static class Program
    {
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
    }
    


    public class SensorNames
    {
        private List<SensorNames> list = new List<SensorNames>();
        public string Sensorname;
        public string id;
        
        public SensorNames(string Sensorname, string ID)
        {
            this.Sensorname = Sensorname;
            this.id = ID;
        }
    }

    public class Grafiek
    {
        public int NRmeetwaarden;
        public string TypeMeting;
        public Grafiek(int NRmeetwaarden, string TypeMeting)
        {
            this.NRmeetwaarden = NRmeetwaarden;
            this.TypeMeting = TypeMeting;
        }
    }

    public class Metingen
    {
        public string temperature;
        public string light;
        public string humidity;
        public string power;
        public string RSSI;
        public string position;
        public Metingen()
        {
            this.temperature = "temperature";
            this.light = "light";
            this.humidity = "humidity";
            this.power = "power";
            this.RSSI = "RSSI";
            this.position = "Position";
        }
    }


    public struct Changes
    {
        public string Active;
        public string AN;
        public string X;
        public string Y;
        public string samplerate;
        public string locrate;
        public string leds;
        public string power;
        public string frequency;
        public string conn;
    }
}








