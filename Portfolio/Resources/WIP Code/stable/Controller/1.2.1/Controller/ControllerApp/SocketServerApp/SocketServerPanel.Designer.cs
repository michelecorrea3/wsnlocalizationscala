namespace SocketServerApp
{
    partial class SocketServerPanel
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
            this.StartSocketServerButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonMySQL = new System.Windows.Forms.RadioButton();
            this.radioButtonDB2 = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartSocketServerButton
            // 
            this.StartSocketServerButton.Location = new System.Drawing.Point(13, 13);
            this.StartSocketServerButton.Name = "StartSocketServerButton";
            this.StartSocketServerButton.Size = new System.Drawing.Size(259, 23);
            this.StartSocketServerButton.TabIndex = 0;
            this.StartSocketServerButton.Text = "Start controller";
            this.StartSocketServerButton.UseVisualStyleBackColor = true;
            this.StartSocketServerButton.Click += new System.EventHandler(this.StartSocketServerButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonMySQL);
            this.groupBox1.Controls.Add(this.radioButtonDB2);
            this.groupBox1.Location = new System.Drawing.Point(13, 66);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 51);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Get pure information-requests from which database?";
            // 
            // radioButtonMySQL
            // 
            this.radioButtonMySQL.AutoSize = true;
            this.radioButtonMySQL.Checked = true;
            this.radioButtonMySQL.Location = new System.Drawing.Point(193, 29);
            this.radioButtonMySQL.Name = "radioButtonMySQL";
            this.radioButtonMySQL.Size = new System.Drawing.Size(60, 17);
            this.radioButtonMySQL.TabIndex = 1;
            this.radioButtonMySQL.TabStop = true;
            this.radioButtonMySQL.Text = "MySQL";
            this.radioButtonMySQL.UseVisualStyleBackColor = true;
            // 
            // radioButtonDB2
            // 
            this.radioButtonDB2.AutoSize = true;
            this.radioButtonDB2.Location = new System.Drawing.Point(6, 29);
            this.radioButtonDB2.Name = "radioButtonDB2";
            this.radioButtonDB2.Size = new System.Drawing.Size(46, 17);
            this.radioButtonDB2.TabIndex = 0;
            this.radioButtonDB2.Text = "DB2";
            this.radioButtonDB2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 37);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(259, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Close controller";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SocketServerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 129);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.StartSocketServerButton);
            this.Name = "SocketServerPanel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SocketServer control panel";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button StartSocketServerButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonMySQL;
        private System.Windows.Forms.RadioButton radioButtonDB2;
        private System.Windows.Forms.Button button1;
    }
}

