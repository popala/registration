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
        private bool _loading = false;
        
        private const String IMPORTED_CLS_TITLE = "Kategorie z importu";

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
            resetAddControls();
        }

        private void loadCategoryList() {

            lvCategories.BeginUpdate();

            List<Category> categories = CategoryDao.getList(true).ToList();
            List<Class> classes = ClassDao.getList().ToList();
            classes.Add(new Class(-1, IMPORTED_CLS_TITLE));

            foreach(Category category in categories) {
                if(category.id < 0) {
                    category.className = IMPORTED_CLS_TITLE;
                }
                Class c = classes.Find(x => x.name.Equals(category.className, StringComparison.CurrentCultureIgnoreCase));
                c.categories.Add(category);
            }

            //Categories broken up by class
            lvCategories.Clear();
            lvCategories.Columns.Add("Kod");
            lvCategories.Columns.Add("Kat.Id");
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

                foreach(Category category in cls.categories) {
                    ListViewItem item = new ListViewItem(new String[] { category.code, category.id.ToString(), category.name, "" }, group);
                    if(category.id > -1) {
                        item.Name = category.id.ToString();
                    }
                    else {
                        item.Name = category.name;
                    }
                    item.UseItemStyleForSubItems = false;
                    lvCategories.Items.Add(item);
                }
            }

            foreach(ColumnHeader header in lvCategories.Columns) {
                header.Width = -2;
            }
            lvCategories.Columns[1].Width = 0;

            lvCategories.EndUpdate();
        }

        private void resetAddControls() {

            //General
            this._ageGroups = AgeGroupDao.getList().ToList();
            this.Text = "Nowa Rejestracja";
            loadCategoryList();

            //Modeler
            txtModelerId.Text = "";
            txtEmail.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            if(txtModelClub.Text.Length == 0) {
                txtModelClub.Text = "Indywidualnie";
            }
            
            cboYearOfBirth.Items.Clear();
            for (int i = 1900; i < DateTime.Now.Year; i++)
                cboYearOfBirth.Items.Add(new ComboBoxItem(i, i.ToString()));
            cboYearOfBirth.SelectedIndex = cboYearOfBirth.FindString("1980");

            btnNewRegistration.Visible = true;

            //Model
            cboModelId.Enabled = false;
            cboModelPublisher.Items.Clear();
            foreach (String item in PublisherDao.getSimpleList())
                cboModelPublisher.Items.Add(item.Trim());
            cboModelPublisher.Sorted = true;

            cboModelScale.Items.Clear();
            foreach (String item in ScaleDao.getSimpleList())
                cboModelScale.Items.Add(item.Trim());
            cboModelScale.SelectedIndex = cboModelScale.FindString("1:33");

            chkPrintRegistrationCard.Checked = true;

            btnNewModel.Visible = true;
            btnAddPrintModel.Visible = true;            
        }

        private void loadModeler(int modelerId) {
            
            Modeler modeler = ModelerDao.get(modelerId);

            //Load modeler info
            txtModelerId.Text = modeler.id.ToString();
            txtFirstName.Text = modeler.firstName;
            txtLastName.Text = modeler.lastName;
            txtEmail.Text = modeler.email;
            txtModelClub.Text = modeler.clubName;
            cboYearOfBirth.SelectedIndex = cboYearOfBirth.FindString(modeler.yearOfBirth.ToString());

            //int age = DateTime.Now.Year - ((ComboBoxItem)cboYearOfBirth.SelectedItem).id;
            //lblAgeGroup.Text = _ageGroups.Where(x => x.bottomAge <= age && x.upperAge >= age).ToArray()[0].name;

            //Load model IDs registered to the modeler
            cboModelId.Items.Clear();
            foreach(Model m in ModelDao.getList(modelerId)) {
                cboModelId.Items.Add(new ComboBoxItem(m.id, m.id.ToString()));
            }
        }

        private void loadModel(int modelId, Model model) {

            Model m;

            if(modelId > -1) {
                m = ModelDao.get(modelId);
            }
            else {
                m = model;
            }

            //Load model textboxes
            cboModelId.SelectedIndex = cboModelId.FindString(m.id.ToString());
            txtModelName.Text = m.name;
            cboModelPublisher.SelectedIndex = cboModelPublisher.FindString(m.publisher.ToLower());
            if(cboModelPublisher.SelectedIndex < 0) {
                cboModelPublisher.Text = m.publisher;
            }
            cboModelScale.SelectedIndex = cboModelScale.FindString(m.scale);

            //Load registration info if any available
            //Check the assigned categories. If model is not assigned to an imported category, not in the system, then hide the entire class
            loadCategoryList();

            bool removeImported = true;
            List<ListViewItem> itemsToDisable;
            List<Registration> regList = RegistrationDao.getList(m.id).ToList();

            int age = DateTime.Now.Year - ((ComboBoxItem)cboYearOfBirth.SelectedItem).id;
            String properAgeGroup = _ageGroups.Where(x => x.bottomAge <= age && x.upperAge >= age).ToArray()[0].name;

            //Compare on category Id, only compare on name if ID = -1
            //Disable the rest of the group for the checked registration
            foreach(Registration reg in regList) {
                
                ListViewItem cat = null;

                if(reg.categoryId > -1) {
                    cat = lvCategories.Items[lvCategories.Items.IndexOfKey(reg.categoryId.ToString())];
                }
                else {
                    cat = lvCategories.Items[lvCategories.Items.IndexOfKey(reg.categoryName)];
                }
                
                cat.Checked = true;
                cat.SubItems[3].Text = reg.ageGroupName;
                cat.Tag = String.Format("{0}", reg.id);

                if(!properAgeGroup.Equals(reg.ageGroupName, StringComparison.CurrentCultureIgnoreCase)) {
                    cat.UseItemStyleForSubItems = false;
                    cat.SubItems[3].Font = new System.Drawing.Font(cat.Font, System.Drawing.FontStyle.Bold);
                    cat.SubItems[3].ForeColor = System.Drawing.Color.Red;
                }

                if((int)cat.Group.Tag < 0) {
                    removeImported = false;
                }

                //Disabled categories in class if one category is already selected
                itemsToDisable = new List<ListViewItem>();
                foreach(ListViewItem item in cat.Group.Items) {
                    if(item != cat) {
                        item.UseItemStyleForSubItems = true;
                        item.ForeColor = System.Drawing.Color.LightGray;
                    }
                }
            }

            //Remove imported categories if they're not selected
            if(removeImported) {
                foreach(ListViewGroup group in lvCategories.Groups) {
                    if((int)group.Tag < 0) {
                        itemsToDisable = new List<ListViewItem>();
                        foreach(ListViewItem item in group.Items) {
                            itemsToDisable.Add(item);
                        }
                        foreach(ListViewItem item in itemsToDisable) {
                            lvCategories.Items.Remove(item);
                        }
                    }
                }
            }
        }

        public void loadRegistration(int modelId)
        {
            this._loading = true;

            //Populate registration info
            Model model = ModelDao.get(modelId);

            if(model == null)
            {
                MessageBox.Show("Model nie znaleziony!");
                this.Close();
            }
            
            loadModeler(model.modelerId);
            loadModel(-1, model);
            
            //Reset controls
            btnNewModel.Visible = false;
            btnNewRegistration.Visible = false;
            chkPrintRegistrationCard.Checked = false;
            cboModelId.Enabled = true;

            btnAddPrintModel.Text = "Zapisz zmiany";
            btnAddPrintModel.Enabled = true;

            this.Text = "Szczegóły Rejestracji";
            validateExistingEntry();
        }

        private bool modelerChanged() {
            Modeler updatedModeler = new Modeler(int.Parse(txtModelerId.Text), txtFirstName.Text, txtLastName.Text, txtModelClub.Text, ((ComboBoxItem)cboYearOfBirth.SelectedItem).id, txtEmail.Text);
            Modeler currentModeler = ModelerDao.get(updatedModeler.id);

            return !currentModeler.Equals(updatedModeler);

        }

        private bool modelChanged() {
            Model updatedModel = new Model(((ComboBoxItem)cboModelId.SelectedItem).id, txtModelName.Text, cboModelPublisher.Text, cboModelScale.Text, int.Parse(txtModelerId.Text));
            Model currentModel = ModelDao.get(updatedModel.id);

            return !currentModel.Equals(updatedModel);
        }

        private bool registrationChanged() {
            List<Registration> updatedRegistration = new List<Registration>();
            List<Registration> currentReg = RegistrationDao.getList(((ComboBoxItem)cboModelId.SelectedItem).id).ToList();

            foreach(ListViewItem item in lvCategories.Items) {
                if(item.Checked) {
                    updatedRegistration.Add(
                        new Registration(
                            DateTime.Now,
                            ((ComboBoxItem)cboModelId.SelectedItem).id,
                            int.Parse(item.SubItems[1].Text),
                            null,
                            item.SubItems[3].Text)
                    );
                }
                else {
                    if(item.Tag != null) {
                        return true;
                    }
                }
            }

            if(updatedRegistration.Count != currentReg.Count) {
                return true;
            }

            foreach(Registration reg in updatedRegistration) {
                int count = currentReg.Where(x => x.categoryId == reg.categoryId && x.ageGroupName == reg.ageGroupName).Count();
                if(count != 1) {
                    return true;
                }
            }

            return false;
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

        private bool validateNewEntry()
        {
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

        private void cboYearOfBirth_TextChanged(object sender, EventArgs e) {
            int age = -1;
            int yearOfBirth = 0;
            lblAgeGroup.Text = "";

            if(cboYearOfBirth.Text.Length < 4) {
                return;
            }

            if(!int.TryParse(cboYearOfBirth.Text, out yearOfBirth)) {
                return;
            }

            age = DateTime.Now.Year - yearOfBirth;

            if(age < 0) {
                MessageBox.Show("Rok urodzenia nie może być w przyszłości", "Wymagane Pola", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboYearOfBirth.SelectAll();
                cboYearOfBirth.Focus();
                return;
            }
            lblAgeGroup.Text = _ageGroups.Where(x => x.bottomAge <= age && x.upperAge >= age).ToArray()[0].name;
        }

        private void saveChanges()
        {
            int modelId = ((ComboBoxItem)cboModelId.SelectedItem).id;

            if(modelerChanged()) {
                ModelerDao.update(int.Parse(txtModelerId.Text), txtFirstName.Text, txtLastName.Text, txtModelClub.Text, ((ComboBoxItem)cboYearOfBirth.SelectedItem).id, txtEmail.Text);
            }
            if(modelChanged()) {
                ModelDao.update(modelId, txtModelName.Text, cboModelPublisher.Text, cboModelScale.Text);
            }
            if(registrationChanged()) {
                DateTime now = DateTime.Now;
                foreach(Registration reg in RegistrationDao.getList(modelId).ToList()) {
                    RegistrationDao.delete(reg.id);
                }
                foreach(ListViewItem item in lvCategories.Items) {
                    if(item.Checked) {
                        RegistrationDao.add(now, modelId, int.Parse(item.SubItems[1].Text), null, item.SubItems[3].Text);
                    }
                }
            }
        }

        private void btnAddPrintModel_Click(object sender, EventArgs e)
        {
            try {
                if (!validateNewEntry()) {
                    return;
                }

                //If this is not a new registration but an update, save changes and exit
                if(btnAddPrintModel.Text.Equals("Zapisz Zmiany", StringComparison.CurrentCultureIgnoreCase)) {
                    saveChanges();
                    return;
                }

                if (cboModelScale.SelectedIndex < 0) {
                    string scale = cboModelScale.Text.Trim();
                    if (MessageBox.Show("Dodać wpisaną skalę do bazy?", "Skala Nie Znaleziona", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes) {

                        if(scale.Contains(":")) {
                            scale = Rejestracja.Data.Objects.Scale.parse(scale);
                        }
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

                int modelerId;

                //Add modeler
                if(txtModelerId.Text.Length == 0) {
                    modelerId = ModelerDao.add(txtFirstName.Text, txtLastName.Text, txtModelClub.Text, ((ComboBoxItem)cboYearOfBirth.SelectedItem).id, txtEmail.Text);
                    txtModelerId.Text = modelerId.ToString();
                }
                else {
                    modelerId = int.Parse(txtModelerId.Text);
                }

                //Add model
                int modelId = ModelDao.add(txtModelName.Text, cboModelPublisher.Text, cboModelScale.Text, modelerId);
                cboModelId.Items.Add(new ComboBoxItem(modelId, modelId.ToString()));
                cboModelId.SelectedIndex = cboModelId.FindStringExact(modelId.ToString());

                foreach(ListViewItem item in lvCategories.CheckedItems) {
                    RegistrationDao.add(
                        DateTime.Now,
                        modelId,
                        int.Parse(item.SubItems[1].Text),
                        null,
                        item.SubItems[3].Text
                    );
                }

                btnAddPrintModel.Enabled = false;
                btnNewModel.Enabled = false;
                btnNewRegistration.Enabled = false;
                btnClose.Enabled = false;

                if(chkPrintRegistrationCard.Checked) {
                    this._parentForm.printRegistrationCards(((ComboBoxItem)cboModelId.SelectedItem).id);
                }

                cboModelId.SelectedIndex = -1;
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
            cboModelId.SelectedIndex = -1;
            txtModelName.Text = "";
            cboModelPublisher.SelectedIndex = -1;
            btnAddPrintModel.Enabled = true;
        }

        private void btnNewRegistration_Click(object sender, EventArgs e) {
            resetAddControls();
            txtEmail.Focus();
        }

        private void lvCategories_ItemCheck(object sender, ItemCheckEventArgs e) {

            if(this._loading)
                return;

            ListViewItem checkedItem = lvCategories.Items[e.Index];
            ListViewGroup group = checkedItem.Group;

            //Disabled all unchecked items in the selected class
            if(!checkedItem.Checked) {

                if(checkedItem.ForeColor == System.Drawing.Color.LightGray) {
                    e.NewValue = e.CurrentValue;
                    return;
                }
                
                int age = DateTime.Now.Year - ((ComboBoxItem)cboYearOfBirth.SelectedItem).id;
                String ageGroupName = _ageGroups.Where(x => x.bottomAge <= age && x.upperAge >= age).ToArray()[0].name;
                checkedItem.SubItems[3].Text = ageGroupName;

                checkedItem.SubItems[3].Font = new System.Drawing.Font(checkedItem.Font, System.Drawing.FontStyle.Regular);
                checkedItem.SubItems[3].ForeColor = System.Drawing.Color.Black;

                foreach(ListViewItem item in group.Items) {
                    if(item != checkedItem) {
                        item.ForeColor = System.Drawing.Color.LightGray;
                    }
                }
                checkedItem.EnsureVisible();
            }
            else {
                checkedItem.SubItems[3].Text = "";
                foreach(ListViewItem item in checkedItem.Group.Items) {
                    item.ForeColor = System.Drawing.Color.Black;
                }                
            }
        }

        private void lvCategories_MouseDoubleClick(object sender, MouseEventArgs e) {
            ListViewHitTestInfo hitTest = this.lvCategories.HitTest(e.X, e.Y);
            if(hitTest.Item != null) {
                ListViewItem item = hitTest.Item;

                if(!item.Checked) {
                    return;
                }

                try {
                    lvCategories.BeginUpdate();

                    int age = DateTime.Now.Year - ((ComboBoxItem)cboYearOfBirth.SelectedItem).id;
                    String properAgeGroup = _ageGroups.Where(x => x.bottomAge <= age && x.upperAge >= age).ToArray()[0].name;

                    int index = item.Index;
                    if(item.SubItems[2].Text.Length > 0) {
                        String ageGroup = item.SubItems[3].Text;
                        int idx = this._ageGroups.FindIndex(x => x.name.Equals(ageGroup, StringComparison.CurrentCultureIgnoreCase));
                        if(idx < 0 || (idx + 1) == this._ageGroups.Count) {
                            item.SubItems[3].Text = this._ageGroups[0].name;
                        }
                        else {
                            item.SubItems[3].Text = this._ageGroups[++idx].name;
                        }
                    }
                    else if(this._ageGroups.Count > 0) {
                        item.SubItems[3].Text = this._ageGroups[0].name;
                    }

                    if(properAgeGroup.Equals(item.SubItems[3].Text, StringComparison.CurrentCultureIgnoreCase)) {
                        item.UseItemStyleForSubItems = true;
                        item.SubItems[3].Font = new System.Drawing.Font(item.Font, System.Drawing.FontStyle.Regular);
                        item.SubItems[3].ForeColor = System.Drawing.Color.Black;
                    }
                    else {
                        item.UseItemStyleForSubItems = false;
                        item.SubItems[3].Font = new System.Drawing.Font(item.Font, System.Drawing.FontStyle.Bold);
                        item.SubItems[3].ForeColor = System.Drawing.Color.Red;
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

        private void frmRegistrationEntry_Load(object sender, EventArgs e) {
            this._loading = false;
        }

        private void cboModelId_SelectedIndexChanged(object sender, EventArgs e) {
            if(this._loading)
                return;

            if(cboModelId.SelectedIndex < 0) {
                txtModelName.Text = "";
                cboModelPublisher.SelectedIndex = -1;
                cboModelScale.SelectedIndex = -1;
                foreach(ListViewItem item in lvCategories.CheckedItems) {
                    item.Checked = false;
                }
            }
            else {
                this._loading = true;
                loadModel(((ComboBoxItem)cboModelId.SelectedItem).id, null);
                this._loading = false;
            }
        }
    }
}
