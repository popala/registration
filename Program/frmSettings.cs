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

            btnMoveUpCategory.Enabled = false;
            btnMoveDownCategory.Enabled = false;

            lvModelCategory.View = View.Details;
            lvModelCategory.FullRowSelect = true;
            lvModelCategory.Columns.Add("Kod");
            lvModelCategory.Columns.Add("Nazwa");
            lvModelCategory.Columns.Add("Kategoria");
            lvModelCategory.CheckBoxes = false;
            lvModelCategory.GridLines = true;
            lvModelCategory.MultiSelect = false;
            lvModelCategory.HideSelection = false;
            
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

            loadAgeGroups();
            if (lvAgeGroup.Items.Count > 0)
                lvAgeGroup.Items[0].Selected = true;

            /** ModelCategory **/
            txtModelClassName.MaxLength = ModelClass.MAX_NAME_LENGTH;

            lvModelClass.View = View.Details;
            lvModelClass.FullRowSelect = true;
            lvModelClass.Columns.Add("Nazwa");
            lvModelClass.CheckBoxes = false;
            lvModelClass.GridLines = true;
            lvModelClass.MultiSelect = false;
            lvModelClass.HideSelection = false;

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

            loadModelScales();
            if (lvModelScales.Items.Count > 0)
                lvModelScales.Items[0].Selected = true;

            loadOptions();
        }

        private void loadOptions()
        {
            txtHeading.Text = Options.get("DocumentHeader");
            txtFooter.Text = Options.get("DocumentFooter");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Options.set("DocumentHeader", txtHeading.Text);
            Options.set("DocumentFooter", txtFooter.Text);
            this.Close();
        }

        private void tcOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelete.Visible = (tcOptions.SelectedIndex != 0);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            switch (tcOptions.SelectedIndex)
            {
                case 1:
                    if (lvAgeGroup.SelectedItems.Count < 1)
                        return;
                    AgeGroupDao.delete((Int64)lvAgeGroup.SelectedItems[0].Tag);
                    loadAgeGroups();
                    break;

                case 2:
                    if (lvModelClass.SelectedItems.Count < 1)
                        return;
                    ModelClassDao.delete((Int64)lvModelClass.SelectedItems[0].Tag);
                    loadModelClasses();
                    break;

                case 3:
                    if (lvModelCategory.SelectedItems.Count < 1)
                        return;
                    ModelCategoryDao.delete((Int64)lvModelCategory.SelectedItems[0].Tag);
                    loadModelCategories();
                    break;

                case 4:
                    if (lvAwards.SelectedItems.Count < 1)
                        return;
                    AwardDao.delete((int)lvAwards.SelectedItems[0].Tag);
                    loadAwards();
                    break;

                case 5:
                    if (lvPublishers.SelectedItems.Count < 1)
                        return;
                    PublisherDao.delete((Int64)lvPublishers.SelectedItems[0].Tag);
                    loadPublishers();
                    break;

                case 6:
                    if (lvModelScales.SelectedItems.Count < 1)
                        return;
                    ModelScaleDao.delete((Int64)lvModelScales.SelectedItems[0].Tag);
                    loadModelScales();
                    break;
            }
        }
        
        private void lvModelClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvModelCategory.SelectedItems.Count > 0)
            {
                btnMoveUpCategory.Enabled = true;
                btnMoveDownCategory.Enabled = true;
                btnDelete.Enabled = true;
                return;
            }

            btnMoveUpCategory.Enabled = false;
            btnMoveDownCategory.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void loadModelCategories()
        {
            lvModelCategory.Items.Clear();

            foreach (ModelCategory mc in ModelCategoryDao.getList())
            {
                ListViewItem li = new ListViewItem(new String[] { mc.code, mc.name, mc.modelClass });
                li.Tag = mc.id;
                lvModelCategory.Items.Add(li);
            }
            lvModelCategory.Columns[0].Width = -2;
            lvModelCategory.Columns[1].Width = -2;
            lvModelCategory.Columns[2].Width = -2;
        }

        private void btnMoveUpCategory_Click(object sender, EventArgs e)
        {
            if (lvModelCategory.SelectedItems.Count < 1)
                return;

            if (lvModelCategory.SelectedItems[0].Index < 1)
                return;

            ListViewItem item = lvModelCategory.SelectedItems[0];
            int index = lvModelCategory.SelectedItems[0].Index;

            lvModelCategory.Items.Remove(item);
            lvModelCategory.Items.Insert(index - 1, item);
            lvModelCategory.Items[index - 1].Selected = true;

            ModelCategoryDao.updateDisplayOrder((Int64)lvModelCategory.Items[index - 1].Tag, index - 1);
            ModelCategoryDao.updateDisplayOrder((Int64)lvModelCategory.Items[index].Tag, index);
        }

        private void btnMoveDownCategory_Click(object sender, EventArgs e)
        {
            if (lvModelCategory.SelectedItems.Count < 1)
                return;

            if (lvModelCategory.SelectedItems[0].Index >= (lvModelCategory.Items.Count - 1))
                return;

            ListViewItem selectedItem = lvModelCategory.SelectedItems[0];
            int index = lvModelCategory.SelectedItems[0].Index;

            lvModelCategory.Items.Remove(selectedItem);
            lvModelCategory.Items.Insert(index + 1, selectedItem);
            selectedItem.Selected = true;

            ModelCategoryDao.updateDisplayOrder((Int64)lvModelCategory.Items[index].Tag, index);
            ModelCategoryDao.updateDisplayOrder((Int64)lvModelCategory.Items[index + 1].Tag, index + 1);
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

            if (ModelCategoryDao.codeExists(txtCategoryCode.Text.Trim()))
            {
                MessageBox.Show("Kod kategorii jest już wykorzystany", "Nowa kategoria", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCategoryCode.Focus();
                txtCategoryCode.SelectAll();
                return;
            }

            ModelCategoryDao.add(txtCategoryCode.Text, txtCategoryName.Text, cboModelClass.Text, ModelCategoryDao.getNextSortFlag());
            loadModelCategories();
            if(lvModelCategory.Items.Count > 0)
                lvModelCategory.Items[lvModelCategory.Items.Count - 1].Selected = true;

            txtCategoryCode.Focus();
            txtCategoryCode.SelectAll();
        }

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

        private void loadAgeGroups()
        {
            lvAgeGroup.Items.Clear();

            foreach (AgeGroup ageGroup in AgeGroupDao.getList())
            {
                ListViewItem li = new ListViewItem(new String[] { ageGroup.name, String.Format("{0} - {1}", ageGroup.bottomAge, ageGroup.upperAge) });
                li.Tag = ageGroup.id;
                lvAgeGroup.Items.Add(li);
            }
            lvAgeGroup.Columns[0].Width = -2;
            lvAgeGroup.Columns[1].Width = -2;
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

            PublisherDao.add(txtPublisherName.Text.Trim());
            loadPublishers();
            if (lvPublishers.Items.Count > 0)
                lvPublishers.Items[0].Selected = true;

            txtPublisherName.Focus();
            txtPublisherName.SelectAll();
        }

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
            AgeGroupDao.add(txtAgeGroup.Text, int.Parse(txtAge.Text));
            loadAgeGroups();
        }

        private void txtAge_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnAddModelClass_Click(object sender, EventArgs e) {
            if (txtModelClassName.Text.Trim().Length < 1) {
                MessageBox.Show("Nazwa klasy wymagana", "Nowa klasa", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtModelClassName.Focus();
                txtModelClassName.SelectAll();
                return;
            }

            if (ModelClassDao.exists(txtModelClassName.Text.Trim())) {
                MessageBox.Show("Klasa istnieje", "Nowa klasa", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtModelClassName.Focus();
                txtModelClassName.SelectAll();
                return;
            }

            ModelClassDao.add(txtModelClassName.Text.Trim());
            loadModelClasses();
            if (lvModelClass.Items.Count > 0)
                lvModelClass.Items[0].Selected = true;

            txtModelClassName.Focus();
            txtModelClassName.SelectAll();
        }

        private void loadModelClasses() {
            lvModelClass.Items.Clear();
            cboModelClass.Items.Clear();

            foreach (ModelClass cls in ModelClassDao.getList()) {
                ListViewItem li = new ListViewItem(new String[] { cls.name });
                li.Tag = cls.id;
                lvModelClass.Items.Add(li);
                
                cboModelClass.Items.Add(cls.name);
            }
            lvModelClass.Columns[0].Width = -2;
            if (cboModelClass.Items.Count > 0) {
                cboModelClass.SelectedIndex = 0;
            }
        }

        private void loadModelScales() {
            lvModelScales.Items.Clear();
            foreach (ModelScale scale in ModelScaleDao.getList()) {
                ListViewItem li = new ListViewItem(new String[] { scale.name });
                li.Tag = scale.id;
                lvModelScales.Items.Add(li);
            }
            lvModelScales.Columns[0].Width = -2;
        }

        private void btnAddModelScale_Click(object sender, EventArgs e) {
            if (ModelScaleDao.exists(txtModelScale.Text)) {
                MessageBox.Show("Skala już istnieje", "Nowa skala", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtModelScale.Focus();
                txtModelScale.SelectAll();
                return;
            }
            ModelScaleDao.add(txtModelScale.Text, ModelScaleDao.getNextSortFlag());
            loadModelScales();
        }

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

            AwardDao.add(awardTitle, AwardDao.getNextSortFlag());
            loadAwards();
            txtAwardTitle.SelectAll();
            txtAwardTitle.Focus();
        }

        private void btnMoveUpAward_Click(object sender, EventArgs e) {
            if (lvAwards.SelectedItems.Count < 1)
                return;

            if (lvAwards.SelectedItems[0].Index < 1)
                return;

            ListViewItem item = lvAwards.SelectedItems[0];
            int index = lvAwards.SelectedItems[0].Index;

            lvAwards.Items.Remove(item);
            lvAwards.Items.Insert(index - 1, item);
            lvAwards.Items[index - 1].Selected = true;

            AwardDao.updateDisplayOrder((int)lvAwards.Items[index - 1].Tag, index - 1);
            AwardDao.updateDisplayOrder((int)lvAwards.Items[index].Tag, index);
        }

        private void btnMoveDownAward_Click(object sender, EventArgs e) {
            if (lvAwards.SelectedItems.Count < 1)
                return;

            if (lvAwards.SelectedItems[0].Index >= (lvAwards.Items.Count - 1))
                return;

            ListViewItem selectedItem = lvAwards.SelectedItems[0];
            int index = lvAwards.SelectedItems[0].Index;

            lvAwards.Items.Remove(selectedItem);
            lvAwards.Items.Insert(index + 1, selectedItem);
            selectedItem.Selected = true;

            AwardDao.updateDisplayOrder((int)lvAwards.Items[index].Tag, index);
            AwardDao.updateDisplayOrder((int)lvAwards.Items[index + 1].Tag, index + 1);
        }
    }
}
