using System.Windows.Forms;
namespace Rejestracja
{
    partial class frmRegistrationEntry
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtModelClub = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtModelName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtEntryId = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblErrors = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.cboYearOfBirth = new System.Windows.Forms.ComboBox();
            this.cboAgeGroup = new System.Windows.Forms.ComboBox();
            this.cboModelPublisher = new System.Windows.Forms.ComboBox();
            this.cboModelScale = new System.Windows.Forms.ComboBox();
            this.cboModelCategory = new System.Windows.Forms.ComboBox();
            this.cboModelClass = new System.Windows.Forms.ComboBox();
            this.btnAddPrintModel = new System.Windows.Forms.Button();
            this.btnNewModel = new System.Windows.Forms.Button();
            this.chkPrintRegistrationCard = new System.Windows.Forms.CheckBox();
            this.btnNewRegistration = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(10, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "MODELARZ";
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(12, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(500, 2);
            this.label2.TabIndex = 1;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(54, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Imię/Nazwisko:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(141, 101);
            this.txtFirstName.MaxLength = 64;
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(135, 20);
            this.txtFirstName.TabIndex = 1;
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(282, 101);
            this.txtLastName.MaxLength = 64;
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(155, 20);
            this.txtLastName.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(54, 148);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "Rok Urodzenia:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(31, 171);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 15);
            this.label5.TabIndex = 7;
            this.label5.Text = "Grupa Wiekowa:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(141, 79);
            this.txtEmail.MaxLength = 256;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(296, 20);
            this.txtEmail.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(54, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 16);
            this.label6.TabIndex = 9;
            this.label6.Text = "Email:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtModelClub
            // 
            this.txtModelClub.Location = new System.Drawing.Point(141, 123);
            this.txtModelClub.MaxLength = 128;
            this.txtModelClub.Name = "txtModelClub";
            this.txtModelClub.Size = new System.Drawing.Size(296, 20);
            this.txtModelClub.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(54, 126);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 15);
            this.label7.TabIndex = 11;
            this.label7.Text = "Klub:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label8.Location = new System.Drawing.Point(12, 247);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(500, 2);
            this.label8.TabIndex = 14;
            this.label8.Text = "label8";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label9.Location = new System.Drawing.Point(15, 225);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "MODEL";
            // 
            // txtModelName
            // 
            this.txtModelName.Location = new System.Drawing.Point(141, 301);
            this.txtModelName.MaxLength = 256;
            this.txtModelName.Name = "txtModelName";
            this.txtModelName.Size = new System.Drawing.Size(296, 20);
            this.txtModelName.TabIndex = 6;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(54, 304);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(81, 15);
            this.label10.TabIndex = 15;
            this.label10.Text = "Nazwa:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtEntryId
            // 
            this.txtEntryId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txtEntryId.Location = new System.Drawing.Point(141, 279);
            this.txtEntryId.Name = "txtEntryId";
            this.txtEntryId.ReadOnly = true;
            this.txtEntryId.Size = new System.Drawing.Size(64, 20);
            this.txtEntryId.TabIndex = 18;
            this.txtEntryId.TabStop = false;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(18, 282);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(117, 15);
            this.label11.TabIndex = 17;
            this.label11.Text = "Numer Startowy:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(54, 372);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(81, 15);
            this.label12.TabIndex = 19;
            this.label12.Text = "Kategoria:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(54, 326);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(81, 15);
            this.label13.TabIndex = 21;
            this.label13.Text = "Wydawca:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(54, 349);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(81, 15);
            this.label14.TabIndex = 23;
            this.label14.Text = "Skala:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(54, 395);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(81, 15);
            this.label15.TabIndex = 25;
            this.label15.Text = "Klasa:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.Controls.Add(this.lblErrors);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Location = new System.Drawing.Point(-1, 506);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(536, 55);
            this.panel1.TabIndex = 27;
            // 
            // lblErrors
            // 
            this.lblErrors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblErrors.ForeColor = System.Drawing.Color.Red;
            this.lblErrors.Location = new System.Drawing.Point(10, 12);
            this.lblErrors.Name = "lblErrors";
            this.lblErrors.Size = new System.Drawing.Size(328, 33);
            this.lblErrors.TabIndex = 5;
            this.lblErrors.Text = "lblErrors";
            this.lblErrors.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(344, 17);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Zapisz Zmiany";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(444, 17);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Zamknij";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cboYearOfBirth
            // 
            this.cboYearOfBirth.FormattingEnabled = true;
            this.cboYearOfBirth.Location = new System.Drawing.Point(141, 145);
            this.cboYearOfBirth.Name = "cboYearOfBirth";
            this.cboYearOfBirth.Size = new System.Drawing.Size(79, 21);
            this.cboYearOfBirth.TabIndex = 4;
            this.cboYearOfBirth.SelectedIndexChanged += new System.EventHandler(this.cboYearOfBirth_SelectedIndexChanged);
            // 
            // cboAgeGroup
            // 
            this.cboAgeGroup.Enabled = false;
            this.cboAgeGroup.FormattingEnabled = true;
            this.cboAgeGroup.Location = new System.Drawing.Point(141, 168);
            this.cboAgeGroup.Name = "cboAgeGroup";
            this.cboAgeGroup.Size = new System.Drawing.Size(198, 21);
            this.cboAgeGroup.TabIndex = 5;
            this.cboAgeGroup.TabStop = false;
            // 
            // cboModelPublisher
            // 
            this.cboModelPublisher.FormattingEnabled = true;
            this.cboModelPublisher.Location = new System.Drawing.Point(141, 323);
            this.cboModelPublisher.Name = "cboModelPublisher";
            this.cboModelPublisher.Size = new System.Drawing.Size(371, 21);
            this.cboModelPublisher.TabIndex = 7;
            // 
            // cboModelScale
            // 
            this.cboModelScale.FormattingEnabled = true;
            this.cboModelScale.Location = new System.Drawing.Point(141, 346);
            this.cboModelScale.Name = "cboModelScale";
            this.cboModelScale.Size = new System.Drawing.Size(100, 21);
            this.cboModelScale.TabIndex = 8;
            // 
            // cboModelCategory
            // 
            this.cboModelCategory.FormattingEnabled = true;
            this.cboModelCategory.Location = new System.Drawing.Point(141, 369);
            this.cboModelCategory.Name = "cboModelCategory";
            this.cboModelCategory.Size = new System.Drawing.Size(371, 21);
            this.cboModelCategory.TabIndex = 9;
            this.cboModelCategory.SelectedIndexChanged += new System.EventHandler(this.cboModelCategory_SelectedIndexChanged);
            // 
            // cboModelClass
            // 
            this.cboModelClass.FormattingEnabled = true;
            this.cboModelClass.Location = new System.Drawing.Point(141, 392);
            this.cboModelClass.Name = "cboModelClass";
            this.cboModelClass.Size = new System.Drawing.Size(198, 21);
            this.cboModelClass.TabIndex = 10;
            // 
            // btnAddPrintModel
            // 
            this.btnAddPrintModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddPrintModel.Location = new System.Drawing.Point(414, 434);
            this.btnAddPrintModel.Name = "btnAddPrintModel";
            this.btnAddPrintModel.Size = new System.Drawing.Size(98, 23);
            this.btnAddPrintModel.TabIndex = 12;
            this.btnAddPrintModel.Text = "Dodaj Model";
            this.btnAddPrintModel.UseVisualStyleBackColor = true;
            this.btnAddPrintModel.Click += new System.EventHandler(this.btnAddPrintModel_Click);
            // 
            // btnNewModel
            // 
            this.btnNewModel.Location = new System.Drawing.Point(414, 215);
            this.btnNewModel.Name = "btnNewModel";
            this.btnNewModel.Size = new System.Drawing.Size(98, 23);
            this.btnNewModel.TabIndex = 11;
            this.btnNewModel.Text = "Nowy Model";
            this.btnNewModel.UseVisualStyleBackColor = true;
            this.btnNewModel.Click += new System.EventHandler(this.btnNewModel_Click);
            // 
            // chkPrintRegistrationCard
            // 
            this.chkPrintRegistrationCard.AutoSize = true;
            this.chkPrintRegistrationCard.Location = new System.Drawing.Point(141, 434);
            this.chkPrintRegistrationCard.Name = "chkPrintRegistrationCard";
            this.chkPrintRegistrationCard.Size = new System.Drawing.Size(145, 17);
            this.chkPrintRegistrationCard.TabIndex = 28;
            this.chkPrintRegistrationCard.Text = "Nie drukuj karty startowej";
            this.chkPrintRegistrationCard.UseVisualStyleBackColor = true;
            // 
            // btnNewRegistration
            // 
            this.btnNewRegistration.Location = new System.Drawing.Point(414, 13);
            this.btnNewRegistration.Name = "btnNewRegistration";
            this.btnNewRegistration.Size = new System.Drawing.Size(98, 23);
            this.btnNewRegistration.TabIndex = 29;
            this.btnNewRegistration.Text = "Nowy Modelarz";
            this.btnNewRegistration.UseVisualStyleBackColor = true;
            this.btnNewRegistration.Click += new System.EventHandler(this.btnNewRegistration_Click);
            // 
            // frmRegistrationEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(534, 560);
            this.Controls.Add(this.btnNewRegistration);
            this.Controls.Add(this.chkPrintRegistrationCard);
            this.Controls.Add(this.btnAddPrintModel);
            this.Controls.Add(this.btnNewModel);
            this.Controls.Add(this.cboModelClass);
            this.Controls.Add(this.cboModelCategory);
            this.Controls.Add(this.cboModelScale);
            this.Controls.Add(this.cboModelPublisher);
            this.Controls.Add(this.cboAgeGroup);
            this.Controls.Add(this.cboYearOfBirth);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtEntryId);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtModelName);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtModelClub);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmRegistrationEntry";
            this.Text = "frmRegistrationEntry";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtModelClub;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtModelName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtEntryId;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox cboYearOfBirth;
        private System.Windows.Forms.ComboBox cboAgeGroup;
        private System.Windows.Forms.ComboBox cboModelPublisher;
        private System.Windows.Forms.ComboBox cboModelScale;
        private System.Windows.Forms.ComboBox cboModelCategory;
        private System.Windows.Forms.ComboBox cboModelClass;
        private Button btnAddPrintModel;
        private Button btnNewModel;
        private Label lblErrors;
        private CheckBox chkPrintRegistrationCard;
        private Button btnNewRegistration;
    }
}