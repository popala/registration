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
using Rejestracja.Data.Dao;
using Rejestracja.Data.Objects;
using Rejestracja.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Rejestracja
{
    public partial class frmRegistrationEntry : Form
    {
        public frmMain _parentForm;
        private List<AgeGroup> _ageGroups;
        //private List<Category> _categories;
        //private List<Class> _classes;

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

            lvCategories.DoubleClickDoesCheck = false;
            resetControls();
        }

        private void loadCategoryList() {
            List<Category> categories = CategoryDao.getList(true).ToList();
            List<Class> classes = ClassDao.getList().ToList();
            classes.Add(new Class(-1, "Kategorie z importu"));

            foreach(Category category in categories) {
                if(category.id < 0) {
                    category.className = "Kategorie z importu";
                }
                Class c = this.classes.Find(x => x.name.Equals(category.className, StringComparison.CurrentCultureIgnoreCase));
                c.categories.Add(category);
            }

            //Categories broken up by class
            lvCategories.Clear();
            lvCategories.Columns.Add("Kod");
            lvCategories.Columns.Add("Kategoria");
            lvCategories.Columns.Add("Grupa Wiekowa");

            lvCategories.View = View.Details;
            lvCategories.GridLines = true;
            lvCategories.FullRowSelect = true;
            lvCategories.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvCategories.CheckBoxes = true;

            //Load classes plus extra one for imported categories that were not in the system
            foreach(Class cls in classes) {
                ListViewGroup group = new ListViewGroup(cls.name, cls.name);
                group.Tag = cls.id;
                lvCategories.Groups.Add(group);

                foreach(Category cat in cls.categories) {
                    ListViewItem item = new ListViewItem(new String[] { cat.code, cat.name, "" }, group);
                    item.Name = cat.name;
                    lvCategories.Items.Add(item);
                }
            }

            foreach(ColumnHeader header in lvCategories.Columns) {
                header.Width = -2;
            }
        }

        private void resetControls() {

            //General
            this._ageGroups = AgeGroupDao.getList().ToList();
            this.Text = "Nowa Rejestracja";
            loadCategoryList();

            //Modeler
            txtModelerId.Text = "";
            txtEmail.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtModelClub.Text = "Indywidualnie";
            
            cboYearOfBirth.Items.Clear();
            for (int i = 1900; i < DateTime.Now.Year; i++)
                cboYearOfBirth.Items.Add(new ComboBoxItem(i, i.ToString()));
            cboYearOfBirth.SelectedIndex = cboYearOfBirth.FindString("1980");

            btnNewRegistration.Visible = true;
            btnAddModeler.Visible = true;

            //Model
            cboModelPublisher.Items.Clear();
            foreach (String item in PublisherDao.getSimpleList())
                cboModelPublisher.Items.Add(item.Trim());
            cboModelPublisher.Sorted = true;

            cboModelScale.Items.Clear();
            foreach (String item in ScaleDao.getSimpleList())
                cboModelScale.Items.Add(item.Trim());
            cboModelScale.SelectedIndex = cboModelScale.FindString("1:33");

            btnNewModel.Visible = true;
            btnAddPrintModel.Visible = true;            
        }

        private void clear()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtModelClub.Text = "";
            cboYearOfBirth.SelectedIndex = cboYearOfBirth.FindString("1970");
            //txtEntryId.Text = "";
            cboEntryId.Items.Clear();
            txtModelName.Text = "";
            cboModelPublisher.SelectedIndex = -1;
            cboModelScale.SelectedIndex = cboModelScale.FindString("1:33");
        }

        public void loadModel(int registrationId, int modelId)
        {
            clear();

            Model model = null;

            if(modelId > -1) {
                model = ModelDao.get(modelId);
            }
            else {
                Registration reg = RegistrationDao.get(registrationId);
                if(reg != null) {
                    model = ModelDao.get(reg.modelId);
                }
            }

            if(model == null)
            {
                MessageBox.Show("Model/rejestracja nie znaleziona!");
                this.Close();
            }

            //Model info
            Modeler modeler = ModelerDao.get(model.modelerId);
            foreach(Model m in ModelDao.getList(model.modelerId)) {
                cboEntryId.Items.Add(new ComboBoxItem(m.id, m.id.ToString()));
                if(m.id == model.id) {
                    cboEntryId.SelectedIndex = cboEntryId.Items.Count - 1;
                    txtModelName.Text = m.name;
                    cboModelPublisher.SelectedIndex = cboModelPublisher.FindString(m.publisher.ToLower());
                    if(cboModelPublisher.SelectedIndex < 0)
                        cboModelPublisher.Text = m.publisher;
                    cboModelScale.SelectedIndex = cboModelScale.FindString(m.scale);
                }
            }

            //Modeler info
            txtModelerId.Text = modeler.id.ToString();
            txtFirstName.Text = modeler.firstName;
            txtLastName.Text = modeler.lastName;
            txtEmail.Text = modeler.email;
            txtModelClub.Text = modeler.clubName;
            cboYearOfBirth.SelectedIndex = cboYearOfBirth.FindString(modeler.yearOfBirth.ToString());

            //Registration
            foreach(ListViewItem item in lvCategories.CheckedItems) {
                item.Checked = false;
                item.SubItems[2].Text = "";
            }

            //Check the assigned categories
            //If model is not assigned to an imported category, not in the system, then hide the entire class
            bool removeImported = true;

            List<Registration> regList = RegistrationDao.getList(model.id).ToList();
            foreach(Registration reg in regList) {
                ListViewItem item = lvCategories.Items[lvCategories.Items.IndexOfKey(reg.categoryName)];
                item.Checked = true;
                item.SubItems[2].Text = reg.ageGroupName;
                if((int)item.Group.Tag < 0) {
                    removeImported = false;
                }
            }
            
            //Remove imported categories if they're not selected
            if(removeImported) {
                foreach(ListViewGroup group in lvCategories.Groups) {
                    if((int)group.Tag < 0) {
                        List<ListViewItem> itemsToRemove = new List<ListViewItem>();
                        foreach(ListViewItem item in group.Items) {
                            itemsToRemove.Add(item);
                        }
                        foreach(ListViewItem item in itemsToRemove) {
                            lvCategories.Items.Remove(item);
                        }
                    }
                }
            }
            
            btnAddPrintModel.Visible = false;
            btnNewModel.Visible = false;
            btnNewRegistration.Visible = false;
            chkPrintRegistrationCard.Visible = false;

            this.Text = "Szczegóły Rejestracji";
            validateExistingEntry();
        }

        private void validateExistingEntry()
        {
            IEnumerable<Category> modelCategories = CategoryDao.getList();
            bool bFound = false;

            lblErrors.Text = "";
            lblErrors.Visible = false;

            //foreach (Category category in modelCategories)
            //{
            //    if (category.fullName.ToLower().Equals(selectedCategory))
            //    {
            //        bFound = true;
            //        break;
            //    }
            //}
            //if (!bFound)
            //{
            //    lblErrors.Text += "Kategoria modelu nie znaleziona w konfiguracji\n";
            //    lblErrors.Visible = true;
            //}
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!validateEntry()) {
                return;
            }

            if (cboModelScale.SelectedIndex < 0) {
                string scale = cboModelScale.Text.Trim();
                if (MessageBox.Show("Dodać wpisaną skalę do bazy?", "Skala Nie Znaleziona", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes) {
                    ScaleDao.add(scale, ScaleDao.getNextSortFlag());

                    cboModelScale.Items.Clear();
                    foreach (String item in ScaleDao.getSimpleList())
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
            //if (cboModelCategory.SelectedIndex > -1) {
            //    categoryId = ((ComboBoxItem)cboModelCategory.SelectedItem).id;
            //}

            try
            {
                //RegistrationEntry entry =
                //    new RegistrationEntry(
                //        long.Parse(txtEntryId.Text),
                //        txtEmail.Text,
                //        txtFirstName.Text,
                //        txtLastName.Text,
                //        txtModelClub.Text,
                //        txtModelName.Text,
                //        cboModelScale.Text,
                //        cboModelPublisher.Text,
                //        categoryId,
                //        int.Parse(cboYearOfBirth.Text));

                //RegistrationEntryDao.update(entry);
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
            //if (cboModelCategory.SelectedIndex < 0) {
            //    MessageBox.Show("Kategoria modelu jest wymagana", "Wymagane pola", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return false;
            //}

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
                //cboAgeGroup.SelectedIndex = -1;
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

            //AgeGroup[] selectedAgeGroup = this._ageGroups.Where(x => x.upperAge >= age && x.bottomAge <= age).ToArray<AgeGroup>();
            //if (selectedAgeGroup.Length > 0) {
            //    cboAgeGroup.SelectedIndex = cboAgeGroup.FindString(selectedAgeGroup[0].name);
            //}
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
                        ScaleDao.add(scale, ScaleDao.getNextSortFlag());

                        cboModelScale.Items.Clear();
                        foreach (String item in ScaleDao.getSimpleList())
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

                //long categoryId = -1;
                //if (cboModelCategory.SelectedIndex > -1) {
                //    categoryId = ((ComboBoxItem)cboModelCategory.SelectedItem).id;
                //}

                //RegistrationEntry entry =
                //    new RegistrationEntry(
                //        DateTime.Now,
                //        txtEmail.Text,
                //        txtFirstName.Text,
                //        txtLastName.Text,
                //        txtModelClub.Text,
                //        cboAgeGroup.Text,
                //        txtModelName.Text,
                //        cboModelClass.Text,
                //        cboModelScale.Text,
                //        cboModelPublisher.Text,
                //        cboModelCategory.Text,
                //        categoryId,
                //        int.Parse(cboYearOfBirth.Text)
                //    );

                btnAddPrintModel.Enabled = false;
                btnNewModel.Enabled = false;
                btnNewRegistration.Enabled = false;
                btnClose.Enabled = false;

                //RegistrationEntryDao.add(entry);
                //txtEntryId.Text = entry.registrationId.ToString();

                //if (!chkPrintRegistrationCard.Checked) {
                //    this._parentForm.printRegistrationCard(entry.registrationId);
                //}

                cboEntryId.SelectedIndex = -1;
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
            cboEntryId.SelectedIndex = -1;
            txtModelName.Text = "";
            cboModelPublisher.SelectedIndex = -1;
            btnAddPrintModel.Enabled = true;
        }

        private void btnNewRegistration_Click(object sender, EventArgs e) {
            resetControls();
            txtEmail.Focus();
        }

        private void lvCategories_ItemCheck(object sender, ItemCheckEventArgs e) {
            
            ListViewItem checkedItem = lvCategories.Items[e.Index];
            ListViewGroup group = checkedItem.Group;

            if(checkedItem.ForeColor == System.Drawing.Color.LightGray) {
                e.NewValue = CheckState.Unchecked;
                return;
            }

            if(!checkedItem.Checked) {
                
                Modeler modeler = ModelerDao.get(int.Parse(txtModelerId.Text));
                int age = DateTime.Now.Year - modeler.yearOfBirth;    
                String ageGroupName = _ageGroups.Where(x => x.bottomAge <= age && x.upperAge >= age).ToArray()[0].name;

                checkedItem.BackColor = System.Drawing.Color.AliceBlue;
                checkedItem.SubItems[2].Text = ageGroupName;

                foreach(ListViewItem item in group.Items) {
                    if(item != checkedItem) {
                        item.ForeColor = System.Drawing.Color.LightGray;
                    }
                }
                checkedItem.EnsureVisible();
            }
            else {
                checkedItem.BackColor = System.Drawing.Color.White;
                checkedItem.SubItems[2].Text = "";

                foreach(ListViewItem item in group.Items) {
                    if(item != checkedItem) {
                        item.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
        }

        private void lvCategories_MouseDoubleClick(object sender, MouseEventArgs e) {
            ListViewHitTestInfo hitTest = this.lvCategories.HitTest(e.X, e.Y);
            if(hitTest.Item != null) {
                ListViewItem item = hitTest.Item;

                if(item.ForeColor == System.Drawing.Color.LightGray) {
                    return;
                }

                try {
                    lvCategories.BeginUpdate();

                    int index = item.Index;
                    if(item.SubItems[2].Text.Length > 0) {
                        String ageGroup = item.SubItems[2].Text;
                        int idx = this._ageGroups.FindIndex(x => x.name.Equals(ageGroup, StringComparison.CurrentCultureIgnoreCase));
                        if(idx < 0 || (idx + 1) == this._ageGroups.Count) {
                            item.SubItems[2].Text = this._ageGroups[0].name;
                        }
                        else {
                            item.SubItems[2].Text = this._ageGroups[++idx].name;
                        }
                    }
                    else if(this._ageGroups.Count > 0) {
                        item.SubItems[2].Text = this._ageGroups[0].name;
                    }
                }
                catch(Exception err) {
                    LogWriter.error(err);
                    MessageBox.Show(err.Message, "Błąd Aplikacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally {
                    lvCategories.EndUpdate();
                }
            }
        }
    }
}
