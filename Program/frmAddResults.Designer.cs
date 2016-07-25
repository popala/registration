namespace Rejestracja
{
    partial class frmAddResults
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddResults));
            this.label2 = new System.Windows.Forms.Label();
            this.cboModelCategory = new System.Windows.Forms.ComboBox();
            this.lvResults = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.cmsResultsRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuRCFirstPlace = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRCSecondPlace = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRCThirdPlace = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuRCClear = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btaAddAwardWinner = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEntryId = new System.Windows.Forms.TextBox();
            this.lvAwardResults = new System.Windows.Forms.ListView();
            this.cboSpecialAward = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmsAwardResultsRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsARCDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsResultsRightClick.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.cmsAwardResultsRightClick.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Kategoria:";
            // 
            // cboModelCategory
            // 
            this.cboModelCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboModelCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModelCategory.FormattingEnabled = true;
            this.cboModelCategory.Location = new System.Drawing.Point(69, 14);
            this.cboModelCategory.Name = "cboModelCategory";
            this.cboModelCategory.Size = new System.Drawing.Size(910, 21);
            this.cboModelCategory.TabIndex = 10;
            this.cboModelCategory.SelectedIndexChanged += new System.EventHandler(this.cboModelCategory_SelectedIndexChanged);
            // 
            // lvResults
            // 
            this.lvResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvResults.Location = new System.Drawing.Point(0, 53);
            this.lvResults.Name = "lvResults";
            this.lvResults.Size = new System.Drawing.Size(989, 457);
            this.lvResults.TabIndex = 12;
            this.lvResults.UseCompatibleStateImageBehavior = false;
            this.lvResults.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvResults_MouseDoubleClick);
            this.lvResults.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvResults_MouseDown);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(909, 542);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Zamknij";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cmsResultsRightClick
            // 
            this.cmsResultsRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRCFirstPlace,
            this.mnuRCSecondPlace,
            this.mnuRCThirdPlace,
            this.toolStripSeparator1,
            this.mnuRCClear});
            this.cmsResultsRightClick.Name = "cmsResultsRightClick";
            this.cmsResultsRightClick.Size = new System.Drawing.Size(124, 98);
            // 
            // mnuRCFirstPlace
            // 
            this.mnuRCFirstPlace.Name = "mnuRCFirstPlace";
            this.mnuRCFirstPlace.Size = new System.Drawing.Size(123, 22);
            this.mnuRCFirstPlace.Text = "Miejsce 1";
            this.mnuRCFirstPlace.Click += new System.EventHandler(this.mnuRCFirstPlace_Click);
            // 
            // mnuRCSecondPlace
            // 
            this.mnuRCSecondPlace.Name = "mnuRCSecondPlace";
            this.mnuRCSecondPlace.Size = new System.Drawing.Size(123, 22);
            this.mnuRCSecondPlace.Text = "Miejsce 2";
            this.mnuRCSecondPlace.Click += new System.EventHandler(this.mnuRCSecondPlace_Click);
            // 
            // mnuRCThirdPlace
            // 
            this.mnuRCThirdPlace.Name = "mnuRCThirdPlace";
            this.mnuRCThirdPlace.Size = new System.Drawing.Size(123, 22);
            this.mnuRCThirdPlace.Text = "Miejsce 3";
            this.mnuRCThirdPlace.Click += new System.EventHandler(this.mnuRCThirdPlace_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(120, 6);
            // 
            // mnuRCClear
            // 
            this.mnuRCClear.Name = "mnuRCClear";
            this.mnuRCClear.Size = new System.Drawing.Size(123, 22);
            this.mnuRCClear.Text = "Wyczyść";
            this.mnuRCClear.Click += new System.EventHandler(this.mnuRCClear_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(997, 536);
            this.tabControl1.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cboModelCategory);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.lvResults);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(989, 510);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Kategorie";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btaAddAwardWinner);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.txtEntryId);
            this.tabPage2.Controls.Add(this.lvAwardResults);
            this.tabPage2.Controls.Add(this.cboSpecialAward);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(989, 510);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Nagrody Specjalne";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btaAddAwardWinner
            // 
            this.btaAddAwardWinner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btaAddAwardWinner.Location = new System.Drawing.Point(903, 12);
            this.btaAddAwardWinner.Name = "btaAddAwardWinner";
            this.btaAddAwardWinner.Size = new System.Drawing.Size(75, 23);
            this.btaAddAwardWinner.TabIndex = 17;
            this.btaAddAwardWinner.Text = "Dodaj";
            this.btaAddAwardWinner.UseVisualStyleBackColor = true;
            this.btaAddAwardWinner.Click += new System.EventHandler(this.btaAddAwardWinner_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(782, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Nr Modelu:";
            // 
            // txtEntryId
            // 
            this.txtEntryId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEntryId.Location = new System.Drawing.Point(847, 14);
            this.txtEntryId.Name = "txtEntryId";
            this.txtEntryId.Size = new System.Drawing.Size(50, 20);
            this.txtEntryId.TabIndex = 15;
            // 
            // lvAwardResults
            // 
            this.lvAwardResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvAwardResults.Location = new System.Drawing.Point(0, 51);
            this.lvAwardResults.Name = "lvAwardResults";
            this.lvAwardResults.Size = new System.Drawing.Size(989, 459);
            this.lvAwardResults.TabIndex = 14;
            this.lvAwardResults.UseCompatibleStateImageBehavior = false;
            this.lvAwardResults.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvAwardResults_MouseDown);
            // 
            // cboSpecialAward
            // 
            this.cboSpecialAward.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboSpecialAward.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSpecialAward.FormattingEnabled = true;
            this.cboSpecialAward.Location = new System.Drawing.Point(65, 14);
            this.cboSpecialAward.Name = "cboSpecialAward";
            this.cboSpecialAward.Size = new System.Drawing.Size(711, 21);
            this.cboSpecialAward.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Nagroda:";
            // 
            // cmsAwardResultsRightClick
            // 
            this.cmsAwardResultsRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsARCDelete});
            this.cmsAwardResultsRightClick.Name = "cmsResultsRightClick";
            this.cmsAwardResultsRightClick.Size = new System.Drawing.Size(131, 26);
            // 
            // cmsARCDelete
            // 
            this.cmsARCDelete.Name = "cmsARCDelete";
            this.cmsARCDelete.Size = new System.Drawing.Size(130, 22);
            this.cmsARCDelete.Text = "Usuń Wpis";
            this.cmsARCDelete.Click += new System.EventHandler(this.cmsARCDelete_Click);
            // 
            // frmAddResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(995, 573);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAddResults";
            this.Text = "Dodawanie Wyników";
            this.Load += new System.EventHandler(this.frmAddResults_Load);
            this.cmsResultsRightClick.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.cmsAwardResultsRightClick.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboModelCategory;
        private System.Windows.Forms.ListView lvResults;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ContextMenuStrip cmsResultsRightClick;
        private System.Windows.Forms.ToolStripMenuItem mnuRCFirstPlace;
        private System.Windows.Forms.ToolStripMenuItem mnuRCSecondPlace;
        private System.Windows.Forms.ToolStripMenuItem mnuRCThirdPlace;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuRCClear;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ComboBox cboSpecialAward;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lvAwardResults;
        private System.Windows.Forms.Button btaAddAwardWinner;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtEntryId;
        private System.Windows.Forms.ContextMenuStrip cmsAwardResultsRightClick;
        private System.Windows.Forms.ToolStripMenuItem cmsARCDelete;
    }
}