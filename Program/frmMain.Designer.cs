namespace Rejestracja
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFNewDataFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFImport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFExport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFOpenFileFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFFDataFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFFDocumentFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFFTemplateFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRegistration = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRNewRegistration = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuRView = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRVStandard = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRVGroupped = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuRPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRPrintSorted = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuJudging = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuJMergeCategories = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuJJudgingForms = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuJAddResults = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuResults = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRResultList = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRCategoryDiplomas = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRAwardDiplomas = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsEntryRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuRCModifyRegistration = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRCDeleteRegistration = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuRCPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuRCCheckAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRCUncheckAll = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.toolStripLabelSpring = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.cmsResultsRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsRCDeleteResult = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRCPrintDiploma = new System.Windows.Forms.ToolStripMenuItem();
            this.tpStats = new System.Windows.Forms.TabPage();
            this.btnRefreshStats = new System.Windows.Forms.Button();
            this.lvStats = new System.Windows.Forms.ListView();
            this.tpResults = new System.Windows.Forms.TabPage();
            this.btnAddResults = new System.Windows.Forms.Button();
            this.lvResults = new System.Windows.Forms.ListView();
            this.tpRegistration = new System.Windows.Forms.TabPage();
            this.btnClearSearch = new System.Windows.Forms.Button();
            this.lblErrorCount = new System.Windows.Forms.Label();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnFilter = new System.Windows.Forms.Button();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.lvEntries = new System.Windows.Forms.ListView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.menuStrip1.SuspendLayout();
            this.cmsEntryRightClick.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.cmsResultsRightClick.SuspendLayout();
            this.tpStats.SuspendLayout();
            this.tpResults.SuspendLayout();
            this.tpRegistration.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuRegistration,
            this.mnuJudging,
            this.mnuResults});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1182, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFNewDataFile,
            this.toolStripSeparator6,
            this.mnuFImport,
            this.mnuFExport,
            this.toolStripSeparator1,
            this.mnuFSettings,
            this.mnuFOpenFileFolder,
            this.toolStripSeparator4,
            this.exitToolStripMenuItem});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(65, 20);
            this.mnuFile.Text = "Program";
            // 
            // mnuFNewDataFile
            // 
            this.mnuFNewDataFile.Name = "mnuFNewDataFile";
            this.mnuFNewDataFile.Size = new System.Drawing.Size(156, 22);
            this.mnuFNewDataFile.Text = "Nowy Plik...";
            this.mnuFNewDataFile.Click += new System.EventHandler(this.mnuFNewDataFile_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(153, 6);
            // 
            // mnuFImport
            // 
            this.mnuFImport.Name = "mnuFImport";
            this.mnuFImport.Size = new System.Drawing.Size(156, 22);
            this.mnuFImport.Text = "&Import Danych";
            this.mnuFImport.Click += new System.EventHandler(this.mnuFImport_Click);
            // 
            // mnuFExport
            // 
            this.mnuFExport.Name = "mnuFExport";
            this.mnuFExport.Size = new System.Drawing.Size(156, 22);
            this.mnuFExport.Text = "&Eksport Danych";
            this.mnuFExport.Click += new System.EventHandler(this.mnuFExport_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(153, 6);
            // 
            // mnuFSettings
            // 
            this.mnuFSettings.Name = "mnuFSettings";
            this.mnuFSettings.Size = new System.Drawing.Size(156, 22);
            this.mnuFSettings.Text = "&Ustawienia";
            this.mnuFSettings.Click += new System.EventHandler(this.mnuFSettings_Click);
            // 
            // mnuFOpenFileFolder
            // 
            this.mnuFOpenFileFolder.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFFDataFolder,
            this.mnuFFDocumentFolder,
            this.mnuFFTemplateFolder});
            this.mnuFOpenFileFolder.Name = "mnuFOpenFileFolder";
            this.mnuFOpenFileFolder.Size = new System.Drawing.Size(156, 22);
            this.mnuFOpenFileFolder.Text = "Foldery";
            // 
            // mnuFFDataFolder
            // 
            this.mnuFFDataFolder.Name = "mnuFFDataFolder";
            this.mnuFFDataFolder.Size = new System.Drawing.Size(182, 22);
            this.mnuFFDataFolder.Text = "Folder Danych";
            this.mnuFFDataFolder.Click += new System.EventHandler(this.mnuFFDataFolder_Click);
            // 
            // mnuFFDocumentFolder
            // 
            this.mnuFFDocumentFolder.Name = "mnuFFDocumentFolder";
            this.mnuFFDocumentFolder.Size = new System.Drawing.Size(182, 22);
            this.mnuFFDocumentFolder.Text = "Folder Dokumentów";
            this.mnuFFDocumentFolder.Click += new System.EventHandler(this.mnuFFDocumentFolder_Click);
            // 
            // mnuFFTemplateFolder
            // 
            this.mnuFFTemplateFolder.Name = "mnuFFTemplateFolder";
            this.mnuFFTemplateFolder.Size = new System.Drawing.Size(182, 22);
            this.mnuFFTemplateFolder.Text = "Folder Wzorców";
            this.mnuFFTemplateFolder.Click += new System.EventHandler(this.mnuFFTemplateFolder_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(153, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.exitToolStripMenuItem.Text = "Zakoń&cz";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // mnuRegistration
            // 
            this.mnuRegistration.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRNewRegistration,
            this.toolStripSeparator7,
            this.mnuRView,
            this.toolStripSeparator5,
            this.mnuRPrint,
            this.mnuRPrintSorted});
            this.mnuRegistration.Name = "mnuRegistration";
            this.mnuRegistration.Size = new System.Drawing.Size(75, 20);
            this.mnuRegistration.Text = "Rejestracja";
            // 
            // mnuRNewRegistration
            // 
            this.mnuRNewRegistration.Name = "mnuRNewRegistration";
            this.mnuRNewRegistration.ShortcutKeyDisplayString = "";
            this.mnuRNewRegistration.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.mnuRNewRegistration.Size = new System.Drawing.Size(227, 22);
            this.mnuRNewRegistration.Text = "Nowa rejestracja...";
            this.mnuRNewRegistration.Click += new System.EventHandler(this.mnuRNewRegistration_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(224, 6);
            // 
            // mnuRView
            // 
            this.mnuRView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRVStandard,
            this.mnuRVGroupped});
            this.mnuRView.Name = "mnuRView";
            this.mnuRView.Size = new System.Drawing.Size(227, 22);
            this.mnuRView.Text = "Widok";
            // 
            // mnuRVStandard
            // 
            this.mnuRVStandard.Checked = true;
            this.mnuRVStandard.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnuRVStandard.Name = "mnuRVStandard";
            this.mnuRVStandard.Size = new System.Drawing.Size(143, 22);
            this.mnuRVStandard.Text = "Standardowy";
            this.mnuRVStandard.Click += new System.EventHandler(this.mnuRVItem_Click);
            // 
            // mnuRVGroupped
            // 
            this.mnuRVGroupped.Name = "mnuRVGroupped";
            this.mnuRVGroupped.Size = new System.Drawing.Size(143, 22);
            this.mnuRVGroupped.Text = "Grupowany";
            this.mnuRVGroupped.Click += new System.EventHandler(this.mnuRVItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(224, 6);
            // 
            // mnuRPrint
            // 
            this.mnuRPrint.Name = "mnuRPrint";
            this.mnuRPrint.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.mnuRPrint.Size = new System.Drawing.Size(227, 22);
            this.mnuRPrint.Text = "Drukuj karty startowe";
            this.mnuRPrint.Click += new System.EventHandler(this.mnuRPrint_Click);
            // 
            // mnuRPrintSorted
            // 
            this.mnuRPrintSorted.CheckOnClick = true;
            this.mnuRPrintSorted.Name = "mnuRPrintSorted";
            this.mnuRPrintSorted.Size = new System.Drawing.Size(227, 22);
            this.mnuRPrintSorted.Text = "Drukuj karty alfabetycznie";
            // 
            // mnuJudging
            // 
            this.mnuJudging.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuJMergeCategories,
            this.mnuJJudgingForms,
            this.mnuJAddResults});
            this.mnuJudging.Name = "mnuJudging";
            this.mnuJudging.Size = new System.Drawing.Size(84, 20);
            this.mnuJudging.Text = "Sędziowanie";
            // 
            // mnuJMergeCategories
            // 
            this.mnuJMergeCategories.Name = "mnuJMergeCategories";
            this.mnuJMergeCategories.Size = new System.Drawing.Size(185, 22);
            this.mnuJMergeCategories.Text = "Łączenie Kategorii";
            this.mnuJMergeCategories.Click += new System.EventHandler(this.mnuJMergeCategories_Click);
            // 
            // mnuJJudgingForms
            // 
            this.mnuJJudgingForms.Name = "mnuJJudgingForms";
            this.mnuJJudgingForms.Size = new System.Drawing.Size(185, 22);
            this.mnuJJudgingForms.Text = "Karty sędziowania";
            this.mnuJJudgingForms.Click += new System.EventHandler(this.mnuJJudgingForms_Click);
            // 
            // mnuJAddResults
            // 
            this.mnuJAddResults.Name = "mnuJAddResults";
            this.mnuJAddResults.Size = new System.Drawing.Size(185, 22);
            this.mnuJAddResults.Text = "Dodawanie Wyników";
            this.mnuJAddResults.Click += new System.EventHandler(this.mnuJAddResults_Click);
            // 
            // mnuResults
            // 
            this.mnuResults.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRResultList,
            this.mnuRCategoryDiplomas,
            this.mnuRAwardDiplomas});
            this.mnuResults.Name = "mnuResults";
            this.mnuResults.Size = new System.Drawing.Size(96, 20);
            this.mnuResults.Text = "Druk Wyników";
            // 
            // mnuRResultList
            // 
            this.mnuRResultList.Name = "mnuRResultList";
            this.mnuRResultList.Size = new System.Drawing.Size(183, 22);
            this.mnuRResultList.Text = "Lista Wyników";
            this.mnuRResultList.Click += new System.EventHandler(this.mnuRResultList_Click);
            // 
            // mnuRCategoryDiplomas
            // 
            this.mnuRCategoryDiplomas.Name = "mnuRCategoryDiplomas";
            this.mnuRCategoryDiplomas.Size = new System.Drawing.Size(183, 22);
            this.mnuRCategoryDiplomas.Text = "Dyplomy - Kategorie";
            this.mnuRCategoryDiplomas.Click += new System.EventHandler(this.mnuRCategoryDiplomas_Click);
            // 
            // mnuRAwardDiplomas
            // 
            this.mnuRAwardDiplomas.Name = "mnuRAwardDiplomas";
            this.mnuRAwardDiplomas.Size = new System.Drawing.Size(183, 22);
            this.mnuRAwardDiplomas.Text = "Dyplomy - Nagrody";
            this.mnuRAwardDiplomas.Click += new System.EventHandler(this.mnuRAwardDiplomas_Click);
            // 
            // cmsEntryRightClick
            // 
            this.cmsEntryRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRCModifyRegistration,
            this.mnuRCDeleteRegistration,
            this.toolStripSeparator2,
            this.mnuRCPrint,
            this.toolStripSeparator3,
            this.mnuRCCheckAll,
            this.mnuRCUncheckAll});
            this.cmsEntryRightClick.Name = "cmsRightClick";
            this.cmsEntryRightClick.Size = new System.Drawing.Size(205, 126);
            // 
            // mnuRCModifyRegistration
            // 
            this.mnuRCModifyRegistration.Name = "mnuRCModifyRegistration";
            this.mnuRCModifyRegistration.Size = new System.Drawing.Size(204, 22);
            this.mnuRCModifyRegistration.Text = "Zmień rejestrację";
            this.mnuRCModifyRegistration.Click += new System.EventHandler(this.mnuRCModifyRegistration_Click);
            // 
            // mnuRCDeleteRegistration
            // 
            this.mnuRCDeleteRegistration.Name = "mnuRCDeleteRegistration";
            this.mnuRCDeleteRegistration.Size = new System.Drawing.Size(204, 22);
            this.mnuRCDeleteRegistration.Text = "Usuń rejestrację";
            this.mnuRCDeleteRegistration.Click += new System.EventHandler(this.mnuRCDeleteRegistration_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(201, 6);
            // 
            // mnuRCPrint
            // 
            this.mnuRCPrint.Name = "mnuRCPrint";
            this.mnuRCPrint.Size = new System.Drawing.Size(204, 22);
            this.mnuRCPrint.Text = "Drukuj kartę startową";
            this.mnuRCPrint.Click += new System.EventHandler(this.mnuRCPrint_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(201, 6);
            // 
            // mnuRCCheckAll
            // 
            this.mnuRCCheckAll.Name = "mnuRCCheckAll";
            this.mnuRCCheckAll.Size = new System.Drawing.Size(204, 22);
            this.mnuRCCheckAll.Text = "Zaznacz wszystkie wpisy";
            this.mnuRCCheckAll.Click += new System.EventHandler(this.mnuRCCheckAll_Click);
            // 
            // mnuRCUncheckAll
            // 
            this.mnuRCUncheckAll.Name = "mnuRCUncheckAll";
            this.mnuRCUncheckAll.Size = new System.Drawing.Size(204, 22);
            this.mnuRCUncheckAll.Text = "Odznacz wszystkie wpisy";
            this.mnuRCUncheckAll.Click += new System.EventHandler(this.mnuRCUncheckAll_Click);
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelSpring,
            this.toolStripStatusLabel,
            this.toolStripProgressBar});
            this.statusBar.Location = new System.Drawing.Point(0, 676);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(1182, 22);
            this.statusBar.TabIndex = 3;
            this.statusBar.Text = "statusStrip1";
            // 
            // toolStripLabelSpring
            // 
            this.toolStripLabelSpring.Name = "toolStripLabelSpring";
            this.toolStripLabelSpring.Size = new System.Drawing.Size(1167, 17);
            this.toolStripLabelSpring.Spring = true;
            this.toolStripLabelSpring.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar.Visible = false;
            // 
            // cmsResultsRightClick
            // 
            this.cmsResultsRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsRCDeleteResult,
            this.mnuRCPrintDiploma});
            this.cmsResultsRightClick.Name = "cmsResultsRightClick";
            this.cmsResultsRightClick.Size = new System.Drawing.Size(155, 70);
            // 
            // cmsRCDeleteResult
            // 
            this.cmsRCDeleteResult.Name = "cmsRCDeleteResult";
            this.cmsRCDeleteResult.Size = new System.Drawing.Size(154, 22);
            this.cmsRCDeleteResult.Text = "Usuń Wpis";
            this.cmsRCDeleteResult.Click += new System.EventHandler(this.cmsRCDeleteResult_Click);
            // 
            // mnuRCPrintDiploma
            // 
            this.mnuRCPrintDiploma.Name = "mnuRCPrintDiploma";
            this.mnuRCPrintDiploma.Size = new System.Drawing.Size(154, 22);
            this.mnuRCPrintDiploma.Text = "Drukuj Dyplom";
            this.mnuRCPrintDiploma.Click += new System.EventHandler(this.mnuRCPrintDiploma_Click);
            // 
            // tpStats
            // 
            this.tpStats.Controls.Add(this.btnRefreshStats);
            this.tpStats.Controls.Add(this.lvStats);
            this.tpStats.Location = new System.Drawing.Point(4, 22);
            this.tpStats.Name = "tpStats";
            this.tpStats.Size = new System.Drawing.Size(1171, 649);
            this.tpStats.TabIndex = 3;
            this.tpStats.Text = "Podsumowanie";
            this.tpStats.UseVisualStyleBackColor = true;
            // 
            // btnRefreshStats
            // 
            this.btnRefreshStats.Location = new System.Drawing.Point(5, 5);
            this.btnRefreshStats.Name = "btnRefreshStats";
            this.btnRefreshStats.Size = new System.Drawing.Size(75, 25);
            this.btnRefreshStats.TabIndex = 1;
            this.btnRefreshStats.Text = "Odśwież";
            this.btnRefreshStats.UseVisualStyleBackColor = true;
            this.btnRefreshStats.Click += new System.EventHandler(this.btnRefreshStats_Click);
            // 
            // lvStats
            // 
            this.lvStats.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvStats.Location = new System.Drawing.Point(0, 35);
            this.lvStats.MultiSelect = false;
            this.lvStats.Name = "lvStats";
            this.lvStats.Size = new System.Drawing.Size(1171, 593);
            this.lvStats.TabIndex = 0;
            this.lvStats.UseCompatibleStateImageBehavior = false;
            // 
            // tpResults
            // 
            this.tpResults.Controls.Add(this.btnAddResults);
            this.tpResults.Controls.Add(this.lvResults);
            this.tpResults.Location = new System.Drawing.Point(4, 22);
            this.tpResults.Name = "tpResults";
            this.tpResults.Size = new System.Drawing.Size(1171, 649);
            this.tpResults.TabIndex = 2;
            this.tpResults.Text = "Wyniki";
            this.tpResults.UseVisualStyleBackColor = true;
            // 
            // btnAddResults
            // 
            this.btnAddResults.Location = new System.Drawing.Point(5, 5);
            this.btnAddResults.Name = "btnAddResults";
            this.btnAddResults.Size = new System.Drawing.Size(154, 25);
            this.btnAddResults.TabIndex = 20;
            this.btnAddResults.Text = "Dodaj Wyniki";
            this.btnAddResults.UseVisualStyleBackColor = true;
            this.btnAddResults.Click += new System.EventHandler(this.btnAddResults_Click);
            // 
            // lvResults
            // 
            this.lvResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvResults.Location = new System.Drawing.Point(0, 36);
            this.lvResults.Name = "lvResults";
            this.lvResults.Size = new System.Drawing.Size(1175, 592);
            this.lvResults.TabIndex = 19;
            this.lvResults.UseCompatibleStateImageBehavior = false;
            this.lvResults.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvResults_MouseDown);
            // 
            // tpRegistration
            // 
            this.tpRegistration.Controls.Add(this.btnClearSearch);
            this.tpRegistration.Controls.Add(this.lblErrorCount);
            this.tpRegistration.Controls.Add(this.btnRegister);
            this.tpRegistration.Controls.Add(this.btnFilter);
            this.tpRegistration.Controls.Add(this.txtFilter);
            this.tpRegistration.Controls.Add(this.lvEntries);
            this.tpRegistration.Location = new System.Drawing.Point(4, 22);
            this.tpRegistration.Name = "tpRegistration";
            this.tpRegistration.Padding = new System.Windows.Forms.Padding(3);
            this.tpRegistration.Size = new System.Drawing.Size(1171, 649);
            this.tpRegistration.TabIndex = 0;
            this.tpRegistration.Text = "Rejestracja";
            this.tpRegistration.UseVisualStyleBackColor = true;
            // 
            // btnClearSearch
            // 
            this.btnClearSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearSearch.Location = new System.Drawing.Point(1069, 5);
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Size = new System.Drawing.Size(92, 25);
            this.btnClearSearch.TabIndex = 8;
            this.btnClearSearch.TabStop = false;
            this.btnClearSearch.Text = "Wyczyść (Esc)";
            this.btnClearSearch.UseVisualStyleBackColor = true;
            this.btnClearSearch.Click += new System.EventHandler(this.btnClearSearch_Click);
            // 
            // lblErrorCount
            // 
            this.lblErrorCount.AutoSize = true;
            this.lblErrorCount.Location = new System.Drawing.Point(177, 11);
            this.lblErrorCount.Name = "lblErrorCount";
            this.lblErrorCount.Size = new System.Drawing.Size(67, 13);
            this.lblErrorCount.TabIndex = 7;
            this.lblErrorCount.Text = "lblErrorCount";
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(5, 5);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(154, 25);
            this.btnRegister.TabIndex = 0;
            this.btnRegister.Text = "Nowa Rejestracja (Ctrl+N)";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // btnFilter
            // 
            this.btnFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilter.Location = new System.Drawing.Point(978, 5);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(85, 25);
            this.btnFilter.TabIndex = 4;
            this.btnFilter.TabStop = false;
            this.btnFilter.Text = "Szukaj (Ctrl+F)";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // txtFilter
            // 
            this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilter.Location = new System.Drawing.Point(801, 8);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(171, 20);
            this.txtFilter.TabIndex = 3;
            this.txtFilter.TabStop = false;
            this.txtFilter.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtFilter_KeyUp);
            // 
            // lvEntries
            // 
            this.lvEntries.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
            this.lvEntries.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvEntries.FullRowSelect = true;
            this.lvEntries.HideSelection = false;
            this.lvEntries.Location = new System.Drawing.Point(0, 34);
            this.lvEntries.MultiSelect = false;
            this.lvEntries.Name = "lvEntries";
            this.lvEntries.Size = new System.Drawing.Size(1168, 594);
            this.lvEntries.TabIndex = 2;
            this.lvEntries.UseCompatibleStateImageBehavior = false;
            this.lvEntries.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEntries_ColumnClick);
            this.lvEntries.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvEntries_ItemChecked);
            this.lvEntries.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvEntries_KeyDown);
            this.lvEntries.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvEntries_MouseDoubleClick);
            this.lvEntries.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvEntries_MouseDown);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpRegistration);
            this.tabControl1.Controls.Add(this.tpResults);
            this.tabControl1.Controls.Add(this.tpStats);
            this.tabControl1.Location = new System.Drawing.Point(3, 23);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1179, 675);
            this.tabControl1.TabIndex = 1;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1182, 698);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "Konkurs";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.cmsEntryRightClick.ResumeLayout(false);
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.cmsResultsRightClick.ResumeLayout(false);
            this.tpStats.ResumeLayout(false);
            this.tpResults.ResumeLayout(false);
            this.tpRegistration.ResumeLayout(false);
            this.tpRegistration.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuFImport;
        private System.Windows.Forms.ToolStripMenuItem mnuFExport;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ContextMenuStrip cmsEntryRightClick;
        private System.Windows.Forms.ToolStripMenuItem mnuRCModifyRegistration;
        private System.Windows.Forms.ToolStripMenuItem mnuRCDeleteRegistration;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuRCPrint;
        private System.Windows.Forms.ToolStripMenuItem mnuJudging;
        private System.Windows.Forms.ToolStripMenuItem mnuJJudgingForms;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripLabelSpring;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mnuRCCheckAll;
        private System.Windows.Forms.ToolStripMenuItem mnuRCUncheckAll;
        private System.Windows.Forms.ToolStripMenuItem mnuFSettings;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ContextMenuStrip cmsResultsRightClick;
        private System.Windows.Forms.ToolStripMenuItem cmsRCDeleteResult;
        private System.Windows.Forms.ToolStripMenuItem mnuRegistration;
        private System.Windows.Forms.ToolStripMenuItem mnuRNewRegistration;
        private System.Windows.Forms.ToolStripMenuItem mnuRPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem mnuRPrintSorted;
        private System.Windows.Forms.ToolStripMenuItem mnuFNewDataFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem mnuFOpenFileFolder;
        private System.Windows.Forms.ToolStripMenuItem mnuFFDataFolder;
        private System.Windows.Forms.ToolStripMenuItem mnuFFDocumentFolder;
        private System.Windows.Forms.ToolStripMenuItem mnuFFTemplateFolder;
        private System.Windows.Forms.ToolStripMenuItem mnuRView;
        private System.Windows.Forms.ToolStripMenuItem mnuRVStandard;
        private System.Windows.Forms.ToolStripMenuItem mnuRVGroupped;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.TabPage tpStats;
        private System.Windows.Forms.Button btnRefreshStats;
        private System.Windows.Forms.ListView lvStats;
        private System.Windows.Forms.TabPage tpResults;
        private System.Windows.Forms.Button btnAddResults;
        private System.Windows.Forms.ListView lvResults;
        private System.Windows.Forms.TabPage tpRegistration;
        private System.Windows.Forms.Button btnClearSearch;
        private System.Windows.Forms.Label lblErrorCount;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.ListView lvEntries;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ToolStripMenuItem mnuResults;
        private System.Windows.Forms.ToolStripMenuItem mnuRResultList;
        private System.Windows.Forms.ToolStripMenuItem mnuRCategoryDiplomas;
        private System.Windows.Forms.ToolStripMenuItem mnuRAwardDiplomas;
        private System.Windows.Forms.ToolStripMenuItem mnuJMergeCategories;
        private System.Windows.Forms.ToolStripMenuItem mnuJAddResults;
        private System.Windows.Forms.ToolStripMenuItem mnuRCPrintDiploma;


    }
}

