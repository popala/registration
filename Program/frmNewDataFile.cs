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
using Rejestracja.Utils;
using System;
using System.IO;
using System.Windows.Forms;

namespace Rejestracja
{
    public partial class frmNewDataFile : Form
    {
        public void setSelectedTab(int selectedTab) {
            if (selectedTab > 0) {
                tabControl1.SelectedIndex = 1;
            }
            else {
                tabControl1.SelectedIndex = 0;
            }
        }

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
