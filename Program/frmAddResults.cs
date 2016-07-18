using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rejestracja
{
    public partial class frmAddResults : Form
    {
        public frmAddResults()
        {
            InitializeComponent();
        }

        private void frmAddResults_Load(object sender, EventArgs e) {

            //Category results tab
            lvResults.View = View.Details;
            lvResults.GridLines = true;
            lvResults.FullRowSelect = true;
            lvResults.HeaderStyle = ColumnHeaderStyle.Nonclickable;

            lvResults.Columns.Add("Nr Modelu");
            lvResults.Columns.Add("Nazwa Modelu");
            lvResults.Columns.Add("Skala");
            lvResults.Columns.Add("Wydawca");
            lvResults.Columns.Add("Miejsce");

            foreach (ModelCategory category in ModelCategory.getList()) {
                cboModelCategory.Items.Add(new ComboBoxItem(category.id, category.getFullName()));
            }
            if (cboModelCategory.Items.Count > 0) {
                cboModelCategory.SelectedIndex = 0;
            }

            //Special award results tab
            lvAwardResults.View = View.Details;
            lvAwardResults.GridLines = true;
            lvAwardResults.FullRowSelect = true;
            lvAwardResults.HeaderStyle = ColumnHeaderStyle.Nonclickable;

            lvAwardResults.Columns.Add("Nr Modelu");
            lvAwardResults.Columns.Add("Nazwa Modelu");
            lvAwardResults.Columns.Add("Skala");
            lvAwardResults.Columns.Add("Wydawca");
            lvAwardResults.Columns.Add("Nagroda");

            foreach (Award award in Award.getList()) {
                cboSpecialAward.Items.Add(new ComboBoxItem(award.id, award.title));
            }
            if (cboSpecialAward.Items.Count > 0) {
                cboSpecialAward.SelectedIndex = 0;
            }
            else {
                btaAddAwardWinner.Enabled = false;
                txtEntryId.Enabled = false;
            }
            loadAwardResults();
        }

        private void cboModelCategory_SelectedIndexChanged(object sender, EventArgs e) {

            Application.UseWaitCursor = true;
            lvResults.BeginUpdate();

            try {
                lvResults.Items.Clear();
                lvResults.Groups.Clear();

                if (cboModelCategory.SelectedIndex == -1) {
                    return;
                }

                long catId = ((ComboBoxItem)cboModelCategory.SelectedItem).id;
                RegistrationEntry[] entries = Results.getEntriesInCategory(ModelCategory.get(catId).getFullName()).ToArray();

                String modelClass = "";
                String ageGroup = "";
                ListViewGroup group = new ListViewGroup("");

                foreach (RegistrationEntry entry in entries) {
                    if (!modelClass.Equals(entry.modelClass) || !ageGroup.Equals(entry.ageGroup)) {
                        modelClass = entry.modelClass;
                        ageGroup = entry.ageGroup;
                        group = new ListViewGroup(String.Format("{0} - {1}", ageGroup, modelClass.ToUpper()));
                        lvResults.Groups.Add(group);
                    }
                    ListViewItem item =
                        new ListViewItem(
                            new String[] { entry.entryId.ToString(), entry.modelName, entry.modelScale, entry.modelPublisher, (entry.place == 0 ? "" : entry.place.ToString()) },
                            group
                        );
                    if (entry.place > 0) {
                        item.Font = new Font(item.Font, FontStyle.Bold);
                    }
                    lvResults.Items.Add(item);
                }

                foreach (ColumnHeader header in lvResults.Columns) {
                    header.Width = -2;
                }
            }
            finally {
                lvResults.EndUpdate();
                Application.UseWaitCursor = false;
            }
        }

        private void lvResults_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (lvResults.SelectedItems.Count < 0) {
                return;
            }

            ListViewItem item = lvResults.SelectedItems[0];            
            int nPlace = 1;

            if (!int.TryParse(item.SubItems[4].Text, out nPlace)) {
                setCategoryResult(item, 1);
                return;
            }

            nPlace++;
            
            if (nPlace > 3) {
                setCategoryResult(item, 0);
            }
            else {
                setCategoryResult(item, nPlace);
            }
        }

        private void lvResults_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Right) {
                ListViewHitTestInfo hitTest = this.lvResults.HitTest(e.X, e.Y);
                if (hitTest.Item != null) {
                    ListViewItem selectedItem = hitTest.Item;
                    foreach (ListViewItem item in lvResults.Items)
                        item.Selected = false;

                    selectedItem.Selected = true;
                    
                    int place = 0;
                    if (selectedItem.SubItems[4].Text.Length > 0) {
                        place = int.Parse(selectedItem.SubItems[4].Text);
                    }

                    mnuRCFirstPlace.Checked = (place == 1);
                    mnuRCSecondPlace.Checked = (place == 2);
                    mnuRCThirdPlace.Checked = (place == 3);
                    
                    cmsResultsRightClick.Show(Cursor.Position);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void setCategoryResult(ListViewItem item, int place) {

            try {

                lvResults.BeginUpdate();

                long entryId = long.Parse(item.SubItems[0].Text);
                String strPlace = item.SubItems[4].Text;



                int nPlace;
                if (int.TryParse(strPlace, out nPlace)) {
                    Results.deleteResult(entryId, nPlace);
                    item.SubItems[4].Text = "";
                    item.Font = new Font(item.Font, FontStyle.Regular);
                }

                if (place > 0) {
                    item.Font = new Font(item.Font, FontStyle.Bold);
                    Results.addCategoryResult(entryId, place);
                    item.SubItems[4].Text = place.ToString();

                    //Adjust the column width after setting bold fond
                    foreach (ColumnHeader header in lvResults.Columns) {
                        header.Width = -2;
                    }
                }
            }
            finally {
                lvResults.EndUpdate();
            }
        }

        private void mnuRCFirstPlace_Click(object sender, EventArgs e) {
            if (lvResults.SelectedItems.Count == 0) {
                return;
            }
            setCategoryResult(lvResults.SelectedItems[0], 1);
        }

        private void mnuRCSecondPlace_Click(object sender, EventArgs e) {
            if (lvResults.SelectedItems.Count == 0) {
                return;
            }
            setCategoryResult(lvResults.SelectedItems[0], 2);
        }

        private void mnuRCThirdPlace_Click(object sender, EventArgs e) {
            if (lvResults.SelectedItems.Count == 0) {
                return;
            }
            setCategoryResult(lvResults.SelectedItems[0], 3);
        }

        private void mnuRCClear_Click(object sender, EventArgs e) {
            if (lvResults.SelectedItems.Count == 0) {
                return;
            }
            setCategoryResult(lvResults.SelectedItems[0], 0);
        }

        private void btaAddAwardWinner_Click(object sender, EventArgs e) {
           
            long entryId;
            
            if(!long.TryParse(txtEntryId.Text, out entryId)) {
                MessageBox.Show("Wpisz poprawny numer modelu", "Dodawanie Wyników", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            //Check if the entry ID exists
            RegistrationEntry entry = RegistrationEntry.get(entryId);
            if(entry == null) {
                MessageBox.Show("Numer modelu nie znaleziony w bazie", "Dodawanie Wyników", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            long awardId = ((ComboBoxItem)cboSpecialAward.SelectedItem).id;
            //Check if it is already added to the award list
            if (Results.awardEntryExists(entryId, awardId)) {
                MessageBox.Show("Wpis istnieje", "Dodawanie Wyników", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //Add it
            Results.addAwardWinner(entryId, awardId);
            loadAwardResults();
            txtEntryId.SelectAll();
            txtEntryId.Focus();
        }

        private void lvAwardResults_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Right) {
                ListViewHitTestInfo hitTest = this.lvAwardResults.HitTest(e.X, e.Y);
                if (hitTest.Item != null) {
                    ListViewItem selectedItem = hitTest.Item;
                    foreach (ListViewItem item in lvAwardResults.Items)
                        item.Selected = false;

                    selectedItem.Selected = true;
                    cmsAwardResultsRightClick.Show(Cursor.Position);
                }
            }
        }

        private void loadAwardResults() {
            lvAwardResults.Items.Clear();
            foreach (String[] item in Results.getAwardResultList()) {
                ListViewItem lvItem = new ListViewItem(item.Skip(1).ToArray());
                lvItem.Tag = long.Parse(item[0]);
                lvAwardResults.Items.Add(lvItem);
            }
            foreach (ColumnHeader header in lvAwardResults.Columns) {
                header.Width = -2;
            }
        }

        private void cmsARCDelete_Click(object sender, EventArgs e) {
            if (lvAwardResults.SelectedItems.Count == 0) {
                return;
            }
            long resultId = (Int64)lvAwardResults.SelectedItems[0].Tag;
            Results.deleteResult(resultId);
            loadAwardResults();
        }
    }
}
