using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rejestracja
{
    public partial class frmNewDataFile : Form
    {
        public frmNewDataFile()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            btnOk.Enabled = false;
            btnCancel.Enabled = false;
            tabControl1.Enabled = false;
            txtContestTitle.Enabled = false;

            String strFilePath = txtDataFileName.Text;
            if (tabControl1.SelectedIndex == 1) {
                strFilePath = lvFileList.SelectedItems[0].Text;
            }

            DataSource ds = new DataSource(strFilePath);
            frmMain parent = (frmMain)this.Owner;
            parent.setShowSettingsForm(true);

            Application.UseWaitCursor = false;

            this.Close();
        }

        private void frmNewDataFile_Shown(object sender, EventArgs e) {
            txtContestTitle.Focus();
        }

        private void frmNewDataFile_Load(object sender, EventArgs e)
        {
            lvFileList.Columns.Add("Nazwa pliku");
            lvFileList.Columns[0].Width = (int)Math.Ceiling(lvFileList.Width * .9);
            lvFileList.HideSelection = false;

            String[] files = Directory.GetFiles(Resources.DataFileFolder, "*.sqlite");
            foreach(String fileName in files)
            {
                lvFileList.Items.Add(new ListViewItem(fileName));
            }
            
            btnOk.Enabled = false;
            lblError.Visible = false;
            txtContestTitle.Focus();
        }

        private void checkNewFileName() {
            txtDataFileName.Text = Resources.removeDiacritics(txtContestTitle.Text).Replace(" ", "_").ToLower();
            if (txtDataFileName.Text.Length < 1) {
                btnOk.Enabled = false;
                return;
            }
            if (!txtDataFileName.Text.EndsWith(".sqlite")) {
                txtDataFileName.Text += ".sqlite";
            }

            String filePath = Path.Combine(Resources.DataFileFolder, txtDataFileName.Text);
            if (File.Exists(filePath)) {
                btnOk.Enabled = false;
                lblError.Text = "Plik istnieje";
                lblError.Visible = true;
                return;
            }

            lblError.Visible = false;
            btnOk.Enabled = true;
        }

        private void checkSelectedFile() {
            if (lvFileList.SelectedItems.Count == 0) {
                btnOk.Enabled = false;
                lblError.Text = "Wybierz plik";
                lblError.Visible = true;
                return;
            }

            lblError.Visible = false;
            btnOk.Enabled = true;
        }

        private void txtContestTitle_TextChanged(object sender, EventArgs e)
        {
            checkNewFileName();
        }

        private void tabControl1_SelectedIndexChanged(Object sender, EventArgs e) {
            if (tabControl1.SelectedIndex == 0) {
                checkNewFileName();
            }
            else {
                checkSelectedFile();
            }
        }

        private void lvFileList_SelectedIndexChanged(Object sender, EventArgs e) {
            checkSelectedFile();
        }
    }
}
