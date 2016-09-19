using Rejestracja.Controls;
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
        private ComboBox[] _cboModelCategory;
        private ComboBox[] _cboCategoryAction;
        private List<ComboBoxItem> _cboItems;

        public frmImportFile() {
            InitializeComponent();
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

            this._cboFields = new ComboBox[] { cboTimeStamp, cboEmail, cboFirstName, cboLastName, cboClubName, cboAgeGroup, cboYearOfBirth, cboModelName, cboModelCategory1, cboModelCategory2, cboModelCategory3, cboModelCategory4, cboModelCategory5, cboModelScale, cboModelPublisher };

            lvFilePreview.View = View.Details;
            lvFilePreview.Enabled = false;
            lvFilePreview.Sorting = SortOrder.None;
            lvFilePreview.GridLines = true;

            _cboModelCategory = new ComboBox[] { cboModelCategory1, cboModelCategory2, cboModelCategory3, cboModelCategory4, cboModelCategory5 };
            _cboCategoryAction = new ComboBox[] { cboCategoryAction2, cboCategoryAction3, cboCategoryAction4, cboCategoryAction5 };

            foreach (ComboBox cb in _cboModelCategory) {
                cb.SelectedIndexChanged += new EventHandler(cboModelCategory_SelectedIndexChanged);
                cb.Enabled = false;
            }
            _cboModelCategory[0].Enabled = true;

            String[] categoryActions = new String[] { "gdy pole puste", "osobny wpis" };
            foreach (ComboBox cb in _cboCategoryAction) {
                //cb.SelectedIndexChanged += new EventHandler(cboCategoryAction_SelectedIndexChanged);
                cb.Items.AddRange(categoryActions);
                cb.Enabled = false;
            }

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

        private void cboModelCategory_SelectedIndexChanged(object sender, EventArgs e) {
            int selItem = -1;
            for (int i = 0; i < _cboModelCategory.Length; i++) {
                if (_cboModelCategory[i] == sender) {
                    selItem = i;
                    break;
                }
            }

            //For the last of the category dropdowns just reset the action list
            if (selItem == 4 && _cboModelCategory[4].SelectedIndex < 1) {
                _cboCategoryAction[3].SelectedIndex = -1;
                return;
            }

            //If sel index < 1 ("not used") disable next item in line, reset action box
            if (_cboModelCategory[selItem].SelectedIndex < 1) {
                //Reset action box
                if (selItem > 0) {
                    _cboCategoryAction[selItem - 1].SelectedIndex = 0;
                }

                //Disable next items
                for (int i = selItem + 1; i < _cboModelCategory.Length; i++) {
                    _cboCategoryAction[i - 1].SelectedIndex = -1;
                    _cboCategoryAction[i - 1].Enabled = false;
                    _cboModelCategory[i].SelectedIndex = -1;
                    _cboModelCategory[i].Enabled = false;
                }
                return;
            }

            _cboCategoryAction[selItem].Enabled = true;
            _cboCategoryAction[selItem].SelectedIndex = 0;
            _cboModelCategory[selItem + 1].Enabled = true;
            _cboModelCategory[selItem + 1].SelectedIndex = 0;
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

                //Modeler
                fieldMap.Email = cboEmail.SelectedIndex - 1;
                fieldMap.FirstName = cboFirstName.SelectedIndex - 1;
                fieldMap.LastName = cboLastName.SelectedIndex - 1;
                fieldMap.ClubName = cboClubName.SelectedIndex - 1;
                fieldMap.AgeGroup = cboAgeGroup.SelectedIndex - 2;
                fieldMap.CalculateAgeGroup = (cboAgeGroup.SelectedIndex == 0);
                fieldMap.YearOfBirth = cboYearOfBirth.SelectedIndex - 1;

                //Scale, publisher, model
                fieldMap.ModelName = cboModelName.SelectedIndex - 1;
                fieldMap.ModelScale = cboModelScale.SelectedIndex - 1;
                fieldMap.ModelPublisher = cboModelPublisher.SelectedIndex - 1;

                //Add different possible category mapping
                List<int> categoryFieldMap = new List<int>();
                fieldMap.ModelCategory = new List<int[]>();

                categoryFieldMap.Add(cboModelCategory1.SelectedIndex - 1);
                for (int i = 1; i < _cboModelCategory.Length; i++) {
                    //If category not selected finish checking
                    if (_cboModelCategory[i].SelectedIndex < 1) {
                        break;
                    }

                    if (_cboCategoryAction[i - 1].SelectedIndex == 0) {
                        categoryFieldMap.Add(_cboModelCategory[i].SelectedIndex - 1);
                    }
                    else {
                        fieldMap.ModelCategory.Add(categoryFieldMap.ToArray());
                        categoryFieldMap = new List<int>();
                        categoryFieldMap.Add(_cboModelCategory[i].SelectedIndex - 1);
                    }
                }
                fieldMap.ModelCategory.Add(categoryFieldMap.ToArray());
                
                DataSource ds = new DataSource();
                if (chkDropExistingRecords.Checked) {
                    ds.dropRegistrationRecords();
                }
                
                String badRecordFile = Path.Combine(Resources.DataFileFolder, String.Format("bledy-import_{0}.csv", DateTime.Now.ToString("yyyy-MM-dd_H-mm-ss")));
                if (File.Exists(badRecordFile)) {
                    File.Delete(badRecordFile);
                }
                
                int badRecordCount = ds.bulkLoadRegistration(lblFileName.Text, fieldMap, chkHasHeaders.Checked, badRecordFile, chkAutoAddScales.Checked, chkAutoAddPublisher.Checked);
                if (badRecordCount > 0) {
                    MessageBox.Show("Wystąpiły błędy przy ładowaniu rejestracji. Dane których nie udało się importować zostały zapisane w pliku:\r\n" + badRecordFile, "Import Danych", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Process.Start(Resources.DataFileFolder);
                }

                ((frmMain)this.Owner).refreshScreen();
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
