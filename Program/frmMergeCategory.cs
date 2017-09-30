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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Rejestracja {
    public partial class frmMergeCategory : Form {
        public frmMergeCategory() {
            InitializeComponent();
        }

        private void frmMergeCategory_Load(object sender, EventArgs e) {
            for(int i = 1; i < 6; i++) {
                cboMaxCount.Items.Add(i);
            }

            lvCategories.View = View.Details;
            lvCategories.GridLines = true;
            lvCategories.FullRowSelect = true;
            lvCategories.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvCategories.CheckBoxes = true;

            lvCategories.Columns.Add("Grupa Wiekowa");
            lvCategories.Columns.Add("Klasa");
            lvCategories.Columns.Add("Liczba Modeli");
            lvCategories.Columns.Add("Opis");

            cboMaxCount.SelectedIndex = 2;
        }

        private void cboMaxCount_SelectedIndexChanged(object sender, EventArgs e) {
            loadSummary((int)cboMaxCount.SelectedItem);
        }

        private void lvCategories_ItemChecked(object sender, ItemCheckedEventArgs e) {
            if (e.Item.Checked) {
                AgeGroup ag = new AgeGroupDao().getOlderAgeGroup(e.Item.Text, -1);
                if (ag == null) {
                    e.Item.Checked = false;
                }
                else {
                    e.Item.SubItems[3].Text = String.Format("Dołącz {0} do {1}", e.Item.Text, ag.name);
                }
            }
            else {
                e.Item.SubItems[3].Text = "";
            }
        }

        private void loadSummary(int maxCountInCategory) {
            lvCategories.BeginUpdate();

            try {
                
                lvCategories.Groups.Clear();
                lvCategories.Items.Clear();

                List<String[]> categories = new RegistrationEntryDao().getListForMergingCategories(maxCountInCategory).ToList();
                long categoryId = -2;
                ListViewGroup group = null;

                foreach (String[] category in categories) {
                    long catId = long.Parse(category[3]);
                    if (categoryId != catId) {
                        if (categoryId > -2) {
                            group.Items[group.Items.Count - 1].ForeColor = SystemColors.GrayText;
                        }
                        categoryId = catId;
                        group = new ListViewGroup(category[3], category[2]);
                        lvCategories.Groups.Add(group);
                    }
                    ListViewItem item = new ListViewItem(new String[] { category[0], category[1], category[4], "" }, group);
                    item.Tag = long.Parse(category[3]);
                    lvCategories.Items.Add(item);
                }
                if (group != null) {
                    group.Items[group.Items.Count - 1].ForeColor = SystemColors.GrayText;
                }

                foreach (ColumnHeader header in lvCategories.Columns) {
                    header.Width = -1;
                    header.Width = -2;
                }
            }
            finally {
                lvCategories.EndUpdate();
            }
        }

        private void btnMerge_Click(object sender, EventArgs e) {
            RegistrationEntryDao registrationEntryDao = new RegistrationEntryDao();
            long categoryId = -1;
            String targetAgeGroup = null;
            String sourceAgeGroup = null;

            Application.UseWaitCursor = true;

            for (int i = 0; i < lvCategories.Items.Count; i++) {
                ListViewItem item = lvCategories.Items[i];
                if (item.Checked) {
                    categoryId = (long)item.Tag;
                    if (categoryId < 0) {
                        continue;
                    }
                    sourceAgeGroup = item.Text;
                    targetAgeGroup = lvCategories.Items[i + 1].Text;
                    registrationEntryDao.mergeAgeGroupsInCategory(categoryId, sourceAgeGroup, targetAgeGroup);
                }
            }

            loadSummary((int)cboMaxCount.SelectedItem);
            Options.set("ValidateAgeGroup", "false");
            Application.UseWaitCursor = false;
        }
    }
}
