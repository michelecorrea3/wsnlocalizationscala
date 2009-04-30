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
            this.label1 = new System.Windows.Forms.Label();
            this.radioButtonCentroidLocalization = new System.Windows.Forms.RadioButton();
            this.radioButtonMinMaxSimpleModel = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Choose your positioning algorithm:";
            // 
            // radioButtonCentroidLocalization
            // 
            this.radioButtonCentroidLocalization.AutoSize = true;
            this.radioButtonCentroidLocalization.Location = new System.Drawing.Point(38, 42);
            this.radioButtonCentroidLocalization.Name = "radioButtonCentroidLocalization";
            this.radioButtonCentroidLocalization.Size = new System.Drawing.Size(123, 17);
            this.radioButtonCentroidLocalization.TabIndex = 1;
            this.radioButtonCentroidLocalization.TabStop = true;
            this.radioButtonCentroidLocalization.Text = "Centroid Localization";
            this.radioButtonCentroidLocalization.UseVisualStyleBackColor = true;
            // 
            // radioButtonMinMaxSimpleModel
            // 
            this.radioButtonMinMaxSimpleModel.AutoSize = true;
            this.radioButtonMinMaxSimpleModel.Location = new System.Drawing.Point(38, 65);
            this.radioButtonMinMaxSimpleModel.Name = "radioButtonMinMaxSimpleModel";
            this.radioButtonMinMaxSimpleModel.Size = new System.Drawing.Size(122, 17);
            this.radioButtonMinMaxSimpleModel.TabIndex = 2;
            this.radioButtonMinMaxSimpleModel.TabStop = true;
            this.radioButtonMinMaxSimpleModel.Text = "MinMaxSimpleModel";
            this.radioButtonMinMaxSimpleModel.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 171);
            this.Controls.Add(this.radioButtonMinMaxSimpleModel);
            this.Controls.Add(this.radioButtonCentroidLocalization);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButtonCentroidLocalization;
        private System.Windows.Forms.RadioButton radioButtonMinMaxSimpleModel;
    }
}

