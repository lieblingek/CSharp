namespace WindowsFormsApplication1
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
            this.connectButton = new System.Windows.Forms.Button();
            this.terminalBox = new System.Windows.Forms.TextBox();
            this.sendData1 = new System.Windows.Forms.Button();
            this.sendTextBox1 = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ClearBtn = new System.Windows.Forms.Button();
            this.Send_time = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // connectButton
            // 
            this.connectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.connectButton.Location = new System.Drawing.Point(97, 420);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 0;
            this.connectButton.Text = "Start";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.init_Click);
            // 
            // terminalBox
            // 
            this.terminalBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.terminalBox.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.terminalBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.terminalBox.Location = new System.Drawing.Point(20, 61);
            this.terminalBox.MinimumSize = new System.Drawing.Size(170, 172);
            this.terminalBox.Multiline = true;
            this.terminalBox.Name = "terminalBox";
            this.terminalBox.ReadOnly = true;
            this.terminalBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.terminalBox.Size = new System.Drawing.Size(452, 306);
            this.terminalBox.TabIndex = 2;
            // 
            // sendData1
            // 
            this.sendData1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sendData1.Enabled = false;
            this.sendData1.Location = new System.Drawing.Point(18, 419);
            this.sendData1.Name = "sendData1";
            this.sendData1.Size = new System.Drawing.Size(75, 23);
            this.sendData1.TabIndex = 5;
            this.sendData1.Text = "Data send";
            this.sendData1.UseVisualStyleBackColor = true;
            this.sendData1.Click += new System.EventHandler(this.button1_Click);
            // 
            // sendTextBox1
            // 
            this.sendTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sendTextBox1.Location = new System.Drawing.Point(18, 373);
            this.sendTextBox1.MaxLength = 64;
            this.sendTextBox1.MinimumSize = new System.Drawing.Size(250, 40);
            this.sendTextBox1.Multiline = true;
            this.sendTextBox1.Name = "sendTextBox1";
            this.sendTextBox1.Size = new System.Drawing.Size(299, 40);
            this.sendTextBox1.TabIndex = 8;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 507);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.ShowItemToolTips = true;
            this.statusStrip1.Size = new System.Drawing.Size(508, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // StatusLabel2
            // 
            this.StatusLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.StatusLabel2.AutoSize = true;
            this.StatusLabel2.Location = new System.Drawing.Point(-3, 482);
            this.StatusLabel2.Name = "StatusLabel2";
            this.StatusLabel2.Size = new System.Drawing.Size(35, 13);
            this.StatusLabel2.TabIndex = 12;
            this.StatusLabel2.Text = "label1";
            this.StatusLabel2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ClearBtn
            // 
            this.ClearBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ClearBtn.Location = new System.Drawing.Point(18, 448);
            this.ClearBtn.Name = "ClearBtn";
            this.ClearBtn.Size = new System.Drawing.Size(75, 23);
            this.ClearBtn.TabIndex = 13;
            this.ClearBtn.Text = "Clear";
            this.ClearBtn.UseVisualStyleBackColor = true;
            this.ClearBtn.Click += new System.EventHandler(this.ClearBtn_Click_1);
            // 
            // Send_time
            // 
            this.Send_time.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Send_time.Location = new System.Drawing.Point(97, 448);
            this.Send_time.Name = "Send_time";
            this.Send_time.Size = new System.Drawing.Size(75, 23);
            this.Send_time.TabIndex = 14;
            this.Send_time.Text = "Send time";
            this.Send_time.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Send_time.UseVisualStyleBackColor = true;
            this.Send_time.Click += new System.EventHandler(this.Send_time_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 529);
            this.Controls.Add(this.Send_time);
            this.Controls.Add(this.ClearBtn);
            this.Controls.Add(this.StatusLabel2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.sendTextBox1);
            this.Controls.Add(this.sendData1);
            this.Controls.Add(this.terminalBox);
            this.Controls.Add(this.connectButton);
            this.MinimumSize = new System.Drawing.Size(516, 556);
            this.Name = "Form1";
            this.Text = "STM32 usblib bulk terminal";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.TextBox terminalBox;
        private System.Windows.Forms.Button sendData1;
        private System.Windows.Forms.TextBox sendTextBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label StatusLabel2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button ClearBtn;
        private System.Windows.Forms.Button Send_time;
    }
}

