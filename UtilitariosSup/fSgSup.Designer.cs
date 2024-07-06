namespace UtilitariosSup
{
    partial class fUtilitarios
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fUtilitarios));
            this.listBoxArquivos = new System.Windows.Forms.ListBox();
            this.pBSgMaster = new System.Windows.Forms.PictureBox();
            this.lblAviso = new System.Windows.Forms.Label();
            this.lblAviso2 = new System.Windows.Forms.Label();
            this.lblAviso3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pBSgMaster)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxArquivos
            // 
            this.listBoxArquivos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxArquivos.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxArquivos.FormattingEnabled = true;
            this.listBoxArquivos.ItemHeight = 20;
            this.listBoxArquivos.Location = new System.Drawing.Point(82, 99);
            this.listBoxArquivos.Name = "listBoxArquivos";
            this.listBoxArquivos.Size = new System.Drawing.Size(202, 22);
            this.listBoxArquivos.TabIndex = 0;
            this.listBoxArquivos.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxArquivos_MouseDoubleClick);
            // 
            // pBSgMaster
            // 
            this.pBSgMaster.Image = global::UtilitariosSup.Properties.Resources.logo_master_220x48px;
            this.pBSgMaster.Location = new System.Drawing.Point(54, 25);
            this.pBSgMaster.Name = "pBSgMaster";
            this.pBSgMaster.Size = new System.Drawing.Size(226, 52);
            this.pBSgMaster.TabIndex = 2;
            this.pBSgMaster.TabStop = false;
            // 
            // lblAviso
            // 
            this.lblAviso.AutoSize = true;
            this.lblAviso.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAviso.ForeColor = System.Drawing.Color.Red;
            this.lblAviso.Location = new System.Drawing.Point(11, 145);
            this.lblAviso.Name = "lblAviso";
            this.lblAviso.Size = new System.Drawing.Size(300, 13);
            this.lblAviso.TabIndex = 3;
            this.lblAviso.Text = "*DUPLO CLICK OU F8 PARA INICIAR DOWNLOAD ";
            // 
            // lblAviso2
            // 
            this.lblAviso2.AutoSize = true;
            this.lblAviso2.BackColor = System.Drawing.Color.Transparent;
            this.lblAviso2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAviso2.Location = new System.Drawing.Point(15, 101);
            this.lblAviso2.Name = "lblAviso2";
            this.lblAviso2.Size = new System.Drawing.Size(66, 18);
            this.lblAviso2.TabIndex = 4;
            this.lblAviso2.Text = "Buscar:";
            // 
            // lblAviso3
            // 
            this.lblAviso3.AutoSize = true;
            this.lblAviso3.BackColor = System.Drawing.Color.Transparent;
            this.lblAviso3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAviso3.Location = new System.Drawing.Point(284, 92);
            this.lblAviso3.Name = "lblAviso3";
            this.lblAviso3.Size = new System.Drawing.Size(27, 31);
            this.lblAviso3.TabIndex = 5;
            this.lblAviso3.Text = "⬍";
            // 
            // fUtilitarios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 182);
            this.Controls.Add(this.lblAviso3);
            this.Controls.Add(this.lblAviso2);
            this.Controls.Add(this.lblAviso);
            this.Controls.Add(this.pBSgMaster);
            this.Controls.Add(this.listBoxArquivos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "fUtilitarios";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SG Sup";
            ((System.ComponentModel.ISupportInitialize)(this.pBSgMaster)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pBSgMaster;
        public System.Windows.Forms.ListBox listBoxArquivos;
        private System.Windows.Forms.Label lblAviso;
        public System.Windows.Forms.Label lblAviso2;
        public System.Windows.Forms.Label lblAviso3;
    }
}

