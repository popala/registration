namespace Rejestracja
{
    partial class frmSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.tcOptions = new System.Windows.Forms.TabControl();
            this.tpDocuments = new System.Windows.Forms.TabPage();
            this.txtHeading = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tpAgeGroup = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.txtAge = new System.Windows.Forms.TextBox();
            this.btnAddAgeGroup = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAgeGroup = new System.Windows.Forms.TextBox();
            this.lvAgeGroup = new System.Windows.Forms.ListView();
            this.tpModelClass = new System.Windows.Forms.TabPage();
            this.btnAddModelClass = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtModelClassName = new System.Windows.Forms.TextBox();
            this.lvModelClass = new System.Windows.Forms.ListView();
            this.tpModelCategory = new System.Windows.Forms.TabPage();
            this.cboModelClass = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnAddModelCategory = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCategoryName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCategoryCode = new System.Windows.Forms.TextBox();
            this.btnMoveUpCategory = new System.Windows.Forms.Button();
            this.btnMoveDownCategory = new System.Windows.Forms.Button();
            this.lvModelCategory = new System.Windows.Forms.ListView();
            this.tpSpecialAwards = new System.Windows.Forms.TabPage();
            this.btnAddAward = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.txtAwardTitle = new System.Windows.Forms.TextBox();
            this.btnMoveUpAward = new System.Windows.Forms.Button();
            this.btnMoveDownAward = new System.Windows.Forms.Button();
            this.lvAwards = new System.Windows.Forms.ListView();
            this.tpPublisher = new System.Windows.Forms.TabPage();
            this.btnAddPublisher = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPublisherName = new System.Windows.Forms.TextBox();
            this.lvPublishers = new System.Windows.Forms.ListView();
            this.tpModelScale = new System.Windows.Forms.TabPage();
            this.btnAddModelScale = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.txtModelScale = new System.Windows.Forms.TextBox();
            this.lvModelScales = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.tcOptions.SuspendLayout();
            this.tpDocuments.SuspendLayout();
            this.tpAgeGroup.SuspendLayout();
            this.tpModelClass.SuspendLayout();
            this.tpModelCategory.SuspendLayout();
            this.tpSpecialAwards.SuspendLayout();
            this.tpPublisher.SuspendLayout();
            this.tpModelScale.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcOptions
            // 
            this.tcOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcOptions.Controls.Add(this.tpDocuments);
            this.tcOptions.Controls.Add(this.tpAgeGroup);
            this.tcOptions.Controls.Add(this.tpModelClass);
            this.tcOptions.Controls.Add(this.tpModelCategory);
            this.tcOptions.Controls.Add(this.tpSpecialAwards);
            this.tcOptions.Controls.Add(this.tpPublisher);
            this.tcOptions.Controls.Add(this.tpModelScale);
            this.tcOptions.Location = new System.Drawing.Point(0, 1);
            this.tcOptions.Name = "tcOptions";
            this.tcOptions.SelectedIndex = 0;
            this.tcOptions.Size = new System.Drawing.Size(666, 388);
            this.tcOptions.TabIndex = 1;
            this.tcOptions.SelectedIndexChanged += new System.EventHandler(this.tcOptions_SelectedIndexChanged);
            // 
            // tpDocuments
            // 
            this.tpDocuments.Controls.Add(this.txtHeading);
            this.tpDocuments.Controls.Add(this.label1);
            this.tpDocuments.Location = new System.Drawing.Point(4, 22);
            this.tpDocuments.Name = "tpDocuments";
            this.tpDocuments.Size = new System.Drawing.Size(658, 362);
            this.tpDocuments.TabIndex = 2;
            this.tpDocuments.Text = "Dokumenty";
            this.tpDocuments.UseVisualStyleBackColor = true;
            // 
            // txtHeading
            // 
            this.txtHeading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHeading.Location = new System.Drawing.Point(98, 21);
            this.txtHeading.MaxLength = 1024;
            this.txtHeading.Multiline = true;
            this.txtHeading.Name = "txtHeading";
            this.txtHeading.Size = new System.Drawing.Size(533, 126);
            this.txtHeading.TabIndex = 0;
            this.txtHeading.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nagłówek\r\ndokumentów:";
            // 
            // tpAgeGroup
            // 
            this.tpAgeGroup.Controls.Add(this.label6);
            this.tpAgeGroup.Controls.Add(this.txtAge);
            this.tpAgeGroup.Controls.Add(this.btnAddAgeGroup);
            this.tpAgeGroup.Controls.Add(this.label5);
            this.tpAgeGroup.Controls.Add(this.txtAgeGroup);
            this.tpAgeGroup.Controls.Add(this.lvAgeGroup);
            this.tpAgeGroup.Location = new System.Drawing.Point(4, 22);
            this.tpAgeGroup.Name = "tpAgeGroup";
            this.tpAgeGroup.Size = new System.Drawing.Size(658, 362);
            this.tpAgeGroup.TabIndex = 3;
            this.tpAgeGroup.Text = "Grupy Wiekowe";
            this.tpAgeGroup.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(299, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Do lat (włącznie):";
            // 
            // txtAge
            // 
            this.txtAge.Location = new System.Drawing.Point(395, 9);
            this.txtAge.MaxLength = 3;
            this.txtAge.Name = "txtAge";
            this.txtAge.Size = new System.Drawing.Size(64, 20);
            this.txtAge.TabIndex = 1;
            this.txtAge.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAge_KeyPress);
            // 
            // btnAddAgeGroup
            // 
            this.btnAddAgeGroup.Location = new System.Drawing.Point(465, 7);
            this.btnAddAgeGroup.Name = "btnAddAgeGroup";
            this.btnAddAgeGroup.Size = new System.Drawing.Size(71, 23);
            this.btnAddAgeGroup.TabIndex = 2;
            this.btnAddAgeGroup.Text = "Dodaj";
            this.btnAddAgeGroup.UseVisualStyleBackColor = true;
            this.btnAddAgeGroup.Click += new System.EventHandler(this.btnAddAgeGroup_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Nazwa:";
            // 
            // txtAgeGroup
            // 
            this.txtAgeGroup.Location = new System.Drawing.Point(67, 9);
            this.txtAgeGroup.Name = "txtAgeGroup";
            this.txtAgeGroup.Size = new System.Drawing.Size(220, 20);
            this.txtAgeGroup.TabIndex = 0;
            // 
            // lvAgeGroup
            // 
            this.lvAgeGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvAgeGroup.Location = new System.Drawing.Point(0, 36);
            this.lvAgeGroup.Name = "lvAgeGroup";
            this.lvAgeGroup.Size = new System.Drawing.Size(658, 326);
            this.lvAgeGroup.TabIndex = 3;
            this.lvAgeGroup.UseCompatibleStateImageBehavior = false;
            // 
            // tpModelClass
            // 
            this.tpModelClass.Controls.Add(this.btnAddModelClass);
            this.tpModelClass.Controls.Add(this.label8);
            this.tpModelClass.Controls.Add(this.txtModelClassName);
            this.tpModelClass.Controls.Add(this.lvModelClass);
            this.tpModelClass.Location = new System.Drawing.Point(4, 22);
            this.tpModelClass.Name = "tpModelClass";
            this.tpModelClass.Size = new System.Drawing.Size(658, 362);
            this.tpModelClass.TabIndex = 4;
            this.tpModelClass.Text = "Klasy";
            this.tpModelClass.UseVisualStyleBackColor = true;
            // 
            // btnAddModelClass
            // 
            this.btnAddModelClass.Location = new System.Drawing.Point(494, 8);
            this.btnAddModelClass.Name = "btnAddModelClass";
            this.btnAddModelClass.Size = new System.Drawing.Size(71, 23);
            this.btnAddModelClass.TabIndex = 12;
            this.btnAddModelClass.Text = "Dodaj";
            this.btnAddModelClass.UseVisualStyleBackColor = true;
            this.btnAddModelClass.Click += new System.EventHandler(this.btnAddModelClass_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Nazwa:";
            // 
            // txtModelClassName
            // 
            this.txtModelClassName.Location = new System.Drawing.Point(62, 10);
            this.txtModelClassName.Name = "txtModelClassName";
            this.txtModelClassName.Size = new System.Drawing.Size(426, 20);
            this.txtModelClassName.TabIndex = 11;
            // 
            // lvModelClass
            // 
            this.lvModelClass.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvModelClass.Location = new System.Drawing.Point(0, 41);
            this.lvModelClass.Name = "lvModelClass";
            this.lvModelClass.Size = new System.Drawing.Size(658, 320);
            this.lvModelClass.TabIndex = 13;
            this.lvModelClass.UseCompatibleStateImageBehavior = false;
            // 
            // tpModelCategory
            // 
            this.tpModelCategory.Controls.Add(this.cboModelClass);
            this.tpModelCategory.Controls.Add(this.label9);
            this.tpModelCategory.Controls.Add(this.btnAddModelCategory);
            this.tpModelCategory.Controls.Add(this.label3);
            this.tpModelCategory.Controls.Add(this.txtCategoryName);
            this.tpModelCategory.Controls.Add(this.label2);
            this.tpModelCategory.Controls.Add(this.txtCategoryCode);
            this.tpModelCategory.Controls.Add(this.btnMoveUpCategory);
            this.tpModelCategory.Controls.Add(this.btnMoveDownCategory);
            this.tpModelCategory.Controls.Add(this.lvModelCategory);
            this.tpModelCategory.Location = new System.Drawing.Point(4, 22);
            this.tpModelCategory.Name = "tpModelCategory";
            this.tpModelCategory.Padding = new System.Windows.Forms.Padding(3);
            this.tpModelCategory.Size = new System.Drawing.Size(658, 362);
            this.tpModelCategory.TabIndex = 0;
            this.tpModelCategory.Text = "Kategorie";
            this.tpModelCategory.UseVisualStyleBackColor = true;
            // 
            // cboModelClass
            // 
            this.cboModelClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModelClass.FormattingEnabled = true;
            this.cboModelClass.Location = new System.Drawing.Point(54, 41);
            this.cboModelClass.Name = "cboModelClass";
            this.cboModelClass.Size = new System.Drawing.Size(354, 21);
            this.cboModelClass.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 44);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Klasa:";
            // 
            // btnAddModelCategory
            // 
            this.btnAddModelCategory.Location = new System.Drawing.Point(574, 10);
            this.btnAddModelCategory.Name = "btnAddModelCategory";
            this.btnAddModelCategory.Size = new System.Drawing.Size(71, 23);
            this.btnAddModelCategory.TabIndex = 2;
            this.btnAddModelCategory.Text = "Dodaj";
            this.btnAddModelCategory.UseVisualStyleBackColor = true;
            this.btnAddModelCategory.Click += new System.EventHandler(this.btnAddCategory_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(151, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Nazwa:";
            // 
            // txtCategoryName
            // 
            this.txtCategoryName.Location = new System.Drawing.Point(200, 12);
            this.txtCategoryName.MaxLength = 256;
            this.txtCategoryName.Name = "txtCategoryName";
            this.txtCategoryName.Size = new System.Drawing.Size(368, 20);
            this.txtCategoryName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Kod:";
            // 
            // txtCategoryCode
            // 
            this.txtCategoryCode.Location = new System.Drawing.Point(41, 12);
            this.txtCategoryCode.MaxLength = 8;
            this.txtCategoryCode.Name = "txtCategoryCode";
            this.txtCategoryCode.Size = new System.Drawing.Size(86, 20);
            this.txtCategoryCode.TabIndex = 0;
            // 
            // btnMoveUpCategory
            // 
            this.btnMoveUpCategory.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnMoveUpCategory.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveUpCategory.Location = new System.Drawing.Point(615, 142);
            this.btnMoveUpCategory.Name = "btnMoveUpCategory";
            this.btnMoveUpCategory.Size = new System.Drawing.Size(34, 29);
            this.btnMoveUpCategory.TabIndex = 3;
            this.btnMoveUpCategory.TabStop = false;
            this.btnMoveUpCategory.Text = "∧";
            this.btnMoveUpCategory.UseVisualStyleBackColor = true;
            this.btnMoveUpCategory.Click += new System.EventHandler(this.btnMoveUpCategory_Click);
            // 
            // btnMoveDownCategory
            // 
            this.btnMoveDownCategory.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnMoveDownCategory.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveDownCategory.Location = new System.Drawing.Point(615, 177);
            this.btnMoveDownCategory.Name = "btnMoveDownCategory";
            this.btnMoveDownCategory.Size = new System.Drawing.Size(35, 29);
            this.btnMoveDownCategory.TabIndex = 2;
            this.btnMoveDownCategory.TabStop = false;
            this.btnMoveDownCategory.Text = "∨";
            this.btnMoveDownCategory.UseVisualStyleBackColor = true;
            this.btnMoveDownCategory.Click += new System.EventHandler(this.btnMoveDownCategory_Click);
            // 
            // lvModelCategory
            // 
            this.lvModelCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvModelCategory.Location = new System.Drawing.Point(0, 69);
            this.lvModelCategory.Name = "lvModelCategory";
            this.lvModelCategory.Size = new System.Drawing.Size(610, 293);
            this.lvModelCategory.TabIndex = 3;
            this.lvModelCategory.UseCompatibleStateImageBehavior = false;
            this.lvModelCategory.SelectedIndexChanged += new System.EventHandler(this.lvModelClass_SelectedIndexChanged);
            // 
            // tpSpecialAwards
            // 
            this.tpSpecialAwards.Controls.Add(this.btnAddAward);
            this.tpSpecialAwards.Controls.Add(this.label11);
            this.tpSpecialAwards.Controls.Add(this.txtAwardTitle);
            this.tpSpecialAwards.Controls.Add(this.btnMoveUpAward);
            this.tpSpecialAwards.Controls.Add(this.btnMoveDownAward);
            this.tpSpecialAwards.Controls.Add(this.lvAwards);
            this.tpSpecialAwards.Location = new System.Drawing.Point(4, 22);
            this.tpSpecialAwards.Name = "tpSpecialAwards";
            this.tpSpecialAwards.Size = new System.Drawing.Size(658, 362);
            this.tpSpecialAwards.TabIndex = 6;
            this.tpSpecialAwards.Text = "Nagrody";
            this.tpSpecialAwards.UseVisualStyleBackColor = true;
            // 
            // btnAddAward
            // 
            this.btnAddAward.Location = new System.Drawing.Point(574, 10);
            this.btnAddAward.Name = "btnAddAward";
            this.btnAddAward.Size = new System.Drawing.Size(71, 23);
            this.btnAddAward.TabIndex = 9;
            this.btnAddAward.Text = "Dodaj";
            this.btnAddAward.UseVisualStyleBackColor = true;
            this.btnAddAward.Click += new System.EventHandler(this.btnAddAward_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 13);
            this.label11.TabIndex = 13;
            this.label11.Text = "Nazwa:";
            // 
            // txtAwardTitle
            // 
            this.txtAwardTitle.Location = new System.Drawing.Point(55, 12);
            this.txtAwardTitle.MaxLength = 256;
            this.txtAwardTitle.Name = "txtAwardTitle";
            this.txtAwardTitle.Size = new System.Drawing.Size(513, 20);
            this.txtAwardTitle.TabIndex = 8;
            // 
            // btnMoveUpAward
            // 
            this.btnMoveUpAward.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnMoveUpAward.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveUpAward.Location = new System.Drawing.Point(615, 142);
            this.btnMoveUpAward.Name = "btnMoveUpAward";
            this.btnMoveUpAward.Size = new System.Drawing.Size(34, 29);
            this.btnMoveUpAward.TabIndex = 11;
            this.btnMoveUpAward.TabStop = false;
            this.btnMoveUpAward.Text = "∧";
            this.btnMoveUpAward.UseVisualStyleBackColor = true;
            this.btnMoveUpAward.Click += new System.EventHandler(this.btnMoveUpAward_Click);
            // 
            // btnMoveDownAward
            // 
            this.btnMoveDownAward.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnMoveDownAward.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveDownAward.Location = new System.Drawing.Point(615, 177);
            this.btnMoveDownAward.Name = "btnMoveDownAward";
            this.btnMoveDownAward.Size = new System.Drawing.Size(35, 29);
            this.btnMoveDownAward.TabIndex = 10;
            this.btnMoveDownAward.TabStop = false;
            this.btnMoveDownAward.Text = "∨";
            this.btnMoveDownAward.UseVisualStyleBackColor = true;
            this.btnMoveDownAward.Click += new System.EventHandler(this.btnMoveDownAward_Click);
            // 
            // lvAwards
            // 
            this.lvAwards.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvAwards.Location = new System.Drawing.Point(0, 39);
            this.lvAwards.Name = "lvAwards";
            this.lvAwards.Size = new System.Drawing.Size(610, 318);
            this.lvAwards.TabIndex = 12;
            this.lvAwards.UseCompatibleStateImageBehavior = false;
            // 
            // tpPublisher
            // 
            this.tpPublisher.Controls.Add(this.btnAddPublisher);
            this.tpPublisher.Controls.Add(this.label4);
            this.tpPublisher.Controls.Add(this.txtPublisherName);
            this.tpPublisher.Controls.Add(this.lvPublishers);
            this.tpPublisher.Location = new System.Drawing.Point(4, 22);
            this.tpPublisher.Name = "tpPublisher";
            this.tpPublisher.Padding = new System.Windows.Forms.Padding(3);
            this.tpPublisher.Size = new System.Drawing.Size(658, 362);
            this.tpPublisher.TabIndex = 1;
            this.tpPublisher.Text = "Wydawcy";
            this.tpPublisher.UseVisualStyleBackColor = true;
            // 
            // btnAddPublisher
            // 
            this.btnAddPublisher.Location = new System.Drawing.Point(494, 9);
            this.btnAddPublisher.Name = "btnAddPublisher";
            this.btnAddPublisher.Size = new System.Drawing.Size(71, 23);
            this.btnAddPublisher.TabIndex = 1;
            this.btnAddPublisher.Text = "Dodaj";
            this.btnAddPublisher.UseVisualStyleBackColor = true;
            this.btnAddPublisher.Click += new System.EventHandler(this.btnAddPublisher_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Nazwa:";
            // 
            // txtPublisherName
            // 
            this.txtPublisherName.Location = new System.Drawing.Point(62, 11);
            this.txtPublisherName.Name = "txtPublisherName";
            this.txtPublisherName.Size = new System.Drawing.Size(426, 20);
            this.txtPublisherName.TabIndex = 0;
            // 
            // lvPublishers
            // 
            this.lvPublishers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvPublishers.Location = new System.Drawing.Point(0, 42);
            this.lvPublishers.Name = "lvPublishers";
            this.lvPublishers.Size = new System.Drawing.Size(658, 320);
            this.lvPublishers.TabIndex = 4;
            this.lvPublishers.UseCompatibleStateImageBehavior = false;
            // 
            // tpModelScale
            // 
            this.tpModelScale.Controls.Add(this.btnAddModelScale);
            this.tpModelScale.Controls.Add(this.label10);
            this.tpModelScale.Controls.Add(this.txtModelScale);
            this.tpModelScale.Controls.Add(this.lvModelScales);
            this.tpModelScale.Location = new System.Drawing.Point(4, 22);
            this.tpModelScale.Name = "tpModelScale";
            this.tpModelScale.Size = new System.Drawing.Size(658, 362);
            this.tpModelScale.TabIndex = 5;
            this.tpModelScale.Text = "Skale";
            this.tpModelScale.UseVisualStyleBackColor = true;
            // 
            // btnAddModelScale
            // 
            this.btnAddModelScale.Location = new System.Drawing.Point(494, 5);
            this.btnAddModelScale.Name = "btnAddModelScale";
            this.btnAddModelScale.Size = new System.Drawing.Size(71, 23);
            this.btnAddModelScale.TabIndex = 12;
            this.btnAddModelScale.Text = "Dodaj";
            this.btnAddModelScale.UseVisualStyleBackColor = true;
            this.btnAddModelScale.Click += new System.EventHandler(this.btnAddModelScale_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 13);
            this.label10.TabIndex = 14;
            this.label10.Text = "Nazwa:";
            // 
            // txtModelScale
            // 
            this.txtModelScale.Location = new System.Drawing.Point(62, 7);
            this.txtModelScale.Name = "txtModelScale";
            this.txtModelScale.Size = new System.Drawing.Size(426, 20);
            this.txtModelScale.TabIndex = 11;
            // 
            // lvModelScales
            // 
            this.lvModelScales.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvModelScales.Location = new System.Drawing.Point(0, 38);
            this.lvModelScales.Name = "lvModelScales";
            this.lvModelScales.Size = new System.Drawing.Size(658, 320);
            this.lvModelScales.TabIndex = 13;
            this.lvModelScales.UseCompatibleStateImageBehavior = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point(578, 391);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Zamknij";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(12, 391);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(106, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Usuń Wybrane";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 418);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tcOptions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSettings";
            this.Text = "Ustawienia";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.tcOptions.ResumeLayout(false);
            this.tpDocuments.ResumeLayout(false);
            this.tpDocuments.PerformLayout();
            this.tpAgeGroup.ResumeLayout(false);
            this.tpAgeGroup.PerformLayout();
            this.tpModelClass.ResumeLayout(false);
            this.tpModelClass.PerformLayout();
            this.tpModelCategory.ResumeLayout(false);
            this.tpModelCategory.PerformLayout();
            this.tpSpecialAwards.ResumeLayout(false);
            this.tpSpecialAwards.PerformLayout();
            this.tpPublisher.ResumeLayout(false);
            this.tpPublisher.PerformLayout();
            this.tpModelScale.ResumeLayout(false);
            this.tpModelScale.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcOptions;
        private System.Windows.Forms.TabPage tpDocuments;
        private System.Windows.Forms.TabPage tpModelCategory;
        private System.Windows.Forms.TabPage tpPublisher;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.TextBox txtHeading;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lvModelCategory;
        private System.Windows.Forms.Button btnMoveDownCategory;
        private System.Windows.Forms.Button btnMoveUpCategory;
        private System.Windows.Forms.ListView lvPublishers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCategoryCode;
        private System.Windows.Forms.Button btnAddModelCategory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCategoryName;
        private System.Windows.Forms.Button btnAddPublisher;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPublisherName;
        private System.Windows.Forms.TabPage tpAgeGroup;
        private System.Windows.Forms.Button btnAddAgeGroup;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAgeGroup;
        private System.Windows.Forms.ListView lvAgeGroup;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtAge;
        private System.Windows.Forms.TabPage tpModelClass;
        private System.Windows.Forms.TabPage tpModelScale;
        private System.Windows.Forms.Button btnAddModelClass;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtModelClassName;
        private System.Windows.Forms.ListView lvModelClass;
        private System.Windows.Forms.ComboBox cboModelClass;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnAddModelScale;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtModelScale;
        private System.Windows.Forms.ListView lvModelScales;
        private System.Windows.Forms.TabPage tpSpecialAwards;
        private System.Windows.Forms.Button btnAddAward;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtAwardTitle;
        private System.Windows.Forms.Button btnMoveUpAward;
        private System.Windows.Forms.Button btnMoveDownAward;
        private System.Windows.Forms.ListView lvAwards;
    }
}