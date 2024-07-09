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
            this.listBoxArquivos = new UtilitariosSup.fUtilitarios.CenteredListBox();
            this.pBSgMaster = new System.Windows.Forms.PictureBox();
            this.lblAviso = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pBSgMaster)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxArquivos
            // 
            this.listBoxArquivos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxArquivos.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxArquivos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxArquivos.FormattingEnabled = true;
            this.listBoxArquivos.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.listBoxArquivos.IntegralHeight = false;
            this.listBoxArquivos.ItemHeight = 16;
            this.listBoxArquivos.Location = new System.Drawing.Point(9, 97);
            this.listBoxArquivos.Name = "listBoxArquivos";
            this.listBoxArquivos.Size = new System.Drawing.Size(294, 185);
            this.listBoxArquivos.TabIndex = 0;
            this.listBoxArquivos.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxArquivos_MouseDoubleClick);
            // 
            // pBSgMaster
            // 
            this.pBSgMaster.Image = global::UtilitariosSup.Properties.Resources.logo_master_220x48px;
            this.pBSgMaster.Location = new System.Drawing.Point(10, 2);
            this.pBSgMaster.Name = "pBSgMaster";
            this.pBSgMaster.Size = new System.Drawing.Size(293, 94);
            this.pBSgMaster.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pBSgMaster.TabIndex = 2;
            this.pBSgMaster.TabStop = false;
            this.pBSgMaster.Click += new System.EventHandler(this.pBSgMaster_Click);
            // 
            // lblAviso
            // 
            this.lblAviso.AutoSize = true;
            this.lblAviso.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAviso.ForeColor = System.Drawing.Color.Red;
            this.lblAviso.Location = new System.Drawing.Point(6, 295);
            this.lblAviso.Name = "lblAviso";
            this.lblAviso.Size = new System.Drawing.Size(300, 13);
            this.lblAviso.TabIndex = 3;
            this.lblAviso.Text = "*DUPLO CLICK OU F8 PARA INICIAR DOWNLOAD ";
            // 
            // fUtilitarios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 322);
            this.Controls.Add(this.lblAviso);
            this.Controls.Add(this.pBSgMaster);
            this.Controls.Add(this.listBoxArquivos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fUtilitarios";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SGBr - Utilitário para download de arquivos";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.fUtilitarios_MouseClick);
            ((System.ComponentModel.ISupportInitialize)(this.pBSgMaster)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pBSgMaster;
        private System.Windows.Forms.Label lblAviso;
        public CenteredListBox listBoxArquivos;
    }
}

