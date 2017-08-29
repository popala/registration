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
using Rejestracja.Controls;
using Rejestracja.Data.Dao;
using Rejestracja.Data.Objects;
using Rejestracja.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Rejestracja
{
    public partial class frmSettings : Form
    {
        private bool _loading = false;
        private Class _selectedClass;

        public frmSettings()
        {
            InitializeComponent();
        }

        private void frmSettings_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if(!validateClassOptions()) {
                e.Cancel = true;
            }
            saveGeneralOptions();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            _loading = true;

            // *** Treeview ***
            TreeNode n = tvSettings.Nodes.Add("dokumenty", "Dokumenty");
            n.Nodes.Add("dokumenty1", "Nagłówek i stopka");
            n.Nodes.Add("dokumenty2", "Wzorce");

            tvSettings.Nodes.Add("grupy wiekowe", "Grupy Wiekowe");
            n = tvSettings.Nodes.Add("klasy", "Klasy i kategorie");
            n.Nodes.Add("tmp", "tmp");

            tvSettings.Nodes.Add("nagrody", "Nagrody");
            tvSettings.Nodes.Add("wydawcy", "Wydawcy");
            tvSettings.Nodes.Add("skale", "Skale");

            btnDelete.Visible = false;

            // *** Model category ***
            txtCategoryCode.MaxLength = 16;
            txtCategoryName.MaxLength = 64;

            lvModelCategory.View = View.Details;
            lvModelCategory.FullRowSelect = true;
            lvModelCategory.Columns.Add("Kod");
            lvModelCategory.Columns.Add("Nazwa");
            lvModelCategory.Columns.Add("Klasa");
            lvModelCategory.CheckBoxes = false;
            lvModelCategory.GridLines = true;
            lvModelCategory.MultiSelect = false;
            lvModelCategory.HideSelection = false;
            lvModelCategory.CheckBoxes = true;

            cboJudgingFormOption.Items.Add("Osobne karty sędziowania dla każdej kategorii i grupy wiekowej");
            cboJudgingFormOption.Items.Add("Osobne karty sędziowania dla każdej grupy wiekowej");
            cboJudgingFormOption.Items.Add("Jedna karta sędziowania dla wszystkich kategorii i grup wiekowych");
            cboJudgingFormOption.SelectedIndex = 0;
            
            // *** Publisher ***
            txtPublisherName.MaxLength = 64;

            lvPublishers.View = View.Details;
            lvPublishers.FullRowSelect = true;
            lvPublishers.Columns.Add("Nazwa");
            lvPublishers.CheckBoxes = false;
            lvPublishers.GridLines = true;
            lvPublishers.MultiSelect = false;
            lvPublishers.HideSelection = false;
            lvPublishers.CheckBoxes = true;

            /** Age groups **/
            lvAgeGroup.View = View.Details;
            lvAgeGroup.FullRowSelect = true;
            lvAgeGroup.Columns.Add("Nazwa");
            lvAgeGroup.Columns.Add("Przedział Wiekowy");
            lvAgeGroup.CheckBoxes = false;
            lvAgeGroup.GridLines = true;
            lvAgeGroup.MultiSelect = false;
            lvAgeGroup.HideSelection = false;
            lvAgeGroup.CheckBoxes = true;

            /** Model class **/
            txtModelClassName.MaxLength = Class.MAX_NAME_LENGTH;

            lvModelClass.View = View.Details;
            lvModelClass.FullRowSelect = true;
            lvModelClass.Columns.Add("Nazwa");
            lvModelClass.CheckBoxes = false;
            lvModelClass.GridLines = true;
            lvModelClass.MultiSelect = false;
            lvModelClass.HideSelection = false;
            lvModelClass.CheckBoxes = true;

            lvClassAgeGroups.View = View.Details;
            lvClassAgeGroups.FullRowSelect = true;
            lvClassAgeGroups.Columns.Add("Nazwa");
            lvClassAgeGroups.Columns.Add("Przedział Wiekowy");
            lvClassAgeGroups.CheckBoxes = false;
            lvClassAgeGroups.GridLines = true;
            lvClassAgeGroups.MultiSelect = false;
            lvClassAgeGroups.HideSelection = false;
            lvClassAgeGroups.CheckBoxes = true;

            nudDistinction.Minimum = nudOne.Minimum = nudTwo.Minimum = 0;
            nudThree.Minimum = 2;
            nudThree.Maximum = nudTwo.Maximum = nudThree.Maximum = 1000;

            lblWarning.Text = "Zmiany na tym ekranie spowodują usunięcie wyników w zmienionej klasie";
            lblWarning.Visible = false;

            /** Special Awards **/
            txtAwardTitle.MaxLength = Award.TITLE_MAX_LENGTH;
            lvAwards.View = View.Details;
            lvAwards.FullRowSelect = true;
            lvAwards.Columns.Add("Nazwa");
            lvAwards.CheckBoxes = false;
            lvAwards.GridLines = true;
            lvAwards.MultiSelect = false;
            lvAwards.HideSelection = false;
            lvAwards.CheckBoxes = true; 

            /** ModelScale **/
            txtModelScale.MaxLength = 128;

            lvModelScales.View = View.Details;
            lvModelScales.FullRowSelect = true;
            lvModelScales.Columns.Add("Nazwa");
            lvModelScales.CheckBoxes = false;
            lvModelScales.GridLines = true;
            lvModelScales.MultiSelect = false;
            lvModelScales.HideSelection = false;
            lvModelScales.CheckBoxes = true;

            loadGeneralOptions();
            setButtons();

            _loading = false;

            tvSettings.SelectedNode = tvSettings.Nodes[0];
        }

        private void btnPath_Click(object sender, EventArgs e) {
            switch(((Button)sender).Name) {
                case "btnRegistrationTemplatePath":
                    selectTemplate(txtRegistrationTemplate, "RegistrationTemplate");
                    break;
                case "btnJudgingTemplatePath":
                    selectTemplate(txtJudgingFormTemplate, "JudgingFormTemplate");
                    break;
                case "btnResultsTemplatePath":
                    selectTemplate(txtResultsTemplate, "ResultsTemplate");
                    break;
                case "btnSummaryTemplatePath":
                    selectTemplate(txtSummaryTemplate, "SummaryTemplate");
                    break;
                case "btnAwardDiplomaTemplatePath":
                    selectTemplate(txtAwardDiplomaTemplate, "AwardDiplomaTemplate");
                    break;
                case "btnCategoryDiplomaTemplatePath":
                    selectTemplate(txtCategoryDiplomaTemplate, "CategoryDiplomaTemplate");
                    break;
            }
        }

        private void selectTemplate(TextBox source, String optionName) {
            String folder = Resources.TemplateFolder;

            if(File.Exists(source.Text)) {
                folder = new FileInfo(source.Text).DirectoryName;
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = folder;
            ofd.Filter = "MS Word Document (*.doc,*.docx)|*.doc;*docx|HTML Document (*.htm,*.html)|*.htm;*.html";
            ofd.Multiselect = false;

            DialogResult result = ofd.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK) {
                source.Text = ofd.FileName;
                if(optionName != null) {
                    Options.set(optionName, ofd.FileName);
                }
            }
        }

        private void loadGeneralOptions()
        {
            txtHeading.Text = Options.get("DocumentHeader");
            txtFooter.Text = Options.get("DocumentFooter");

            String templatePath = Options.get("RegistrationTemplate");
            if(templatePath != null) {
                txtRegistrationTemplate.Text = templatePath;
            }
            else {
                txtRegistrationTemplate.Text = Resources.resolvePath("templateKartyModelu");
            }

            templatePath = Options.get("JudgingFormTemplate");
            if(templatePath != null) {
                txtJudgingFormTemplate.Text = templatePath;
            }
            else {
                txtJudgingFormTemplate.Text = Resources.resolvePath("templateKartySędziowania");
            }

            templatePath = Options.get("ResultsTemplate");
            if(templatePath != null) {
                txtResultsTemplate.Text = templatePath;
            }
            else {
                txtResultsTemplate.Text = Resources.resolvePath("templateWynikow");
            }

            templatePath = Options.get("SummaryTemplate");
            if(templatePath != null) {
                txtSummaryTemplate.Text = templatePath;
            }
            else {
                txtSummaryTemplate.Text = Resources.resolvePath("templatePodsumowania");
            }

            templatePath = Options.get("AwardDiplomaTemplate");
            if(templatePath != null) {
                txtAwardDiplomaTemplate.Text = templatePath;
            }
            else {
                txtAwardDiplomaTemplate.Text = Resources.resolvePath("templateDyplomuNagrody");
            }

            templatePath = Options.get("CategoryDiplomaTemplate");
            if(templatePath != null) {
                txtCategoryDiplomaTemplate.Text = templatePath;
            }
            else {
                txtCategoryDiplomaTemplate.Text = Resources.resolvePath("templateDyplomuKategorii");
            }
        }

        private void saveGeneralOptions() {
            //Document header/footer
            Options.set("DocumentHeader", txtHeading.Text);
            Options.set("DocumentFooter", txtFooter.Text);

            //Age group validation
            Options.set("ValidateAgeGroup", chkValidateAge.Checked.ToString().ToLower());

            //Document templates
            Options.set("RegistrationTemplate", txtRegistrationTemplate.Text);
            Options.set("JudgingFormTemplate", txtJudgingFormTemplate.Text);
            Options.set("ResultsTemplate", txtResultsTemplate.Text);
            Options.set("SummaryTemplate", txtSummaryTemplate.Text);
            Options.set("AwardDiplomaTemplate", txtAwardDiplomaTemplate.Text);
            Options.set("CategoryDiplomaTemplate", txtCategoryDiplomaTemplate.Text);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tcOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tcOptions.SelectedIndex) {
                case 1:
                    txtAgeGroup.SelectAll();
                    txtAgeGroup.Focus();
                    break;
                case 2:
                    txtModelClassName.SelectAll();
                    txtModelClassName.Focus();
                    break;
                case 3:
                    txtCategoryCode.SelectAll();
                    txtCategoryCode.Focus();
                    break;
                case 4:
                    txtAwardTitle.SelectAll();
                    txtAwardTitle.Focus();
                    break;
                case 5:
                    txtPublisherName.SelectAll();
                    txtPublisherName.Focus();
                    break;
                case 6:
                    txtModelScale.SelectAll();
                    txtModelScale.Focus();
                    break;
            }
            setButtons();
        }

        private void setButtons() {

            switch (tcOptions.SelectedIndex) {
                case 0:
                case 7:
                    btnDelete.Visible = false;
                    return;
                    //break;
                case 1:
                    btnDelete.Enabled = (lvAgeGroup.CheckedItems.Count > 0);
                    break;
                case 2:
                    btnDelete.Enabled = (lvModelClass.CheckedItems.Count > 0);
                    break;
                case 3:
                    btnDelete.Enabled = (lvModelCategory.CheckedItems.Count > 0);
                    btnMoveUpCategory.Enabled = btnMoveDownCategory.Enabled = (lvModelCategory.CheckedItems.Count == 1);
                    break;
                case 4:
                    btnDelete.Enabled = (lvAwards.CheckedItems.Count > 0);
                    btnMoveUpAward.Enabled = btnMoveDownAward.Enabled = (lvAwards.CheckedItems.Count == 1);
                    break;
                case 5:
                    btnDelete.Enabled = (lvPublishers.CheckedItems.Count > 0);
                    break;
                case 6:
                    btnDelete.Enabled = (lvModelScales.CheckedItems.Count > 0);
                    break;
            }

            btnDelete.Visible = true;
        }

        private void anyListView_ItemChecked(object sender, ItemCheckedEventArgs e) {
            setButtons();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            switch (tcOptions.SelectedIndex)
            {
                case 1:
                    if (lvAgeGroup.CheckedItems.Count < 1)
                        return;
                    deleteAgeGroups();
                    break;

                case 2:
                    if (lvModelClass.CheckedItems.Count < 1)
                        return;
                    deleteModelClasses();
                    break;

                case 3:
                    if (lvModelCategory.CheckedItems.Count < 1)
                        return;
                    deleteModelCategories();
                    break;

                case 4:
                    if (lvAwards.CheckedItems.Count < 1)
                        return;
                    deleteAwards();
                    break;

                case 5:
                    if (lvPublishers.CheckedItems.Count < 1)
                        return;
                    deletePublishers();
                    break;

                case 6:
                    if (lvModelScales.CheckedItems.Count < 1)
                        return;
                    deleteModelScales();
                    break;
            }
            setButtons();
        }

        #region ModelCategories

        private void loadModelCategories(String className)
        {
            lvModelCategory.BeginUpdate();
            lvModelCategory.Items.Clear();

            foreach (Category mc in CategoryDao.getList(false, className))
            {
                ListViewItem li = new ListViewItem(new String[] { mc.code, mc.name, mc.className });
                li.Tag = mc.id;
                lvModelCategory.Items.Add(li);
            }
            lvModelCategory.Columns[0].Width = -2;
            lvModelCategory.Columns[1].Width = -2;
            lvModelCategory.Columns[2].Width = -2;
            lvModelCategory.EndUpdate();
        }

        private void deleteModelCategories() {
            if (lvModelCategory.CheckedItems.Count == 0) {
                return;
            }
            if (MessageBox.Show("Usunięcie kategorii jest nieodwracalne. Wpisy używające usuniętych kategorii muszą być poprawione.", "Kategorie", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Cancel) {
                return;
            }

            lvModelCategory.BeginUpdate();
            foreach (ListViewItem item in lvModelCategory.CheckedItems) {
                int categoryId = (int)item.Tag;
                CategoryDao.delete(categoryId);
                lvModelCategory.Items.Remove(item);
            }
            lvModelCategory.EndUpdate();
        }

        private void btnMoveUpCategory_Click(object sender, EventArgs e)
        {
            ListViewItem item = lvModelCategory.CheckedItems[0];
            int index = lvModelCategory.CheckedItems[0].Index;

            if (index == 0) {
                return;
            }

            lvModelCategory.BeginUpdate();
            btnMoveUpCategory.Enabled = false;

            lvModelCategory.Items.Remove(item);
            lvModelCategory.Items.Insert(index - 1, item);
            lvModelCategory.Items[index - 1].Selected = true;

            CategoryDao.updateDisplayOrder((int)lvModelCategory.Items[index - 1].Tag, index - 1);
            CategoryDao.updateDisplayOrder((int)lvModelCategory.Items[index].Tag, index);

            item.EnsureVisible();
            lvModelCategory.EndUpdate();
            btnMoveUpCategory.Enabled = true;
        }

        private void btnMoveDownCategory_Click(object sender, EventArgs e)
        {
            ListViewItem selectedItem = lvModelCategory.CheckedItems[0];
            int index = lvModelCategory.CheckedItems[0].Index;

            if (index == (lvModelCategory.Items.Count - 1)) {
                return;
            }

            lvModelCategory.BeginUpdate();
            btnMoveDownCategory.Enabled = false;

            lvModelCategory.Items.Remove(selectedItem);
            lvModelCategory.Items.Insert(index + 1, selectedItem);
            selectedItem.Selected = true;

            CategoryDao.updateDisplayOrder((int)lvModelCategory.Items[index].Tag, index);
            CategoryDao.updateDisplayOrder((int)lvModelCategory.Items[index + 1].Tag, index + 1);

            selectedItem.EnsureVisible();
            lvModelCategory.EndUpdate();
            btnMoveDownCategory.Enabled = true;
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            if(txtCategoryCode.Text.Trim().Length < 1 || txtCategoryName.Text.Trim().Length < 1)
            {
                MessageBox.Show("Kod i nazwa są wymagane", "Nowa kategoria", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (CategoryDao.codeExists(txtCategoryCode.Text.Trim()))
            {
                MessageBox.Show("Kod kategorii jest już wykorzystany", "Nowa kategoria", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCategoryCode.Focus();
                txtCategoryCode.SelectAll();
                return;
            }

            int catId = CategoryDao.add(txtCategoryCode.Text, txtCategoryName.Text, tvSettings.SelectedNode.Text, CategoryDao.getNextSortFlag());
            loadModelCategories(tvSettings.SelectedNode.Text);
            setButtons();
            foreach (ListViewItem item in lvModelCategory.Items) {
                if ((int)item.Tag == catId) {
                    item.EnsureVisible();
                    item.Selected = true;
                    break;
                }
            }

            txtCategoryName.Text = "";
            txtCategoryCode.Text = "";
            txtCategoryCode.Focus();
        }

        #endregion

        #region Publishers

        private void loadPublishers()
        {
            lvPublishers.Items.Clear();

            foreach (Publisher pub in PublisherDao.getList())
            {
                ListViewItem li = new ListViewItem(new String[] { pub.name });
                li.Tag = pub.id;
                lvPublishers.Items.Add(li);
            }
            lvPublishers.Columns[0].Width = -2;
        }

        private void deletePublishers() {
            if (lvPublishers.CheckedItems.Count == 0) {
                return;
            }
            if (MessageBox.Show("Usunięcie wydawcy jest nieodwracalne. Wpisy używające usuniętych wydawców muszą być poprawione.", "Wydawcy", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Cancel) {
                return;
            }

            lvPublishers.BeginUpdate();
            foreach (ListViewItem item in lvPublishers.CheckedItems) {
                int publisherId = (int)item.Tag;
                PublisherDao.delete(publisherId);
                lvPublishers.Items.Remove(item);
            }
            lvPublishers.EndUpdate();
        }

        private void btnAddPublisher_Click(object sender, EventArgs e)
        {
            if (txtPublisherName.Text.Trim().Length < 1)
            {
                MessageBox.Show("Nazwa wydawcy wymagana", "Nowy wydawca", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPublisherName.Focus();
                txtPublisherName.SelectAll();
                return;
            }

            if (PublisherDao.exists(txtPublisherName.Text.Trim()))
            {
                MessageBox.Show("Nazwa wydawcy jest już w bazie", "Nowy wydawca", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPublisherName.Focus();
                txtPublisherName.SelectAll();
                return;
            }

            int pubId = PublisherDao.add(txtPublisherName.Text.Trim());
            loadPublishers();
            setButtons();
            foreach (ListViewItem item in lvPublishers.Items) {
                if ((int)item.Tag == pubId) {
                    item.EnsureVisible();
                    item.Selected = true;
                    break;
                }
            }

            txtPublisherName.Focus();
            txtPublisherName.SelectAll();
        }

        #endregion

        #region AgeGroups

        private void btnAddAgeGroup_Click(object sender, EventArgs e)
        {
            if(!Regex.IsMatch(txtAge.Text, "[0-9]"))
            {
                MessageBox.Show(this, "Wiek musi być liczbą > 0", "Błędne dane", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (AgeGroupDao.exists(int.Parse(txtAge.Text), -1)) {
                MessageBox.Show("Nowa kategoria wiekowa musi różnić się o przynajmniej 2 lata od już istniejących", "Nowa grupa wiekowa", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtAge.Focus();
                txtAge.SelectAll();
                return;
            }
            int agId = AgeGroupDao.add(txtAgeGroup.Text, int.Parse(txtAge.Text), -1);
            loadAgeGroups();
            setButtons();

            foreach (ListViewItem item in lvAgeGroup.Items) {
                if ((int)item.Tag == agId) {
                    item.EnsureVisible();
                    item.Selected = true;
                    break;
                }
            }

            txtAgeGroup.Focus();
            txtAgeGroup.SelectAll();
        }

        private void loadAgeGroups() {
            lvAgeGroup.Items.Clear();

            foreach (AgeGroup ageGroup in AgeGroupDao.getList(-1)) {
                ListViewItem li = new ListViewItem(new String[] { ageGroup.name, String.Format("{0} - {1}", ageGroup.bottomAge, ageGroup.upperAge) });
                li.Tag = ageGroup.id;
                lvAgeGroup.Items.Add(li);
            }
            lvAgeGroup.Columns[0].Width = -2;
            lvAgeGroup.Columns[1].Width = -2;

            chkValidateAge.Checked = (Options.get("ValidateAgeGroup") == null || !Options.get("ValidateAgeGroup").Equals("false"));
        }

        private void deleteAgeGroups() {
            if (lvAgeGroup.CheckedItems.Count == 0) {
                return;
            }
            if (MessageBox.Show("Usunięcie group wiekowych jest nieodwracalne. Wpisy używające usuniętych group wiekowych muszą być poprawione.", "Grupy Wiekowe", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Cancel) {
                return;
            }

            lvAgeGroup.BeginUpdate();
            foreach (ListViewItem item in lvAgeGroup.CheckedItems) {
                int ageGroupId = (int)item.Tag;
                AgeGroupDao.delete(ageGroupId);
                lvAgeGroup.Items.Remove(item);
            }
            lvAgeGroup.EndUpdate();
        }

        private void txtAge_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        #endregion

        #region ModelClasses

        private bool validateClassOptions() {
            foreach(Class cls in ClassDao.getList()) {
                if(cls.useCustomAgeGroups) {
                    List<AgeGroup> ageGroups = AgeGroupDao.getList(cls.id);
                    if(ageGroups.Count == 0) {
                        MessageBox.Show("Klasa \"" + cls.name + "\" nie ma zdefiniowanych żadnych group wiekowych pomimo że odznaczono opcję \"Używaj standardowych grup wiekowych\".", "Błąd w ustawieniach klasy \"" + cls.name + "\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            return true;
        }

        private void btnAddModelClass_Click(object sender, EventArgs e) {
            if (txtModelClassName.Text.Trim().Length < 1) {
                MessageBox.Show("Nazwa klasy wymagana", "Nowa klasa", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtModelClassName.Focus();
                txtModelClassName.SelectAll();
                return;
            }

            if (ClassDao.exists(txtModelClassName.Text.Trim())) {
                MessageBox.Show("Klasa istnieje", "Nowa klasa", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtModelClassName.Focus();
                txtModelClassName.SelectAll();
                return;
            }

            int mcId = ClassDao.add(txtModelClassName.Text.Trim());
            loadModelClasses();
            setButtons();
            
            foreach (ListViewItem item in lvModelClass.Items) {
                if ((int)item.Tag == mcId) {
                    item.EnsureVisible();
                    item.Selected = true;
                    break;
                }
            }

            txtModelClassName.Text = "";
            txtModelClassName.Focus();
        }

        private void loadModelClasses() {

            TreeNode classNode = tvSettings.Nodes.Find("klasy", true)[0];
            classNode.Nodes.Clear();


            lvModelClass.Items.Clear();

            foreach (Class cls in ClassDao.getList()) {
                ListViewItem li = new ListViewItem(new String[] { cls.name });
                li.Tag = cls.id;
                lvModelClass.Items.Add(li);
                classNode.Nodes.Add("cls:" + cls.id.ToString(), cls.name);
            }
            lvModelClass.Columns[0].Width = -2;
        }

        private void deleteModelClasses() {
            if (lvModelClass.CheckedItems.Count == 0) {
                return;
            }
            if (MessageBox.Show("Usunięcie klas jest nieodwracalne. Usunięte zostaną również wszystkie kategorie przypisane do usuniętych klas.", "Klasy", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Cancel) {
                return;
            }

            lvModelClass.BeginUpdate();

            //Here we cannot just remove because the ComboBox on category screen
            //has to get reloaded as well
            foreach (ListViewItem item in lvModelClass.CheckedItems) {
                int clsId = (int)item.Tag;
                ClassDao.delete(clsId);
            }
            loadModelClasses();
            loadModelCategories(tvSettings.SelectedNode.Text);
            
            lvModelClass.EndUpdate();
        }

        private void loadClassDetails(String className) {
            _loading = true;
            
            _selectedClass = ClassDao.get(className);

            loadModelCategories(className);

            chkUseCustomRegistrationCard.Checked = (!String.IsNullOrWhiteSpace(_selectedClass.registrationCardTemplate));
            txtClassRegistrationTemplate.Text = _selectedClass.registrationCardTemplate;
            chkUseCustomJudgingCard.Checked = (!String.IsNullOrWhiteSpace(_selectedClass.judgingFormTemplate));
            txtClassJudgingFormTemplate.Text = _selectedClass.judgingFormTemplate;
            chkUseCustomDiploma.Checked = (!String.IsNullOrWhiteSpace(_selectedClass.diplomaTemplate));
            txtClassCategoryDiplomaTemplate.Text = _selectedClass.diplomaTemplate;

            cboJudgingFormOption.SelectedIndex = (int)_selectedClass.scoringCardType;

            txtClassAgeGroup.Text = "";
            txtClassAge.Text = "";
            lvClassAgeGroups.Items.Clear();

            chkUseStandardAgeGroups.Checked = (!_selectedClass.useCustomAgeGroups);
            if(_selectedClass.useCustomAgeGroups) {
                loadClassAgeGroups(_selectedClass.id);
            }

            tcClassOptions.SelectedIndex = 0;

            rbPlaces.Checked = (_selectedClass.classificationType == Class.ClassificationType.Places);
            rbMedals.Checked = (_selectedClass.classificationType == Class.ClassificationType.Medals);
            rbDistinctions.Checked = (_selectedClass.classificationType == Class.ClassificationType.Distinctions);

            nudDistinction.Visible = nudOne.Enabled = nudTwo.Enabled = nudThree.Enabled = chkPointBasedClassification.Checked = _selectedClass.usePointRange;
            chkDistinctions.Checked = _selectedClass.useDistinctions;
            chkPointBasedClassification.Checked = _selectedClass.usePointRange;
            if(_selectedClass.usePointRange) {
                nudOne.Value = _selectedClass.pointRanges[0];
                nudTwo.Value = _selectedClass.pointRanges[1];
                nudThree.Value = _selectedClass.pointRanges[2];
            }

            _loading = false;
        }

        private void loadClassAgeGroups(int classId) {
            lvClassAgeGroups.Items.Clear();

            List<AgeGroup> ageGroups = AgeGroupDao.getList(classId);
            foreach(AgeGroup ageGroup in ageGroups) {
                ListViewItem li = new ListViewItem(new String[] { ageGroup.name, String.Format("{0} - {1}", ageGroup.bottomAge, ageGroup.upperAge) });
                li.Tag = ageGroup.id;
                lvClassAgeGroups.Items.Add(li);
            }
            foreach(ColumnHeader column in lvClassAgeGroups.Columns) {
                column.Width = -2;
            }
        }

        private void chkUseStandardAgeGroups_CheckedChanged(object sender, EventArgs e) {

            if(chkUseStandardAgeGroups.Checked) {

                if(_selectedClass.ageGroups.Count > 0 && MessageBox.Show("Usunąć niestandardowe grupy wiekowe?", "Grupy wiekowe w klasie", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes) {
                    return;
                }
                AgeGroupDao.deleteClassAgeGroups(_selectedClass.id);

                txtClassAgeGroup.Enabled = false;
                txtClassAge.Enabled = false;
                btnAddClassAgeGroup.Enabled = false;
                lvClassAgeGroups.Items.Clear();
                lvClassAgeGroups.Enabled = false;
            }
            else {
                txtClassAgeGroup.Enabled = true;
                txtClassAge.Enabled = true;
                btnAddClassAgeGroup.Enabled = true;
                lvClassAgeGroups.Enabled = true;
            }

            _selectedClass.useCustomAgeGroups = (!chkUseStandardAgeGroups.Checked);
            ClassDao.update(_selectedClass);
        }

        private void btnAddClassAgeGroup_Click(object sender, EventArgs e) {
            int clsId = int.Parse(tvSettings.SelectedNode.Name.ToString().Split(':')[1]);
            int age = 0;

            if(!int.TryParse(txtClassAge.Text, out age)) {
                MessageBox.Show("Wiek musi być wartością numeryczną", "Nowa grupa wiekowa", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtClassAge.SelectAll();
                txtClassAge.Focus();
                return;
            }

            if(AgeGroupDao.exists(age, clsId)) {
                MessageBox.Show("Grupa wiekowa już istnieje", "Nowa grupa wiekowa", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            AgeGroupDao.add(txtClassAgeGroup.Text, age, clsId);
            txtClassAgeGroup.Text = "";
            txtClassAge.Text = "";
            loadClassAgeGroups(clsId);
            txtClassAgeGroup.Focus();
        }

        private void tcClassOptions_SelectedIndexChanged(object sender, EventArgs e) {

            switch(tcClassOptions.SelectedIndex) {
                case 0:
                case 2:
                    btnDelete.Visible = true;
                    lblWarning.Visible = true;
                    break;
                case 1:
                    lblWarning.Visible = false;
                    break;
                default:
                    btnDelete.Visible = false;
                    lblWarning.Visible = true;
                    break;
            }
        }

        private void cboJudgingFormOption_SelectedIndexChanged(object sender, EventArgs e) {
            if(_loading) {
                return;
            }

            _selectedClass.scoringCardType = (Class.ScoringCardType)cboJudgingFormOption.SelectedIndex;
            ClassDao.update(_selectedClass);
        }

        private void chkUseCustomRegistrationCard_CheckedChanged(object sender, EventArgs e) {
            if(chkUseCustomRegistrationCard.Checked) {
                btnCustomRegistrationCard.Enabled = true;
                if(_loading) {
                    return;
                }

                txtClassRegistrationTemplate.Text = "";
                selectTemplate(txtClassRegistrationTemplate, null);
                if(String.IsNullOrWhiteSpace(txtClassRegistrationTemplate.Text)) {
                    chkUseCustomRegistrationCard.Checked = false;
                    return;
                }
            }
            else {
                btnCustomRegistrationCard.Enabled = false;
                txtClassRegistrationTemplate.Text = "";
                if(_loading) {
                    return;
                }
            }

            _selectedClass.registrationCardTemplate = txtClassRegistrationTemplate.Text;
            ClassDao.update(_selectedClass);
        }

        private void radioButton_Checked(object sender, EventArgs e) {
            if(rbDistinctions.Checked) {
                chkDistinctions.Checked = false;
                chkDistinctions.Enabled = false;
                chkPointBasedClassification.Checked = false;
                chkPointBasedClassification.Enabled = false;
                nudDistinction.Visible = false;
            }
            else {
                chkDistinctions.Enabled = true;
                chkPointBasedClassification.Enabled = true;
                if(chkPointBasedClassification.Checked) {
                    nudDistinction.Visible = true;
                    nudDistinction.Enabled = chkDistinctions.Checked;
                }
                else {
                    nudDistinction.Visible = false;
                }
            }

            if(_loading) {
                return;
            }

            if(rbPlaces.Checked) {
                _selectedClass.classificationType = Class.ClassificationType.Places;
                _selectedClass.useDistinctions = chkDistinctions.Checked;
            }
            else if(rbMedals.Checked) {
                _selectedClass.classificationType = Class.ClassificationType.Medals;
                _selectedClass.useDistinctions = chkDistinctions.Checked;
            }
            else {
                _selectedClass.classificationType = Class.ClassificationType.Distinctions;
                _selectedClass.useDistinctions = false;
            }
            ClassDao.update(_selectedClass);
        }

        private void chkPointBasedClassification_CheckedChanged(object sender, EventArgs e) {
            if(chkPointBasedClassification.Checked) {
                nudOne.Value = 95;
                nudTwo.Value = 90;
                nudThree.Value = 85;
                nudOne.Enabled = nudTwo.Enabled = nudThree.Enabled = true;
                nudDistinction.Visible = true;
            }
            else {
                nudOne.Enabled = nudTwo.Enabled = nudThree.Enabled = false;
                nudOne.Minimum = nudTwo.Minimum = nudThree.Minimum = 0;
                nudOne.Value = 0;
                nudTwo.Value = 0;
                nudThree.Value = 0;
                nudDistinction.Visible = false;
            }
            nudDistinction.Enabled = chkDistinctions.Checked;
            if(nudDistinction.Enabled) {
                nudDistinction.Value = 80;
            }
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e) {
            NumericUpDown ctl = sender as NumericUpDown;

            if(!ctl.Enabled || _loading) {
                return;
            }

            switch(ctl.Name) {
                case "nudDistinction":
                    if(nudDistinction.Value >= nudThree.Value) {
                        nudDistinction.Value = nudThree.Value - 1;
                    }
                    break;

                case "nudOne":
                    if(nudOne.Value <= nudTwo.Value) {
                        nudTwo.Value = nudOne.Value - 1;
                    }
                    break;

                case "nudTwo":
                    if(nudTwo.Value >= nudOne.Value) {
                        nudTwo.Value = nudOne.Value - 1;
                    }
                    if(nudTwo.Value <= nudThree.Value) {
                        nudThree.Value = nudTwo.Value - 1;
                    }
                    break;

                case "nudThree":
                    if(nudThree.Value >= nudTwo.Value) {
                        nudThree.Value = nudTwo.Value - 1;
                    }
                    if(nudDistinction.Enabled && nudThree.Value <= nudDistinction.Value) {
                        nudDistinction.Value = nudThree.Value - 1;
                    }
                    break;
            }
        }

        #endregion

        #region ModelScales

        private void loadModelScales() {
            lvModelScales.Items.Clear();
            foreach (Scale scale in ScaleDao.getList()) {
                ListViewItem li = new ListViewItem(new String[] { scale.name });
                li.Tag = scale.id;
                lvModelScales.Items.Add(li);
            }
            if (lvModelScales.Items.Count > 0) {
                lvModelScales.Columns[0].Width = -2;
            }
        }

        private void deleteModelScales() {
            if (lvModelScales.CheckedItems.Count == 0) {
                return;
            }
            if (MessageBox.Show("Usunięcie skali jest nieodwracalne. Wpisy używające usuniętych skali muszą być poprawione.", "Skale", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Cancel) {
                return;
            }

            lvModelScales.BeginUpdate();
            foreach (ListViewItem item in lvModelScales.CheckedItems) {
                int scaleId = (int)item.Tag;
                ScaleDao.delete(scaleId);
                lvModelScales.Items.Remove(item);
            }
            lvModelScales.EndUpdate();
        }

        private void btnAddModelScale_Click(object sender, EventArgs e) {
            if (ScaleDao.exists(txtModelScale.Text)) {
                MessageBox.Show("Skala już istnieje", "Nowa skala", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtModelScale.Focus();
                txtModelScale.SelectAll();
                return;
            }
            
            int scaleId = ScaleDao.add(txtModelScale.Text);
            loadModelScales();
            setButtons();

            foreach (ListViewItem item in lvModelScales.Items) {
                if ((int)item.Tag == scaleId) {
                    item.EnsureVisible();
                    item.Selected = true;
                    break;
                }
            }

            txtModelScale.Focus();
            txtModelScale.SelectAll();
        }

        #endregion

        #region Awards

        private void loadAwards() {
            lvAwards.Items.Clear();
            foreach(Award award in AwardDao.getList()) {
                ListViewItem item = new ListViewItem(award.title);
                item.Tag = award.id;
                lvAwards.Items.Add(item);
            }
            foreach (ColumnHeader header in lvAwards.Columns) {
                header.Width = -2;
            }
        }

        private void deleteAwards() {
            if (lvAwards.CheckedItems.Count == 0) {
                return;
            }
            if (MessageBox.Show("Usunięcie nagrody jest nieodwracalne. Unięte zostaną również wpisy w wynikach używające usuniętych nagród.", "Nagrody", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Cancel) {
                return;
            }

            lvAwards.BeginUpdate();
            foreach (ListViewItem item in lvAwards.CheckedItems) {
                int awardId = (int)item.Tag;
                AwardDao.delete(awardId);
                lvAwards.Items.Remove(item);
            }
            lvAwards.EndUpdate();
        }

        private void btnAddAward_Click(object sender, EventArgs e) {
            String awardTitle = txtAwardTitle.Text.Trim();
            if (awardTitle.Length == 0) {
                MessageBox.Show("Wypełnij tytuł nagrody", "Nowa Nagroda", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (AwardDao.exists(awardTitle)) {
                MessageBox.Show("Nagroda już istnieje", "Nowa Nagroda", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int awardId = AwardDao.add(awardTitle, AwardDao.getNextSortFlag());
            loadAwards();
            setButtons();

            foreach (ListViewItem item in lvAwards.Items) {
                if ((int)item.Tag == awardId) {
                    item.EnsureVisible();
                    item.Selected = true;
                    break;
                }
            }

            txtAwardTitle.SelectAll();
            txtAwardTitle.Focus();
        }

        private void btnMoveUpAward_Click(object sender, EventArgs e) {
            if (lvAwards.CheckedItems.Count < 1)
                return;

            if (lvAwards.CheckedItems[0].Index < 1)
                return;

            lvAwards.BeginUpdate();

            ListViewItem item = lvAwards.CheckedItems[0];
            int index = lvAwards.CheckedItems[0].Index;

            lvAwards.Items.Remove(item);
            lvAwards.Items.Insert(index - 1, item);
            lvAwards.Items[index - 1].Selected = true;

            AwardDao.updateDisplayOrder((int)lvAwards.Items[index - 1].Tag, index - 1);
            AwardDao.updateDisplayOrder((int)lvAwards.Items[index].Tag, index);

            item.EnsureVisible();
            lvAwards.EndUpdate();
        }

        private void btnMoveDownAward_Click(object sender, EventArgs e) {
            if (lvAwards.CheckedItems.Count < 1)
                return;

            if (lvAwards.CheckedItems[0].Index >= (lvAwards.Items.Count - 1))
                return;

            lvAwards.BeginUpdate();

            ListViewItem item = lvAwards.CheckedItems[0];
            int index = lvAwards.CheckedItems[0].Index;

            lvAwards.Items.Remove(item);
            lvAwards.Items.Insert(index + 1, item);
            item.Selected = true;

            AwardDao.updateDisplayOrder((int)lvAwards.Items[index].Tag, index);
            AwardDao.updateDisplayOrder((int)lvAwards.Items[index + 1].Tag, index + 1);

            item.EnsureVisible();
            lvAwards.EndUpdate();
        }

        #endregion

        private void tvSettings_BeforeExpand(object sender, TreeViewCancelEventArgs e) {
            if(e.Node == null) {
                return;
            }

            if(e.Action == TreeViewAction.Expand) {
                switch(e.Node.Name) {
                    case "klasy": {
                            TreeNode classNode = tvSettings.Nodes.Find("klasy", true)[0];
                            classNode.Nodes.Clear();

                            foreach(Class cls in ClassDao.getList()) {
                                classNode.Nodes.Add("cls:" + cls.id.ToString(), cls.name);
                            }
                            break;
                        }
                }
            }
        }

        private void tvSettings_AfterSelect(object sender, TreeViewEventArgs e) {
            if(e.Node == null) {
                return;
            }

            if(e.Node.Name.Contains(":")) {
                loadClassDetails(e.Node.Text);
                tcOptions.SelectedIndex = 3;
                lblWarning.Visible = true;
                return;
            }

            switch(e.Node.Name) {
                case "dokumenty":
                case "dokumenty1":
                    tcOptions.SelectedIndex = 0;
                    break;
                case "dokumenty2":
                    tcOptions.SelectedIndex = 7;
                    break;
                case "grupy wiekowe":
                    loadAgeGroups(); tcOptions.SelectedIndex = 1;
                    break;
                case "klasy":
                    loadModelClasses(); tcOptions.SelectedIndex = 2;
                    lblWarning.Visible = true;
                    return;
                    //break;
                case "nagrody":
                    loadAwards(); tcOptions.SelectedIndex = 4;
                    break;
                case "wydawcy":
                    loadPublishers(); tcOptions.SelectedIndex = 5;
                    break;
                case "skale":
                    loadModelScales(); tcOptions.SelectedIndex = 6;
                    break;
            }
            lblWarning.Visible = false;
        }

        private void chkDistinctions_CheckedChanged(object sender, EventArgs e) {
            if(_loading) {
                return;
            }

            if(chkDistinctions.Checked) {
                if(chkPointBasedClassification.Checked) {
                    nudDistinction.Enabled = true;
                    nudDistinction.Value = nudThree.Value - 5;
                }
            }
            else {
                nudDistinction.Enabled = false;
                nudDistinction.ResetText();
            }

            _selectedClass.useDistinctions = chkDistinctions.Checked;
            ClassDao.update(_selectedClass);
        }
    }
}
