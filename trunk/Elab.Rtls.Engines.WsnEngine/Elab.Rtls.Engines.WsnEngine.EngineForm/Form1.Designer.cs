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
            this.radioButtonMedianFilter = new System.Windows.Forms.RadioButton();
            this.radioButtonAverageFilter = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonWeigthedCentroidLocalization = new System.Windows.Forms.RadioButton();
            this.radioButtonExtendedMinMax = new System.Windows.Forms.RadioButton();
            this.radioButtonExtendedClusteredTrilateration = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButtonNoFilter = new System.Windows.Forms.RadioButton();
            this.checkBoxUseMultihop = new System.Windows.Forms.CheckBox();
            this.radioButtonLeastSquaresTriLateration = new System.Windows.Forms.RadioButton();
            this.groupBoxCalibration = new System.Windows.Forms.GroupBox();
            this.radioButtonCalibrationDisabled = new System.Windows.Forms.RadioButton();
            this.radioButtonCalibrationNormal = new System.Windows.Forms.RadioButton();
            this.radioButtonCalibrationLeastSquares = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBoxCalibration.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButtonCentroidLocalization
            // 
            this.radioButtonCentroidLocalization.AutoSize = true;
            this.radioButtonCentroidLocalization.Checked = true;
            this.radioButtonCentroidLocalization.Location = new System.Drawing.Point(20, 20);
            this.radioButtonCentroidLocalization.Name = "radioButtonCentroidLocalization";
            this.radioButtonCentroidLocalization.Size = new System.Drawing.Size(123, 17);
            this.radioButtonCentroidLocalization.TabIndex = 1;
            this.radioButtonCentroidLocalization.TabStop = true;
            this.radioButtonCentroidLocalization.Text = "Centroid Localization";
            this.radioButtonCentroidLocalization.UseVisualStyleBackColor = true;
            this.radioButtonCentroidLocalization.CheckedChanged += new System.EventHandler(this.radioButtonCentroidLocalization_CheckedChanged);
            // 
            // radioButtonMedianFilter
            // 
            this.radioButtonMedianFilter.AutoSize = true;
            this.radioButtonMedianFilter.Location = new System.Drawing.Point(22, 19);
            this.radioButtonMedianFilter.Name = "radioButtonMedianFilter";
            this.radioButtonMedianFilter.Size = new System.Drawing.Size(60, 17);
            this.radioButtonMedianFilter.TabIndex = 4;
            this.radioButtonMedianFilter.Text = "Median";
            this.radioButtonMedianFilter.UseVisualStyleBackColor = true;
            this.radioButtonMedianFilter.CheckedChanged += new System.EventHandler(this.radioButtonMedianFilter_CheckedChanged);
            // 
            // radioButtonAverageFilter
            // 
            this.radioButtonAverageFilter.AutoSize = true;
            this.radioButtonAverageFilter.Checked = true;
            this.radioButtonAverageFilter.Location = new System.Drawing.Point(22, 42);
            this.radioButtonAverageFilter.Name = "radioButtonAverageFilter";
            this.radioButtonAverageFilter.Size = new System.Drawing.Size(65, 17);
            this.radioButtonAverageFilter.TabIndex = 5;
            this.radioButtonAverageFilter.TabStop = true;
            this.radioButtonAverageFilter.Text = "Average";
            this.radioButtonAverageFilter.UseVisualStyleBackColor = true;
            this.radioButtonAverageFilter.CheckedChanged += new System.EventHandler(this.radioButtonAverageFilter_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonLeastSquaresTriLateration);
            this.groupBox1.Controls.Add(this.radioButtonWeigthedCentroidLocalization);
            this.groupBox1.Controls.Add(this.radioButtonExtendedMinMax);
            this.groupBox1.Controls.Add(this.radioButtonExtendedClusteredTrilateration);
            this.groupBox1.Controls.Add(this.radioButtonCentroidLocalization);
            this.groupBox1.Location = new System.Drawing.Point(22, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 175);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Positioning algorithm";
            // 
            // radioButtonWeigthedCentroidLocalization
            // 
            this.radioButtonWeigthedCentroidLocalization.AutoSize = true;
            this.radioButtonWeigthedCentroidLocalization.Location = new System.Drawing.Point(20, 51);
            this.radioButtonWeigthedCentroidLocalization.Name = "radioButtonWeigthedCentroidLocalization";
            this.radioButtonWeigthedCentroidLocalization.Size = new System.Drawing.Size(172, 17);
            this.radioButtonWeigthedCentroidLocalization.TabIndex = 6;
            this.radioButtonWeigthedCentroidLocalization.TabStop = true;
            this.radioButtonWeigthedCentroidLocalization.Text = "Weigthed Centroid Localization";
            this.radioButtonWeigthedCentroidLocalization.UseVisualStyleBackColor = true;
            this.radioButtonWeigthedCentroidLocalization.CheckedChanged += new System.EventHandler(this.radioButtonWeigthedCentroidLocalization_CheckedChanged);
            // 
            // radioButtonExtendedMinMax
            // 
            this.radioButtonExtendedMinMax.AutoSize = true;
            this.radioButtonExtendedMinMax.Location = new System.Drawing.Point(20, 144);
            this.radioButtonExtendedMinMax.Name = "radioButtonExtendedMinMax";
            this.radioButtonExtendedMinMax.Size = new System.Drawing.Size(62, 17);
            this.radioButtonExtendedMinMax.TabIndex = 5;
            this.radioButtonExtendedMinMax.TabStop = true;
            this.radioButtonExtendedMinMax.Text = "MinMax";
            this.radioButtonExtendedMinMax.UseVisualStyleBackColor = true;
            this.radioButtonExtendedMinMax.CheckedChanged += new System.EventHandler(this.radioButtonExtendedMinMax_CheckedChanged);
            // 
            // radioButtonExtendedClusteredTrilateration
            // 
            this.radioButtonExtendedClusteredTrilateration.AutoSize = true;
            this.radioButtonExtendedClusteredTrilateration.Location = new System.Drawing.Point(20, 82);
            this.radioButtonExtendedClusteredTrilateration.Name = "radioButtonExtendedClusteredTrilateration";
            this.radioButtonExtendedClusteredTrilateration.Size = new System.Drawing.Size(80, 17);
            this.radioButtonExtendedClusteredTrilateration.TabIndex = 4;
            this.radioButtonExtendedClusteredTrilateration.TabStop = true;
            this.radioButtonExtendedClusteredTrilateration.Text = "Trilateration";
            this.radioButtonExtendedClusteredTrilateration.UseVisualStyleBackColor = true;
            this.radioButtonExtendedClusteredTrilateration.CheckedChanged += new System.EventHandler(this.radioButtonExtendedClusteredTrilateration_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButtonNoFilter);
            this.groupBox2.Controls.Add(this.radioButtonMedianFilter);
            this.groupBox2.Controls.Add(this.radioButtonAverageFilter);
            this.groupBox2.Location = new System.Drawing.Point(22, 213);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 93);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RSS filter";
            // 
            // radioButtonNoFilter
            // 
            this.radioButtonNoFilter.AutoSize = true;
            this.radioButtonNoFilter.Location = new System.Drawing.Point(22, 66);
            this.radioButtonNoFilter.Name = "radioButtonNoFilter";
            this.radioButtonNoFilter.Size = new System.Drawing.Size(64, 17);
            this.radioButtonNoFilter.TabIndex = 6;
            this.radioButtonNoFilter.Text = "No Filter";
            this.radioButtonNoFilter.UseVisualStyleBackColor = true;
            this.radioButtonNoFilter.CheckedChanged += new System.EventHandler(this.radioButtonNoFilter_CheckedChanged);
            // 
            // checkBoxUseMultihop
            // 
            this.checkBoxUseMultihop.AutoSize = true;
            this.checkBoxUseMultihop.Enabled = false;
            this.checkBoxUseMultihop.Location = new System.Drawing.Point(42, 462);
            this.checkBoxUseMultihop.Name = "checkBoxUseMultihop";
            this.checkBoxUseMultihop.Size = new System.Drawing.Size(142, 17);
            this.checkBoxUseMultihop.TabIndex = 9;
            this.checkBoxUseMultihop.Text = "Use Multihop Positioning";
            this.checkBoxUseMultihop.UseVisualStyleBackColor = true;
            this.checkBoxUseMultihop.Visible = false;
            this.checkBoxUseMultihop.CheckedChanged += new System.EventHandler(this.checkBoxUseMultihop_CheckedChanged);
            // 
            // radioButtonLeastSquaresTriLateration
            // 
            this.radioButtonLeastSquaresTriLateration.AutoSize = true;
            this.radioButtonLeastSquaresTriLateration.Location = new System.Drawing.Point(20, 113);
            this.radioButtonLeastSquaresTriLateration.Name = "radioButtonLeastSquaresTriLateration";
            this.radioButtonLeastSquaresTriLateration.Size = new System.Drawing.Size(151, 17);
            this.radioButtonLeastSquaresTriLateration.TabIndex = 7;
            this.radioButtonLeastSquaresTriLateration.TabStop = true;
            this.radioButtonLeastSquaresTriLateration.Text = "Least Squares Trilateration";
            this.radioButtonLeastSquaresTriLateration.UseVisualStyleBackColor = true;
            // 
            // groupBoxCalibration
            // 
            this.groupBoxCalibration.Controls.Add(this.radioButtonCalibrationLeastSquares);
            this.groupBoxCalibration.Controls.Add(this.radioButtonCalibrationNormal);
            this.groupBoxCalibration.Controls.Add(this.radioButtonCalibrationDisabled);
            this.groupBoxCalibration.Location = new System.Drawing.Point(22, 322);
            this.groupBoxCalibration.Name = "groupBoxCalibration";
            this.groupBoxCalibration.Size = new System.Drawing.Size(200, 100);
            this.groupBoxCalibration.TabIndex = 10;
            this.groupBoxCalibration.TabStop = false;
            this.groupBoxCalibration.Text = "Calibration";
            // 
            // radioButtonCalibrationDisabled
            // 
            this.radioButtonCalibrationDisabled.AutoSize = true;
            this.radioButtonCalibrationDisabled.Checked = true;
            this.radioButtonCalibrationDisabled.Location = new System.Drawing.Point(22, 19);
            this.radioButtonCalibrationDisabled.Name = "radioButtonCalibrationDisabled";
            this.radioButtonCalibrationDisabled.Size = new System.Drawing.Size(66, 17);
            this.radioButtonCalibrationDisabled.TabIndex = 0;
            this.radioButtonCalibrationDisabled.TabStop = true;
            this.radioButtonCalibrationDisabled.Text = "Disabled";
            this.radioButtonCalibrationDisabled.UseVisualStyleBackColor = true;
            this.radioButtonCalibrationDisabled.CheckedChanged += new System.EventHandler(this.radioButtonCalibrationDisabled_CheckedChanged);
            // 
            // radioButtonCalibrationNormal
            // 
            this.radioButtonCalibrationNormal.AutoSize = true;
            this.radioButtonCalibrationNormal.Location = new System.Drawing.Point(22, 42);
            this.radioButtonCalibrationNormal.Name = "radioButtonCalibrationNormal";
            this.radioButtonCalibrationNormal.Size = new System.Drawing.Size(61, 17);
            this.radioButtonCalibrationNormal.TabIndex = 1;
            this.radioButtonCalibrationNormal.TabStop = true;
            this.radioButtonCalibrationNormal.Text = "Normal ";
            this.radioButtonCalibrationNormal.UseVisualStyleBackColor = true;
            this.radioButtonCalibrationNormal.CheckedChanged += new System.EventHandler(this.radioButtonCalibrationNormal_CheckedChanged);
            // 
            // radioButtonCalibrationLeastSquares
            // 
            this.radioButtonCalibrationLeastSquares.AutoSize = true;
            this.radioButtonCalibrationLeastSquares.Location = new System.Drawing.Point(22, 65);
            this.radioButtonCalibrationLeastSquares.Name = "radioButtonCalibrationLeastSquares";
            this.radioButtonCalibrationLeastSquares.Size = new System.Drawing.Size(93, 17);
            this.radioButtonCalibrationLeastSquares.TabIndex = 2;
            this.radioButtonCalibrationLeastSquares.TabStop = true;
            this.radioButtonCalibrationLeastSquares.Text = "Least Squares";
            this.radioButtonCalibrationLeastSquares.UseVisualStyleBackColor = true;
            this.radioButtonCalibrationLeastSquares.CheckedChanged += new System.EventHandler(this.radioButtonCalibrationLeastSquares_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 446);
            this.Controls.Add(this.groupBoxCalibration);
            this.Controls.Add(this.checkBoxUseMultihop);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBoxCalibration.ResumeLayout(false);
            this.groupBoxCalibration.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonCentroidLocalization;
        private System.Windows.Forms.RadioButton radioButtonMedianFilter;
        private System.Windows.Forms.RadioButton radioButtonAverageFilter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxUseMultihop;
        private System.Windows.Forms.RadioButton radioButtonExtendedMinMax;
        private System.Windows.Forms.RadioButton radioButtonExtendedClusteredTrilateration;
        private System.Windows.Forms.RadioButton radioButtonNoFilter;
        private System.Windows.Forms.RadioButton radioButtonWeigthedCentroidLocalization;
        private System.Windows.Forms.RadioButton radioButtonLeastSquaresTriLateration;
        private System.Windows.Forms.GroupBox groupBoxCalibration;
        private System.Windows.Forms.RadioButton radioButtonCalibrationLeastSquares;
        private System.Windows.Forms.RadioButton radioButtonCalibrationNormal;
        private System.Windows.Forms.RadioButton radioButtonCalibrationDisabled;
    }
}

