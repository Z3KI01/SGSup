using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace UtilitariosSup.Forms
{
    public partial class FLogin : Form
    {
        public FLogin()
        {
            InitializeComponent();
        }

        private void txtLogin_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLogin.Text) || txtLogin.Text != "1")
            {
                MessageBox.Show("Informe seu login.", "SGBr Sistemas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLogin.Focus();
                return;
            }
        }

        private void txtSenha_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSenha.Text) || txtSenha.Text != Convert.ToString(DateTime.Today.Day * 900))
            {
                MessageBox.Show("Senha incorreta.", "SGBr Sistemas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSenha.Clear();
                txtSenha.Focus();
                return;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void FLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
            }

            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void FLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
            {
                if (MessageBox.Show("Deseja cancelar o login?", "SGBr Sistemas", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) == DialogResult.No)
                    e.Cancel = true;
            }
        }
    }
}
