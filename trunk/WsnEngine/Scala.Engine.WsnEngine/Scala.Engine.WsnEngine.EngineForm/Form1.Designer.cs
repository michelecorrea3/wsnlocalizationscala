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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButtonCentroidLocalization
            // 
            this.radioButtonCentroidLocalization.AutoSize = true;
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
            this.radioButtonMinMaxSimpleModel.Size = new System.Drawing.Size(122, 17);
            this.radioButtonMinMaxSimpleModel.TabIndex = 2;
            this.radioButtonMinMaxSimpleModel.TabStop = true;
            this.radioButtonMinMaxSimpleModel.Text = "MinMaxSimpleModel";
            this.radioButtonMinMaxSimpleModel.UseVisualStyleBackColor = true;
            this.radioButtonMinMaxSimpleModel.CheckedChanged += new System.EventHandler(this.radioButtonMinMaxSimpleModel_CheckedChanged);
            // 
            // radioButtonMedianFilter
            // 
            this.radioButtonMedianFilter.AutoSize = true;
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
            this.radioButtonAverageFilter.TabStop = true;
            this.radioButtonAverageFilter.Text = "Average";
            this.radioButtonAverageFilter.UseVisualStyleBackColor = true;
            this.radioButtonAverageFilter.CheckedChanged += new System.EventHandler(this.radioButtonAverageFilter_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonCentroidLocalization);
            this.groupBox1.Controls.Add(this.radioButtonMinMaxSimpleModel);
            this.groupBox1.Location = new System.Drawing.Point(22, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose your positioning algorithm";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButtonMedianFilter);
            this.groupBox2.Controls.Add(this.radioButtonAverageFilter);
            this.groupBox2.Location = new System.Drawing.Point(22, 146);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 87);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Choose your RSS filter";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 331);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonCentroidLocalization;
        private System.Windows.Forms.RadioButton radioButtonMinMaxSimpleModel;
        private System.Windows.Forms.RadioButton radioButtonMedianFilter;
        private System.Windows.Forms.RadioButton radioButtonAverageFilter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}

