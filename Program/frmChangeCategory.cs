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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rejestracja {
    public partial class frmChangeCategory : Form {

        private frmMain _parent;

        public void setParent(frmMain parentForm) {
            this._parent = parentForm;
        }

        public frmChangeCategory() {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e) {

            Application.UseWaitCursor = true;

            cboModelCategory.Enabled = false;
            btnSave.Enabled = false;
            btnClose.Enabled = false;

            int catId = ((ComboBoxItem)cboModelCategory.SelectedItem).id;

            try {
                this._parent.changeCategoryInSelected(catId);
            }
            finally {
                Application.UseWaitCursor = false;
            }
            this.Close();
        }

        private void frmChangeCategory_Load(object sender, EventArgs e) {
            ModelCategory[] categories = ModelCategoryDao.getList().ToArray();
            foreach (ModelCategory category in categories) {
                cboModelCategory.Items.Add(new ComboBoxItem(category.id, category.fullName));
            }
            if (cboModelCategory.Items.Count > 0) {
                btnSave.Enabled = true;
                cboModelCategory.SelectedIndex = 0;
            }
            else {
                btnSave.Enabled = false;
            }
        }
    }
}
