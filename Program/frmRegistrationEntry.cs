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
using Rejestracja.Data.Dao;
using Rejestracja.Data.Objects;
using Rejestracja.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Rejestracja
{
    public partial class frmRegistrationEntry : Form
    {
        private RegistrationEntry _entry;
        public frmMain _parentForm;
        private List<AgeGroup> _ageGroups;

        public void setParent(frmMain parentForm) {
            this._parentForm = parentForm;
        }

        public frmRegistrationEntry()
        {
            InitializeComponent();

            foreach (Control ctl in this.Controls)
            {
                if (ctl is TextBox)
                {
                    ctl.GotFocus += new System.EventHandler(this.textBox_Focus);
                }
                else if (ctl is ComboBox)
                {
                    ctl.GotFocus += new System.EventHandler(this.comboBox_Focus);
                }
            }

            resetControls();
        }

        private void resetControls() {

            txtModelClub.Text = "Indywidualnie";

            _ageGroups = AgeGroupDao.getList().ToList();
            cboAgeGroup.Items.Clear();
            foreach(AgeGroup item in _ageGroups)
                cboAgeGroup.Items.Add(new ComboBoxItem(item.id, item.name));

            cboYearOfBirth.Items.Clear();
            for (int i = 1900; i < 2015; i++)
                cboYearOfBirth.Items.Add(i.ToString());
            cboYearOfBirth.SelectedIndex = cboYearOfBirth.FindString("1970");

            cboModelPublisher.Items.Clear();
            foreach (String item in PublisherDao.getSimpleList())
                cboModelPublisher.Items.Add(item.Trim());
            cboModelPublisher.Sorted = true;

            cboModelScale.Items.Clear();
            foreach (String item in ModelScaleDao.getSimpleList())
                cboModelScale.Items.Add(item.Trim());
            cboModelScale.SelectedIndex = cboModelScale.FindString("1:33");

            cboModelCategory.Items.Clear();
            foreach (ModelCategory item in ModelCategoryDao.getList())
                cboModelCategory.Items.Add(new ComboBoxItem(item.id, item.fullName));
            if (cboModelCategory.Items.Count > 0) {
                cboModelCategory.SelectedIndex = 0;
            }

            cboModelClass.Items.Clear();
            foreach (String item in ModelClassDao.getSimpleList())
                cboModelClass.Items.Add(item.Trim());
            cboModelClass.SelectedIndex = cboModelClass.FindString("standard");
            cboModelClass.Enabled = false;

            btnAddPrintModel.Visible = true;
            btnNewModel.Visible = true;
            btnSave.Visible = false;

            this.Text = "Nowa Rejestracja";
        }

        private void clear()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtModelClub.Text = "";
            cboYearOfBirth.SelectedIndex = cboYearOfBirth.FindString("1970");
            cboAgeGroup.SelectedIndex = cboAgeGroup.FindString("senior");
            txtEntryId.Text = "";
            txtModelName.Text = "";
            cboModelPublisher.SelectedIndex = -1;
            cboModelScale.SelectedIndex = cboModelScale.FindString("1:33");
            if (cboModelCategory.Items.Count > 0) {
                cboModelCategory.SelectedIndex = 0;
            }
            cboModelClass.SelectedIndex = cboModelClass.FindString("standard");
        }

        public void loadEntry(int entryId)
        {
            clear();

            RegistrationEntry entry = RegistrationEntryDao.get(entryId);

            if(entry == null)
            {
                MessageBox.Show("Numer startowy nie figuruje w bazie!");
                this.Close();
            }

            txtFirstName.Text = entry.firstName;
            txtLastName.Text = entry.lastName;
            txtEmail.Text = entry.email;
            txtModelClub.Text = entry.clubName;
            cboYearOfBirth.SelectedIndex = cboYearOfBirth.FindString(entry.yearOfBirth.ToString());
            cboAgeGroup.SelectedIndex = cboAgeGroup.FindString(entry.ageGroup);
            txtEntryId.Text = entry.entryId.ToString();
            txtModelName.Text = entry.modelName;
            cboModelPublisher.SelectedIndex = cboModelPublisher.FindString(entry.modelPublisher.ToLower());
            if (cboModelPublisher.SelectedIndex < 0)
                cboModelPublisher.Text = entry.modelPublisher;
            cboModelScale.SelectedIndex = cboModelScale.FindString(entry.modelScale);
            cboModelCategory.SelectedIndex = cboModelCategory.FindString(entry.modelCategory);
            cboModelClass.SelectedIndex = cboModelClass.FindString(entry.modelClass);

            btnAddPrintModel.Visible = false;
            btnNewModel.Visible = false;
            btnNewRegistration.Visible = false;
            btnSave.Visible = true;
            chkPrintRegistrationCard.Visible = false;

            this._entry = entry;
            this.Text = "Szczegóły Rejestracji";

            validateExistingEntry();
        }

        private void validateExistingEntry()
        {
            IEnumerable<ModelCategory> modelCategories = ModelCategoryDao.getList();
            String selectedCategory = cboModelCategory.Text.ToLower();
            bool bFound = false;

            lblErrors.Text = "";
            lblErrors.Visible = false;

            ////Only seniors are allowed in non-standard category
            //if (cboAgeGroup.Text.ToLower() != "senior" && cboModelClass.Text.ToLower() != "standard")
            //{
            //    lblErrors.Text = "Tylko Senior powinien startować w kategorii Open.\n";
            //    lblErrors.Visible = true;
            //}

            foreach (ModelCategory category in modelCategories)
            {
                if (category.fullName.ToLower().Equals(selectedCategory))
                {
                    bFound = true;
                    break;
                }
            }
            if (!bFound)
            {
                lblErrors.Text += "Kategoria modelu nie znaleziona w konfiguracji\n";
                lblErrors.Visible = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!validateEntry()) {
                return;
            }

            if (cboModelScale.SelectedIndex < 0) {
                string scale = cboModelScale.Text.Trim();
                if (MessageBox.Show("Dodać wpisaną skalę do bazy?", "Skala Nie Znaleziona", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes) {
                    ModelScaleDao.add(scale, ModelScaleDao.getNextSortFlag());

                    cboModelScale.Items.Clear();
                    foreach (String item in ModelScaleDao.getSimpleList())
                        cboModelScale.Items.Add(item.Trim());
                    cboModelScale.SelectedIndex = cboModelScale.FindString(scale);
                }
            }

            if (cboModelPublisher.SelectedIndex < 0) {
                string publisher = cboModelPublisher.Text.Trim();
                if (MessageBox.Show("Dodać wpisanego wydawcę do bazy?", "Wydawca Nie Znaleziony", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes) {
                    PublisherDao.add(publisher);

                    cboModelPublisher.Items.Clear();
                    foreach (String item in PublisherDao.getSimpleList())
                        cboModelPublisher.Items.Add(item.Trim());
                    cboModelPublisher.Sorted = true;
                    cboModelPublisher.SelectedIndex = cboModelPublisher.FindString(publisher);
                }
            }
            
            btnSave.Enabled = false;

            long categoryId = -1;
            if (cboModelCategory.SelectedIndex > -1) {
                categoryId = ((ComboBoxItem)cboModelCategory.SelectedItem).id;
            }

            try
            {
                RegistrationEntry entry =
                    new RegistrationEntry(
                        long.Parse(txtEntryId.Text),
                        txtEmail.Text,
                        txtFirstName.Text,
                        txtLastName.Text,
                        txtModelClub.Text,
                        cboAgeGroup.Text,
                        txtModelName.Text,
                        cboModelClass.Text,
                        cboModelScale.Text,
                        cboModelPublisher.Text,
                        cboModelCategory.Text,
                        categoryId,
                        int.Parse(cboYearOfBirth.Text));

                RegistrationEntryDao.update(entry);
                this.Close();
            }
            catch(Exception err)
            {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Błąd Aplikacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool validateEntry()
        {
            if (cboModelCategory.SelectedIndex < 0) {
                MessageBox.Show("Kategoria modelu jest wymagana", "Wymagane pola", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (txtFirstName.Text.Length == 0 || txtLastName.Text.Length == 0) {
                MessageBox.Show("Imię i nazwisko są wymagane", "Wymagane pola", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (txtModelName.Text.Length == 0) {
                MessageBox.Show("Nazwa modelu jest wymagana", "Wymagane pola", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (cboModelScale.Text.Length == 0) {
                MessageBox.Show("Wpisz lub wybierz skalę", "Nowa Rejestracja", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (cboModelPublisher.Text.Length == 0) {
                MessageBox.Show("Wpisz lub wybierz wydawcę", "Nowa Rejestracja", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            int age = 0;
            if(!int.TryParse(cboYearOfBirth.Text, out age))
            {
                MessageBox.Show("Poprawny rok urodzenia jest wymagany", "Wymagane pola", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox_Focus(object sender, EventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        private void comboBox_Focus(object sender, EventArgs e)
        {
            (sender as ComboBox).SelectAll();
        }

        private void cboYearOfBirth_SelectedIndexChanged(object sender, EventArgs e)
        {
            int age = -1;
            int yearOfBirth = 0;

            if(!int.TryParse(cboYearOfBirth.Text, out yearOfBirth))
            {
                cboAgeGroup.SelectedIndex = -1;
                return;
            }

            age = DateTime.Now.Year - yearOfBirth;

            if(age < 0)
            {
                MessageBox.Show("Rok urodzenia nie może być w przyszłości", "Wymagane Pola", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboYearOfBirth.SelectAll();
                cboYearOfBirth.Focus();
                return;
            }

            AgeGroup[] selectedAgeGroup = this._ageGroups.Where(x => x.upperAge >= age && x.bottomAge <= age).ToArray<AgeGroup>();
            if (selectedAgeGroup.Length > 0) {
                cboAgeGroup.SelectedIndex = cboAgeGroup.FindString(selectedAgeGroup[0].name);
            }
        }

        private void btnAddPrintModel_Click(object sender, EventArgs e)
        {
            try {
                if (!validateEntry()) {
                    return;
                }

                if (cboModelScale.SelectedIndex < 0) {
                    string scale = cboModelScale.Text.Trim();
                    if (MessageBox.Show("Dodać wpisaną skalę do bazy?", "Skala Nie Znaleziona", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes) {
                        ModelScaleDao.add(scale, ModelScaleDao.getNextSortFlag());

                        cboModelScale.Items.Clear();
                        foreach (String item in ModelScaleDao.getSimpleList())
                            cboModelScale.Items.Add(item.Trim());
                        cboModelScale.SelectedIndex = cboModelScale.FindString(scale);
                    }
                }

                if (cboModelPublisher.SelectedIndex < 0) {
                    string publisher = cboModelPublisher.Text.Trim();
                    if (MessageBox.Show("Dodać wpisanego wydawcę do bazy?", "Wydawca Nie Znaleziony", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes) {
                        PublisherDao.add(publisher);
                        
                        cboModelPublisher.Items.Clear();
                        foreach (String item in PublisherDao.getSimpleList())
                            cboModelPublisher.Items.Add(item.Trim());
                        cboModelPublisher.Sorted = true;
                        cboModelPublisher.SelectedIndex = cboModelPublisher.FindString(publisher);
                    }
                }

                long categoryId = -1;
                if (cboModelCategory.SelectedIndex > -1) {
                    categoryId = ((ComboBoxItem)cboModelCategory.SelectedItem).id;
                }

                RegistrationEntry entry =
                    new RegistrationEntry(
                        DateTime.Now,
                        txtEmail.Text,
                        txtFirstName.Text,
                        txtLastName.Text,
                        txtModelClub.Text,
                        cboAgeGroup.Text,
                        txtModelName.Text,
                        cboModelClass.Text,
                        cboModelScale.Text,
                        cboModelPublisher.Text,
                        cboModelCategory.Text,
                        categoryId,
                        int.Parse(cboYearOfBirth.Text)
                    );

                btnAddPrintModel.Enabled = false;
                btnNewModel.Enabled = false;
                btnNewRegistration.Enabled = false;
                btnClose.Enabled = false;

                RegistrationEntryDao.add(entry);
                txtEntryId.Text = entry.entryId.ToString();

                if (!chkPrintRegistrationCard.Checked) {
                    this._parentForm.printRegistrationCard(entry.entryId);
                }

                txtEntryId.Text = "";
                txtModelName.Text = "";
                cboModelPublisher.SelectedIndex = -1;
                btnAddPrintModel.Enabled = true;
                txtModelName.Focus();
            }
            catch (Exception err) {
                LogWriter.error(err);
                MessageBox.Show(err.Message, "Błąd Aplikacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally {
                btnAddPrintModel.Enabled = true;
                btnNewModel.Enabled = true;
                btnNewRegistration.Enabled = true;
                btnClose.Enabled = true;
            }
        }

        private void btnNewModel_Click(object sender, EventArgs e)
        {
            txtEntryId.Text = "";
            txtModelName.Text = "";
            cboModelPublisher.SelectedIndex = -1;
            btnAddPrintModel.Enabled = true;
        }

        private void cboModelCategory_SelectedIndexChanged(object sender, EventArgs e) {

            if (cboModelCategory.SelectedIndex > -1) {
                long catId = ((ComboBoxItem)cboModelCategory.SelectedItem).id;
                ModelCategory cat = ModelCategoryDao.get(catId);
                cboModelClass.SelectedIndex = cboModelClass.FindString(cat.modelClass);
            }
            else {
                cboModelClass.SelectedIndex = -1;
            }
        }

        private void btnNewRegistration_Click(object sender, EventArgs e) {
            resetControls();
            txtEmail.Focus();
        }
    }
}
