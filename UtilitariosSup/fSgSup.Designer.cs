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
            this.BtnDownload = new System.Windows.Forms.Button();
            this.TbPesquisar = new System.Windows.Forms.TextBox();
            this.pbButtonPesquisar = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pBSgMaster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbButtonPesquisar)).BeginInit();
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
            this.listBoxArquivos.Location = new System.Drawing.Point(9, 92);
            this.listBoxArquivos.Name = "listBoxArquivos";
            this.listBoxArquivos.Size = new System.Drawing.Size(294, 207);
            this.listBoxArquivos.TabIndex = 0;
            this.listBoxArquivos.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listBoxArquivos_MouseClick);
            this.listBoxArquivos.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxArquivos_MouseDoubleClick);
            // 
            // pBSgMaster
            // 
            this.pBSgMaster.Image = global::UtilitariosSup.Properties.Resources.logo_master_220x48px;
            this.pBSgMaster.Location = new System.Drawing.Point(9, 12);
            this.pBSgMaster.Name = "pBSgMaster";
            this.pBSgMaster.Size = new System.Drawing.Size(293, 74);
            this.pBSgMaster.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pBSgMaster.TabIndex = 2;
            this.pBSgMaster.TabStop = false;
            this.pBSgMaster.Click += new System.EventHandler(this.pBSgMaster_Click);
            // 
            // lblAviso
            // 
            this.lblAviso.AutoSize = true;
            this.lblAviso.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAviso.ForeColor = System.Drawing.Color.Red;
            this.lblAviso.Location = new System.Drawing.Point(8, 358);
            this.lblAviso.Name = "lblAviso";
            this.lblAviso.Size = new System.Drawing.Size(305, 12);
            this.lblAviso.TabIndex = 3;
            this.lblAviso.Text = "*OU DUPLO CLICK NO NOME PARA INICIAR DONWLOAD";
            this.lblAviso.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lblAviso_MouseClick);
            // 
            // BtnDownload
            // 
            this.BtnDownload.BackColor = System.Drawing.Color.Blue;
            this.BtnDownload.FlatAppearance.BorderSize = 0;
            this.BtnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDownload.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnDownload.ForeColor = System.Drawing.Color.White;
            this.BtnDownload.Location = new System.Drawing.Point(12, 313);
            this.BtnDownload.Name = "BtnDownload";
            this.BtnDownload.Size = new System.Drawing.Size(99, 28);
            this.BtnDownload.TabIndex = 4;
            this.BtnDownload.Text = "DONWLOAD - F8";
            this.BtnDownload.UseVisualStyleBackColor = false;
            this.BtnDownload.Click += new System.EventHandler(this.BtnDownload_Click);
            // 
            // TbPesquisar
            // 
            this.TbPesquisar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TbPesquisar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TbPesquisar.ForeColor = System.Drawing.Color.DarkGray;
            this.TbPesquisar.Location = new System.Drawing.Point(117, 313);
            this.TbPesquisar.Multiline = true;
            this.TbPesquisar.Name = "TbPesquisar";
            this.TbPesquisar.Size = new System.Drawing.Size(156, 28);
            this.TbPesquisar.TabIndex = 5;
            this.TbPesquisar.Text = "BUSCAR (F2)";
            this.TbPesquisar.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TbPesquisar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TbPesquisar_MouseDown);
            // 
            // pbButtonPesquisar
            // 
            this.pbButtonPesquisar.Image = global::UtilitariosSup.Properties.Resources.lupa_pequena;
            this.pbButtonPesquisar.Location = new System.Drawing.Point(275, 313);
            this.pbButtonPesquisar.Name = "pbButtonPesquisar";
            this.pbButtonPesquisar.Size = new System.Drawing.Size(27, 28);
            this.pbButtonPesquisar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbButtonPesquisar.TabIndex = 6;
            this.pbButtonPesquisar.TabStop = false;
            this.pbButtonPesquisar.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbButtonPesquisar_MouseClick);
            // 
            // fUtilitarios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 379);
            this.Controls.Add(this.pbButtonPesquisar);
            this.Controls.Add(this.TbPesquisar);
            this.Controls.Add(this.BtnDownload);
            this.Controls.Add(this.lblAviso);
            this.Controls.Add(this.pBSgMaster);
            this.Controls.Add(this.listBoxArquivos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fUtilitarios";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SGBr - Utilitário para download de arquivos";
            this.Load += new System.EventHandler(this.fUtilitarios_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.fUtilitarios_MouseClick);
            ((System.ComponentModel.ISupportInitialize)(this.pBSgMaster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbButtonPesquisar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pBSgMaster;
        private System.Windows.Forms.Label lblAviso;
        public CenteredListBox listBoxArquivos;
        private System.Windows.Forms.Button BtnDownload;
        private System.Windows.Forms.TextBox TbPesquisar;
        private System.Windows.Forms.PictureBox pbButtonPesquisar;
    }
}

