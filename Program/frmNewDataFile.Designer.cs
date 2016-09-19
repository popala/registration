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
            this.tabControl1.SuspendLayout();
            this.tpNewFile.SuspendLayout();
            this.tpOpenFile.SuspendLayout();
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
            this.tpNewFile.Text = "Stwórz Nowy Plik";
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
            this.tpOpenFile.Text = "Otwórz Istniejący Plik";
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
    }
}