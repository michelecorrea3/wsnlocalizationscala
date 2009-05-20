namespace Elab.Rtls.Engines.WsnEngine.EngineForm
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
            this.radioButtonCentroidLocalization = new System.Windows.Forms.RadioButton();
            this.radioButtonMinMaxSimpleModel = new System.Windows.Forms.RadioButton();
            this.radioButtonMedianFilter = new System.Windows.Forms.RadioButton();
            this.radioButtonAverageFilter = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonExtendedMinMax = new System.Windows.Forms.RadioButton();
            this.radioButtonExtendedClusteredTrilateration = new System.Windows.Forms.RadioButton();
            this.radioButtonClusteredTriLateration = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxCalibration = new System.Windows.Forms.CheckBox();
            this.checkBoxUseMultihop = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButtonCentroidLocalization
            // 
            this.radioButtonCentroidLocalization.AutoSize = true;
            this.radioButtonCentroidLocalization.Checked = true;
            this.radioButtonCentroidLocalization.Location = new System.Drawing.Point(22, 32);
            this.radioButtonCentroidLocalization.Name = "radioButtonCentroidLocalization";
            this.radioButtonCentroidLocalization.Size = new System.Drawing.Size(123, 17);
            this.radioButtonCentroidLocalization.TabIndex = 1;
            this.radioButtonCentroidLocalization.TabStop = true;
            this.radioButtonCentroidLocalization.Text = "Centroid Localization";
            this.radioButtonCentroidLocalization.UseVisualStyleBackColor = true;
            this.radioButtonCentroidLocalization.CheckedChanged += new System.EventHandler(this.radioButtonCentroidLocalization_CheckedChanged);
            // 
            // radioButtonMinMaxSimpleModel
            // 
            this.radioButtonMinMaxSimpleModel.AutoSize = true;
            this.radioButtonMinMaxSimpleModel.Location = new System.Drawing.Point(22, 55);
            this.radioButtonMinMaxSimpleModel.Name = "radioButtonMinMaxSimpleModel";
            this.radioButtonMinMaxSimpleModel.Size = new System.Drawing.Size(62, 17);
            this.radioButtonMinMaxSimpleModel.TabIndex = 2;
            this.radioButtonMinMaxSimpleModel.Text = "MinMax";
            this.radioButtonMinMaxSimpleModel.UseVisualStyleBackColor = true;
            this.radioButtonMinMaxSimpleModel.CheckedChanged += new System.EventHandler(this.radioButtonMinMaxSimpleModel_CheckedChanged);
            // 
            // radioButtonMedianFilter
            // 
            this.radioButtonMedianFilter.AutoSize = true;
            this.radioButtonMedianFilter.Checked = true;
            this.radioButtonMedianFilter.Location = new System.Drawing.Point(22, 30);
            this.radioButtonMedianFilter.Name = "radioButtonMedianFilter";
            this.radioButtonMedianFilter.Size = new System.Drawing.Size(60, 17);
            this.radioButtonMedianFilter.TabIndex = 4;
            this.radioButtonMedianFilter.TabStop = true;
            this.radioButtonMedianFilter.Text = "Median";
            this.radioButtonMedianFilter.UseVisualStyleBackColor = true;
            this.radioButtonMedianFilter.CheckedChanged += new System.EventHandler(this.radioButtonMedianFilter_CheckedChanged);
            // 
            // radioButtonAverageFilter
            // 
            this.radioButtonAverageFilter.AutoSize = true;
            this.radioButtonAverageFilter.Location = new System.Drawing.Point(22, 53);
            this.radioButtonAverageFilter.Name = "radioButtonAverageFilter";
            this.radioButtonAverageFilter.Size = new System.Drawing.Size(65, 17);
            this.radioButtonAverageFilter.TabIndex = 5;
            this.radioButtonAverageFilter.Text = "Average";
            this.radioButtonAverageFilter.UseVisualStyleBackColor = true;
            this.radioButtonAverageFilter.CheckedChanged += new System.EventHandler(this.radioButtonAverageFilter_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonExtendedMinMax);
            this.groupBox1.Controls.Add(this.radioButtonExtendedClusteredTrilateration);
            this.groupBox1.Controls.Add(this.radioButtonClusteredTriLateration);
            this.groupBox1.Controls.Add(this.radioButtonCentroidLocalization);
            this.groupBox1.Controls.Add(this.radioButtonMinMaxSimpleModel);
            this.groupBox1.Location = new System.Drawing.Point(22, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 189);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose your positioning algorithm";
            // 
            // radioButtonExtendedMinMax
            // 
            this.radioButtonExtendedMinMax.AutoSize = true;
            this.radioButtonExtendedMinMax.Location = new System.Drawing.Point(22, 127);
            this.radioButtonExtendedMinMax.Name = "radioButtonExtendedMinMax";
            this.radioButtonExtendedMinMax.Size = new System.Drawing.Size(108, 17);
            this.radioButtonExtendedMinMax.TabIndex = 5;
            this.radioButtonExtendedMinMax.TabStop = true;
            this.radioButtonExtendedMinMax.Text = "eXtendedMinMax";
            this.radioButtonExtendedMinMax.UseVisualStyleBackColor = true;
            this.radioButtonExtendedMinMax.CheckedChanged += new System.EventHandler(this.radioButtonExtendedMinMax_CheckedChanged);
            // 
            // radioButtonExtendedClusteredTrilateration
            // 
            this.radioButtonExtendedClusteredTrilateration.AutoSize = true;
            this.radioButtonExtendedClusteredTrilateration.Location = new System.Drawing.Point(22, 103);
            this.radioButtonExtendedClusteredTrilateration.Name = "radioButtonExtendedClusteredTrilateration";
            this.radioButtonExtendedClusteredTrilateration.Size = new System.Drawing.Size(126, 17);
            this.radioButtonExtendedClusteredTrilateration.TabIndex = 4;
            this.radioButtonExtendedClusteredTrilateration.TabStop = true;
            this.radioButtonExtendedClusteredTrilateration.Text = "eXtendedTrilateration";
            this.radioButtonExtendedClusteredTrilateration.UseVisualStyleBackColor = true;
            this.radioButtonExtendedClusteredTrilateration.CheckedChanged += new System.EventHandler(this.radioButtonExtendedClusteredTrilateration_CheckedChanged);
            // 
            // radioButtonClusteredTriLateration
            // 
            this.radioButtonClusteredTriLateration.AutoSize = true;
            this.radioButtonClusteredTriLateration.Location = new System.Drawing.Point(22, 79);
            this.radioButtonClusteredTriLateration.Name = "radioButtonClusteredTriLateration";
            this.radioButtonClusteredTriLateration.Size = new System.Drawing.Size(124, 17);
            this.radioButtonClusteredTriLateration.TabIndex = 3;
            this.radioButtonClusteredTriLateration.Text = "ClusteredTrilateration";
            this.radioButtonClusteredTriLateration.UseVisualStyleBackColor = true;
            this.radioButtonClusteredTriLateration.CheckedChanged += new System.EventHandler(this.radioButtonClusteredTriLateration_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButtonMedianFilter);
            this.groupBox2.Controls.Add(this.radioButtonAverageFilter);
            this.groupBox2.Location = new System.Drawing.Point(22, 217);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 87);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Choose your RSS filter";
            // 
            // checkBoxCalibration
            // 
            this.checkBoxCalibration.AutoSize = true;
            this.checkBoxCalibration.Enabled = false;
            this.checkBoxCalibration.Location = new System.Drawing.Point(44, 319);
            this.checkBoxCalibration.Name = "checkBoxCalibration";
            this.checkBoxCalibration.Size = new System.Drawing.Size(134, 17);
            this.checkBoxCalibration.TabIndex = 8;
            this.checkBoxCalibration.Text = "Use Anchor Calibration";
            this.checkBoxCalibration.UseVisualStyleBackColor = true;
            this.checkBoxCalibration.CheckedChanged += new System.EventHandler(this.checkBoxCalibration_CheckedChanged);
            // 
            // checkBoxUseMultihop
            // 
            this.checkBoxUseMultihop.AutoSize = true;
            this.checkBoxUseMultihop.Enabled = false;
            this.checkBoxUseMultihop.Location = new System.Drawing.Point(44, 343);
            this.checkBoxUseMultihop.Name = "checkBoxUseMultihop";
            this.checkBoxUseMultihop.Size = new System.Drawing.Size(142, 17);
            this.checkBoxUseMultihop.TabIndex = 9;
            this.checkBoxUseMultihop.Text = "Use Multihop Positioning";
            this.checkBoxUseMultihop.UseVisualStyleBackColor = true;
            this.checkBoxUseMultihop.CheckedChanged += new System.EventHandler(this.checkBoxUseMultihop_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 426);
            this.Controls.Add(this.checkBoxUseMultihop);
            this.Controls.Add(this.checkBoxCalibration);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonCentroidLocalization;
        private System.Windows.Forms.RadioButton radioButtonMinMaxSimpleModel;
        private System.Windows.Forms.RadioButton radioButtonMedianFilter;
        private System.Windows.Forms.RadioButton radioButtonAverageFilter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButtonClusteredTriLateration;
        private System.Windows.Forms.CheckBox checkBoxCalibration;
        private System.Windows.Forms.CheckBox checkBoxUseMultihop;
        private System.Windows.Forms.RadioButton radioButtonExtendedMinMax;
        private System.Windows.Forms.RadioButton radioButtonExtendedClusteredTrilateration;
    }
}

