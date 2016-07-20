using Rejestracja.Data.Dao;
using Rejestracja.Data.Objects;
using Rejestracja.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Rejestracja {
    public partial class frmMain : Form {
        private Timer _txtFilterTimer;
        private int _registrationSortColumn = 0;
        private bool _registrationSortAscending = true;
        private bool _showSettingsForm = false;

        public void setShowSettingsForm(bool value) {
            this._showSettingsForm = value;
        }

        public frmMain() {
            InitializeComponent();
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

        private void frmMain_Load(object sender, EventArgs e) {
            //devTasks();

            String size = Options.get("frmMainSize");
            if (size != null) {
                String[] pos = size.Split(',');
                this.Left = int.Parse(pos[0]);
                this.Top = int.Parse(pos[1]);
                this.Width = int.Parse(pos[2]);
                this.Height = int.Parse(pos[3]);
            }

            _txtFilterTimer = new Timer();
            _txtFilterTimer.Interval = 1000;
            _txtFilterTimer.Tick += new EventHandler(this.txtFilterTimer_Tick);

            lblErrorCount.Visible = false;
            lblErrorCount.ForeColor = System.Drawing.Color.Red;

            mnuRVStandard.Checked = true;
            mnuRVGroupped.Checked = false;
            mnuRPrintSorted.Checked = true;
            
            //Ensure that a data file exists
            String dataFile = Properties.Settings.Default.DataFile;
            if (String.IsNullOrWhiteSpace(dataFile) || !File.Exists(dataFile)) {
                uiEnabled(false);
                frmNewDataFile f = new frmNewDataFile();
                f.StartPosition = FormStartPosition.CenterScreen;
                f.ShowDialog(this);
                if (_showSettingsForm) {
                    frmSettings fs = new frmSettings();
                    fs.StartPosition = FormStartPosition.CenterScreen;
                    fs.ShowDialog(this);
                    populateUI();
                    uiEnabled(true);
                    this._showSettingsForm = false;
                }
                return;
            }

            if (Options.get("RegistrationView").Equals("groupped")) {
                mnuRVStandard.Checked = false;
                mnuRVGroupped.Checked = true;
            }

            try {
                //DATA FILE
                DataSource ds = new DataSource();
                populateUI();
                uiEnabled(true);
            }
            catch (Exception err) {
                uiEnabled(false);
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void uiEnabled(bool isEnabled) {
            tabControl1.Enabled = isEnabled;
            mnuRegistration.Enabled = isEnabled;
            mnuJudging.Enabled = isEnabled;
            mnuFImport.Enabled = isEnabled;
            mnuFExport.Enabled = isEnabled;
            mnuFSettings.Enabled = isEnabled;
            mnuResults.Enabled = isEnabled;
        }

        public void populateUI() {

            Application.UseWaitCursor = true;

            String[] headers;

            //REGISTRATION PANEL
            lvEntries.View = View.Details;
            lvEntries.GridLines = true;
            lvEntries.FullRowSelect = true;
            lvEntries.HeaderStyle = ColumnHeaderStyle.Clickable;
            lvEntries.CheckBoxes = true;

            lvEntries.Columns.Clear();
            headers = new String[] { "Nr Rej.", "Dodane", "Email", "Imię", "Nazwisko", "Rok Ur.", "Klub", "Grupa Wiekowa", "Nazwa Modelu", "Kategoria", "Klasa", "Skala", "Wydawnictwo" };
            foreach (String header in headers) {
                lvEntries.Columns.Add(header.Trim());
            }

            _registrationSortColumn = int.Parse(Options.get("RegistrationSortColumn"));
            _registrationSortAscending = !Options.get("RegistrationSortOrder").Equals("1");

            loadRegistrationList(null);

            mnuRPrint.Enabled = false;

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

            loadResultList();

            //stats
            lvStats.View = View.Details;
            lvStats.GridLines = true;
            lvStats.FullRowSelect = true;
            lvStats.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            
            lvStats.Columns.Clear();
            lvStats.Columns.Add("");
            lvStats.Columns.Add("Nazwa");
            lvStats.Columns.Add("Wartość");
            lvStats.Columns[1].TextAlign = HorizontalAlignment.Right;

            loadStats();

            Application.UseWaitCursor = false;
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
            }
            catch (Exception err) {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally 
            {
                lvResults.EndUpdate();
            }
        }

        private void loadRegistrationList(String searchValue) {

            if (mnuRVStandard.Checked) {
                loadSortedRegistrationList(searchValue);
            }
            else {
                loadGrouppedRegistrationList(searchValue);
            }
        }

        private void loadSortedRegistrationList(String searchValue) {

            try {
                List<string[]> entries;

                Application.UseWaitCursor = true;
                lvEntries.BeginUpdate();

                //lvEntries.CheckBoxes = false;
                lvEntries.Items.Clear();
                lvEntries.Groups.Clear();

                if (String.IsNullOrWhiteSpace(searchValue)) {
                    entries = RegistrationEntryDao.getList(null, _registrationSortColumn, _registrationSortAscending).ToList();
                }
                else {
                    entries = RegistrationEntryDao.getList(searchValue, _registrationSortColumn, _registrationSortAscending).ToList();
                }

                foreach (string[] entry in entries) {
                    lvEntries.Items.Add(new ListViewItem(entry));
                }

                foreach (ColumnHeader header in lvEntries.Columns) {
                    header.Width = -2;
                }

                highlightInvalidRegistrationEntries();
            }
            catch (Exception err) {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally {
                lvEntries.EndUpdate();
                Application.UseWaitCursor = false;
            }
        }

        private void loadGrouppedRegistrationList(String searchValue) {
            try {
                Application.UseWaitCursor = true;
                lvEntries.BeginUpdate();

                List<string[]> entries;

                lvEntries.Items.Clear();
                lvEntries.Groups.Clear();

                if (String.IsNullOrWhiteSpace(searchValue)) {
                    entries = RegistrationEntryDao.getGrouppedList().ToList();
                }
                else {
                    entries = RegistrationEntryDao.getGrouppedList(searchValue).ToList();
                }

                String categoryName = "";
                ListViewGroup group = new ListViewGroup("");

                foreach (string[] entry in entries) {
                    if (!categoryName.Equals(entry[9])) {
                        categoryName = entry[9];
                        group = new ListViewGroup(categoryName);
                        lvEntries.Groups.Add(group);
                    }
                    lvEntries.Items.Add(new ListViewItem(entry, group));
                }

                foreach (ColumnHeader header in lvEntries.Columns) {
                    header.Width = -2;
                }

                highlightInvalidRegistrationEntries();
            }
            catch (Exception err) {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally {
                lvEntries.EndUpdate();
                Application.UseWaitCursor = false;
            }
        }

        private void highlightInvalidRegistrationEntries() {
            List<ModelCategory> modelCategories = ModelCategoryDao.getList().ToList<ModelCategory>();
            int errorCount = 0;

            foreach (ListViewItem item in lvEntries.Items) {
                item.ToolTipText = "";

                //TODO: Make this an option
                //Only seniors are allowed in non-standard category
                if (item.SubItems[7].Text.ToLower() != "senior" && item.SubItems[10].Text.ToLower() != "standard") {
                    item.ForeColor = System.Drawing.Color.Red;
                    item.ToolTipText = "Tylko Senior może startować w kategorii Open.";
                    lvEntries.ShowItemToolTips = true;
                    errorCount++;
                    continue;
                }
                else {
                    item.ForeColor = System.Drawing.Color.Black;
                }

                //Check if model category is listed in the resources
                ModelCategory [] catFound = modelCategories.Where(x => x.fullName.ToLower().Equals(item.SubItems[9].Text.ToLower())).ToArray<ModelCategory>();
                if (catFound.Length > 0) {
                    continue;
                }
                else {
                    item.ForeColor = System.Drawing.Color.Red;
                    item.ToolTipText += " Kategoria modelu nie znaleziona w konfiguracji.";
                    lvEntries.ShowItemToolTips = true;
                    errorCount++;
                }
            }

            if (errorCount > 0) {
                lblErrorCount.Text = string.Format("Znaleziono błędy w {0} {1}", errorCount, (errorCount == 1 ? "rejestracji" : "rejestracjach"));
                lblErrorCount.Visible = true;
            }
            else {
                lblErrorCount.Visible = false;
            }
        }

        private void txtFilter_KeyUp(object sender, KeyEventArgs e) {
            _txtFilterTimer.Stop();

            Debug.Print(e.KeyCode.ToString());
            Debug.Print(e.Control.ToString());

            if (e.Alt || e.Control || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.Tab) {
                return;
            }

            switch (e.KeyCode) {
                case Keys.Enter:
                    loadRegistrationList(txtFilter.Text);
                    break;

                case Keys.Escape:
                    txtFilter.Text = "";
                    loadRegistrationList(null);
                    break;

                default:
                    _txtFilterTimer.Start();
                    break;
            }
        }

        private void btnFilter_Click(object sender, EventArgs e) {
            loadRegistrationList(txtFilter.Text);
        }

        private void lvEntries_MouseDoubleClick(object sender, MouseEventArgs e) {

            if (lvEntries.Items.Count == 0) {
                return;
            }

            ListViewHitTestInfo hitTest = this.lvEntries.HitTest(e.X, e.Y);
            if (hitTest.Item != null) {
                ListViewItem item = hitTest.Item;
                int index = item.Index;
                item.Checked = !item.Checked;

                int entryId = int.Parse(item.SubItems[0].Text);
                frmRegistrationEntry f = new frmRegistrationEntry();
                f.StartPosition = FormStartPosition.CenterParent;
                f.loadEntry(entryId);
                f.ShowDialog(this);
                loadRegistrationList(txtFilter.Text);
                if (lvEntries.Items.Count > index) {
                    lvEntries.Items[index].Selected = true;
                }
            }
        }

        private void lvEntries_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Right) {
                ListViewHitTestInfo hitTest = this.lvEntries.HitTest(e.X, e.Y);
                if (hitTest.Item != null) {
                    hitTest.Item.Selected = true;
                    mnuRCDeleteRegistration.Enabled = true;
                    mnuRCModifyRegistration.Enabled = true;
                    mnuRCPrint.Enabled = true;
                }
                else {
                    mnuRCDeleteRegistration.Enabled = false;
                    mnuRCModifyRegistration.Enabled = false;
                    mnuRCPrint.Enabled = false;
                }
                cmsEntryRightClick.Show(Cursor.Position);
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
                    MessageBox.Show(err.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void mnuFImport_Click(object sender, EventArgs e) {

            if (MessageBox.Show("Import usunie dane w lokalnej bazie i nadpisze je danymi z importowanego pliku", "Możliwa utrata danych", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.OK)
                return;

            frmImportFile f = new frmImportFile();
            f.StartPosition = FormStartPosition.CenterParent;
            f.Show(this);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void btnRegister_Click(object sender, EventArgs e) {
            frmRegistrationEntry f = new frmRegistrationEntry();
            f.StartPosition = FormStartPosition.CenterScreen;
            f._parentForm = this;
            f.ShowDialog(this);
            loadRegistrationList(txtFilter.Text);
        }

        private void mnuRCModifyRegistration_Click(object sender, EventArgs e) {
            int entryId = int.Parse(lvEntries.SelectedItems[0].SubItems[0].Text);
            frmRegistrationEntry f = new frmRegistrationEntry();
            f.loadEntry(entryId);
            f.ShowDialog(this);

            RegistrationEntry entry = RegistrationEntryDao.get(entryId);

            lvEntries.SelectedItems[0].SubItems[2].Text = entry.email;
            lvEntries.SelectedItems[0].SubItems[3].Text = entry.firstName;
            lvEntries.SelectedItems[0].SubItems[4].Text = entry.lastName;
            lvEntries.SelectedItems[0].SubItems[5].Text = entry.clubName;
            lvEntries.SelectedItems[0].SubItems[6].Text = entry.ageGroup;
            lvEntries.SelectedItems[0].SubItems[7].Text = entry.modelName;
            lvEntries.SelectedItems[0].SubItems[8].Text = entry.modelCategory;
            lvEntries.SelectedItems[0].SubItems[9].Text = entry.modelScale;
            lvEntries.SelectedItems[0].SubItems[10].Text = entry.modelPublisher;
            lvEntries.SelectedItems[0].SubItems[11].Text = entry.modelClass;
            lvEntries.SelectedItems[0].SubItems[12].Text = entry.yearOfBirth.ToString();

            highlightInvalidRegistrationEntries();
        }

        private void deleteRegistrationItem(int entryId) {
            if (MessageBox.Show("Usunięcie rejestracji jest nieodwracalne.  Jesteś pewien?", "Usuń Rejestrację", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != System.Windows.Forms.DialogResult.OK) {
                return;
            }

            try {
                RegistrationEntryDao.delete(entryId);
                lvEntries.SelectedItems[0].Remove();
                highlightInvalidRegistrationEntries();
            }
            catch (Exception err) {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuRCDeleteRegistration_Click(object sender, EventArgs e) {
            int entryId = int.Parse(lvEntries.SelectedItems[0].SubItems[0].Text);
            deleteRegistrationItem(entryId);
        }

        private void mnuRCPrint_Click(object sender, EventArgs e) {
            int entryId = int.Parse(lvEntries.SelectedItems[0].SubItems[0].Text);
            printRegistrationCard(entryId);
        }

        public void printRegistrationCard(long entryId) {
            try {
                RegistrationEntry entry = RegistrationEntryDao.get(entryId);
                if (entry == null) {
                    MessageBox.Show("Numer startowy nie został znaleziony", "Błędne dane", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                String directory = Path.GetDirectoryName(String.Format("{0}\\{1}\\", Resources.resolvePath("folderDokumentów"), "karty"));
                if (!Directory.Exists(directory)) {
                    Directory.CreateDirectory(directory);
                }

                string outFile = String.Format("{0}\\rejestracja_{1}.docx", directory, entry.entryId);
                File.Delete(outFile);

                DocHandler dc = new DocHandler();
                dc.generateRegistrationCard(Resources.resolvePath("templateKartyModelu"), outFile, entry);
                dc.printWordDoc(outFile);
            }
            catch (Exception err) {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lvEntries_ItemChecked(object sender, ItemCheckedEventArgs e) {
            if (e.Item.Checked) {
                mnuRPrint.Enabled = true;
                return;
            }

            foreach (ListViewItem item in lvEntries.Items) {
                if (item.Checked) {
                    mnuRPrint.Enabled = true;
                    return;
                }
            }
            mnuRPrint.Enabled = false;
        }

        private void mnuJJudgingForms_Click(object sender, EventArgs e) {
            DialogResult q = MessageBox.Show("Wydrukować dokumenty po utworzeniu?", "Karty Sędziowania - Druk", MessageBoxButtons.YesNoCancel);
            if (q == System.Windows.Forms.DialogResult.Cancel)
                return;
            Boolean printForms = (q == System.Windows.Forms.DialogResult.Yes);

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

                DocHandler dc = new DocHandler();
                dc.generateJudgingForms(Resources.resolvePath("templateKartySędziowania"), directory, this);
                toolStripLabelSpring.Text = "Dokumenty gotowe";

                if (printForms) {
                    toolStripLabelSpring.Text = "Przesyłanie do druku...";
                    toolStripProgressBar.Value = 0;

                    FileInfo[] files = dir.GetFiles("*.docx");
                    toolStripProgressBar.Maximum = files.Length;

                    foreach (FileInfo file in files) {
                        dc.printWordDoc(file.FullName);
                    }

                    showStripLabelMessage("Dokumenty wysłane do druku");
                }
            }
            catch (Exception err) {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void mnuRResultList_Click(object sender, EventArgs e) {
            String outFile = String.Format("{0}\\{1}", Resources.resolvePath("folderDokumentów"), "wyniki.html");
            String templateFile = Resources.resolvePath("templateWynikow");

            String directory = Path.GetDirectoryName(outFile);
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }

            if (File.Exists(outFile)) {
                if (MessageBox.Show("Plik \"wyniki.html\" istnieje. Nadpisać instniejący plik?", "Możliwa utrata danych", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.OK)
                    return;
                File.Delete(outFile);
            }

            DocHandler dc = new DocHandler();
            dc.generateHtmlResults(templateFile, outFile);
            System.Diagnostics.Process.Start(outFile);
        }

        private void mnuRCCheckAll_Click(object sender, EventArgs e) {
            foreach (ListViewItem item in lvEntries.Items) {
                item.Checked = true;
            }
        }

        private void printEntryCards() {
            List<KeyValuePair<string, int>> entries = new List<KeyValuePair<string, int>>();

            Application.UseWaitCursor = true;

            resetProgressBar(lvEntries.CheckedItems.Count);
            toolStripProgressBar.Visible = true;
            toolStripLabelSpring.Text = "Wysyłanie dokumentów do druku...";

            foreach (ListViewItem item in lvEntries.Items) {
                if (item.Checked) {
                    entries.Add(new KeyValuePair<string, int>(item.SubItems[4].Text, int.Parse(item.SubItems[0].Text)));
                }
            }

            if (mnuRPrintSorted.Checked) {
                entries.Sort((x, y) => x.Key.ToLower().CompareTo(y.Key.ToLower()));
            }

            foreach (KeyValuePair<string, int> entry in entries) {
                printRegistrationCard(entry.Value);
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

        private void mnuFSettings_Click(object sender, EventArgs e) {
            frmSettings f = new frmSettings();
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog(this);
            loadResultList();
            highlightInvalidRegistrationEntries();
        }

        private void cmsRCDeleteResult_Click(object sender, EventArgs e) {
            int resultId = int.Parse(lvResults.SelectedItems[0].SubItems[0].Text);

            ResultDao.delete(resultId);
            loadResultList();
        }

        private void btnRefreshStats_Click(object sender, EventArgs e) {
            loadStats();
        }

        private void loadStats() {
            try {
                Application.UseWaitCursor = true;
                lvStats.BeginUpdate();

                lvStats.Items.Clear();
                lvStats.Groups.Clear();

                ListViewGroup group = new ListViewGroup("");

                foreach (KeyValuePair<string, string> stat in ResultDao.getRegistrationStats()) {
                    if (stat.Key.StartsWith("GROUP")) {
                        group = new ListViewGroup(stat.Value);
                        lvStats.Groups.Add(group);
                    }
                    else {
                        ListViewItem item = new ListViewItem("", group);
                        item.SubItems.Add(stat.Key);
                        item.SubItems.Add(stat.Value);
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
            btnRegister_Click(sender, e);
        }

        private void mnuRPrint_Click(object sender, EventArgs e) {
            printEntryCards();
        }

        private void txtFilterTimer_Tick(object sender, EventArgs e) {
            _txtFilterTimer.Stop();
            btnFilter_Click(sender, e);
        }

        private void lvEntries_KeyDown(object sender, KeyEventArgs e) {
            int entryId;

            switch (e.KeyCode) {
                case Keys.Delete:
                    entryId = int.Parse(lvEntries.SelectedItems[0].SubItems[0].Text);
                    deleteRegistrationItem(entryId);
                    break;

                case Keys.Enter:
                    if (lvEntries.SelectedItems.Count == 0) {
                        return;
                    }

                    entryId = int.Parse(lvEntries.SelectedItems[0].SubItems[0].Text);
                    frmRegistrationEntry f = new frmRegistrationEntry();
                    f.loadEntry(entryId);
                    f.setParent(this);
                    f.ShowDialog(this);

                    RegistrationEntry entry = RegistrationEntryDao.get(entryId);

                    lvEntries.SelectedItems[0].SubItems[2].Text = entry.email;
                    lvEntries.SelectedItems[0].SubItems[3].Text = entry.firstName;
                    lvEntries.SelectedItems[0].SubItems[4].Text = entry.lastName;
                    lvEntries.SelectedItems[0].SubItems[5].Text = entry.clubName;
                    lvEntries.SelectedItems[0].SubItems[6].Text = entry.ageGroup;
                    lvEntries.SelectedItems[0].SubItems[7].Text = entry.modelName;
                    lvEntries.SelectedItems[0].SubItems[8].Text = entry.modelCategory;
                    lvEntries.SelectedItems[0].SubItems[9].Text = entry.modelScale;
                    lvEntries.SelectedItems[0].SubItems[10].Text = entry.modelPublisher;
                    lvEntries.SelectedItems[0].SubItems[11].Text = entry.modelClass;
                    lvEntries.SelectedItems[0].SubItems[12].Text = entry.yearOfBirth.ToString();

                    highlightInvalidRegistrationEntries();
                    break;
            }
        }

        private void btnClearSearch_Click(object sender, EventArgs e) {
            txtFilter.Text = "";
            loadRegistrationList(null);
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
                mnuRVStandard.Checked = false;
                mnuRVGroupped.Checked = true;
                lvStats.HeaderStyle = ColumnHeaderStyle.Nonclickable;

                Options.set("RegistrationView", "groupped");
            }
            else {
                mnuRVStandard.Checked = true;
                mnuRVGroupped.Checked = false;
                lvStats.HeaderStyle = ColumnHeaderStyle.Clickable;

                Options.set("RegistrationView", "standard");
            }
            loadRegistrationList(txtFilter.Text);
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

            loadRegistrationList(txtFilter.Text);
        }

        private void btnAddResults_Click(object sender, EventArgs e) {
            mnuJAddResults_Click(sender, e);
        }

        private void mnuRCategoryDiplomas_Click(object sender, EventArgs e) {
            DialogResult q = MessageBox.Show("Wydrukować dokumenty po utworzeniu?", "Dyplomy - Druk", MessageBoxButtons.YesNoCancel);
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

                DocHandler dc = new DocHandler();
                IEnumerable<Result> results;

                String templateFile = Resources.resolvePath("templateDyplomuKategorii");
                results = ResultDao.getCategoryResults();

                foreach (Result result in results) {
                    String outputFile = Path.Combine(outputDirectory, String.Format("dyplom_{0}.docx", result.resultId));
                    dc.generateDiploma(templateFile, outputFile, result);
                    if (printForms) {
                        dc.printWordDoc(outputFile);
                    }
                    incrementProgressBar();
                }
                showStripLabelMessage("Dokumenty gotowe");
                Process.Start(outputDirectory);
            }
            catch (Exception err) {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            loadResultList();
        }

        private void mnuRAwardDiplomas_Click(object sender, EventArgs e) {
            DialogResult q = MessageBox.Show("Wydrukować dokumenty po utworzeniu?", "Dyplomy - Druk", MessageBoxButtons.YesNoCancel);
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

                DocHandler dc = new DocHandler();
                IEnumerable<Result> results = ResultDao.getAwardResults();
                String templateFile = Resources.resolvePath("templateDyplomuNagrody");
                resetProgressBar(results.Count());

                foreach (Result result in results) {
                    String outputFile = Path.Combine(outputDirectory, String.Format("dyplom_{0}.docx", result.resultId));
                    dc.generateDiploma(templateFile, outputFile, result);
                    if (printForms) {
                        dc.printWordDoc(outputFile);
                    }
                    incrementProgressBar();
                }
                showStripLabelMessage("Dokumenty gotowe");
                Process.Start(outputDirectory);
            }
            catch (Exception err) {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            loadRegistrationList(txtFilter.Text);
            loadResultList();
            loadStats();
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

                    DocHandler dc = new DocHandler();
                    String templateFile = Resources.resolvePath("templateDyplomuKategorii");
                    String outputFile = Path.Combine(outputDirectory, String.Format("dyplom_{0}.docx", result.resultId));
                    File.Delete(outputFile);
                    dc.generateDiploma(templateFile, outputFile, result);
                    dc.printWordDoc(outputFile);

                    showStripLabelMessage("Dokument wysłany do druku");
                }
                else if (result.award != null) {

                    String outputDirectory = Path.Combine(Resources.resolvePath("folderDokumentów"), "dyplomy", "nagrody");
                    if (!Directory.Exists(outputDirectory)) {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    DocHandler dc = new DocHandler();
                    String templateFile = Resources.resolvePath("templateDyplomuNagrody");
                    String outputFile = Path.Combine(outputDirectory, String.Format("dyplom_{0}.docx", result.resultId));
                    File.Delete(outputFile);

                    dc.generateDiploma(templateFile, outputFile, result);
                    dc.printWordDoc(outputFile);

                    showStripLabelMessage("Dokument wysłany do druku");
                }
                else {
                    LogWriter.info("ERROR printing diploma: Result did not contain place or award");
                    MessageBox.Show("Wpis nie posiada numeru nagrody lub miejsca", "Błąd Aplikacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception err) {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                toolStripLabelSpring.Visible = false;
            }
            finally {
                Application.UseWaitCursor = false;
            }
        }
    }
}
