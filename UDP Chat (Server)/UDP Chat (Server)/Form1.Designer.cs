namespace UDP_Chat__Server_
{
    partial class Form
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
            this.tbChat = new System.Windows.Forms.TextBox();
            this.tbUsers = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
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
            this.tbChat.Size = new System.Drawing.Size(621, 481);
            this.tbChat.TabIndex = 1;
            // 
            // tbUsers
            // 
            this.tbUsers.BackColor = System.Drawing.SystemColors.Window;
            this.tbUsers.Location = new System.Drawing.Point(642, 13);
            this.tbUsers.Margin = new System.Windows.Forms.Padding(4);
            this.tbUsers.Multiline = true;
            this.tbUsers.Name = "tbUsers";
            this.tbUsers.ReadOnly = true;
            this.tbUsers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbUsers.Size = new System.Drawing.Size(190, 481);
            this.tbUsers.TabIndex = 2;
            this.tbUsers.Text = "Users: ";
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 507);
            this.Controls.Add(this.tbUsers);
            this.Controls.Add(this.tbChat);
            this.Name = "Form";
            this.Text = "UDP Chat | Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbChat;
        private System.Windows.Forms.TextBox tbUsers;
    }
}

