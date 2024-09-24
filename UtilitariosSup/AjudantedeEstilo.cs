using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UtilitariosSup
{
    public static class AjudantedeEstilo
    {
        public static void ReformulaLblAviso(Label label, string mensagem, int tamanhoFonte, FontStyle estiloFonte = FontStyle.Bold)
        {
            label.Font = new Font(label.Font.FontFamily, tamanhoFonte, estiloFonte);
            label.Text = mensagem;
        }

        public static void ReformulaTxtBox(TextBox txtBox, int tamanhoFonte, HorizontalAlignment alinhamento, string mensagem = null, FontStyle estiloFonte = FontStyle.Bold, Color? corFonte = null)
        {
            txtBox.Font = new Font(txtBox.Font.FontFamily, tamanhoFonte, estiloFonte);
            txtBox.ForeColor = corFonte ?? Color.DarkGray;
            txtBox.TextAlign = alinhamento;
            txtBox.Text = mensagem;
        }
    }
}
