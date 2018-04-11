namespace UDP_Chat__Client_
{
    partial class MainForm
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
            this.tbUsers = new System.Windows.Forms.ListBox();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.tbChat = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbUsers
            // 
            this.tbUsers.FormattingEnabled = true;
            this.tbUsers.ItemHeight = 16;
            this.tbUsers.Location = new System.Drawing.Point(506, 15);
            this.tbUsers.Margin = new System.Windows.Forms.Padding(4);
            this.tbUsers.Name = "tbUsers";
            this.tbUsers.Size = new System.Drawing.Size(136, 340);
            this.tbUsers.TabIndex = 8;
            // 
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(13, 330);
            this.tbMessage.Margin = new System.Windows.Forms.Padding(4);
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(375, 22);
            this.tbMessage.TabIndex = 7;
            this.tbMessage.TextChanged += new System.EventHandler(this.tbMessage_TextChanged);
            this.tbMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbMessage_KeyDown);
            // 
            // tbChat
            // 
            this.tbChat.BackColor = System.Drawing.SystemColors.Window;
            this.tbChat.Location = new System.Drawing.Point(13, 13);
            this.tbChat.Margin = new System.Windows.Forms.Padding(4);
            this.tbChat.Multiline = true;
            this.tbChat.Name = "tbChat";
            this.tbChat.ReadOnly = true;
            this.tbChat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbChat.Size = new System.Drawing.Size(483, 309);
            this.tbChat.TabIndex = 6;
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnSend.Enabled = false;
            this.btnSend.Location = new System.Drawing.Point(397, 330);
            this.btnSend.Margin = new System.Windows.Forms.Padding(4);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(100, 26);
            this.btnSend.TabIndex = 5;
            this.btnSend.Text = "&Send";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(653, 367);
            this.Controls.Add(this.tbUsers);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.tbChat);
            this.Controls.Add(this.btnSend);
            this.Name = "MainForm";
            this.Text = "UDP Chat ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox tbUsers;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.TextBox tbChat;
        private System.Windows.Forms.Button btnSend;
    }
}