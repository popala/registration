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
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtModelClub = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtModelName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblErrors = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.cboYearOfBirth = new System.Windows.Forms.ComboBox();
            this.cboModelPublisher = new System.Windows.Forms.ComboBox();
            this.cboModelScale = new System.Windows.Forms.ComboBox();
            this.btnAddPrintModel = new System.Windows.Forms.Button();
            this.btnNewModel = new System.Windows.Forms.Button();
            this.chkPrintRegistrationCard = new System.Windows.Forms.CheckBox();
            this.btnNewRegistration = new System.Windows.Forms.Button();
            this.txtModelerId = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.btnAddModeler = new System.Windows.Forms.Button();
            this.lvCategories = new Rejestracja.Controls.CustListView(); //new System.Windows.Forms.ListView();
            this.cboEntryId = new System.Windows.Forms.ComboBox();
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
            this.label2.Size = new System.Drawing.Size(636, 2);
            this.label2.TabIndex = 1;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Imię/Nazwisko:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(103, 108);
            this.txtFirstName.MaxLength = 64;
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(166, 20);
            this.txtFirstName.TabIndex = 1;
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(272, 108);
            this.txtLastName.MaxLength = 64;
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(187, 20);
            this.txtLastName.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 155);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Rok Urodzenia:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(103, 86);
            this.txtEmail.MaxLength = 256;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(356, 20);
            this.txtEmail.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(62, 89);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Email:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtModelClub
            // 
            this.txtModelClub.Location = new System.Drawing.Point(103, 130);
            this.txtModelClub.MaxLength = 128;
            this.txtModelClub.Name = "txtModelClub";
            this.txtModelClub.Size = new System.Drawing.Size(356, 20);
            this.txtModelClub.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(66, 133);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Klub:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label8.Location = new System.Drawing.Point(12, 263);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(636, 2);
            this.label8.TabIndex = 14;
            this.label8.Text = "label8";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label9.Location = new System.Drawing.Point(15, 241);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "MODELE";
            // 
            // txtModelName
            // 
            this.txtModelName.Location = new System.Drawing.Point(103, 311);
            this.txtModelName.MaxLength = 256;
            this.txtModelName.Name = "txtModelName";
            this.txtModelName.Size = new System.Drawing.Size(371, 20);
            this.txtModelName.TabIndex = 6;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(54, 314);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Nazwa:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 290);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(85, 13);
            this.label11.TabIndex = 17;
            this.label11.Text = "Numer Startowy:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(42, 380);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(55, 13);
            this.label12.TabIndex = 19;
            this.label12.Text = "Kategorie:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(39, 336);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(58, 13);
            this.label13.TabIndex = 21;
            this.label13.Text = "Wydawca:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(60, 359);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(37, 13);
            this.label14.TabIndex = 23;
            this.label14.Text = "Skala:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.Controls.Add(this.lblErrors);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Location = new System.Drawing.Point(-1, 614);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(666, 55);
            this.panel1.TabIndex = 27;
            // 
            // lblErrors
            // 
            this.lblErrors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblErrors.ForeColor = System.Drawing.Color.Red;
            this.lblErrors.Location = new System.Drawing.Point(10, 12);
            this.lblErrors.Name = "lblErrors";
            this.lblErrors.Size = new System.Drawing.Size(489, 33);
            this.lblErrors.TabIndex = 5;
            this.lblErrors.Text = "lblErrors";
            this.lblErrors.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(574, 19);
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
            this.cboYearOfBirth.Location = new System.Drawing.Point(103, 152);
            this.cboYearOfBirth.Name = "cboYearOfBirth";
            this.cboYearOfBirth.Size = new System.Drawing.Size(98, 21);
            this.cboYearOfBirth.TabIndex = 4;
            this.cboYearOfBirth.SelectedIndexChanged += new System.EventHandler(this.cboYearOfBirth_SelectedIndexChanged);
            // 
            // cboModelPublisher
            // 
            this.cboModelPublisher.FormattingEnabled = true;
            this.cboModelPublisher.Location = new System.Drawing.Point(103, 333);
            this.cboModelPublisher.Name = "cboModelPublisher";
            this.cboModelPublisher.Size = new System.Drawing.Size(277, 21);
            this.cboModelPublisher.TabIndex = 7;
            // 
            // cboModelScale
            // 
            this.cboModelScale.FormattingEnabled = true;
            this.cboModelScale.Location = new System.Drawing.Point(103, 356);
            this.cboModelScale.Name = "cboModelScale";
            this.cboModelScale.Size = new System.Drawing.Size(115, 21);
            this.cboModelScale.TabIndex = 8;
            // 
            // btnAddPrintModel
            // 
            this.btnAddPrintModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddPrintModel.Location = new System.Drawing.Point(103, 579);
            this.btnAddPrintModel.Name = "btnAddPrintModel";
            this.btnAddPrintModel.Size = new System.Drawing.Size(98, 23);
            this.btnAddPrintModel.TabIndex = 12;
            this.btnAddPrintModel.Text = "Dodaj Model";
            this.btnAddPrintModel.UseVisualStyleBackColor = true;
            this.btnAddPrintModel.Click += new System.EventHandler(this.btnAddPrintModel_Click);
            // 
            // btnNewModel
            // 
            this.btnNewModel.Location = new System.Drawing.Point(550, 237);
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
            this.chkPrintRegistrationCard.Location = new System.Drawing.Point(216, 583);
            this.chkPrintRegistrationCard.Name = "chkPrintRegistrationCard";
            this.chkPrintRegistrationCard.Size = new System.Drawing.Size(127, 17);
            this.chkPrintRegistrationCard.TabIndex = 28;
            this.chkPrintRegistrationCard.Text = "Drukuj kartę startową";
            this.chkPrintRegistrationCard.UseVisualStyleBackColor = true;
            // 
            // btnNewRegistration
            // 
            this.btnNewRegistration.Location = new System.Drawing.Point(550, 19);
            this.btnNewRegistration.Name = "btnNewRegistration";
            this.btnNewRegistration.Size = new System.Drawing.Size(98, 23);
            this.btnNewRegistration.TabIndex = 29;
            this.btnNewRegistration.Text = "Nowy Modelarz";
            this.btnNewRegistration.UseVisualStyleBackColor = true;
            this.btnNewRegistration.Click += new System.EventHandler(this.btnNewRegistration_Click);
            // 
            // txtModelerId
            // 
            this.txtModelerId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txtModelerId.Location = new System.Drawing.Point(103, 64);
            this.txtModelerId.Name = "txtModelerId";
            this.txtModelerId.ReadOnly = true;
            this.txtModelerId.Size = new System.Drawing.Size(64, 20);
            this.txtModelerId.TabIndex = 31;
            this.txtModelerId.TabStop = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(56, 67);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(41, 13);
            this.label16.TabIndex = 30;
            this.label16.Text = "Numer:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnAddModeler
            // 
            this.btnAddModeler.Location = new System.Drawing.Point(103, 179);
            this.btnAddModeler.Name = "btnAddModeler";
            this.btnAddModeler.Size = new System.Drawing.Size(115, 23);
            this.btnAddModeler.TabIndex = 32;
            this.btnAddModeler.Text = "Dodaj Modelarza";
            this.btnAddModeler.UseVisualStyleBackColor = true;
            // 
            // lvCategories
            // 
            this.lvCategories.CheckBoxes = true;
            this.lvCategories.FullRowSelect = true;
            this.lvCategories.GridLines = true;
            this.lvCategories.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvCategories.Location = new System.Drawing.Point(103, 379);
            this.lvCategories.MultiSelect = false;
            this.lvCategories.Name = "lvCategories";
            this.lvCategories.Size = new System.Drawing.Size(533, 188);
            this.lvCategories.TabIndex = 33;
            this.lvCategories.UseCompatibleStateImageBehavior = false;
            this.lvCategories.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lvCategories_ItemCheck);
            this.lvCategories.MouseDoubleClick += new MouseEventHandler(this.lvCategories_MouseDoubleClick);
            // 
            // cboEntryId
            // 
            this.cboEntryId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEntryId.FormattingEnabled = true;
            this.cboEntryId.Location = new System.Drawing.Point(103, 287);
            this.cboEntryId.Name = "cboEntryId";
            this.cboEntryId.Size = new System.Drawing.Size(84, 21);
            this.cboEntryId.TabIndex = 34;
            // 
            // frmRegistrationEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(664, 668);
            this.Controls.Add(this.cboEntryId);
            this.Controls.Add(this.lvCategories);
            this.Controls.Add(this.btnAddModeler);
            this.Controls.Add(this.txtModelerId);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.btnNewRegistration);
            this.Controls.Add(this.chkPrintRegistrationCard);
            this.Controls.Add(this.btnAddPrintModel);
            this.Controls.Add(this.btnNewModel);
            this.Controls.Add(this.cboModelScale);
            this.Controls.Add(this.cboModelPublisher);
            this.Controls.Add(this.cboYearOfBirth);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtModelName);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtModelClub);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.label6);
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
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtModelClub;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtModelName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox cboYearOfBirth;
        private System.Windows.Forms.ComboBox cboModelPublisher;
        private System.Windows.Forms.ComboBox cboModelScale;
        private Button btnAddPrintModel;
        private Button btnNewModel;
        private Label lblErrors;
        private CheckBox chkPrintRegistrationCard;
        private Button btnNewRegistration;
        private TextBox txtModelerId;
        private Label label16;
        private Button btnAddModeler;
        private Rejestracja.Controls.CustListView lvCategories;
        private ComboBox cboEntryId;
    }
}