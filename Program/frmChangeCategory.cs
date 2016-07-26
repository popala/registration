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
