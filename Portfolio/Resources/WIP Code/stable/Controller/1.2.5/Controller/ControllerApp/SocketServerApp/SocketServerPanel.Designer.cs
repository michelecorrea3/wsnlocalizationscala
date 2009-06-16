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
            this.button1 = new System.Windows.Forms.Button();
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
            this.Controls.Add(this.StartSocketServerButton);
            this.Name = "SocketServerPanel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SocketServer control panel";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button StartSocketServerButton;
        private System.Windows.Forms.Button button1;
    }
}

