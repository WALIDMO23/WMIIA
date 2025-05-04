namespace FTPAPPLICATION
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtserverIp = new TextBox();
            txtUsername = new TextBox();
            txtCommand = new TextBox();
            btnConnect = new Button();
            LSTOutput = new ListBox();
            txtPassword = new TextBox();
            btnSendCommand = new Button();
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            SuspendLayout();
            // 
            // txtserverIp
            // 
            txtserverIp.Location = new Point(104, 22);
            txtserverIp.Name = "txtserverIp";
            txtserverIp.Size = new Size(125, 27);
            txtserverIp.TabIndex = 0;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(104, 77);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(125, 27);
            txtUsername.TabIndex = 1;
            // 
            // txtCommand
            // 
            txtCommand.Location = new Point(634, 73);
            txtCommand.Name = "txtCommand";
            txtCommand.Size = new Size(125, 27);
            txtCommand.TabIndex = 2;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(287, 73);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(144, 29);
            btnConnect.TabIndex = 3;
            btnConnect.Text = "Connect to FTP";
            btnConnect.UseVisualStyleBackColor = true;
            // 
            // LSTOutput
            // 
            LSTOutput.FormattingEnabled = true;
            LSTOutput.Location = new Point(46, 234);
            LSTOutput.Name = "LSTOutput";
            LSTOutput.Size = new Size(699, 164);
            LSTOutput.TabIndex = 4;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(104, 125);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(125, 27);
            txtPassword.TabIndex = 5;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // btnSendCommand
            // 
            btnSendCommand.Location = new Point(651, 125);
            btnSendCommand.Name = "btnSendCommand";
            btnSendCommand.Size = new Size(94, 29);
            btnSendCommand.TabIndex = 6;
            btnSendCommand.Text = "SEND";
            btnSendCommand.UseVisualStyleBackColor = true;
            btnSendCommand.Click += btnSend_Click;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog1";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 25);
            label1.Name = "label1";
            label1.Size = new Size(76, 20);
            label1.TabIndex = 7;
            label1.Text = "IP SERVER";
            
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 77);
            label2.Name = "label2";
            label2.Size = new Size(86, 20);
            label2.TabIndex = 8;
            label2.Text = "USERNAME";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(13, 125);
            label3.Name = "label3";
            label3.Size = new Size(87, 20);
            label3.TabIndex = 9;
            label3.Text = "PASSWORD";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(13, 197);
            label4.Name = "label4";
            label4.Size = new Size(101, 20);
            label4.TabIndex = 10;
            label4.Text = "VIEW RESULT ";
            
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(499, 73);
            label5.Name = "label5";
            label5.Size = new Size(114, 20);
            label5.TabIndex = 11;
            label5.Text = "FTP COMMAND";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnSendCommand);
            Controls.Add(txtPassword);
            Controls.Add(LSTOutput);
            Controls.Add(btnConnect);
            Controls.Add(txtCommand);
            Controls.Add(txtUsername);
            Controls.Add(txtserverIp);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtserverIp;
        private TextBox txtUsername;
        private TextBox txtCommand;
        private Button btnConnect;
        private ListBox LSTOutput;
        private TextBox txtPassword;
        private Button btnSendCommand;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
    }
}
