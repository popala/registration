namespace Rejestracja {
    partial class frmMergeCategory {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMergeCategory));
            this.label1 = new System.Windows.Forms.Label();
            this.cboMaxCount = new System.Windows.Forms.ComboBox();
            this.lvCategories = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnMerge = new System.Windows.Forms.Button();
            this.lblErrors = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pokaż kategorie z sumą rejestracji poniżej:";
            // 
            // cboMaxCount
            // 
            this.cboMaxCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMaxCount.FormattingEnabled = true;
            this.cboMaxCount.Location = new System.Drawing.Point(226, 13);
            this.cboMaxCount.Name = "cboMaxCount";
            this.cboMaxCount.Size = new System.Drawing.Size(61, 21);
            this.cboMaxCount.TabIndex = 1;
            this.cboMaxCount.SelectedIndexChanged += new System.EventHandler(this.cboMaxCount_SelectedIndexChanged);
            // 
            // lvCategories
            // 
            this.lvCategories.Location = new System.Drawing.Point(0, 47);
            this.lvCategories.Name = "lvCategories";
            this.lvCategories.Size = new System.Drawing.Size(869, 446);
            this.lvCategories.TabIndex = 4;
            this.lvCategories.UseCompatibleStateImageBehavior = false;
            this.lvCategories.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvCategories_ItemChecked);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(782, 507);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Zamknij";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnMerge
            // 
            this.btnMerge.Location = new System.Drawing.Point(662, 507);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(114, 23);
            this.btnMerge.TabIndex = 6;
            this.btnMerge.Text = "Połącz Wybrane";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // lblErrors
            // 
            this.lblErrors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblErrors.ForeColor = System.Drawing.Color.Red;
            this.lblErrors.Location = new System.Drawing.Point(14, 512);
            this.lblErrors.Name = "lblErrors";
            this.lblErrors.Size = new System.Drawing.Size(416, 18);
            this.lblErrors.TabIndex = 7;
            this.lblErrors.Text = "Połączenie kategorii spowoduje usunięcie wyników w zlikwidowanych kategoriach";
            this.lblErrors.Visible = false;
            // 
            // frmMergeCategory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(869, 542);
            this.Controls.Add(this.lblErrors);
            this.Controls.Add(this.btnMerge);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvCategories);
            this.Controls.Add(this.cboMaxCount);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMergeCategory";
            this.Text = "frmMergeCategory";
            this.Load += new System.EventHandler(this.frmMergeCategory_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboMaxCount;
        private System.Windows.Forms.ListView lvCategories;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.Label lblErrors;
    }
}