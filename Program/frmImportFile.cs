/*
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
using Rejestracja.Data.Objects;
using Rejestracja.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Rejestracja {
    public partial class frmImportFile : Form {
        
        private String _selectedFile;
        private List<String[]> _dataSample;
        private ComboBox [] _cboFields;
        private List<ComboBoxItem> _cboItems;

        public frmImportFile() {
            InitializeComponent();
        }

        public void resetMapping() {
            cboTimeStamp.SelectedIndex = -1;
        }

        public void previewFile(bool? hasHeaders) {

            if (this._selectedFile == null) {
                return;
            }

            lvFilePreview.Columns.Clear();
            lvFilePreview.Items.Clear();

            bool bHeaders = (hasHeaders.HasValue ? hasHeaders.Value : chkHasHeaders.Checked);
            this._dataSample = DataSource.getFileSample(this._selectedFile, "#", ",", true, 11);
            int columnCount = this._dataSample[0].Length;
            int i;

            //Populate the dropdown boxes
            this._cboItems = new List<ComboBoxItem>();
            this._cboItems.Add(new ComboBoxItem(-1, "Nie dodana"));
            for (i = 0; i < columnCount; i++) {
                this._cboItems.Add(new ComboBoxItem(i, String.Format("[Kolumna nr {1}]: {0}", this._dataSample[0][i], i + 1)));
            }

            foreach (ComboBox cboField in this._cboFields) {
                cboField.Items.Clear();
                if (cboField == cboAgeGroup) {
                    cboField.Items.Add(new ComboBoxItem(-2, "Oblicz grupę wiekową z roku urodzenia"));
                }
                else if (cboField == cboModelClass) {
                    cboField.Items.Add(new ComboBoxItem(-2, "Wybierz klasę w oparciu o kategorię modelu"));
                }
                foreach (ComboBoxItem item in this._cboItems) {
                    cboField.Items.Add(item);
                }
                cboField.SelectedIndex = 0;
            }

                //Populate the headers
            if (bHeaders) {
                foreach (String col in this._dataSample[0]) {
                    lvFilePreview.Columns.Add(col);
                }
            }
            else {
                for (i = 1; i <= columnCount; i++) {
                    lvFilePreview.Columns.Add(String.Format("Col {0}", i));
                }
            }

            foreach (String[] item in this._dataSample.Skip(bHeaders ? 1 : 0)) {
                lvFilePreview.Items.Add(new ListViewItem(item));
            }

            foreach (ColumnHeader header in lvFilePreview.Columns) {
                header.Width = -2;
            }

            lvFilePreview.Enabled = true;
            chkHasHeaders.Enabled = true;
            btnImport.Enabled = true;
            cboAgeGroup.SelectedIndex = 0;
            tabControl1.SelectedIndex = 1;
        }

        private void frmImportFile_Load(object sender, EventArgs e) {

            this._cboFields = new ComboBox[] { cboTimeStamp, cboEmail, cboFirstName, cboLastName, cboClubName, cboAgeGroup, cboYearOfBirth, cboModelName, cboModelCategory1, cboModelCategory2, cboModelCategory3, cboModelScale, cboModelClass, cboModelPublisher };

            lvFilePreview.View = View.Details;
            lvFilePreview.Enabled = false;
            lvFilePreview.Sorting = SortOrder.None;
            lvFilePreview.GridLines = true;

            lblFileName.Text = "";
            chkHasHeaders.Checked = true;
            chkDropExistingRecords.Checked = true;
            btnImport.Enabled = false;
        }

        private void selectFile() {
            OpenFileDialog ofDialog = new OpenFileDialog();
            ofDialog.Filter = "CSV Files (.csv)|*.csv";
            ofDialog.Multiselect = false;

            DialogResult result = ofDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
                lblFileName.Text = ofDialog.FileName;
                this._selectedFile = ofDialog.FileName;
                previewFile(null);
            }
        }

        private void cboModelCategory1_SelectedIndexChanged(object sender, EventArgs e) {
            if (cboModelCategory1.SelectedIndex < 1) {
                cboModelCategory3.SelectedIndex = -1;
                cboModelCategory3.Enabled = false;

                cboModelCategory2.SelectedIndex = -1;
                cboModelCategory2.Enabled = false;
            }
            else {
                cboModelCategory2.Enabled = true;
            }
        }

        private void cboModelCategory2_SelectedIndexChanged(object sender, EventArgs e) {
            if (cboModelCategory2.SelectedIndex < 1) {
                cboModelCategory3.SelectedIndex = -1;
                cboModelCategory3.Enabled = false;
            }
            else {
                cboModelCategory3.Enabled = true;
            }
        }

        private void cboField_SelectedIndexChanged(object sender, EventArgs e) {

            if (cboFirstName.SelectedIndex < 1 || cboLastName.SelectedIndex < 1 || cboYearOfBirth.SelectedIndex < 1 || cboModelName.SelectedIndex < 1 || cboModelCategory1.SelectedIndex < 1
                    || cboModelScale.SelectedIndex < 1) {
                lblImportMessage.Text = "Nie wszystkie wymagane pola zostały zmapowane.  Sprawdź ustawienia.";
            }
            else {
                lblImportMessage.Text = "";
            }
        }

        private void frmImportFile_Shown(object sender, EventArgs e) {
            selectFile();
        }

        private void btnSelectFile_Click(object sender, EventArgs e) {
            selectFile();
        }

        private void chkHasHeaders_Click(object sender, EventArgs e) {
            previewFile(chkHasHeaders.Checked);
        }

        private void btnImport_Click(object sender, EventArgs e) {

            if (chkDropExistingRecords.Checked) {
                if (MessageBox.Show("Import usunie dane w lokalnej bazie i nadpisze je danymi z importowanego pliku", "Import Danych Rejestracji", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.OK)
                    return;
            }
            else {
                if (MessageBox.Show("Import doda dane z importowanego pliku bez usuwania istniejących wpisów", "Import Danych Rejestracji", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.OK)
                    return;
            }

            Application.UseWaitCursor = true;

            btnCancel.Enabled = false;
            btnImport.Enabled = false;
            btnSelectFile.Enabled = false;
            chkHasHeaders.Enabled = false;
            tabControl1.Enabled = false;

            try {
                FileImportFieldMap fieldMap = new FileImportFieldMap();
                fieldMap.TimeStamp = cboTimeStamp.SelectedIndex - 1;
                fieldMap.Email = cboEmail.SelectedIndex - 1;

                fieldMap.FirstName = cboFirstName.SelectedIndex - 1;
                fieldMap.LastName = cboLastName.SelectedIndex - 1;
                fieldMap.ClubName = cboClubName.SelectedIndex - 1;

                fieldMap.AgeGroup = cboAgeGroup.SelectedIndex - 2;
                fieldMap.CalculateAgeGroup = (cboAgeGroup.SelectedIndex == 0);
                fieldMap.YearOfBirth = cboYearOfBirth.SelectedIndex - 1;

                fieldMap.ModelName = cboModelName.SelectedIndex - 1;
                fieldMap.ModelCategory = new int[] { cboModelCategory1.SelectedIndex - 1, cboModelCategory2.SelectedIndex - 1, cboModelCategory3.SelectedIndex - 1 };
                fieldMap.ModelScale = cboModelScale.SelectedIndex - 1;
                fieldMap.ModelPublisher = cboModelPublisher.SelectedIndex - 1;
                fieldMap.DeriveClassFromCategory = (cboModelClass.SelectedIndex == 0);
                fieldMap.ModelClass = cboModelClass.SelectedIndex - 2;

                DataSource ds = new DataSource();
                if (chkDropExistingRecords.Checked) {
                    ds.dropRegistrationRecords();
                }
                
                String badRecordFile = Path.Combine(Resources.DataFileFolder, String.Format("bledy-import_{0}.csv", DateTime.Now.ToString("yyyy-MM-dd_H-mm-ss")));
                if (File.Exists(badRecordFile)) {
                    File.Delete(badRecordFile);
                }
                
                int badRecordCount = ds.bulkLoadRegistration(lblFileName.Text, fieldMap, chkHasHeaders.Checked, badRecordFile);
                if (badRecordCount > 0) {
                    MessageBox.Show("Wystąpiły błędy przy ładowaniu rejestracji. Dane których nie udało się importować zostały zapisane w pliku:\r\n" + badRecordFile, "Import Danych", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Process.Start(Resources.DataFileFolder);
                }

                ((frmMain)this.Owner).populateUI();
                this.Close();
            }
            catch (Exception err) {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Błąd Aplikacji", MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnCancel.Enabled = true;
                btnImport.Enabled = true;
                btnSelectFile.Enabled = true;
                chkHasHeaders.Enabled = true;
                tabControl1.Enabled = true;
            }
            finally {
                Application.UseWaitCursor = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
