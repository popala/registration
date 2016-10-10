namespace Rejestracja {
    partial class frmImportFile {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.lblFileName = new System.Windows.Forms.Label();
            this.chkHasHeaders = new System.Windows.Forms.CheckBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lvFilePreview = new System.Windows.Forms.ListView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chkAutoAddPublisher = new System.Windows.Forms.CheckBox();
            this.chkAutoAddScales = new System.Windows.Forms.CheckBox();
            this.lblImportMessage = new System.Windows.Forms.Label();
            this.cboModelPublisher = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cboModelScale = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cboModelName = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cboClubName = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboLastName = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboFirstName = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboEmail = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboTimeStamp = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cboModelCategory1 = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cboCategoryAction5 = new System.Windows.Forms.ComboBox();
            this.cboModelCategory5 = new System.Windows.Forms.ComboBox();
            this.cboCategoryAction4 = new System.Windows.Forms.ComboBox();
            this.cboModelCategory4 = new System.Windows.Forms.ComboBox();
            this.cboCategoryAction3 = new System.Windows.Forms.ComboBox();
            this.cboCategoryAction2 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cboModelCategory3 = new System.Windows.Forms.ComboBox();
            this.cboModelCategory2 = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.chkDropExistingRecords = new System.Windows.Forms.CheckBox();
            this.cboYearOfBirth = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(12, 12);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(102, 23);
            this.btnSelectFile.TabIndex = 3;
            this.btnSelectFile.Text = "Wybierz Plik...";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(120, 17);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(61, 13);
            this.lblFileName.TabIndex = 4;
            this.lblFileName.Text = "lblFileName";
            // 
            // chkHasHeaders
            // 
            this.chkHasHeaders.AutoSize = true;
            this.chkHasHeaders.Location = new System.Drawing.Point(45, 45);
            this.chkHasHeaders.Name = "chkHasHeaders";
            this.chkHasHeaders.Size = new System.Drawing.Size(107, 17);
            this.chkHasHeaders.TabIndex = 5;
            this.chkHasHeaders.Text = "Plik ma nagłówki";
            this.chkHasHeaders.UseVisualStyleBackColor = true;
            this.chkHasHeaders.Click += new System.EventHandler(this.chkHasHeaders_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(538, 607);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 1;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(619, 607);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Anuluj";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(3, 70);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(699, 531);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lvFilePreview);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(691, 505);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Podgląd Pliku";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lvFilePreview
            // 
            this.lvFilePreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFilePreview.FullRowSelect = true;
            this.lvFilePreview.Location = new System.Drawing.Point(0, 0);
            this.lvFilePreview.Name = "lvFilePreview";
            this.lvFilePreview.Size = new System.Drawing.Size(691, 505);
            this.lvFilePreview.TabIndex = 1;
            this.lvFilePreview.UseCompatibleStateImageBehavior = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chkAutoAddPublisher);
            this.tabPage2.Controls.Add(this.chkAutoAddScales);
            this.tabPage2.Controls.Add(this.lblImportMessage);
            this.tabPage2.Controls.Add(this.cboModelPublisher);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.cboModelScale);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.cboModelName);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.cboClubName);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.cboLastName);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.cboFirstName);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.cboEmail);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.cboTimeStamp);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(691, 505);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Mapowanie Pól";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chkAutoAddPublisher
            // 
            this.chkAutoAddPublisher.AutoSize = true;
            this.chkAutoAddPublisher.Location = new System.Drawing.Point(503, 444);
            this.chkAutoAddPublisher.Name = "chkAutoAddPublisher";
            this.chkAutoAddPublisher.Size = new System.Drawing.Size(163, 17);
            this.chkAutoAddPublisher.TabIndex = 11;
            this.chkAutoAddPublisher.Text = "Dodaj nie znalezione w bazie";
            this.chkAutoAddPublisher.UseVisualStyleBackColor = true;
            // 
            // chkAutoAddScales
            // 
            this.chkAutoAddScales.AutoSize = true;
            this.chkAutoAddScales.Location = new System.Drawing.Point(503, 417);
            this.chkAutoAddScales.Name = "chkAutoAddScales";
            this.chkAutoAddScales.Size = new System.Drawing.Size(163, 17);
            this.chkAutoAddScales.TabIndex = 9;
            this.chkAutoAddScales.Text = "Dodaj nie znalezione w bazie";
            this.chkAutoAddScales.UseVisualStyleBackColor = true;
            // 
            // lblImportMessage
            // 
            this.lblImportMessage.AutoSize = true;
            this.lblImportMessage.ForeColor = System.Drawing.Color.Red;
            this.lblImportMessage.Location = new System.Drawing.Point(18, 477);
            this.lblImportMessage.Name = "lblImportMessage";
            this.lblImportMessage.Size = new System.Drawing.Size(89, 13);
            this.lblImportMessage.TabIndex = 24;
            this.lblImportMessage.Text = "lblImportMessage";
            // 
            // cboModelPublisher
            // 
            this.cboModelPublisher.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModelPublisher.FormattingEnabled = true;
            this.cboModelPublisher.Location = new System.Drawing.Point(124, 442);
            this.cboModelPublisher.Name = "cboModelPublisher";
            this.cboModelPublisher.Size = new System.Drawing.Size(373, 21);
            this.cboModelPublisher.TabIndex = 10;
            this.cboModelPublisher.SelectedIndexChanged += new System.EventHandler(this.cboField_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(22, 445);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(96, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "Wydawca Modelu:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cboModelScale
            // 
            this.cboModelScale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModelScale.FormattingEnabled = true;
            this.cboModelScale.Location = new System.Drawing.Point(124, 415);
            this.cboModelScale.Name = "cboModelScale";
            this.cboModelScale.Size = new System.Drawing.Size(373, 21);
            this.cboModelScale.TabIndex = 8;
            this.cboModelScale.SelectedIndexChanged += new System.EventHandler(this.cboField_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label9.Location = new System.Drawing.Point(30, 418);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Skala Modelu:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cboModelName
            // 
            this.cboModelName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModelName.FormattingEnabled = true;
            this.cboModelName.Location = new System.Drawing.Point(124, 225);
            this.cboModelName.Name = "cboModelName";
            this.cboModelName.Size = new System.Drawing.Size(527, 21);
            this.cboModelName.TabIndex = 6;
            this.cboModelName.SelectedIndexChanged += new System.EventHandler(this.cboField_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label7.Location = new System.Drawing.Point(24, 228);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Nazwa Modelu:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cboClubName
            // 
            this.cboClubName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboClubName.FormattingEnabled = true;
            this.cboClubName.Location = new System.Drawing.Point(124, 122);
            this.cboClubName.Name = "cboClubName";
            this.cboClubName.Size = new System.Drawing.Size(527, 21);
            this.cboClubName.TabIndex = 4;
            this.cboClubName.SelectedIndexChanged += new System.EventHandler(this.cboField_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(87, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Klub:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cboLastName
            // 
            this.cboLastName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLastName.FormattingEnabled = true;
            this.cboLastName.Location = new System.Drawing.Point(124, 95);
            this.cboLastName.Name = "cboLastName";
            this.cboLastName.Size = new System.Drawing.Size(527, 21);
            this.cboLastName.TabIndex = 3;
            this.cboLastName.SelectedIndexChanged += new System.EventHandler(this.cboField_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(53, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Nazwisko:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cboFirstName
            // 
            this.cboFirstName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFirstName.FormattingEnabled = true;
            this.cboFirstName.Location = new System.Drawing.Point(124, 68);
            this.cboFirstName.Name = "cboFirstName";
            this.cboFirstName.Size = new System.Drawing.Size(527, 21);
            this.cboFirstName.TabIndex = 2;
            this.cboFirstName.SelectedIndexChanged += new System.EventHandler(this.cboField_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(84, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Imię:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cboEmail
            // 
            this.cboEmail.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEmail.FormattingEnabled = true;
            this.cboEmail.Location = new System.Drawing.Point(124, 41);
            this.cboEmail.Name = "cboEmail";
            this.cboEmail.Size = new System.Drawing.Size(527, 21);
            this.cboEmail.TabIndex = 1;
            this.cboEmail.SelectedIndexChanged += new System.EventHandler(this.cboField_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(83, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Email:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cboTimeStamp
            // 
            this.cboTimeStamp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTimeStamp.FormattingEnabled = true;
            this.cboTimeStamp.Location = new System.Drawing.Point(124, 14);
            this.cboTimeStamp.Name = "cboTimeStamp";
            this.cboTimeStamp.Size = new System.Drawing.Size(527, 21);
            this.cboTimeStamp.TabIndex = 0;
            this.cboTimeStamp.SelectedIndexChanged += new System.EventHandler(this.cboField_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Data Rejestracji:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.AliceBlue;
            this.panel2.Controls.Add(this.cboModelCategory1);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.cboCategoryAction5);
            this.panel2.Controls.Add(this.cboModelCategory5);
            this.panel2.Controls.Add(this.cboCategoryAction4);
            this.panel2.Controls.Add(this.cboModelCategory4);
            this.panel2.Controls.Add(this.cboCategoryAction3);
            this.panel2.Controls.Add(this.cboCategoryAction2);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.cboModelCategory3);
            this.panel2.Controls.Add(this.cboModelCategory2);
            this.panel2.Location = new System.Drawing.Point(-4, 256);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(699, 149);
            this.panel2.TabIndex = 7;
            // 
            // cboModelCategory1
            // 
            this.cboModelCategory1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModelCategory1.FormattingEnabled = true;
            this.cboModelCategory1.Location = new System.Drawing.Point(248, 8);
            this.cboModelCategory1.Name = "cboModelCategory1";
            this.cboModelCategory1.Size = new System.Drawing.Size(407, 21);
            this.cboModelCategory1.TabIndex = 0;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(152, 11);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(90, 13);
            this.label14.TabIndex = 42;
            this.label14.Text = "Sprawdź najpierw";
            // 
            // cboCategoryAction5
            // 
            this.cboCategoryAction5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCategoryAction5.FormattingEnabled = true;
            this.cboCategoryAction5.Location = new System.Drawing.Point(128, 116);
            this.cboCategoryAction5.Name = "cboCategoryAction5";
            this.cboCategoryAction5.Size = new System.Drawing.Size(114, 21);
            this.cboCategoryAction5.TabIndex = 7;
            // 
            // cboModelCategory5
            // 
            this.cboModelCategory5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModelCategory5.FormattingEnabled = true;
            this.cboModelCategory5.Location = new System.Drawing.Point(248, 116);
            this.cboModelCategory5.Name = "cboModelCategory5";
            this.cboModelCategory5.Size = new System.Drawing.Size(407, 21);
            this.cboModelCategory5.TabIndex = 8;
            // 
            // cboCategoryAction4
            // 
            this.cboCategoryAction4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCategoryAction4.FormattingEnabled = true;
            this.cboCategoryAction4.Location = new System.Drawing.Point(128, 89);
            this.cboCategoryAction4.Name = "cboCategoryAction4";
            this.cboCategoryAction4.Size = new System.Drawing.Size(114, 21);
            this.cboCategoryAction4.TabIndex = 5;
            // 
            // cboModelCategory4
            // 
            this.cboModelCategory4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModelCategory4.FormattingEnabled = true;
            this.cboModelCategory4.Location = new System.Drawing.Point(248, 89);
            this.cboModelCategory4.Name = "cboModelCategory4";
            this.cboModelCategory4.Size = new System.Drawing.Size(407, 21);
            this.cboModelCategory4.TabIndex = 6;
            // 
            // cboCategoryAction3
            // 
            this.cboCategoryAction3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCategoryAction3.FormattingEnabled = true;
            this.cboCategoryAction3.Location = new System.Drawing.Point(128, 62);
            this.cboCategoryAction3.Name = "cboCategoryAction3";
            this.cboCategoryAction3.Size = new System.Drawing.Size(114, 21);
            this.cboCategoryAction3.TabIndex = 3;
            // 
            // cboCategoryAction2
            // 
            this.cboCategoryAction2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCategoryAction2.FormattingEnabled = true;
            this.cboCategoryAction2.Location = new System.Drawing.Point(128, 35);
            this.cboCategoryAction2.Name = "cboCategoryAction2";
            this.cboCategoryAction2.Size = new System.Drawing.Size(114, 21);
            this.cboCategoryAction2.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label8.Location = new System.Drawing.Point(12, 14);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(110, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Kategoria Modelu:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cboModelCategory3
            // 
            this.cboModelCategory3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModelCategory3.FormattingEnabled = true;
            this.cboModelCategory3.Location = new System.Drawing.Point(248, 62);
            this.cboModelCategory3.Name = "cboModelCategory3";
            this.cboModelCategory3.Size = new System.Drawing.Size(407, 21);
            this.cboModelCategory3.TabIndex = 4;
            // 
            // cboModelCategory2
            // 
            this.cboModelCategory2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModelCategory2.FormattingEnabled = true;
            this.cboModelCategory2.Location = new System.Drawing.Point(248, 35);
            this.cboModelCategory2.Name = "cboModelCategory2";
            this.cboModelCategory2.Size = new System.Drawing.Size(407, 21);
            this.cboModelCategory2.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.AliceBlue;
            this.panel1.Controls.Add(this.cboYearOfBirth);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Location = new System.Drawing.Point(-4, 152);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(699, 66);
            this.panel1.TabIndex = 5;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label12.Location = new System.Drawing.Point(27, 13);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(95, 13);
            this.label12.TabIndex = 28;
            this.label12.Text = "Rok Urodzenia:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkDropExistingRecords
            // 
            this.chkDropExistingRecords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkDropExistingRecords.AutoSize = true;
            this.chkDropExistingRecords.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkDropExistingRecords.Location = new System.Drawing.Point(237, 611);
            this.chkDropExistingRecords.Name = "chkDropExistingRecords";
            this.chkDropExistingRecords.Size = new System.Drawing.Size(289, 17);
            this.chkDropExistingRecords.TabIndex = 0;
            this.chkDropExistingRecords.Text = "Usuń instniejące rejestracje przed rozpoczęciem importu";
            this.chkDropExistingRecords.UseVisualStyleBackColor = true;
            // 
            // cboYearOfBirth
            // 
            this.cboYearOfBirth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboYearOfBirth.FormattingEnabled = true;
            this.cboYearOfBirth.Location = new System.Drawing.Point(128, 10);
            this.cboYearOfBirth.Name = "cboYearOfBirth";
            this.cboYearOfBirth.Size = new System.Drawing.Size(527, 21);
            this.cboYearOfBirth.TabIndex = 0;
            // 
            // frmImportFile
            // 
            this.AcceptButton = this.btnImport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(704, 642);
            this.Controls.Add(this.chkDropExistingRecords);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.chkHasHeaders);
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.btnSelectFile);
            this.MinimumSize = new System.Drawing.Size(720, 680);
            this.Name = "frmImportFile";
            this.Text = "Import Danych";
            this.Load += new System.EventHandler(this.frmImportFile_Load);
            this.Shown += new System.EventHandler(this.frmImportFile_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.CheckBox chkHasHeaders;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListView lvFilePreview;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ComboBox cboTimeStamp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboFirstName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboEmail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboLastName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboClubName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboModelName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboModelScale;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboModelPublisher;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblImportMessage;
        private System.Windows.Forms.ComboBox cboModelCategory2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkDropExistingRecords;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cboCategoryAction5;
        private System.Windows.Forms.ComboBox cboModelCategory5;
        private System.Windows.Forms.ComboBox cboCategoryAction4;
        private System.Windows.Forms.ComboBox cboModelCategory4;
        private System.Windows.Forms.ComboBox cboCategoryAction3;
        private System.Windows.Forms.ComboBox cboCategoryAction2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboModelCategory3;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox chkAutoAddPublisher;
        private System.Windows.Forms.CheckBox chkAutoAddScales;
        private System.Windows.Forms.ComboBox cboModelCategory1;
        private System.Windows.Forms.ComboBox cboYearOfBirth;
    }
}