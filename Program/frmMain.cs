﻿/*
 * Copyright (C) 2016 Paweł Opała https://github.com/popala/registration
 *
 * This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License 
 * as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with this program.  If not, see http://www.gnu.org/licenses/.
 */
using Rejestracja.Data.Dao;
using Rejestracja.Data.Objects;
using Rejestracja.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rejestracja {
    public partial class frmMain : Form {
        private Timer _txtFilterTimer;
        private int _registrationSortColumn = 0;
        private bool _registrationSortAscending = true;
        private bool _showSettingsForm = false;
        private ListViewItem _selectedItem = null;
        private bool _refreshList = false;

        public void changeCategoryInSelected(int categoryId) {
            Category category = CategoryDao.get(categoryId);
            if (category == null) {
                return;
            }

            foreach(ListViewItem item in lvEntries.Items) {
                if (item.Checked) {
                    RegistrationEntryDao.changeCategory((int)item.Tag, category.id);
                }
            }

            this._refreshList = true;
        }

        public void setShowSettingsForm(bool value) {
            this._showSettingsForm = value;
        }

        public frmMain() {
            InitializeComponent();
            //lvEntries.DoubleClickDoesCheck = false;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e) {
            //nothing to do
        }

        private void frmMain_Resize(object sender, EventArgs e) {
            if (this.WindowState != FormWindowState.Maximized) {
                String size = string.Format("{0},{1},{2},{3}", this.Left, this.Top, this.Width, this.Height);
                Options.set("frmMainSize", size);
            }
        }

        private void frmMain_Shown(object sender, EventArgs e) {
            //nothing
        }

        private void devTasks() {
            //Properties.Settings.Default.Reset();
            //ModelCategory.createTable();
            //ModelScale.createTable();
            //AgeGroup.createTable();
            //ModelClass.createTable();
        }

        private void resizeScreen(int left, int top, int width, int height) {

            Rectangle screenArea = Screen.FromControl(this).WorkingArea;

            this.Left = (left < 0 || left > screenArea.Width) ? 0 : left;
            this.Top = (top < 0 || top > screenArea.Height) ? 0 : top;
            this.Width = (width < 700 || width > screenArea.Width) ? 700 : width;
            this.Height = (height < 400 || height > screenArea.Height) ? 400 : height;
        }

        private void frmMain_Load(object sender, EventArgs e) {

            bool bHasValidFile = false;

            //devTasks();
            _txtFilterTimer = new Timer();
            _txtFilterTimer.Interval = 1000;
            _txtFilterTimer.Tick += new EventHandler(this.txtFilterTimer_Tick);

            //Ensure that a data file exists
            String dataFile = Properties.Settings.Default.DataFile;
            bHasValidFile = !String.IsNullOrWhiteSpace(dataFile) && File.Exists(dataFile);

            initUi(bHasValidFile);

            if (!bHasValidFile) {
                uiEnabled(false);
                frmNewDataFile f = new frmNewDataFile();
                f.StartPosition = FormStartPosition.CenterScreen;
                f.ShowDialog(this);
                if (_showSettingsForm) {
                    frmSettings fs = new frmSettings(); 
                    fs.StartPosition = FormStartPosition.CenterScreen;
                    fs.ShowDialog(this);
                    setViewMenus(!Options.get("RegistrationView").Equals("groupped"));
                    refreshScreen();
                    uiEnabled(true);
                    this._showSettingsForm = false;
                }
                return;
            }
            else {
                String size = Options.get("frmMainSize");
                if (size != null) {
                    String[] pos = size.Split(',');
                    resizeScreen(int.Parse(pos[0]), int.Parse(pos[1]), int.Parse(pos[2]), int.Parse(pos[3]));
                }
                if (Options.get("RegistrationView") == null) {
                    Options.set("RegistrationView", "groupped");
                }
                setViewMenus(!Options.get("RegistrationView").Equals("groupped"));
                if (Options.get("ColumnWidth") == null) {
                    Options.set("ColumnWidth", "auto");
                }
                mnuRVAutoWidth.Checked = Options.get("ColumnWidth").Equals("auto");
            }

            try {
                //DATA FILE
                DataSource ds = new DataSource();
                refreshScreen();
                uiEnabled(true);
            }
            catch (Exception err) {
                uiEnabled(false);
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Błąd Aplikacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void setViewMenus(bool standard) {
            mnuRVStandard.Checked = standard;
            tsBVStandard.Checked = standard;
            mnuRVGroupped.Checked = !standard;
            tsBVGroupped.Checked = !standard;
        }

        public void uiEnabled(bool isEnabled) {
            tabControl1.Enabled = isEnabled;
            mnuRegistration.Enabled = isEnabled;
            mnuJudging.Enabled = isEnabled;
            mnuFImport.Enabled = isEnabled;
            mnuFExport.Enabled = isEnabled;
            mnuResults.Enabled = isEnabled;
            toolStrip1.Enabled = isEnabled;
        }

        private void initUi(bool hasValidFile) {
            String[] headers;

            tsBtnErrorCount.Visible = false;
            tsErrorSeparator.Visible = false;

            setPrintOrderOption(true);

            mnuRSelected.Enabled = false;
            tsBtnPrintSelected.Enabled = false;
            tsBtnChangeCategory.Enabled = false;
            tsBtnDeleteSelected.Enabled = false;
            tsBtnClearFilter.Visible = false;

            //REGISTRATION PANEL
            lvEntries.View = View.Details;
            lvEntries.GridLines = true;
            lvEntries.FullRowSelect = true;
            lvEntries.HeaderStyle = ColumnHeaderStyle.Clickable;
            lvEntries.CheckBoxes = true;
            lvEntries.MultiSelect = false;
            lvEntries.ShowItemToolTips = true;

            lvEntries.Columns.Clear();
            headers = new String[] { "Nr Mod", "Data Rej.", "Email", "Imię", "Nazwisko", "Rok Ur.", "Klub", "GW", "Klasa", "KK", "Kategoria", "Nazwa Modelu", "Skala", "Wydawnictwo" };
            foreach (String header in headers) {
                lvEntries.Columns.Add(header.Trim());
            }

            if (hasValidFile) {
                _registrationSortColumn = int.Parse(Options.get("RegistrationSortColumn"));
                _registrationSortAscending = !Options.get("RegistrationSortOrder").Equals("1");
            }
            else {
                _registrationSortColumn = 0;
                _registrationSortAscending = true;
            }

            //RESULT ENTRY PANEL
            lvResults.View = View.Details;
            lvResults.GridLines = true;
            lvResults.FullRowSelect = true;
            lvResults.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvResults.CheckBoxes = false;

            lvResults.Columns.Clear();
            headers = new String[] { "Id", "Grupa Wiekowa", "Klasa", "Kategoria / Nagroda", "Nr Modelu", "Model", "Miejsce" };
            foreach (String header in headers) {
                lvResults.Columns.Add(header.Trim());
            }

            //STATISTICAL SUMMARY PANEL
            lvStats.View = View.Details;
            lvStats.GridLines = true;
            lvStats.FullRowSelect = true;
            lvStats.HeaderStyle = ColumnHeaderStyle.Nonclickable;

            lvStats.Columns.Clear();
            lvStats.Columns.Add("");
            lvStats.Columns.Add("Nazwa");
            lvStats.Columns.Add("Wartość");
            lvStats.Columns[1].TextAlign = HorizontalAlignment.Right;
        }

        private void loadResultList() {
            try {
                IEnumerable<string[]> results;

                lvResults.BeginUpdate();

                lvResults.Items.Clear();
                lvResults.Groups.Clear();

                ListViewGroup group = new ListViewGroup();
                String ageGroup = "";

                results = ResultDao.getCategoryResultList();

                foreach (string[] result in results) {
                    if (!ageGroup.Equals(result[1])) {
                        ageGroup = result[1];
                        group = new ListViewGroup("Wyniki w Kategoriach - " + ageGroup.ToUpper());
                        lvResults.Groups.Add(group);
                    }
                    lvResults.Items.Add(new ListViewItem(result, group));
                }

                System.Drawing.Color color = System.Drawing.Color.White;

                for (int i = 1; i < lvResults.Items.Count; i++) {
                    ListViewItem prev = lvResults.Items[i - 1];
                    ListViewItem cur = lvResults.Items[i];
                    if (cur.SubItems[1].Text.ToLower() != prev.SubItems[1].Text.ToLower() ||
                        cur.SubItems[2].Text.ToLower() != prev.SubItems[2].Text.ToLower() ||
                        cur.SubItems[3].Text.ToLower() != prev.SubItems[3].Text.ToLower()) {
                            color = (color == System.Drawing.Color.AliceBlue ? System.Drawing.Color.White : System.Drawing.Color.AliceBlue);
                    }
                    cur.BackColor = color;
                }

                group = new ListViewGroup("Nagrody Specjalne");
                lvResults.Groups.Add(group);

                results = ResultDao.getAwardResultList();
                foreach (String[] result in results) {
                    lvResults.Items.Add(new ListViewItem(result, group));
                }

                //Adjust the column width after adding results
                foreach (ColumnHeader header in lvResults.Columns) {
                    header.Width = -2;
                }
                lvResults.Columns[0].Width = 0;
            }
            catch (Exception err) {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Błąd Aplikacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally 
            {
                lvResults.EndUpdate();
            }
        }

        private void loadRegistrationList() {

            Application.UseWaitCursor = true;
            lvEntries.BeginUpdate();

            lvEntries.Items.Clear();
            lvEntries.Groups.Clear();

            if (mnuRVStandard.Checked) {
                loadSortedRegistrationList(tsTxtSearch.Text);
            }
            else {
                loadGrouppedRegistrationList(tsTxtSearch.Text);
            }

            if (mnuRVAutoWidth.Checked) {
                foreach (ColumnHeader header in lvEntries.Columns) {
                    header.Width = -2;
                }
            }

            highlightInvalidRegistrationEntries();

            lvEntries.EndUpdate();
            Application.UseWaitCursor = false;
        }

        private void loadSortedRegistrationList(String searchValue) {

            try {
                List<string[]> entries;

                if (String.IsNullOrWhiteSpace(searchValue)) {
                    entries = RegistrationEntryDao.getList(null, _registrationSortColumn, _registrationSortAscending).ToList();
                }
                else {
                    entries = RegistrationEntryDao.getList(searchValue, _registrationSortColumn, _registrationSortAscending).ToList();
                }

                foreach (string[] entry in entries) {
                    ListViewItem item = new ListViewItem(entry);
                    item.Tag = int.Parse(entry[0]);
                    lvEntries.Items.Add(item);
                }
            }
            catch (Exception err) {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Błąd Aplikacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadGrouppedRegistrationList(String searchValue) {
            try {
                List<string[]> entries;

                if (String.IsNullOrWhiteSpace(searchValue)) {
                    entries = RegistrationEntryDao.getGrouppedList().ToList();
                }
                else {
                    entries = RegistrationEntryDao.getGrouppedList(searchValue).ToList();
                }

                String categoryName = "";
                ListViewGroup group = new ListViewGroup("");

                foreach (string[] entry in entries) {
                    if (!categoryName.Equals(entry[10], StringComparison.CurrentCultureIgnoreCase)) {
                        categoryName = entry[10];
                        group = new ListViewGroup(String.Format("{0} - {1} ({2})", entry[8], entry[10], entry[9]));
                        lvEntries.Groups.Add(group);
                    }
                    ListViewItem item = new ListViewItem(entry, group);
                    item.Tag = int.Parse(entry[0]);
                    lvEntries.Items.Add(item);
                }
            }
            catch (Exception err) {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Błąd Aplikacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void editRegistrationItem(ListViewItem item) {
            int modelId = (int)item.Tag;
            bool showOnlyInvalid = (tsBtnErrorCount.Visible && tsBtnErrorCount.Checked);

            frmRegistrationEntry f = new frmRegistrationEntry();
            f.StartPosition = FormStartPosition.CenterParent;
            f.loadRegistration(modelId);
            f.ShowDialog(this);

            try {
                refreshScreen();
                if(showOnlyInvalid && tsBtnErrorCount.Visible) {
                    tsBtnErrorCount.Checked = true;
                    tsBtnErrorCount_Click(tsBtnErrorCount, new EventArgs());
                }
                else {
                    tsBtnClearFilter_Click(tsBtnErrorCount, new EventArgs());
                }

                foreach(ListViewItem it in lvEntries.Items) {
                    if((int)it.Tag == modelId) {
                        it.EnsureVisible();
                        it.Selected = true;
                        break;
                    }
                }
            }
            catch(Exception err) {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Błąd Aplikacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void highlightErrorCell(ListViewItem item, int subitem) {

            Color color = System.Drawing.Color.Red;

            item.UseItemStyleForSubItems = false;
            item.SubItems[0].ForeColor = color;
            item.SubItems[0].Font = new Font(item.Font, FontStyle.Bold);

            item.SubItems[subitem].ForeColor = color;
            item.SubItems[subitem].Font = new Font(item.Font, FontStyle.Bold);
        }

        private void highlightInvalidRegistrationEntries() {
            //TODO: validate using Classes!
            List<Category> modelCategories = CategoryDao.getList().ToList();
            List<AgeGroup> ageGroups = AgeGroupDao.getList(-1);
            
            int badEntryCount = 0;
            int year = DateTime.Now.Year;
            bool checkAgeGroupAge = (Options.get("ValidateAgeGroup") == null || !Options.get("ValidateAgeGroup").ToLower().Equals("false"));

            foreach (ListViewItem item in lvEntries.Items) {

                StringBuilder sb = new StringBuilder();
                item.ToolTipText = "";

                //Check if model category is listed in the resources
                Category [] catFound = modelCategories.Where(
                    x => x.name.Equals(item.SubItems[10].Text, StringComparison.CurrentCultureIgnoreCase) &&
                         x.code.Equals(item.SubItems[9].Text, StringComparison.CurrentCultureIgnoreCase)
                    ).ToArray();
                if (catFound.Length == 0) {
                    sb.Append("Kategoria modelu nie znaleziona w konfiguracji. ");
                    badEntryCount++;
                    highlightErrorCell(item, 10);
                }
                else if (!catFound[0].className.Equals(item.SubItems[8].Text, StringComparison.CurrentCultureIgnoreCase)) {
                    if (sb.Length == 0) {
                        badEntryCount++;
                    }
                    sb.Append("Kategoria i klasa modelu nie zgadzają się. ");
                    highlightErrorCell(item, 9);
                    highlightErrorCell(item, 10);
                }

                AgeGroup [] agFound = ageGroups.Where(x => x.name.Equals(item.SubItems[7].Text, StringComparison.CurrentCultureIgnoreCase)).ToArray();
                if(agFound.Length == 0) {
                    if (sb.Length == 0) {
                        badEntryCount++;
                    }
                    sb.Append("Grupa wiekowa nie znaleziona. ");
                    highlightErrorCell(item, 7);
                }

                if (checkAgeGroupAge) {
                    int age = year - int.Parse(item.SubItems[5].Text);
                    if (age > agFound[0].upperAge || age < agFound[0].bottomAge) {
                        if (sb.Length == 0) {
                            badEntryCount++;
                        }
                        sb.Append("Wiek modelarza wykracza poza wybraną grupę wiekową. ");
                        highlightErrorCell(item, 5);
                        highlightErrorCell(item, 7);
                    }
                }

                if (sb.Length > 0) {
                    item.ToolTipText = sb.ToString();
                }
            }

            if (badEntryCount > 0) {
                tsBtnErrorCount.Text = string.Format("Znaleziono błędy w {0} {1}", badEntryCount, (badEntryCount == 1 ? "rejestracji" : "rejestracjach"));
                tsBtnErrorCount.Visible = true;
                tsErrorSeparator.Visible = true;
            }
            else {
                tsBtnErrorCount.Visible = false;
                tsErrorSeparator.Visible = false;
            }
        }

        private void tsTxtSearch_KeyUp(object sender, KeyEventArgs e) {

            _txtFilterTimer.Stop();

            if (e.Alt || e.Control || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.Tab) {
                return;
            }

            switch (e.KeyCode) {
                case Keys.Enter:
                    refreshScreen();
                    break;

                default:
                    _txtFilterTimer.Start();
                    break;
            }
        }

        private void lvEntries_MouseDoubleClick(object sender, MouseEventArgs e) {

            if (lvEntries.Items.Count == 0) {
                return;
            }

            ListViewHitTestInfo hitTest = this.lvEntries.HitTest(e.X, e.Y);
            if (hitTest.Item != null) {
                ListViewItem item = hitTest.Item;
                int index = item.Index;
                editRegistrationItem(item);
            }
        }

        private void lvEntries_MouseUp(object sender, MouseEventArgs e) {
            if(e.Button == System.Windows.Forms.MouseButtons.Right) {

                ListViewHitTestInfo hitTest = this.lvEntries.HitTest(e.X, e.Y);

                if(hitTest.Location == ListViewHitTestLocations.StateImage) {
                    return;
                }

                if(hitTest.Item != null) {
                    hitTest.Item.Selected = true;
                    mnuRCDeleteRegistration.Enabled = true;
                    mnuRCModifyRegistration.Enabled = true;
                    mnuRCPrint.Enabled = true;
                    this._selectedItem = hitTest.Item;
                }
                else {
                    mnuRCDeleteRegistration.Enabled = false;
                    mnuRCModifyRegistration.Enabled = false;
                    mnuRCPrint.Enabled = false;
                    this._selectedItem = null;
                }
                cmsEntryRightClick.Show(Cursor.Position);
            }
        }

        private void lvEntries_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Right) {
                
                ListViewHitTestInfo hitTest = this.lvEntries.HitTest(e.X, e.Y);

                if(hitTest.Location == ListViewHitTestLocations.StateImage) {
                    return;
                }

                if (hitTest.Item != null) {
                    hitTest.Item.Selected = true;
                    this._selectedItem = hitTest.Item;
                }
                else {
                    this._selectedItem = null;
                }
            }
        }

        private void lvResults_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Right) {
                ListViewHitTestInfo hitTest = this.lvResults.HitTest(e.X, e.Y);
                if (hitTest.Item != null) {
                    foreach (ListViewItem item in lvResults.Items)
                        item.Selected = false;

                    hitTest.Item.Selected = true;
                    cmsResultsRightClick.Show(Cursor.Position);
                }
            }
        }

        private void mnuFExport_Click(object sender, EventArgs e) {
            OpenFileDialog ofDialog = new OpenFileDialog();
            ofDialog.CheckFileExists = false;
            ofDialog.AddExtension = true;
            ofDialog.DefaultExt = "csv";
            DialogResult result = ofDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
                if (File.Exists(ofDialog.FileName)) {
                    if (MessageBox.Show("Export nadpisze wybrany plik", "Możliwa utrata danych", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.OK)
                        return;

                    File.Delete(ofDialog.FileName);
                }

                result = MessageBox.Show("Export w formacie Excel 2013 (YES) czy w formacie Excel 2003 (NO)", "Format pliku", MessageBoxButtons.YesNo);

                try {
                    DataSource ds = new DataSource();
                    ds.export(ofDialog.FileName, (result == System.Windows.Forms.DialogResult.No));
                    System.Diagnostics.Process.Start(ofDialog.FileName);
                }
                catch (Exception err) {
                    LogWriter.error(err);
                    MessageBox.Show(err.Message, "Błąd Aplikacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void mnuFImport_Click(object sender, EventArgs e) {
            frmImportFile f = new frmImportFile();
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog(this);
            tabControl1.SelectedIndex = 0;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void mnuRCModifyRegistration_Click(object sender, EventArgs e) {
            if (this._selectedItem == null) {
                return;
            }
            editRegistrationItem(this._selectedItem);
        }

        private void deleteRegistrationItem(int entryId) {
            try {
                RegistrationEntryDao.delete(entryId);
                lvEntries.Items.Remove(this._selectedItem);
                this._selectedItem = null;
            }
            catch (Exception err) {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Błąd Aplikacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuRCDeleteRegistration_Click(object sender, EventArgs e) {
            
            if (this._selectedItem == null) {
                return;
            }
            if (MessageBox.Show("Usunięcie rejestracji jest nieodwracalne.  Jesteś pewien?", "Usuń Rejestrację", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != System.Windows.Forms.DialogResult.OK) {
                return;
            }
            /*
             * TODO: fix delete registration
             * - Check if this is the only registration for this model
             * - If so remove the model then check if this was the only model for the modeler
             * - If so remove the modeler
            */

            int modelId = int.Parse(this._selectedItem.SubItems[0].Text);
            deleteRegistrationItem(modelId);
            highlightInvalidRegistrationEntries();
        }

        private void mnuRCPrint_Click(object sender, EventArgs e) {
            int entryId = int.Parse(lvEntries.SelectedItems[0].SubItems[0].Text);
            DocHandler.printRegistrationCards(entryId);
        }

        private void lvEntries_ItemChecked(object sender, ItemCheckedEventArgs e) {

            if (e.Item.Checked) {
                mnuRSelected.Enabled = true;
                tsBtnPrintSelected.Enabled = true;
                tsBtnChangeCategory.Enabled = true;
                tsBtnDeleteSelected.Enabled = true;
                return;
            }

            foreach (ListViewItem item in lvEntries.Items) {
                if (item.Checked) {
                    mnuRSelected.Enabled = true;
                    tsBtnPrintSelected.Enabled = true;
                    tsBtnChangeCategory.Enabled = true;
                    tsBtnDeleteSelected.Enabled = true;
                    return;
                }
            }
            mnuRSelected.Enabled = false;
            tsBtnPrintSelected.Enabled = false;
            tsBtnChangeCategory.Enabled = false;
            tsBtnDeleteSelected.Enabled = false;
        }

        private void mnuJJudgingForms_Click(object sender, EventArgs e) {
            DialogResult q = MessageBox.Show("Wydrukować dokumenty po utworzeniu?", "Karty Sędziowania - Druk", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (q == System.Windows.Forms.DialogResult.Cancel)
                return;

            try {
                Application.UseWaitCursor = true;

                toolStripLabelSpring.Text = "Tworzenie dokumentów...";
                toolStripLabelSpring.Visible = true;
                toolStripProgressBar.Value = 0;
                toolStripProgressBar.Visible = true;

                String directory = Path.GetDirectoryName(String.Format("{0}\\{1}\\", Resources.resolvePath("folderDokumentów"), "sędziowanie"));
                if (!Directory.Exists(directory)) {
                    Directory.CreateDirectory(directory);
                }

                DirectoryInfo dir = new DirectoryInfo(directory);
                foreach (FileInfo file in dir.GetFiles("*.docx")) {
                    file.Delete();
                }

                toolStripProgressBar.Value = 0;

                DocHandler.generateJudgingForms(Resources.resolvePath("templateKartySędziowania"), directory, this);
                toolStripLabelSpring.Text = "Dokumenty gotowe";

                if (q == System.Windows.Forms.DialogResult.Yes) {
                    toolStripLabelSpring.Text = "Przesyłanie do druku...";
                    toolStripProgressBar.Value = 0;

                    FileInfo[] files = dir.GetFiles("*.docx");
                    toolStripProgressBar.Maximum = files.Length;

                    foreach (FileInfo file in files) {
                        DocHandler.PrintDocument(file.FullName);
                    }

                    showStripLabelMessage("Dokumenty wysłane do druku");
                }
                Process.Start(directory);
            }
            catch (Exception err) {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Błąd Aplikacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                toolStripLabelSpring.Visible = false;
            }
            finally {
                toolStripProgressBar.Visible = false;
                Application.UseWaitCursor = false;
            }
        }

        private void showStripLabelMessage(String message) {
            toolStripLabelSpring.Text = message;
            Timer timer = new Timer();
            timer.Interval = 1000 * 5;
            timer.Tick += new EventHandler(this.statusTextTimer_Tick);
            timer.Start();
        }

        private void statusTextTimer_Tick(object sender, EventArgs e) {
            toolStripLabelSpring.Text = "";
        }

        public void resetProgressBar(int maxValue) {
            toolStripProgressBar.Value = 0;
            toolStripProgressBar.Maximum = maxValue;
        }

        public void incrementProgressBar() {
            if (toolStripProgressBar.Value < toolStripProgressBar.Maximum)
                toolStripProgressBar.Value++;
        }

        private void mnuRsResultList_Click(object sender, EventArgs e) {
            DialogResult q = MessageBox.Show("Wydrukować wyniki po utworzeniu?", "Lista Wyników", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (q == System.Windows.Forms.DialogResult.Cancel) {
                return;
            }

            Application.UseWaitCursor = true;

            try {
                String outFile = Path.Combine(Resources.resolvePath("folderDokumentów"), "wyniki.html");
                String templateFile = Resources.resolvePath("templateWynikow");

                String directory = Path.GetDirectoryName(outFile);
                if (!Directory.Exists(directory)) {
                    Directory.CreateDirectory(directory);
                }

                if (File.Exists(outFile)) {
                    File.Delete(outFile);
                }

                DocHandler.generateHtmlResultsV2(templateFile, outFile);
                if (q == System.Windows.Forms.DialogResult.Yes) {
                    DocHandler.PrintDocument(outFile);
                }
                System.Diagnostics.Process.Start(outFile);
            }
            catch (Exception err) {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Błąd Aplikacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally {
                Application.UseWaitCursor = false;
            }
        }

        private void mnuRCCheckAll_Click(object sender, EventArgs e) {
            foreach (ListViewItem item in lvEntries.Items) {
                item.Checked = true;
            }
        }

        private void printEntryCards(bool printAll) {
            List<KeyValuePair<string, int>> entries = new List<KeyValuePair<string, int>>();

            Application.UseWaitCursor = true;

            resetProgressBar(lvEntries.CheckedItems.Count);
            toolStripProgressBar.Visible = true;
            toolStripLabelSpring.Text = "Wysyłanie dokumentów do druku...";

            if (printAll) {
                foreach (ListViewItem item in lvEntries.Items) {
                    entries.Add(new KeyValuePair<string, int>(item.SubItems[4].Text, int.Parse(item.SubItems[0].Text)));
                }
            }
            else {
                foreach (ListViewItem item in lvEntries.Items) {
                    if (item.Checked) {
                        entries.Add(new KeyValuePair<string, int>(item.SubItems[4].Text, int.Parse(item.SubItems[0].Text)));
                    }
                }
            }

            if (mnuRPrintSorted.Checked) {
                entries.Sort((x, y) => x.Key.ToLower().CompareTo(y.Key.ToLower()));
            }

            foreach (KeyValuePair<string, int> entry in entries) {
                DocHandler.printRegistrationCards(entry.Value);
                incrementProgressBar();
            }

            toolStripProgressBar.Visible = false;
            showStripLabelMessage("Dokumenty wydrukowane");

            Application.UseWaitCursor = false;
        }

        private void mnuRCUncheckAll_Click(object sender, EventArgs e) {
            foreach (ListViewItem item in lvEntries.Items) {
                item.Checked = false;
            }
        }

        private void cmsRCDeleteResult_Click(object sender, EventArgs e) {
            int resultId = int.Parse(lvResults.SelectedItems[0].SubItems[0].Text);

            ResultDao.delete(resultId);
            refreshScreen();
        }

        private void loadStats() {
            try {
                Application.UseWaitCursor = true;
                lvStats.BeginUpdate();

                lvStats.Items.Clear();
                lvStats.Groups.Clear();

                ListViewGroup group = new ListViewGroup("");

                foreach (KeyValuePair<string, string> stat in RegistrationEntryDao.getRegistrationStats()) {
                    if (stat.Key.StartsWith("GROUP")) {
                        group = new ListViewGroup(stat.Value);
                        lvStats.Groups.Add(group);
                    }
                    else {
                        ListViewItem item = new ListViewItem(new String[] { "", stat.Key, stat.Value }, group);
                        if (stat.Key.StartsWith("*")) {
                            item.SubItems[1].Text = stat.Key.Substring(1);
                            item.Font = new Font(item.Font, FontStyle.Bold);
                        }
                        lvStats.Items.Add(item);
                    }
                }

                lvStats.Columns[0].Width = 0;
                lvStats.Columns[1].Width = -2;
                lvStats.Columns[2].Width = -2;
            }
            finally {
                lvStats.EndUpdate();
                Application.UseWaitCursor = false;
            }
        }

        private void mnuRNewRegistration_Click(object sender, EventArgs e) {
            tabControl1.SelectTab(0);
            frmRegistrationEntry f = new frmRegistrationEntry();
            f.StartPosition = FormStartPosition.CenterScreen;
            f.ShowDialog(this);
            refreshScreen();
        }

        private void txtFilterTimer_Tick(object sender, EventArgs e) {
            _txtFilterTimer.Stop();
            refreshScreen();
        }

        private void lvEntries_KeyDown(object sender, KeyEventArgs e) {

            switch (e.KeyCode) {
                case Keys.Delete:
                    if (MessageBox.Show("Usunięcie rejestracji jest nieodwracalne.  Jesteś pewien?", "Usuń Rejestrację", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != System.Windows.Forms.DialogResult.OK) {
                        return;
                    }
                    int entryId = int.Parse(lvEntries.SelectedItems[0].SubItems[0].Text);
                    deleteRegistrationItem(entryId);
                    highlightInvalidRegistrationEntries();
                    break;

                case Keys.Enter:
                    if (lvEntries.SelectedItems.Count == 0) {
                        return;
                    }
                    editRegistrationItem(lvEntries.SelectedItems[0]);
                    break;
            }
        }

        private void mnuFNewDataFile_Click(object sender, EventArgs e) {
            frmNewDataFile f = new frmNewDataFile();
            f.setSelectedTab(0);
            f.ShowDialog(this);
        }

        private void mnuFOpenDataFile_Click(object sender, EventArgs e) {
            frmNewDataFile f = new frmNewDataFile();
            f.setSelectedTab(1);
            f.ShowDialog(this);

            initUi(true);

            if (Options.get("RegistrationView") == null) {
                Options.set("RegistrationView", "groupped");
            }
            setViewMenus(!Options.get("RegistrationView").Equals("groupped"));

            if (Options.get("ColumnWidth") == null) {
                Options.set("ColumnWidth", "auto");
            }
            mnuRVAutoWidth.Checked = Options.get("ColumnWidth").Equals("auto");

            refreshScreen();
            uiEnabled(true);
            this._showSettingsForm = false;
        }

        private void mnuFFDataFolder_Click(object sender, EventArgs e) {
            Process.Start(Resources.DataFileFolder);
        }

        private void mnuFFDocumentFolder_Click(object sender, EventArgs e) {
            Process.Start(Resources.DocumentFolder);
        }

        private void mnuFFTemplateFolder_Click(object sender, EventArgs e) {
            Process.Start(Resources.TemplateFolder);
        }

        private void mnuRVItem_Click(object sender, EventArgs e) {
            if (mnuRVStandard.Checked) {
                setViewMenus(false);
                lvStats.HeaderStyle = ColumnHeaderStyle.Nonclickable;

                Options.set("RegistrationView", "groupped");
            }
            else {
                setViewMenus(true);
                lvStats.HeaderStyle = ColumnHeaderStyle.Clickable;

                Options.set("RegistrationView", "standard");
            }
            refreshScreen();
        }

        private void lvEntries_ColumnClick(object sender, ColumnClickEventArgs e) {

            if (!mnuRVStandard.Checked) {
                return;
            }

            if (_registrationSortColumn == e.Column) {
                _registrationSortAscending = !_registrationSortAscending;
            }
            else {
                _registrationSortColumn = e.Column;
                _registrationSortAscending = true;
            }

            Options.set("RegistrationSortColumn", _registrationSortColumn.ToString());
            Options.set("RegistrationSortOrder", _registrationSortAscending ? "0" : "1");

            refreshScreen();
        }

        private void mnuRsCategoryDiplomas_Click(object sender, EventArgs e) {
            DialogResult q = MessageBox.Show("Wydrukować dokumenty po utworzeniu?", "Dyplomy - Druk", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (q == System.Windows.Forms.DialogResult.Cancel)
                return;
            Boolean printForms = (q == System.Windows.Forms.DialogResult.Yes);

            try {
                Application.UseWaitCursor = true;

                toolStripLabelSpring.Text = "Tworzenie dokumentów...";
                toolStripLabelSpring.Visible = true;
                toolStripProgressBar.Value = 0;
                toolStripProgressBar.Visible = true;

                String outputDirectory = Path.Combine(Resources.resolvePath("folderDokumentów"), "dyplomy", "kategorie");
                if (!Directory.Exists(outputDirectory)) {
                    Directory.CreateDirectory(outputDirectory);
                }

                DirectoryInfo dir = new DirectoryInfo(outputDirectory);
                foreach (FileInfo file in dir.GetFiles("*.docx")) {
                    file.Delete();
                }

                toolStripProgressBar.Value = 0;

                IEnumerable<Result> results;

                String templateFile = Resources.resolvePath("templateDyplomuKategorii");
                results = ResultDao.getCategoryResults();

                foreach (Result result in results) {
                    String outputFile = Path.Combine(outputDirectory, String.Format("dyplom_{0}.docx", result.resultId));
                    DocHandler.generateDiploma(templateFile, outputFile, result);
                    if (printForms) {
                        DocHandler.PrintDocument(outputFile);
                    }
                    incrementProgressBar();
                }
                showStripLabelMessage("Dokumenty gotowe");
                Process.Start(outputDirectory);
            }
            catch (Exception err) {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Błąd Aplikacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                toolStripLabelSpring.Visible = false;
            }
            finally {
                toolStripProgressBar.Visible = false;
                Application.UseWaitCursor = false;
            }
        }

        private void mnuJAddResults_Click(object sender, EventArgs e) {
            frmAddResults f = new frmAddResults();
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog(this);
            if(tabControl1.SelectedTab.Text.Equals("Wyniki")) {
                refreshScreen();
            }
        }

        private void mnuRsAwardDiplomas_Click(object sender, EventArgs e) {
            DialogResult q = MessageBox.Show("Wydrukować dokumenty po utworzeniu?", "Dyplomy - Druk", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (q == System.Windows.Forms.DialogResult.Cancel)
                return;
            Boolean printForms = (q == System.Windows.Forms.DialogResult.Yes);

            try {
                Application.UseWaitCursor = true;

                toolStripLabelSpring.Text = "Tworzenie dokumentów...";
                toolStripLabelSpring.Visible = true;
                toolStripProgressBar.Value = 0;
                toolStripProgressBar.Visible = true;

                String outputDirectory = Path.Combine(Resources.resolvePath("folderDokumentów"), "dyplomy", "nagrody");
                if (!Directory.Exists(outputDirectory)) {
                    Directory.CreateDirectory(outputDirectory);
                }

                DirectoryInfo dir = new DirectoryInfo(outputDirectory);
                foreach (FileInfo file in dir.GetFiles("*.docx")) {
                    file.Delete();
                }

                toolStripProgressBar.Value = 0;

                IEnumerable<Result> results = ResultDao.getAwardResults();
                String templateFile = Resources.resolvePath("templateDyplomuNagrody");
                resetProgressBar(results.Count());

                foreach (Result result in results) {
                    String outputFile = Path.Combine(outputDirectory, String.Format("dyplom_{0}.docx", result.resultId));
                    DocHandler.generateDiploma(templateFile, outputFile, result);
                    if (printForms) {
                        DocHandler.PrintDocument(outputFile);
                    }
                    incrementProgressBar();
                }
                showStripLabelMessage("Dokumenty gotowe");
                Process.Start(outputDirectory);
            }
            catch (Exception err) {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Błąd Aplikacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                toolStripLabelSpring.Visible = false;
            }
            finally {
                toolStripProgressBar.Visible = false;
                Application.UseWaitCursor = false;
            }
        }

        private void mnuJMergeCategories_Click(object sender, EventArgs e) {
            frmMergeCategory f = new frmMergeCategory();
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog(this);
            refreshScreen();
        }

        private void mnuRCPrintDiploma_Click(object sender, EventArgs e) {

            Application.UseWaitCursor = true;

            try {

                int resultId = int.Parse(lvResults.SelectedItems[0].SubItems[0].Text);
                Result result = ResultDao.get(resultId);
                toolStripLabelSpring.Text = "Tworzenie dokumentu...";
                toolStripLabelSpring.Visible = true;

                if (result.place > 0) {

                    String outputDirectory = Path.Combine(Resources.resolvePath("folderDokumentów"), "dyplomy", "kategorie");
                    if (!Directory.Exists(outputDirectory)) {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    String templateFile = Resources.resolvePath("templateDyplomuKategorii");
                    String outputFile = Path.Combine(outputDirectory, String.Format("dyplom_{0}.docx", result.resultId));
                    File.Delete(outputFile);
                    DocHandler.generateDiploma(templateFile, outputFile, result);
                    DocHandler.PrintDocument(outputFile);

                    showStripLabelMessage("Dokument wysłany do druku");
                }
                else if (result.award != null) {

                    String outputDirectory = Path.Combine(Resources.resolvePath("folderDokumentów"), "dyplomy", "nagrody");
                    if (!Directory.Exists(outputDirectory)) {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    String templateFile = Resources.resolvePath("templateDyplomuNagrody");
                    String outputFile = Path.Combine(outputDirectory, String.Format("dyplom_{0}.docx", result.resultId));
                    File.Delete(outputFile);

                    DocHandler.generateDiploma(templateFile, outputFile, result);
                    DocHandler.PrintDocument(outputFile);

                    showStripLabelMessage("Dokument wysłany do druku");
                }
                else {
                    LogWriter.info("ERROR printing diploma: Result did not contain place or award");
                    MessageBox.Show("Wpis nie posiada numeru nagrody lub miejsca", "Błąd Aplikacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception err) {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Błąd Aplikacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                toolStripLabelSpring.Visible = false;
            }
            finally {
                Application.UseWaitCursor = false;
            }
        }

        private void mnuRSPrintChecked_Click(object sender, EventArgs e) {
            printEntryCards(false);
        }

        private void mnuRSChangeCategory_Click(object sender, EventArgs e) {
            
            this._refreshList = false;
            frmChangeCategory f = new frmChangeCategory();
            f.StartPosition = FormStartPosition.CenterParent;
            f.setParent(this);
            f.ShowDialog();

            bool showOnlyErrors = tsBtnErrorCount.Visible && tsBtnErrorCount.Checked;
            if (this._refreshList) {

                refreshScreen();
                if (showOnlyErrors && tsBtnErrorCount.Visible) {
                    hideValidEntries();
                }
            }
        }

        private void mnuRSDelete_Click(object sender, EventArgs e) {
            if (lvEntries.CheckedItems.Count == 0) {
                return;
            }
            if (MessageBox.Show("Usunięcie rejestracji jest nieodwracalne.  Jesteś pewien?", "Usuń Rejestrację", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != System.Windows.Forms.DialogResult.OK) {
                return;
            }

            Application.UseWaitCursor = true;
            uiEnabled(false);

            try {
                foreach (ListViewItem item in lvEntries.CheckedItems) {
                    deleteRegistrationItem((int)item.Tag);
                    lvEntries.Items.Remove(item);
                }
                if (tsBtnErrorCount.Checked && lvEntries.Items.Count == 0) {
                    tsBtnErrorCount.Checked = false;
                    refreshScreen();
                }
                else {
                    highlightInvalidRegistrationEntries();
                }
            }
            catch (Exception err) {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Błąd Aplikacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally {
                uiEnabled(true);
                Application.UseWaitCursor = false;
            }
        }

        private void mnuRSettings_Click(object sender, EventArgs e) {
            frmSettings f = new frmSettings();
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog(this);
            refreshScreen();
        }

        private void tsTxtSearch_TextChanged(object sender, EventArgs e) {
            if (tsTxtSearch.Text.Length == 0) {
                tsBtnClearFilter.Visible = false;
                tsBtnRefresh.Visible = true;
            }
            else {
                tsBtnClearFilter.Visible = true;
                tsBtnRefresh.Visible = false;
            }
        }

        private void frmMain_KeyUp(object sender, KeyEventArgs e) {
            if (e.Control) {
                if (e.KeyCode == Keys.F) {
                    tsTxtSearch.Focus();
                }
                return;
            }
            if (e.KeyCode == Keys.F5) {
                refreshScreen();
                //loadRegistrationList();
                //loadResultList();
                //loadStats();
                return;
            }
            if (e.KeyCode == Keys.Escape) {
                if (tsTxtSearch.Text.Length > 0) {
                    tsTxtSearch.Text = "";
                    //loadRegistrationList(null);
                    refreshScreen();
                }
            }
        }

        private void mnuRPrintAll_Click(object sender, EventArgs e) {
            printEntryCards(true);
        }

        private void mnuRsSummary_Click(object sender, EventArgs e) {
            DialogResult q = MessageBox.Show("Wydrukować dokument po utworzeniu?", "Podsumowanie Konkursu", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (q == System.Windows.Forms.DialogResult.Cancel) {
                return;
            }

            Application.UseWaitCursor = true;

            try {
                String outFile = String.Format("{0}\\{1}", Resources.resolvePath("folderDokumentów"), "podsumowanie.html");
                String templateFile = Resources.resolvePath("templatePosumowania");

                String directory = Path.GetDirectoryName(outFile);
                if (!Directory.Exists(directory)) {
                    Directory.CreateDirectory(directory);
                }

                if (File.Exists(outFile)) {
                    File.Delete(outFile);
                }

                DocHandler.generateHtmlSummary(templateFile, outFile);
                if (q == System.Windows.Forms.DialogResult.Yes) {
                    DocHandler.PrintDocument(outFile);
                }
                System.Diagnostics.Process.Start(outFile);
            }
            catch (Exception err) {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Błąd Aplikacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally {
                Application.UseWaitCursor = false;
            }
        }

        private void tsBtnView_ButtonClick(object sender, EventArgs e) {
            mnuRVItem_Click(sender, e);
        }

        private void tsBVStandard_Click(object sender, EventArgs e) {
            if (!tsBVStandard.Checked) {
                mnuRVItem_Click(sender, e);
            }
        }

        private void tsBVGroupped_Click(object sender, EventArgs e) {
            if (!tsBVGroupped.Checked) {
                mnuRVItem_Click(sender, e);
            }
        }

        private void tsBtnNewRegistration_Click(object sender, EventArgs e) {
            mnuRNewRegistration_Click(sender, e);
        }

        private void hideValidEntries() {
            lvEntries.BeginUpdate();
            foreach (ListViewItem item in lvEntries.Items) {
                if (item.ForeColor != System.Drawing.Color.Red) {
                    item.Remove();
                }
            }
            lvEntries.EndUpdate();
        }

        private void tsBtnErrorCount_Click(object sender, EventArgs e) {
            
            Application.UseWaitCursor = true;

            if (tsBtnErrorCount.Checked) {
                tsTxtSearch.Text = "";
                tsTxtSearch.Enabled = false;
                tsBtnRefresh.Visible = false;
                tsBtnClearFilter.Visible = true;
                hideValidEntries();
            }
            else {
                tsBtnClearFilter_Click(sender, e);
            }
            Application.UseWaitCursor = false;
        }

        private void setPrintOrderOption(bool printSortedAlphabetically) {
            tsBPAlphabetic.Checked = printSortedAlphabetically;
            mnuRPrintSorted.Checked = printSortedAlphabetically;
            tsBPById.Checked = !printSortedAlphabetically;
            mnuRPrintById.Checked = !printSortedAlphabetically;
        }

        private void mnuRPrintSorted_Click(object sender, EventArgs e) {
            setPrintOrderOption(true);
        }

        private void tsBPAlphabetic_Click(object sender, EventArgs e) {
            setPrintOrderOption(true);
        }

        private void tsBPById_Click(object sender, EventArgs e) {
            setPrintOrderOption(false);
        }

        private void mnuRPrintById_Click(object sender, EventArgs e) {
            setPrintOrderOption(false);
        }

        private void btnSettings_Click(object sender, EventArgs e) {
            mnuRSettings_Click(sender, e);
        }

        private void tsBtnMergeCategories_Click(object sender, EventArgs e) {
            mnuJMergeCategories_Click(sender, e);
        }

        private void tsBtnJudgingForms_Click(object sender, EventArgs e) {
            mnuJJudgingForms_Click(sender, e);
        }

        private void tsBtnAddResults_Click(object sender, EventArgs e) {
            mnuJAddResults_Click(sender, e);
        }

        private void tsBtnResults_Click(object sender, EventArgs e) {
            mnuRsResultList_Click(sender, e);
        }

        private void tsBtnCategoryDiplomas_Click(object sender, EventArgs e) {
            mnuRsCategoryDiplomas_Click(sender, e);
        }

        private void tsBtnAwardDiplomas_Click(object sender, EventArgs e) {
            mnuRsAwardDiplomas_Click(sender, e);
        }

        private void tsBtnSummary_Click(object sender, EventArgs e) {
            mnuRsSummary_Click(sender, e);
        }

        private void tsBtnRefresh_Click(object sender, EventArgs e) {
            refreshScreen();
        }

        private void tsBtnClearFilter_Click(object sender, EventArgs e) {
            tsBtnClearFilter.Visible = false;
            tsBtnRefresh.Visible = true;
            tsTxtSearch.Text = "";
            tsTxtSearch.Enabled = true;
            tsBtnErrorCount.Checked = false;
            refreshScreen();
        }

        private void tsBtnChangeCategory_Click(object sender, EventArgs e) {
            mnuRSChangeCategory_Click(sender, e);
        }

        private void btnPrintAll_ButtonClick(object sender, EventArgs e) {
            mnuRPrintAll_Click(sender, e);
        }

        private void tsBtnPrintSelected_Click(object sender, EventArgs e) {
            mnuRSPrintChecked_Click(sender, e);
        }

        private void tsBtnDeleteSelected_Click(object sender, EventArgs e) {
            mnuRSDelete_Click(sender, e);
        }

        private void mnuHAbout_Click(object sender, EventArgs e) {
            frmAbout f = new frmAbout();
            f.ShowDialog(this);
        }

        private void mnuHHelp_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("https://github.com/popala/registration/wiki");
        }

        private void mnuRVAutoWidth_Click(object sender, EventArgs e) {
            if (mnuRVAutoWidth.Checked) {
                Options.set("ColumnWidth", "auto");
            }
            else {
                Options.set("ColumnWidth", "manual");
            }
        }

        public void refreshScreen() {
            switch(tabControl1.SelectedTab.Text) {
                case "Rejestracja":
                    loadRegistrationList();
                    break;
                case "Wyniki":
                    loadResultList();
                    break;
                case "Podsumowanie":
                    loadStats();
                    break;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) {
            switch(tabControl1.SelectedTab.Text) {
                case "Rejestracja":
                    tsTxtSearch.Enabled = true;
                    tsBtnRefresh.Enabled = true;
                    break;
                default: 
                    tsTxtSearch.Enabled = false;
                    tsBtnRefresh.Enabled = false;
                    refreshScreen();
                    break;
            }
            
        }
    }
}
