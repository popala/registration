using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;
using Rejestracja.Data.Objects;
using Rejestracja.Data.Dao;

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
            btnSave.Visible = true;

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

            //Only seniors are allowed in non-standard category
            if (cboAgeGroup.Text.ToLower() != "senior" && cboModelClass.Text.ToLower() != "standard")
            {
                lblErrors.Text = "Tylko Senior powinien startować w kategorii Open.\n";
                lblErrors.Visible = true;
            }

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
            if (!validateEntry())
                return;
            
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
                MessageBox.Show(err.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //btnSave.Enabled = true;
            //validateExistingEntry();
        }

        private bool validateEntry()
        {
            if(txtFirstName.Text.Length == 0 || txtLastName.Text.Length ==0)
            {
                MessageBox.Show("Imię i nazwisko są wymagane", "Wymagane pola", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(txtModelName.Text.Length == 0 || String.IsNullOrWhiteSpace(cboModelPublisher.Text) || cboModelScale.SelectedIndex < 0)
            {
                MessageBox.Show("Nazwa, wydawca i skala modelu są wymagane", "Wymagane pola", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(cboModelCategory.SelectedIndex < 0)
            {
                MessageBox.Show("Kategoria modelu jest wymagana", "Wymagane pola", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (!validateEntry()) {
                return;
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

            RegistrationEntryDao.add(entry);
            txtEntryId.Text = entry.entryId.ToString();

            btnAddPrintModel.Enabled = false;

            if (!chkPrintRegistrationCard.Checked) {
                this._parentForm.printRegistrationCard(entry.entryId);
            }

            txtEntryId.Text = "";
            txtModelName.Text = "";
            cboModelPublisher.SelectedIndex = -1;
            btnAddPrintModel.Enabled = true;
            txtModelName.Focus();
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
