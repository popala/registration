namespace Rejestracja
{
    partial class frmNewDataFile
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpNewFile = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtContestTitle = new System.Windows.Forms.TextBox();
            this.txtDataFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tpOpenFile = new System.Windows.Forms.TabPage();
            this.lvFileList = new System.Windows.Forms.ListView();
            this.lblError = new System.Windows.Forms.Label();
            this.tpOpenDatabase = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tpNewFile.SuspendLayout();
            this.tpOpenFile.SuspendLayout();
            this.tpOpenDatabase.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(366, 261);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(447, 261);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Anuluj";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpNewFile);
            this.tabControl1.Controls.Add(this.tpOpenFile);
            this.tabControl1.Controls.Add(this.tpOpenDatabase);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(536, 255);
            this.tabControl1.TabIndex = 4;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tpNewFile
            // 
            this.tpNewFile.Controls.Add(this.label2);
            this.tpNewFile.Controls.Add(this.label7);
            this.tpNewFile.Controls.Add(this.txtContestTitle);
            this.tpNewFile.Controls.Add(this.txtDataFileName);
            this.tpNewFile.Controls.Add(this.label1);
            this.tpNewFile.Location = new System.Drawing.Point(4, 22);
            this.tpNewFile.Name = "tpNewFile";
            this.tpNewFile.Padding = new System.Windows.Forms.Padding(3);
            this.tpNewFile.Size = new System.Drawing.Size(528, 229);
            this.tpNewFile.TabIndex = 0;
            this.tpNewFile.Text = "Nowy Plik";
            this.tpNewFile.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(22, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(481, 32);
            this.label2.TabIndex = 18;
            this.label2.Text = "Program nie ma zapamiętanego pliku danych. Proszę wpisać nazwę z której utworzy n" +
    "owy plik danych lub na panelu \"Otwórz Plik\" wybrać istniejący już plik.\r\n";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 62);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Nazwa Konkursu:";
            // 
            // txtContestTitle
            // 
            this.txtContestTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContestTitle.Location = new System.Drawing.Point(114, 59);
            this.txtContestTitle.MaxLength = 256;
            this.txtContestTitle.Name = "txtContestTitle";
            this.txtContestTitle.Size = new System.Drawing.Size(372, 20);
            this.txtContestTitle.TabIndex = 0;
            this.txtContestTitle.TextChanged += new System.EventHandler(this.txtContestTitle_TextChanged);
            // 
            // txtDataFileName
            // 
            this.txtDataFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDataFileName.Location = new System.Drawing.Point(114, 85);
            this.txtDataFileName.MaxLength = 200;
            this.txtDataFileName.Name = "txtDataFileName";
            this.txtDataFileName.ReadOnly = true;
            this.txtDataFileName.Size = new System.Drawing.Size(372, 20);
            this.txtDataFileName.TabIndex = 3;
            this.txtDataFileName.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Nazwa pliku:";
            // 
            // tpOpenFile
            // 
            this.tpOpenFile.Controls.Add(this.lvFileList);
            this.tpOpenFile.Location = new System.Drawing.Point(4, 22);
            this.tpOpenFile.Name = "tpOpenFile";
            this.tpOpenFile.Padding = new System.Windows.Forms.Padding(3);
            this.tpOpenFile.Size = new System.Drawing.Size(528, 229);
            this.tpOpenFile.TabIndex = 1;
            this.tpOpenFile.Text = "Istniejący Plik";
            this.tpOpenFile.UseVisualStyleBackColor = true;
            // 
            // lvFileList
            // 
            this.lvFileList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFileList.FullRowSelect = true;
            this.lvFileList.Location = new System.Drawing.Point(3, 3);
            this.lvFileList.Name = "lvFileList";
            this.lvFileList.Size = new System.Drawing.Size(522, 226);
            this.lvFileList.TabIndex = 0;
            this.lvFileList.UseCompatibleStateImageBehavior = false;
            this.lvFileList.View = System.Windows.Forms.View.Details;
            this.lvFileList.SelectedIndexChanged += new System.EventHandler(this.lvFileList_SelectedIndexChanged);
            this.lvFileList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvFileList_MouseDoubleClick);
            // 
            // lblError
            // 
            this.lblError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblError.AutoSize = true;
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(12, 266);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(39, 13);
            this.lblError.TabIndex = 5;
            this.lblError.Text = "lblError";
            // 
            // tpOpenDatabase
            // 
            this.tpOpenDatabase.Controls.Add(this.label9);
            this.tpOpenDatabase.Controls.Add(this.textBox1);
            this.tpOpenDatabase.Controls.Add(this.txtPort);
            this.tpOpenDatabase.Controls.Add(this.btnCreate);
            this.tpOpenDatabase.Controls.Add(this.btnTest);
            this.tpOpenDatabase.Controls.Add(this.label6);
            this.tpOpenDatabase.Controls.Add(this.txtDatabase);
            this.tpOpenDatabase.Controls.Add(this.label5);
            this.tpOpenDatabase.Controls.Add(this.txtPassword);
            this.tpOpenDatabase.Controls.Add(this.label4);
            this.tpOpenDatabase.Controls.Add(this.txtUser);
            this.tpOpenDatabase.Controls.Add(this.label3);
            this.tpOpenDatabase.Controls.Add(this.txtServer);
            this.tpOpenDatabase.Location = new System.Drawing.Point(4, 22);
            this.tpOpenDatabase.Name = "tpOpenDatabase";
            this.tpOpenDatabase.Size = new System.Drawing.Size(528, 229);
            this.tpOpenDatabase.TabIndex = 3;
            this.tpOpenDatabase.Text = "Baza MySQL";
            this.tpOpenDatabase.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Serwer/Port:";
            // 
            // txtServer
            // 
            this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServer.Location = new System.Drawing.Point(117, 23);
            this.txtServer.MaxLength = 256;
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(291, 20);
            this.txtServer.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(46, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Użytkownik:";
            // 
            // txtUser
            // 
            this.txtUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUser.Location = new System.Drawing.Point(117, 49);
            this.txtUser.MaxLength = 256;
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(291, 20);
            this.txtUser.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(72, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Hasło:";
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(117, 75);
            this.txtPassword.MaxLength = 256;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(291, 20);
            this.txtPassword.TabIndex = 22;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(77, 104);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Baza:";
            // 
            // txtDatabase
            // 
            this.txtDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDatabase.Location = new System.Drawing.Point(117, 101);
            this.txtDatabase.MaxLength = 256;
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(291, 20);
            this.txtDatabase.TabIndex = 24;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(414, 187);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 26;
            this.btnTest.Text = "Testuj";
            this.btnTest.UseVisualStyleBackColor = true;
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(333, 187);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 26;
            this.btnCreate.Text = "Stwórz";
            this.btnCreate.UseVisualStyleBackColor = true;
            // 
            // txtPort
            // 
            this.txtPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPort.Location = new System.Drawing.Point(414, 23);
            this.txtPort.MaxLength = 256;
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(75, 20);
            this.txtPort.TabIndex = 27;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(33, 152);
            this.textBox1.MaxLength = 256;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(456, 20);
            this.textBox1.TabIndex = 28;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(30, 136);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(297, 13);
            this.label9.TabIndex = 30;
            this.label9.Text = "Dodatkowe parametry połączenia (używaj na własne ryzyko):";
            // 
            // frmNewDataFile
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(534, 292);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Name = "frmNewDataFile";
            this.Text = "Plik Konkursu";
            this.Load += new System.EventHandler(this.frmNewDataFile_Load);
            this.Shown += new System.EventHandler(this.frmNewDataFile_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tpNewFile.ResumeLayout(false);
            this.tpNewFile.PerformLayout();
            this.tpOpenFile.ResumeLayout(false);
            this.tpOpenDatabase.ResumeLayout(false);
            this.tpOpenDatabase.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpNewFile;
        private System.Windows.Forms.TextBox txtDataFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tpOpenFile;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtContestTitle;
        private System.Windows.Forms.ListView lvFileList;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tpOpenDatabase;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox txtPort;
    }
}