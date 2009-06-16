namespace GUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "",
            "",
            ""}, -1);
            this.timerSensor = new System.Windows.Forms.Timer(this.components);
            this.timerLoc = new System.Windows.Forms.Timer(this.components);
            this.timerSensorFetch = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tabOptions = new System.Windows.Forms.TabPage();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.textBoxSensorRateMax = new System.Windows.Forms.TextBox();
            this.textBoxLocRateMax = new System.Windows.Forms.TextBox();
            this.textBoxYmax = new System.Windows.Forms.TextBox();
            this.textBoxXmax = new System.Windows.Forms.TextBox();
            this.textBoxSensorRateMin = new System.Windows.Forms.TextBox();
            this.textBoxLocRateMin = new System.Windows.Forms.TextBox();
            this.textBoxYmin = new System.Windows.Forms.TextBox();
            this.textBoxXmin = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.maskedTextBox1 = new System.Windows.Forms.MaskedTextBox();
            this.textBoxSensorFetchUpdateInterval = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.radioButtonSensorTimeOut = new System.Windows.Forms.RadioButton();
            this.radioButtonGetSensors = new System.Windows.Forms.RadioButton();
            this.radioButtonDiscovery = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.controllerPort = new System.Windows.Forms.TextBox();
            this.label44 = new System.Windows.Forms.Label();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.label45 = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.controllerIP = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBoxGraphUpdateInterval = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textLocRefresh = new System.Windows.Forms.TextBox();
            this.textSensRefresh = new System.Windows.Forms.TextBox();
            this.label47 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.tabWSNAdmin = new System.Windows.Forms.TabPage();
            this.panelControl = new System.Windows.Forms.Panel();
            this.checkBoxLedBlue = new System.Windows.Forms.CheckBox();
            this.checkBoxLedGreen = new System.Windows.Forms.CheckBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.checkBoxLedRed = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxControlX = new System.Windows.Forms.TextBox();
            this.checkBoxActive = new System.Windows.Forms.CheckBox();
            this.checkBoxAnchorNode = new System.Windows.Forms.CheckBox();
            this.textBoxControlY = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.textBoxSampleRate = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label60 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.numericUpDownFrequency = new System.Windows.Forms.NumericUpDown();
            this.label61 = new System.Windows.Forms.Label();
            this.textBoxLocRate = new System.Windows.Forms.TextBox();
            this.numericUpDownPower = new System.Windows.Forms.NumericUpDown();
            this.label62 = new System.Windows.Forms.Label();
            this.label63 = new System.Windows.Forms.Label();
            this.label64 = new System.Windows.Forms.Label();
            this.textBoxConn = new System.Windows.Forms.TextBox();
            this.buttonControlDiscard = new System.Windows.Forms.Button();
            this.label58 = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.label55 = new System.Windows.Forms.Label();
            this.buttonWSNControl = new System.Windows.Forms.Button();
            this.listBoxControl = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.listView2 = new System.Windows.Forms.ListView();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.checkBoxPlotUpdate = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.zg1 = new ZedGraph.ZedGraphControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.textBoxLocUpdate = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.textBoxNodeID = new System.Windows.Forms.TextBox();
            this.label53 = new System.Windows.Forms.Label();
            this.textBoxANodeID = new System.Windows.Forms.TextBox();
            this.label52 = new System.Windows.Forms.Label();
            this.textBoxRSSI = new System.Windows.Forms.TextBox();
            this.label51 = new System.Windows.Forms.Label();
            this.textBoxX = new System.Windows.Forms.TextBox();
            this.label50 = new System.Windows.Forms.Label();
            this.textBoxY = new System.Windows.Forms.TextBox();
            this.label49 = new System.Windows.Forms.Label();
            this.textBoxZ = new System.Windows.Forms.TextBox();
            this.listBoxLoc = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxSensUpdate = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.timerGraph = new System.Windows.Forms.Timer(this.components);
            this.toolTipActive = new System.Windows.Forms.ToolTip(this.components);
            this.timerStatus = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabOptions.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabWSNAdmin.SuspendLayout();
            this.panelControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPower)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerSensor
            // 
            this.timerSensor.Interval = 1000;
            this.timerSensor.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timerLoc
            // 
            this.timerLoc.Interval = 1000;
            this.timerLoc.Tag = "timer for the localization data update";
            this.timerLoc.Tick += new System.EventHandler(this.timerLoc_Tick);
            // 
            // timerSensorFetch
            // 
            this.timerSensorFetch.Interval = 60000;
            this.timerSensorFetch.Tag = "timer for the discovery update";
            this.timerSensorFetch.Tick += new System.EventHandler(this.timerDiscovery_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 454);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(779, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(78, 17);
            this.toolStripStatusLabel.Text = "No Connection";
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.Transparent;
            this.tabPage4.Controls.Add(this.pictureBox1);
            this.tabPage4.Controls.Add(this.richTextBox2);
            this.tabPage4.Controls.Add(this.richTextBox1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(761, 444);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "About";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(255, 30);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(213, 65);
            this.pictureBox1.TabIndex = 18;
            this.pictureBox1.TabStop = false;
            // 
            // richTextBox2
            // 
            this.richTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox2.Location = new System.Drawing.Point(4, 30);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(754, 65);
            this.richTextBox2.TabIndex = 17;
            this.richTextBox2.Text = "";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(8, 118);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(750, 323);
            this.richTextBox1.TabIndex = 16;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // tabOptions
            // 
            this.tabOptions.Controls.Add(this.groupBox8);
            this.tabOptions.Controls.Add(this.groupBox7);
            this.tabOptions.Controls.Add(this.groupBox5);
            this.tabOptions.Controls.Add(this.groupBox4);
            this.tabOptions.Location = new System.Drawing.Point(4, 22);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabOptions.Size = new System.Drawing.Size(761, 444);
            this.tabOptions.TabIndex = 6;
            this.tabOptions.Text = "Options";
            this.tabOptions.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.textBoxSensorRateMax);
            this.groupBox8.Controls.Add(this.textBoxLocRateMax);
            this.groupBox8.Controls.Add(this.textBoxYmax);
            this.groupBox8.Controls.Add(this.textBoxXmax);
            this.groupBox8.Controls.Add(this.textBoxSensorRateMin);
            this.groupBox8.Controls.Add(this.textBoxLocRateMin);
            this.groupBox8.Controls.Add(this.textBoxYmin);
            this.groupBox8.Controls.Add(this.textBoxXmin);
            this.groupBox8.Controls.Add(this.label25);
            this.groupBox8.Controls.Add(this.label24);
            this.groupBox8.Controls.Add(this.label23);
            this.groupBox8.Controls.Add(this.label22);
            this.groupBox8.Controls.Add(this.label17);
            this.groupBox8.Controls.Add(this.label16);
            this.groupBox8.Location = new System.Drawing.Point(357, 221);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(361, 168);
            this.groupBox8.TabIndex = 16;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Control parameter bounds";
            // 
            // textBoxSensorRateMax
            // 
            this.textBoxSensorRateMax.Location = new System.Drawing.Point(192, 133);
            this.textBoxSensorRateMax.Name = "textBoxSensorRateMax";
            this.textBoxSensorRateMax.Size = new System.Drawing.Size(100, 20);
            this.textBoxSensorRateMax.TabIndex = 13;
            // 
            // textBoxLocRateMax
            // 
            this.textBoxLocRateMax.Location = new System.Drawing.Point(192, 103);
            this.textBoxLocRateMax.Name = "textBoxLocRateMax";
            this.textBoxLocRateMax.Size = new System.Drawing.Size(100, 20);
            this.textBoxLocRateMax.TabIndex = 12;
            // 
            // textBoxYmax
            // 
            this.textBoxYmax.Location = new System.Drawing.Point(192, 73);
            this.textBoxYmax.Name = "textBoxYmax";
            this.textBoxYmax.Size = new System.Drawing.Size(100, 20);
            this.textBoxYmax.TabIndex = 11;
            // 
            // textBoxXmax
            // 
            this.textBoxXmax.Location = new System.Drawing.Point(192, 43);
            this.textBoxXmax.Name = "textBoxXmax";
            this.textBoxXmax.Size = new System.Drawing.Size(100, 20);
            this.textBoxXmax.TabIndex = 10;
            // 
            // textBoxSensorRateMin
            // 
            this.textBoxSensorRateMin.Location = new System.Drawing.Point(73, 133);
            this.textBoxSensorRateMin.Name = "textBoxSensorRateMin";
            this.textBoxSensorRateMin.Size = new System.Drawing.Size(100, 20);
            this.textBoxSensorRateMin.TabIndex = 9;
            // 
            // textBoxLocRateMin
            // 
            this.textBoxLocRateMin.Location = new System.Drawing.Point(73, 103);
            this.textBoxLocRateMin.Name = "textBoxLocRateMin";
            this.textBoxLocRateMin.Size = new System.Drawing.Size(100, 20);
            this.textBoxLocRateMin.TabIndex = 8;
            // 
            // textBoxYmin
            // 
            this.textBoxYmin.Location = new System.Drawing.Point(73, 73);
            this.textBoxYmin.Name = "textBoxYmin";
            this.textBoxYmin.Size = new System.Drawing.Size(100, 20);
            this.textBoxYmin.TabIndex = 7;
            // 
            // textBoxXmin
            // 
            this.textBoxXmin.Location = new System.Drawing.Point(73, 43);
            this.textBoxXmin.Name = "textBoxXmin";
            this.textBoxXmin.Size = new System.Drawing.Size(100, 20);
            this.textBoxXmin.TabIndex = 6;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(219, 24);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(30, 13);
            this.label25.TabIndex = 5;
            this.label25.Text = "Max:";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(112, 22);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(27, 13);
            this.label24.TabIndex = 4;
            this.label24.Text = "Min:";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(6, 133);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(66, 13);
            this.label23.TabIndex = 3;
            this.label23.Text = "SensorRate:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(6, 103);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(51, 13);
            this.label22.TabIndex = 2;
            this.label22.Text = "LocRate:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 73);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(17, 13);
            this.label17.TabIndex = 1;
            this.label17.Text = "Y:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 43);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(17, 13);
            this.label16.TabIndex = 0;
            this.label16.Text = "X:";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.maskedTextBox1);
            this.groupBox7.Controls.Add(this.textBoxSensorFetchUpdateInterval);
            this.groupBox7.Controls.Add(this.label27);
            this.groupBox7.Controls.Add(this.label26);
            this.groupBox7.Controls.Add(this.radioButtonSensorTimeOut);
            this.groupBox7.Controls.Add(this.radioButtonGetSensors);
            this.groupBox7.Controls.Add(this.radioButtonDiscovery);
            this.groupBox7.Location = new System.Drawing.Point(23, 221);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(279, 168);
            this.groupBox7.TabIndex = 15;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "SensorFetch";
            // 
            // maskedTextBox1
            // 
            this.maskedTextBox1.BeepOnError = true;
            this.maskedTextBox1.Location = new System.Drawing.Point(120, 132);
            this.maskedTextBox1.Mask = "0.00:00:00";
            this.maskedTextBox1.Name = "maskedTextBox1";
            this.maskedTextBox1.Size = new System.Drawing.Size(100, 20);
            this.maskedTextBox1.TabIndex = 18;
            this.maskedTextBox1.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.maskedTextBox1_MaskInputRejected);
            this.maskedTextBox1.TypeValidationCompleted += new System.Windows.Forms.TypeValidationEventHandler(this.maskedTextBox1_TypeValidationCompleted);
            // 
            // textBoxSensorFetchUpdateInterval
            // 
            this.textBoxSensorFetchUpdateInterval.Location = new System.Drawing.Point(120, 95);
            this.textBoxSensorFetchUpdateInterval.Name = "textBoxSensorFetchUpdateInterval";
            this.textBoxSensorFetchUpdateInterval.Size = new System.Drawing.Size(100, 20);
            this.textBoxSensorFetchUpdateInterval.TabIndex = 17;
            this.textBoxSensorFetchUpdateInterval.TextChanged += new System.EventHandler(this.textBoxSensorFetchUpdateInterval_TextChanged);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(28, 133);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(73, 13);
            this.label27.TabIndex = 16;
            this.label27.Text = "TimeOut Time";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(28, 103);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(88, 13);
            this.label26.TabIndex = 15;
            this.label26.Text = "SensorFetch rate";
            // 
            // radioButtonSensorTimeOut
            // 
            this.radioButtonSensorTimeOut.AutoSize = true;
            this.radioButtonSensorTimeOut.Checked = true;
            this.radioButtonSensorTimeOut.Location = new System.Drawing.Point(19, 66);
            this.radioButtonSensorTimeOut.Name = "radioButtonSensorTimeOut";
            this.radioButtonSensorTimeOut.Size = new System.Drawing.Size(98, 17);
            this.radioButtonSensorTimeOut.TabIndex = 14;
            this.radioButtonSensorTimeOut.TabStop = true;
            this.radioButtonSensorTimeOut.Text = "SensorTimeOut";
            this.radioButtonSensorTimeOut.UseVisualStyleBackColor = true;
            // 
            // radioButtonGetSensors
            // 
            this.radioButtonGetSensors.AutoSize = true;
            this.radioButtonGetSensors.Enabled = false;
            this.radioButtonGetSensors.Location = new System.Drawing.Point(19, 20);
            this.radioButtonGetSensors.Name = "radioButtonGetSensors";
            this.radioButtonGetSensors.Size = new System.Drawing.Size(80, 17);
            this.radioButtonGetSensors.TabIndex = 12;
            this.radioButtonGetSensors.Text = "GetSensors";
            this.radioButtonGetSensors.UseVisualStyleBackColor = true;
            // 
            // radioButtonDiscovery
            // 
            this.radioButtonDiscovery.AutoSize = true;
            this.radioButtonDiscovery.Enabled = false;
            this.radioButtonDiscovery.Location = new System.Drawing.Point(19, 43);
            this.radioButtonDiscovery.Name = "radioButtonDiscovery";
            this.radioButtonDiscovery.Size = new System.Drawing.Size(72, 17);
            this.radioButtonDiscovery.TabIndex = 13;
            this.radioButtonDiscovery.Text = "Discovery";
            this.radioButtonDiscovery.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.controllerPort);
            this.groupBox5.Controls.Add(this.label44);
            this.groupBox5.Controls.Add(this.buttonDisconnect);
            this.groupBox5.Controls.Add(this.label45);
            this.groupBox5.Controls.Add(this.buttonConnect);
            this.groupBox5.Controls.Add(this.controllerIP);
            this.groupBox5.Location = new System.Drawing.Point(23, 34);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(279, 155);
            this.groupBox5.TabIndex = 11;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Controller ";
            // 
            // controllerPort
            // 
            this.controllerPort.Location = new System.Drawing.Point(86, 67);
            this.controllerPort.Name = "controllerPort";
            this.controllerPort.Size = new System.Drawing.Size(100, 20);
            this.controllerPort.TabIndex = 1;
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(28, 30);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(17, 13);
            this.label44.TabIndex = 2;
            this.label44.Text = "IP";
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(134, 107);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(75, 23);
            this.buttonDisconnect.TabIndex = 9;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(28, 70);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(26, 13);
            this.label45.TabIndex = 3;
            this.label45.Text = "Port";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(31, 107);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 4;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // controllerIP
            // 
            this.controllerIP.Location = new System.Drawing.Point(86, 27);
            this.controllerIP.Name = "controllerIP";
            this.controllerIP.Size = new System.Drawing.Size(100, 20);
            this.controllerIP.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBoxGraphUpdateInterval);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.textLocRefresh);
            this.groupBox4.Controls.Add(this.textSensRefresh);
            this.groupBox4.Controls.Add(this.label47);
            this.groupBox4.Controls.Add(this.label46);
            this.groupBox4.Location = new System.Drawing.Point(357, 34);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(361, 155);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Polling rates";
            // 
            // textBoxGraphUpdateInterval
            // 
            this.textBoxGraphUpdateInterval.Location = new System.Drawing.Point(149, 116);
            this.textBoxGraphUpdateInterval.Name = "textBoxGraphUpdateInterval";
            this.textBoxGraphUpdateInterval.Size = new System.Drawing.Size(100, 20);
            this.textBoxGraphUpdateInterval.TabIndex = 10;
            this.textBoxGraphUpdateInterval.TextChanged += new System.EventHandler(this.textBoxGraphUpdateInterval_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(26, 119);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(94, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "Graph Refreshrate";
            // 
            // textLocRefresh
            // 
            this.textLocRefresh.Location = new System.Drawing.Point(149, 71);
            this.textLocRefresh.Name = "textLocRefresh";
            this.textLocRefresh.Size = new System.Drawing.Size(100, 20);
            this.textLocRefresh.TabIndex = 6;
            this.textLocRefresh.TextChanged += new System.EventHandler(this.textLocRefresh_TextChanged);
            // 
            // textSensRefresh
            // 
            this.textSensRefresh.Location = new System.Drawing.Point(149, 31);
            this.textSensRefresh.Name = "textSensRefresh";
            this.textSensRefresh.Size = new System.Drawing.Size(100, 20);
            this.textSensRefresh.TabIndex = 5;
            this.textSensRefresh.TextChanged += new System.EventHandler(this.textSensRefresh_TextChanged);
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(26, 71);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(86, 13);
            this.label47.TabIndex = 8;
            this.label47.Text = "Loc. Refreshrate";
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(26, 31);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(98, 13);
            this.label46.TabIndex = 7;
            this.label46.Text = "Sensor Refreshrate";
            // 
            // tabWSNAdmin
            // 
            this.tabWSNAdmin.Controls.Add(this.panelControl);
            this.tabWSNAdmin.Controls.Add(this.buttonControlDiscard);
            this.tabWSNAdmin.Controls.Add(this.label58);
            this.tabWSNAdmin.Controls.Add(this.label59);
            this.tabWSNAdmin.Controls.Add(this.label56);
            this.tabWSNAdmin.Controls.Add(this.label57);
            this.tabWSNAdmin.Controls.Add(this.label55);
            this.tabWSNAdmin.Controls.Add(this.buttonWSNControl);
            this.tabWSNAdmin.Controls.Add(this.listBoxControl);
            this.tabWSNAdmin.Location = new System.Drawing.Point(4, 22);
            this.tabWSNAdmin.Name = "tabWSNAdmin";
            this.tabWSNAdmin.Padding = new System.Windows.Forms.Padding(3);
            this.tabWSNAdmin.Size = new System.Drawing.Size(761, 444);
            this.tabWSNAdmin.TabIndex = 5;
            this.tabWSNAdmin.Text = "WSN Admin";
            this.tabWSNAdmin.UseVisualStyleBackColor = true;
            // 
            // panelControl
            // 
            this.panelControl.Controls.Add(this.checkBoxLedBlue);
            this.panelControl.Controls.Add(this.checkBoxLedGreen);
            this.panelControl.Controls.Add(this.label18);
            this.panelControl.Controls.Add(this.label20);
            this.panelControl.Controls.Add(this.label19);
            this.panelControl.Controls.Add(this.checkBoxLedRed);
            this.panelControl.Controls.Add(this.label13);
            this.panelControl.Controls.Add(this.textBoxControlX);
            this.panelControl.Controls.Add(this.checkBoxActive);
            this.panelControl.Controls.Add(this.checkBoxAnchorNode);
            this.panelControl.Controls.Add(this.textBoxControlY);
            this.panelControl.Controls.Add(this.label15);
            this.panelControl.Controls.Add(this.textBoxSampleRate);
            this.panelControl.Controls.Add(this.label21);
            this.panelControl.Controls.Add(this.label60);
            this.panelControl.Controls.Add(this.label11);
            this.panelControl.Controls.Add(this.numericUpDownFrequency);
            this.panelControl.Controls.Add(this.label61);
            this.panelControl.Controls.Add(this.textBoxLocRate);
            this.panelControl.Controls.Add(this.numericUpDownPower);
            this.panelControl.Controls.Add(this.label62);
            this.panelControl.Controls.Add(this.label63);
            this.panelControl.Controls.Add(this.label64);
            this.panelControl.Controls.Add(this.textBoxConn);
            this.panelControl.Location = new System.Drawing.Point(241, 23);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(502, 254);
            this.panelControl.TabIndex = 38;
            // 
            // checkBoxLedBlue
            // 
            this.checkBoxLedBlue.AutoSize = true;
            this.checkBoxLedBlue.Location = new System.Drawing.Point(256, 152);
            this.checkBoxLedBlue.Name = "checkBoxLedBlue";
            this.checkBoxLedBlue.Size = new System.Drawing.Size(70, 17);
            this.checkBoxLedBlue.TabIndex = 37;
            this.checkBoxLedBlue.Text = "Red Led ";
            this.checkBoxLedBlue.UseVisualStyleBackColor = true;
            // 
            // checkBoxLedGreen
            // 
            this.checkBoxLedGreen.AutoSize = true;
            this.checkBoxLedGreen.Location = new System.Drawing.Point(256, 128);
            this.checkBoxLedGreen.Name = "checkBoxLedGreen";
            this.checkBoxLedGreen.Size = new System.Drawing.Size(79, 17);
            this.checkBoxLedGreen.TabIndex = 36;
            this.checkBoxLedGreen.Text = "Green Led ";
            this.checkBoxLedGreen.UseVisualStyleBackColor = true;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(263, 224);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(65, 13);
            this.label18.TabIndex = 33;
            this.label18.Text = "Connectivity";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(254, 68);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(57, 13);
            this.label20.TabIndex = 44;
            this.label20.Text = "Frequency";
            this.toolTipActive.SetToolTip(this.label20, "Sets the transmission frequency of a node");
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(253, 15);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(63, 13);
            this.label19.TabIndex = 34;
            this.label19.Text = "Sample rate";
            // 
            // checkBoxLedRed
            // 
            this.checkBoxLedRed.AutoSize = true;
            this.checkBoxLedRed.Location = new System.Drawing.Point(256, 104);
            this.checkBoxLedRed.Name = "checkBoxLedRed";
            this.checkBoxLedRed.Size = new System.Drawing.Size(68, 17);
            this.checkBoxLedRed.TabIndex = 35;
            this.checkBoxLedRed.Text = "Blue Led";
            this.checkBoxLedRed.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(23, 68);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(14, 13);
            this.label13.TabIndex = 27;
            this.label13.Text = "X";
            // 
            // textBoxControlX
            // 
            this.textBoxControlX.Enabled = false;
            this.textBoxControlX.Location = new System.Drawing.Point(87, 70);
            this.textBoxControlX.Name = "textBoxControlX";
            this.textBoxControlX.Size = new System.Drawing.Size(100, 20);
            this.textBoxControlX.TabIndex = 5;
            // 
            // checkBoxActive
            // 
            this.checkBoxActive.AutoSize = true;
            this.checkBoxActive.Location = new System.Drawing.Point(26, 11);
            this.checkBoxActive.Name = "checkBoxActive";
            this.checkBoxActive.Size = new System.Drawing.Size(56, 17);
            this.checkBoxActive.TabIndex = 39;
            this.checkBoxActive.Text = "Active";
            this.toolTipActive.SetToolTip(this.checkBoxActive, "Sets the node to active or inactive for the localization algorithm");
            this.checkBoxActive.UseVisualStyleBackColor = true;
            this.checkBoxActive.CheckedChanged += new System.EventHandler(this.checkBoxActive_CheckedChanged);
            // 
            // checkBoxAnchorNode
            // 
            this.checkBoxAnchorNode.AutoSize = true;
            this.checkBoxAnchorNode.Location = new System.Drawing.Point(26, 38);
            this.checkBoxAnchorNode.Name = "checkBoxAnchorNode";
            this.checkBoxAnchorNode.Size = new System.Drawing.Size(87, 17);
            this.checkBoxAnchorNode.TabIndex = 40;
            this.checkBoxAnchorNode.Text = "Anchor node";
            this.toolTipActive.SetToolTip(this.checkBoxAnchorNode, "Sets the node as AN or BN");
            this.checkBoxAnchorNode.UseVisualStyleBackColor = true;
            this.checkBoxAnchorNode.CheckedChanged += new System.EventHandler(this.checkBoxAnchorNode_CheckedChanged);
            // 
            // textBoxControlY
            // 
            this.textBoxControlY.Enabled = false;
            this.textBoxControlY.Location = new System.Drawing.Point(87, 103);
            this.textBoxControlY.Name = "textBoxControlY";
            this.textBoxControlY.Size = new System.Drawing.Size(100, 20);
            this.textBoxControlY.TabIndex = 6;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(22, 143);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(46, 13);
            this.label15.TabIndex = 29;
            this.label15.Text = "Loc rate";
            // 
            // textBoxSampleRate
            // 
            this.textBoxSampleRate.Location = new System.Drawing.Point(343, 12);
            this.textBoxSampleRate.Name = "textBoxSampleRate";
            this.textBoxSampleRate.Size = new System.Drawing.Size(100, 20);
            this.textBoxSampleRate.TabIndex = 7;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(254, 43);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(37, 13);
            this.label21.TabIndex = 43;
            this.label21.Text = "Power";
            this.toolTipActive.SetToolTip(this.label21, "Sets the transmission power of a node");
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Location = new System.Drawing.Point(235, 147);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(0, 13);
            this.label60.TabIndex = 23;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(23, 103);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(14, 13);
            this.label11.TabIndex = 28;
            this.label11.Text = "Y";
            // 
            // numericUpDownFrequency
            // 
            this.numericUpDownFrequency.Location = new System.Drawing.Point(343, 66);
            this.numericUpDownFrequency.Maximum = new decimal(new int[] {
            26,
            0,
            0,
            0});
            this.numericUpDownFrequency.Minimum = new decimal(new int[] {
            11,
            0,
            0,
            0});
            this.numericUpDownFrequency.Name = "numericUpDownFrequency";
            this.numericUpDownFrequency.Size = new System.Drawing.Size(43, 20);
            this.numericUpDownFrequency.TabIndex = 42;
            this.numericUpDownFrequency.Value = new decimal(new int[] {
            11,
            0,
            0,
            0});
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Location = new System.Drawing.Point(235, 129);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(0, 13);
            this.label61.TabIndex = 22;
            // 
            // textBoxLocRate
            // 
            this.textBoxLocRate.Enabled = false;
            this.textBoxLocRate.Location = new System.Drawing.Point(87, 140);
            this.textBoxLocRate.Name = "textBoxLocRate";
            this.textBoxLocRate.Size = new System.Drawing.Size(100, 20);
            this.textBoxLocRate.TabIndex = 8;
            // 
            // numericUpDownPower
            // 
            this.numericUpDownPower.Location = new System.Drawing.Point(343, 41);
            this.numericUpDownPower.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.numericUpDownPower.Name = "numericUpDownPower";
            this.numericUpDownPower.Size = new System.Drawing.Size(43, 20);
            this.numericUpDownPower.TabIndex = 41;
            this.numericUpDownPower.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Location = new System.Drawing.Point(235, 26);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(0, 13);
            this.label62.TabIndex = 21;
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Location = new System.Drawing.Point(235, 39);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(0, 13);
            this.label63.TabIndex = 20;
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Location = new System.Drawing.Point(235, 3);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(0, 13);
            this.label64.TabIndex = 19;
            // 
            // textBoxConn
            // 
            this.textBoxConn.Enabled = false;
            this.textBoxConn.Location = new System.Drawing.Point(360, 217);
            this.textBoxConn.Name = "textBoxConn";
            this.textBoxConn.Size = new System.Drawing.Size(100, 20);
            this.textBoxConn.TabIndex = 12;
            // 
            // buttonControlDiscard
            // 
            this.buttonControlDiscard.Location = new System.Drawing.Point(625, 407);
            this.buttonControlDiscard.Name = "buttonControlDiscard";
            this.buttonControlDiscard.Size = new System.Drawing.Size(106, 23);
            this.buttonControlDiscard.TabIndex = 24;
            this.buttonControlDiscard.Text = "Discard Changes";
            this.toolTipActive.SetToolTip(this.buttonControlDiscard, "Discard any changes that you made");
            this.buttonControlDiscard.UseVisualStyleBackColor = true;
            this.buttonControlDiscard.Click += new System.EventHandler(this.buttonControlDiscard_Click);
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Location = new System.Drawing.Point(177, 181);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(0, 13);
            this.label58.TabIndex = 18;
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(177, 146);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(0, 13);
            this.label59.TabIndex = 17;
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(177, 101);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(0, 13);
            this.label56.TabIndex = 16;
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(177, 66);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(0, 13);
            this.label57.TabIndex = 15;
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(177, 23);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(0, 13);
            this.label55.TabIndex = 14;
            // 
            // buttonWSNControl
            // 
            this.buttonWSNControl.Location = new System.Drawing.Point(485, 407);
            this.buttonWSNControl.Name = "buttonWSNControl";
            this.buttonWSNControl.Size = new System.Drawing.Size(107, 23);
            this.buttonWSNControl.TabIndex = 1;
            this.buttonWSNControl.Text = "Apply Changes";
            this.toolTipActive.SetToolTip(this.buttonWSNControl, "Send the changes to the WSN");
            this.buttonWSNControl.UseVisualStyleBackColor = true;
            this.buttonWSNControl.Click += new System.EventHandler(this.buttonWSNControl_Click);
            // 
            // listBoxControl
            // 
            this.listBoxControl.FormattingEnabled = true;
            this.listBoxControl.Location = new System.Drawing.Point(34, 27);
            this.listBoxControl.Name = "listBoxControl";
            this.listBoxControl.Size = new System.Drawing.Size(87, 264);
            this.listBoxControl.TabIndex = 0;
            this.listBoxControl.SelectedIndexChanged += new System.EventHandler(this.listBoxControl_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Transparent;
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.groupBox13);
            this.tabPage2.Controls.Add(this.zg1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(761, 444);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Graph";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label10.Location = new System.Drawing.Point(298, 3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(141, 24);
            this.label10.TabIndex = 6;
            this.label10.Text = "WSN Monitor";
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.groupBox15);
            this.groupBox13.Controls.Add(this.groupBox14);
            this.groupBox13.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox13.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.groupBox13.Location = new System.Drawing.Point(8, 40);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(233, 396);
            this.groupBox13.TabIndex = 5;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Graph";
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.listView2);
            this.groupBox15.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox15.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.groupBox15.Location = new System.Drawing.Point(6, 122);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(219, 268);
            this.groupBox15.TabIndex = 3;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Values";
            // 
            // listView2
            // 
            this.listView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4});
            this.listView2.FullRowSelect = true;
            this.listView2.GridLines = true;
            this.listView2.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView2.HideSelection = false;
            listViewItem6.IndentCount = 10;
            listViewItem6.Tag = "";
            this.listView2.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem6});
            this.listView2.Location = new System.Drawing.Point(6, 21);
            this.listView2.Name = "listView2";
            this.listView2.Scrollable = false;
            this.listView2.Size = new System.Drawing.Size(207, 241);
            this.listView2.TabIndex = 9;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Value";
            this.columnHeader4.Width = 219;
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.checkBoxPlotUpdate);
            this.groupBox14.Controls.Add(this.button2);
            this.groupBox14.Controls.Add(this.comboBox2);
            this.groupBox14.Controls.Add(this.comboBox1);
            this.groupBox14.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox14.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.groupBox14.Location = new System.Drawing.Point(6, 24);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(219, 92);
            this.groupBox14.TabIndex = 2;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Parameters";
            // 
            // checkBoxPlotUpdate
            // 
            this.checkBoxPlotUpdate.AutoSize = true;
            this.checkBoxPlotUpdate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxPlotUpdate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxPlotUpdate.Location = new System.Drawing.Point(152, 35);
            this.checkBoxPlotUpdate.Name = "checkBoxPlotUpdate";
            this.checkBoxPlotUpdate.Size = new System.Drawing.Size(49, 20);
            this.checkBoxPlotUpdate.TabIndex = 9;
            this.checkBoxPlotUpdate.Text = "Live";
            this.checkBoxPlotUpdate.UseVisualStyleBackColor = true;
            this.checkBoxPlotUpdate.CheckedChanged += new System.EventHandler(this.checkBoxPlotUpdate_CheckedChanged);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button2.Location = new System.Drawing.Point(152, 60);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(61, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Plot";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // comboBox2
            // 
            this.comboBox2.AccessibleDescription = "";
            this.comboBox2.AccessibleName = "";
            this.comboBox2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(6, 29);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(133, 24);
            this.comboBox2.TabIndex = 8;
            this.comboBox2.Tag = "";
            this.comboBox2.Text = "Choose sensor";
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.White;
            this.comboBox1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(6, 59);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(133, 24);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.Text = "Choose parameter";
            // 
            // zg1
            // 
            this.zg1.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.zg1.Location = new System.Drawing.Point(264, 40);
            this.zg1.Margin = new System.Windows.Forms.Padding(2);
            this.zg1.Name = "zg1";
            this.zg1.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.zg1.ScrollGrace = 0;
            this.zg1.ScrollMaxX = 0;
            this.zg1.ScrollMaxY = 0;
            this.zg1.ScrollMaxY2 = 0;
            this.zg1.ScrollMinX = 0;
            this.zg1.ScrollMinY = 0;
            this.zg1.ScrollMinY2 = 0;
            this.zg1.Size = new System.Drawing.Size(478, 294);
            this.zg1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.groupBox6);
            this.tabPage1.Controls.Add(this.listBoxLoc);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(761, 444);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Monitor";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(26, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = "WSN id";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.textBoxLocUpdate);
            this.groupBox6.Controls.Add(this.label12);
            this.groupBox6.Controls.Add(this.label48);
            this.groupBox6.Controls.Add(this.textBoxNodeID);
            this.groupBox6.Controls.Add(this.label53);
            this.groupBox6.Controls.Add(this.textBoxANodeID);
            this.groupBox6.Controls.Add(this.label52);
            this.groupBox6.Controls.Add(this.textBoxRSSI);
            this.groupBox6.Controls.Add(this.label51);
            this.groupBox6.Controls.Add(this.textBoxX);
            this.groupBox6.Controls.Add(this.label50);
            this.groupBox6.Controls.Add(this.textBoxY);
            this.groupBox6.Controls.Add(this.label49);
            this.groupBox6.Controls.Add(this.textBoxZ);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox6.Location = new System.Drawing.Point(167, 40);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(275, 304);
            this.groupBox6.TabIndex = 26;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Localization";
            // 
            // textBoxLocUpdate
            // 
            this.textBoxLocUpdate.Cursor = System.Windows.Forms.Cursors.No;
            this.textBoxLocUpdate.Enabled = false;
            this.textBoxLocUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLocUpdate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxLocUpdate.Location = new System.Drawing.Point(158, 269);
            this.textBoxLocUpdate.Name = "textBoxLocUpdate";
            this.textBoxLocUpdate.Size = new System.Drawing.Size(100, 20);
            this.textBoxLocUpdate.TabIndex = 30;
            this.textBoxLocUpdate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label12.Location = new System.Drawing.Point(17, 275);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(66, 13);
            this.label12.TabIndex = 27;
            this.label12.Text = "Last update:";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Enabled = false;
            this.label48.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label48.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label48.Location = new System.Drawing.Point(17, 28);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(47, 13);
            this.label48.TabIndex = 19;
            this.label48.Text = "Node ID";
            this.toolTipActive.SetToolTip(this.label48, "The nodeif of the selected node");
            // 
            // textBoxNodeID
            // 
            this.textBoxNodeID.Enabled = false;
            this.textBoxNodeID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxNodeID.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxNodeID.Location = new System.Drawing.Point(158, 28);
            this.textBoxNodeID.Name = "textBoxNodeID";
            this.textBoxNodeID.Size = new System.Drawing.Size(100, 20);
            this.textBoxNodeID.TabIndex = 13;
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Enabled = false;
            this.label53.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label53.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label53.Location = new System.Drawing.Point(17, 228);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(14, 13);
            this.label53.TabIndex = 24;
            this.label53.Text = "Z";
            // 
            // textBoxANodeID
            // 
            this.textBoxANodeID.Enabled = false;
            this.textBoxANodeID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxANodeID.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxANodeID.Location = new System.Drawing.Point(158, 63);
            this.textBoxANodeID.Name = "textBoxANodeID";
            this.textBoxANodeID.Size = new System.Drawing.Size(100, 20);
            this.textBoxANodeID.TabIndex = 14;
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Enabled = false;
            this.label52.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label52.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label52.Location = new System.Drawing.Point(17, 189);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(14, 13);
            this.label52.TabIndex = 23;
            this.label52.Text = "Y";
            // 
            // textBoxRSSI
            // 
            this.textBoxRSSI.Enabled = false;
            this.textBoxRSSI.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRSSI.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxRSSI.Location = new System.Drawing.Point(158, 102);
            this.textBoxRSSI.Name = "textBoxRSSI";
            this.textBoxRSSI.Size = new System.Drawing.Size(100, 20);
            this.textBoxRSSI.TabIndex = 15;
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Enabled = false;
            this.label51.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label51.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label51.Location = new System.Drawing.Point(17, 152);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(14, 13);
            this.label51.TabIndex = 22;
            this.label51.Text = "X";
            // 
            // textBoxX
            // 
            this.textBoxX.Enabled = false;
            this.textBoxX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxX.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxX.Location = new System.Drawing.Point(158, 149);
            this.textBoxX.Name = "textBoxX";
            this.textBoxX.Size = new System.Drawing.Size(100, 20);
            this.textBoxX.TabIndex = 16;
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Enabled = false;
            this.label50.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label50.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label50.Location = new System.Drawing.Point(17, 105);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(32, 13);
            this.label50.TabIndex = 21;
            this.label50.Text = "RSSI";
            this.toolTipActive.SetToolTip(this.label50, "RSS ");
            // 
            // textBoxY
            // 
            this.textBoxY.Enabled = false;
            this.textBoxY.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxY.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxY.Location = new System.Drawing.Point(158, 186);
            this.textBoxY.Name = "textBoxY";
            this.textBoxY.Size = new System.Drawing.Size(100, 20);
            this.textBoxY.TabIndex = 17;
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Enabled = false;
            this.label49.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label49.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label49.Location = new System.Drawing.Point(17, 66);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(55, 13);
            this.label49.TabIndex = 20;
            this.label49.Text = "Anchor ID";
            this.toolTipActive.SetToolTip(this.label49, "The WSNid of the Anchor Node");
            // 
            // textBoxZ
            // 
            this.textBoxZ.Enabled = false;
            this.textBoxZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxZ.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxZ.Location = new System.Drawing.Point(158, 225);
            this.textBoxZ.Name = "textBoxZ";
            this.textBoxZ.Size = new System.Drawing.Size(100, 20);
            this.textBoxZ.TabIndex = 18;
            // 
            // listBoxLoc
            // 
            this.listBoxLoc.FormattingEnabled = true;
            this.listBoxLoc.Location = new System.Drawing.Point(26, 47);
            this.listBoxLoc.Name = "listBoxLoc";
            this.listBoxLoc.Size = new System.Drawing.Size(89, 238);
            this.listBoxLoc.TabIndex = 25;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxSensUpdate);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.groupBox1.Location = new System.Drawing.Point(472, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(236, 304);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sensor";
            // 
            // textBoxSensUpdate
            // 
            this.textBoxSensUpdate.Cursor = System.Windows.Forms.Cursors.No;
            this.textBoxSensUpdate.Enabled = false;
            this.textBoxSensUpdate.Location = new System.Drawing.Point(130, 269);
            this.textBoxSensUpdate.Name = "textBoxSensUpdate";
            this.textBoxSensUpdate.Size = new System.Drawing.Size(83, 25);
            this.textBoxSensUpdate.TabIndex = 29;
            this.textBoxSensUpdate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label14.Location = new System.Drawing.Point(16, 275);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(66, 13);
            this.label14.TabIndex = 28;
            this.label14.Text = "Last update:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox3);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.textBox6);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.textBox5);
            this.groupBox3.Controls.Add(this.textBox4);
            this.groupBox3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox3.Location = new System.Drawing.Point(19, 111);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 147);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Measurements";
            // 
            // textBox3
            // 
            this.textBox3.Cursor = System.Windows.Forms.Cursors.No;
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(111, 21);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(83, 22);
            this.textBox3.TabIndex = 7;
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(19, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 14);
            this.label4.TabIndex = 2;
            this.label4.Text = "Temperature";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(19, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 14);
            this.label5.TabIndex = 3;
            this.label5.Text = "Light";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(19, 89);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 14);
            this.label6.TabIndex = 4;
            this.label6.Text = "Humidity";
            // 
            // textBox6
            // 
            this.textBox6.Cursor = System.Windows.Forms.Cursors.No;
            this.textBox6.Enabled = false;
            this.textBox6.Location = new System.Drawing.Point(111, 112);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(83, 22);
            this.textBox6.TabIndex = 7;
            this.textBox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(19, 120);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 14);
            this.label7.TabIndex = 5;
            this.label7.Text = "Power";
            // 
            // textBox5
            // 
            this.textBox5.Cursor = System.Windows.Forms.Cursors.No;
            this.textBox5.Enabled = false;
            this.textBox5.Location = new System.Drawing.Point(111, 81);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(83, 22);
            this.textBox5.TabIndex = 7;
            this.textBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox4
            // 
            this.textBox4.Cursor = System.Windows.Forms.Cursors.No;
            this.textBox4.Enabled = false;
            this.textBox4.Location = new System.Drawing.Point(111, 50);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(83, 22);
            this.textBox4.TabIndex = 7;
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.SystemColors.InfoText;
            this.groupBox2.Location = new System.Drawing.Point(19, 29);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 76);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "General info";
            // 
            // textBox2
            // 
            this.textBox2.Cursor = System.Windows.Forms.Cursors.No;
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(111, 50);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(83, 22);
            this.textBox2.TabIndex = 7;
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(19, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(19, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "Type Reader";
            // 
            // textBox1
            // 
            this.textBox1.Cursor = System.Windows.Forms.Cursors.No;
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(111, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(83, 22);
            this.textBox1.TabIndex = 6;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label3.Location = new System.Drawing.Point(299, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(162, 24);
            this.label3.TabIndex = 1;
            this.label3.Text = "TelosB Monitor";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabWSNAdmin);
            this.tabControl1.Controls.Add(this.tabOptions);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(0, -1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.ShowToolTips = true;
            this.tabControl1.Size = new System.Drawing.Size(769, 470);
            this.tabControl1.TabIndex = 10;
            this.tabControl1.Tag = "";
            // 
            // timerGraph
            // 
            this.timerGraph.Interval = 10000;
            this.timerGraph.Tick += new System.EventHandler(this.timerGraph_Tick);
            // 
            // toolTipActive
            // 
            this.toolTipActive.Tag = "";
            // 
            // timerStatus
            // 
            this.timerStatus.Interval = 10000;
            this.timerStatus.Tick += new System.EventHandler(this.timerStatus_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 476);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "WSN Monitor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize_1);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabOptions.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabWSNAdmin.ResumeLayout(false);
            this.tabWSNAdmin.PerformLayout();
            this.panelControl.ResumeLayout(false);
            this.panelControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPower)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox15.ResumeLayout(false);
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        
        
        #endregion

        private System.Windows.Forms.Timer timerSensor;
        private System.Windows.Forms.Timer timerLoc;
        private System.Windows.Forms.Timer timerSensorFetch;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TabPage tabOptions;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox controllerPort;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.TextBox controllerIP;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textLocRefresh;
        private System.Windows.Forms.TextBox textSensRefresh;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.TabPage tabWSNAdmin;
        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBoxControlX;
        private System.Windows.Forms.TextBox textBoxControlY;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBoxSampleRate;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.TextBox textBoxLocRate;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.TextBox textBoxConn;
        private System.Windows.Forms.CheckBox checkBoxLedBlue;
        private System.Windows.Forms.CheckBox checkBoxLedGreen;
        private System.Windows.Forms.CheckBox checkBoxLedRed;
        private System.Windows.Forms.Button buttonControlDiscard;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Button buttonWSNControl;
        private System.Windows.Forms.ListBox listBoxControl;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private ZedGraph.ZedGraphControl zg1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListBox listBoxLoc;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.TextBox textBoxZ;
        private System.Windows.Forms.TextBox textBoxY;
        private System.Windows.Forms.TextBox textBoxX;
        private System.Windows.Forms.TextBox textBoxRSSI;
        private System.Windows.Forms.TextBox textBoxANodeID;
        private System.Windows.Forms.TextBox textBoxNodeID;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxLocUpdate;
        private System.Windows.Forms.TextBox textBoxSensUpdate;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox checkBoxActive;
        private System.Windows.Forms.CheckBox checkBoxAnchorNode;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.NumericUpDown numericUpDownFrequency;
        private System.Windows.Forms.NumericUpDown numericUpDownPower;
        private System.Windows.Forms.RadioButton radioButtonSensorTimeOut;
        private System.Windows.Forms.RadioButton radioButtonDiscovery;
        private System.Windows.Forms.RadioButton radioButtonGetSensors;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox checkBoxPlotUpdate;
        private System.Windows.Forms.Timer timerGraph;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxGraphUpdateInterval;
        private System.Windows.Forms.ToolTip toolTipActive;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBoxSensorRateMax;
        private System.Windows.Forms.TextBox textBoxLocRateMax;
        private System.Windows.Forms.TextBox textBoxYmax;
        private System.Windows.Forms.TextBox textBoxXmax;
        private System.Windows.Forms.TextBox textBoxSensorRateMin;
        private System.Windows.Forms.TextBox textBoxLocRateMin;
        private System.Windows.Forms.TextBox textBoxYmin;
        private System.Windows.Forms.TextBox textBoxXmin;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Timer timerStatus;
        private System.Windows.Forms.MaskedTextBox maskedTextBox1;
        private System.Windows.Forms.TextBox textBoxSensorFetchUpdateInterval;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label26;

    }
}

