using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UtilitariosSup
{
    public partial class Floading : Form
    {
        public Floading()
        {
            InitializeComponent();
        }

        public void SetProgress(int percentual)
        {
            PBLoading.Value = percentual;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Pen pen = new Pen(Color.Black, 5))
            {
                e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
            }
        }
    }
}
