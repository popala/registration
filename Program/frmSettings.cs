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
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Rejestracja
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
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
            
            loadModelCategories();
            if(lvModelCategory.Items.Count > 0)
                lvModelCategory.Items[0].Selected = true;

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

            loadPublishers();
            if (lvPublishers.Items.Count > 0)
                lvPublishers.Items[0].Selected = true;

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

            loadAgeGroups();
            if (lvAgeGroup.Items.Count > 0)
                lvAgeGroup.Items[0].Selected = true;

            /** ModelCategory **/
            txtModelClassName.MaxLength = Class.MAX_NAME_LENGTH;

            lvModelClass.View = View.Details;
            lvModelClass.FullRowSelect = true;
            lvModelClass.Columns.Add("Nazwa");
            lvModelClass.CheckBoxes = false;
            lvModelClass.GridLines = true;
            lvModelClass.MultiSelect = false;
            lvModelClass.HideSelection = false;
            lvModelClass.CheckBoxes = true;

            loadModelClasses();
            if (lvModelClass.Items.Count > 0)
                lvModelClass.Items[0].Selected = true;

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

            loadAwards();
            if (lvAwards.Items.Count > 0)
                lvAwards.Items[0].Selected = true;

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

            loadModelScales();
            if (lvModelScales.Items.Count > 0)
                lvModelScales.Items[0].Selected = true;

            loadGeneralOptions();
            setButtons();
        }

        private void loadGeneralOptions()
        {
            txtHeading.Text = Options.get("DocumentHeader");
            txtFooter.Text = Options.get("DocumentFooter");
            chkValidateAge.Checked = (Options.get("ValidateAgeGroup") == null || !Options.get("ValidateAgeGroup").Equals("false"));
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Options.set("DocumentHeader", txtHeading.Text);
            Options.set("DocumentFooter", txtFooter.Text);
            Options.set("ValidateAgeGroup", chkValidateAge.Checked.ToString().ToLower());
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
                    btnDelete.Visible = false;
                    break;
                case 1:
                    btnDelete.Visible = true;
                    btnDelete.Enabled = (lvAgeGroup.CheckedItems.Count > 0);
                    break;
                case 2:
                    btnDelete.Visible = true;
                    btnDelete.Enabled = (lvModelClass.CheckedItems.Count > 0);
                    break;
                case 3:
                    btnDelete.Visible = true;
                    btnDelete.Enabled = (lvModelCategory.CheckedItems.Count > 0);
                    btnMoveUpCategory.Enabled = btnMoveDownCategory.Enabled = (lvModelCategory.CheckedItems.Count == 1);
                    break;
                case 4:
                    btnDelete.Visible = true;
                    btnDelete.Enabled = (lvAwards.CheckedItems.Count > 0);
                    btnMoveUpAward.Enabled = btnMoveDownAward.Enabled = (lvAwards.CheckedItems.Count == 1);
                    break;
                case 5:
                    btnDelete.Visible = true;
                    btnDelete.Enabled = (lvPublishers.CheckedItems.Count > 0);
                    break;
                case 6:
                    btnDelete.Visible = true;
                    btnDelete.Enabled = (lvModelScales.CheckedItems.Count > 0);
                    btnMoveUpScale.Enabled = btnMoveDownScale.Enabled = (lvModelScales.CheckedItems.Count == 1);
                    break;
            }
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

        private void loadModelCategories()
        {
            lvModelCategory.Items.Clear();

            foreach (Category mc in CategoryDao.getList())
            {
                ListViewItem li = new ListViewItem(new String[] { mc.code, mc.name, mc.className });
                li.Tag = mc.id;
                lvModelCategory.Items.Add(li);
            }
            lvModelCategory.Columns[0].Width = -2;
            lvModelCategory.Columns[1].Width = -2;
            lvModelCategory.Columns[2].Width = -2;
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
            if (cboModelClass.Items.Count == 0) {
                MessageBox.Show("Dodaj przynajmniej jedną klasę zanim dodasz kategorię", "Nowa kategoria", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
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

            int catId = CategoryDao.add(txtCategoryCode.Text, txtCategoryName.Text, cboModelClass.Text, CategoryDao.getNextSortFlag());
            loadModelCategories();
            setButtons();
            foreach (ListViewItem item in lvModelCategory.Items) {
                if ((int)item.Tag == catId) {
                    item.EnsureVisible();
                    item.Selected = true;
                    break;
                }
            }

            txtCategoryCode.Focus();
            txtCategoryCode.SelectAll();
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

        #region AgeGroup

        private void btnAddAgeGroup_Click(object sender, EventArgs e)
        {
            if(!Regex.IsMatch(txtAge.Text, "[0-9]"))
            {
                MessageBox.Show(this, "Wiek musi być liczbą > 0", "Błędne dane", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (AgeGroupDao.exists(int.Parse(txtAge.Text))) {
                MessageBox.Show("Nowa kategoria wiekowa musi różnić się o przynajmniej 2 lata od już istniejących", "Nowa grupa wiekowa", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtAge.Focus();
                txtAge.SelectAll();
                return;
            }
            int agId = AgeGroupDao.add(txtAgeGroup.Text, int.Parse(txtAge.Text));
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

            foreach (AgeGroup ageGroup in AgeGroupDao.getList()) {
                ListViewItem li = new ListViewItem(new String[] { ageGroup.name, String.Format("{0} - {1}", ageGroup.bottomAge, ageGroup.upperAge) });
                li.Tag = ageGroup.id;
                lvAgeGroup.Items.Add(li);
            }
            lvAgeGroup.Columns[0].Width = -2;
            lvAgeGroup.Columns[1].Width = -2;
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

            txtModelClassName.Focus();
            txtModelClassName.SelectAll();
        }

        private void loadModelClasses() {
            lvModelClass.Items.Clear();
            cboModelClass.Items.Clear();

            foreach (Class cls in ClassDao.getList()) {
                ListViewItem li = new ListViewItem(new String[] { cls.name });
                li.Tag = cls.id;
                lvModelClass.Items.Add(li);
                
                cboModelClass.Items.Add(new ComboBoxItem(cls.id, cls.name));
            }
            lvModelClass.Columns[0].Width = -2;
            if (cboModelClass.Items.Count > 0) {
                cboModelClass.SelectedIndex = 0;
            }
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
            loadModelCategories();
            
            lvModelClass.EndUpdate();
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
            lvModelScales.Columns[0].Width = -2;
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

        private void btnMoveUpScale_Click(object sender, EventArgs e) {
            if (lvModelScales.CheckedItems.Count < 1)
                return;

            if (lvModelScales.CheckedItems[0].Index < 1)
                return;

            lvModelScales.BeginUpdate();

            ListViewItem item = lvModelScales.CheckedItems[0];
            int index = lvModelScales.CheckedItems[0].Index;

            lvModelScales.Items.Remove(item);
            lvModelScales.Items.Insert(index - 1, item);
            lvModelScales.Items[index - 1].Selected = true;

            ScaleDao.updateDisplayOrder((int)lvModelScales.Items[index - 1].Tag, index - 1);
            ScaleDao.updateDisplayOrder((int)lvModelScales.Items[index].Tag, index);

            item.EnsureVisible();
            lvModelScales.EndUpdate();
        }

        private void btnMoveDownScale_Click(object sender, EventArgs e) {
            if (lvModelScales.CheckedItems.Count < 1)
                return;

            if (lvModelScales.CheckedItems[0].Index >= (lvModelScales.Items.Count - 1))
                return;

            lvModelScales.BeginUpdate();

            ListViewItem item = lvModelScales.CheckedItems[0];
            int index = lvModelScales.CheckedItems[0].Index;

            lvModelScales.Items.Remove(item);
            lvModelScales.Items.Insert(index + 1, item);
            item.Selected = true;

            ScaleDao.updateDisplayOrder((int)lvModelScales.Items[index].Tag, index);
            ScaleDao.updateDisplayOrder((int)lvModelScales.Items[index + 1].Tag, index + 1);

            item.EnsureVisible();
            lvModelScales.EndUpdate();
        }

        private void btnAddModelScale_Click(object sender, EventArgs e) {
            if (ScaleDao.exists(txtModelScale.Text)) {
                MessageBox.Show("Skala już istnieje", "Nowa skala", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtModelScale.Focus();
                txtModelScale.SelectAll();
                return;
            }
            
            int scaleId = ScaleDao.add(txtModelScale.Text, ScaleDao.getNextSortFlag());
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
    }
}
