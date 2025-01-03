﻿namespace UtilitariosSup
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fUtilitarios));
            this.listBoxDownload = new UtilitariosSup.fUtilitarios.CenteredListBox();
            this.lblAviso = new System.Windows.Forms.Label();
            this.TbPesquisar = new System.Windows.Forms.TextBox();
            this.tcListaArquivos = new System.Windows.Forms.TabControl();
            this.tbpDowwnload = new System.Windows.Forms.TabPage();
            this.tbpUpload = new System.Windows.Forms.TabPage();
            this.listBoxUpload = new UtilitariosSup.fUtilitarios.CenteredListBox();
            this.PBLoading = new System.Windows.Forms.ProgressBar();
            this.lblPercentual = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pbAvisoDeleteArquivos = new System.Windows.Forms.PictureBox();
            this.btnExcluir = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.pbButtonPesquisar = new System.Windows.Forms.PictureBox();
            this.pBSgMaster = new System.Windows.Forms.PictureBox();
            this.tcListaArquivos.SuspendLayout();
            this.tbpDowwnload.SuspendLayout();
            this.tbpUpload.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAvisoDeleteArquivos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbButtonPesquisar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBSgMaster)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxDownload
            // 
            this.listBoxDownload.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxDownload.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxDownload.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxDownload.FormattingEnabled = true;
            this.listBoxDownload.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.listBoxDownload.IntegralHeight = false;
            this.listBoxDownload.ItemHeight = 16;
            this.listBoxDownload.Location = new System.Drawing.Point(-4, 0);
            this.listBoxDownload.Name = "listBoxDownload";
            this.listBoxDownload.Size = new System.Drawing.Size(330, 252);
            this.listBoxDownload.TabIndex = 0;
            this.listBoxDownload.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listBoxArquivos_MouseClick);
            this.listBoxDownload.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxArquivos_KeyDown);
            this.listBoxDownload.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxArquivos_MouseDoubleClick);
            // 
            // lblAviso
            // 
            this.lblAviso.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblAviso.AutoSize = true;
            this.lblAviso.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAviso.ForeColor = System.Drawing.Color.Red;
            this.lblAviso.Location = new System.Drawing.Point(3, 406);
            this.lblAviso.Name = "lblAviso";
            this.lblAviso.Size = new System.Drawing.Size(352, 15);
            this.lblAviso.TabIndex = 3;
            this.lblAviso.Text = "*Ou duplo click / enter no nome para iniciar download";
            this.lblAviso.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lblAviso_MouseClick);
            // 
            // TbPesquisar
            // 
            this.TbPesquisar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.TbPesquisar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TbPesquisar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TbPesquisar.ForeColor = System.Drawing.Color.DarkGray;
            this.TbPesquisar.Location = new System.Drawing.Point(158, 366);
            this.TbPesquisar.Multiline = true;
            this.TbPesquisar.Name = "TbPesquisar";
            this.TbPesquisar.Size = new System.Drawing.Size(156, 30);
            this.TbPesquisar.TabIndex = 5;
            this.TbPesquisar.Text = "BUSCAR (F2)";
            this.TbPesquisar.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TbPesquisar.TextChanged += new System.EventHandler(this.TbPesquisar_TextChanged);
            this.TbPesquisar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TbPesquisar_KeyDown);
            this.TbPesquisar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TbPesquisar_MouseDown);
            // 
            // tcListaArquivos
            // 
            this.tcListaArquivos.Controls.Add(this.tbpDowwnload);
            this.tcListaArquivos.Controls.Add(this.tbpUpload);
            this.tcListaArquivos.Location = new System.Drawing.Point(13, 77);
            this.tcListaArquivos.Name = "tcListaArquivos";
            this.tcListaArquivos.Padding = new System.Drawing.Point(62, 3);
            this.tcListaArquivos.SelectedIndex = 0;
            this.tcListaArquivos.Size = new System.Drawing.Size(334, 278);
            this.tcListaArquivos.TabIndex = 8;
            this.tcListaArquivos.SelectedIndexChanged += new System.EventHandler(this.tcListaArquivos_SelectedIndexChanged);
            // 
            // tbpDowwnload
            // 
            this.tbpDowwnload.Controls.Add(this.listBoxDownload);
            this.tbpDowwnload.Location = new System.Drawing.Point(4, 22);
            this.tbpDowwnload.Name = "tbpDowwnload";
            this.tbpDowwnload.Padding = new System.Windows.Forms.Padding(3);
            this.tbpDowwnload.Size = new System.Drawing.Size(326, 252);
            this.tbpDowwnload.TabIndex = 0;
            this.tbpDowwnload.Text = "Download";
            this.tbpDowwnload.UseVisualStyleBackColor = true;
            // 
            // tbpUpload
            // 
            this.tbpUpload.Controls.Add(this.listBoxUpload);
            this.tbpUpload.Location = new System.Drawing.Point(4, 22);
            this.tbpUpload.Name = "tbpUpload";
            this.tbpUpload.Padding = new System.Windows.Forms.Padding(3);
            this.tbpUpload.Size = new System.Drawing.Size(326, 252);
            this.tbpUpload.TabIndex = 1;
            this.tbpUpload.Text = "Upload";
            this.tbpUpload.UseVisualStyleBackColor = true;
            // 
            // listBoxUpload
            // 
            this.listBoxUpload.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxUpload.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxUpload.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxUpload.FormattingEnabled = true;
            this.listBoxUpload.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.listBoxUpload.IntegralHeight = false;
            this.listBoxUpload.ItemHeight = 16;
            this.listBoxUpload.Location = new System.Drawing.Point(-4, 0);
            this.listBoxUpload.Name = "listBoxUpload";
            this.listBoxUpload.Size = new System.Drawing.Size(330, 256);
            this.listBoxUpload.Sorted = true;
            this.listBoxUpload.TabIndex = 1;
            this.listBoxUpload.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listBoxUpload_MouseClick);
            this.listBoxUpload.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxUpload_KeyDown);
            this.listBoxUpload.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxUpload_MouseDoubleClick);
            // 
            // PBLoading
            // 
            this.PBLoading.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.PBLoading.Location = new System.Drawing.Point(13, 425);
            this.PBLoading.Name = "PBLoading";
            this.PBLoading.Size = new System.Drawing.Size(305, 23);
            this.PBLoading.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.PBLoading.TabIndex = 10;
            // 
            // lblPercentual
            // 
            this.lblPercentual.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblPercentual.AutoSize = true;
            this.lblPercentual.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPercentual.Location = new System.Drawing.Point(318, 429);
            this.lblPercentual.Name = "lblPercentual";
            this.lblPercentual.Size = new System.Drawing.Size(27, 15);
            this.lblPercentual.TabIndex = 11;
            this.lblPercentual.Text = "0%";
            // 
            // pbAvisoDeleteArquivos
            // 
            this.pbAvisoDeleteArquivos.Image = global::UtilitariosSup.Properties.Resources.exclamacaoAzul__1_;
            this.pbAvisoDeleteArquivos.Location = new System.Drawing.Point(320, 6);
            this.pbAvisoDeleteArquivos.Name = "pbAvisoDeleteArquivos";
            this.pbAvisoDeleteArquivos.Size = new System.Drawing.Size(27, 26);
            this.pbAvisoDeleteArquivos.TabIndex = 12;
            this.pbAvisoDeleteArquivos.TabStop = false;
            this.toolTip1.SetToolTip(this.pbAvisoDeleteArquivos, resources.GetString("pbAvisoDeleteArquivos.ToolTip"));
            // 
            // btnExcluir
            // 
            this.btnExcluir.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnExcluir.BackColor = System.Drawing.Color.Blue;
            this.btnExcluir.FlatAppearance.BorderSize = 0;
            this.btnExcluir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcluir.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExcluir.ForeColor = System.Drawing.Color.White;
            this.btnExcluir.Image = global::UtilitariosSup.Properties.Resources.menos;
            this.btnExcluir.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExcluir.Location = new System.Drawing.Point(105, 366);
            this.btnExcluir.Name = "btnExcluir";
            this.btnExcluir.Size = new System.Drawing.Size(39, 30);
            this.btnExcluir.TabIndex = 9;
            this.toolTip1.SetToolTip(this.btnExcluir, "Excluir - F6");
            this.btnExcluir.UseVisualStyleBackColor = false;
            this.btnExcluir.Click += new System.EventHandler(this.btnExcluir_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnUpload.BackColor = System.Drawing.Color.Blue;
            this.btnUpload.FlatAppearance.BorderSize = 0;
            this.btnUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpload.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpload.ForeColor = System.Drawing.Color.White;
            this.btnUpload.Image = global::UtilitariosSup.Properties.Resources.setacima;
            this.btnUpload.Location = new System.Drawing.Point(59, 366);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(39, 30);
            this.btnUpload.TabIndex = 7;
            this.toolTip1.SetToolTip(this.btnUpload, "Upload - F9");
            this.btnUpload.UseVisualStyleBackColor = false;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnDownload.BackColor = System.Drawing.Color.Blue;
            this.btnDownload.FlatAppearance.BorderSize = 0;
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownload.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownload.ForeColor = System.Drawing.Color.White;
            this.btnDownload.Image = global::UtilitariosSup.Properties.Resources.setaabaixo;
            this.btnDownload.Location = new System.Drawing.Point(13, 366);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(39, 30);
            this.btnDownload.TabIndex = 4;
            this.toolTip1.SetToolTip(this.btnDownload, "Download - F8");
            this.btnDownload.UseVisualStyleBackColor = false;
            this.btnDownload.Click += new System.EventHandler(this.BtnDownload_Click);
            // 
            // pbButtonPesquisar
            // 
            this.pbButtonPesquisar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pbButtonPesquisar.Image = global::UtilitariosSup.Properties.Resources.lupa_pequena;
            this.pbButtonPesquisar.Location = new System.Drawing.Point(320, 366);
            this.pbButtonPesquisar.Name = "pbButtonPesquisar";
            this.pbButtonPesquisar.Size = new System.Drawing.Size(27, 28);
            this.pbButtonPesquisar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbButtonPesquisar.TabIndex = 6;
            this.pbButtonPesquisar.TabStop = false;
            this.pbButtonPesquisar.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbButtonPesquisar_MouseClick);
            // 
            // pBSgMaster
            // 
            this.pBSgMaster.Image = global::UtilitariosSup.Properties.Resources.logo_master_220x48px;
            this.pBSgMaster.Location = new System.Drawing.Point(13, 6);
            this.pBSgMaster.Name = "pBSgMaster";
            this.pBSgMaster.Size = new System.Drawing.Size(334, 65);
            this.pBSgMaster.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pBSgMaster.TabIndex = 2;
            this.pBSgMaster.TabStop = false;
            this.pBSgMaster.Click += new System.EventHandler(this.pBSgMaster_Click);
            // 
            // fUtilitarios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 456);
            this.Controls.Add(this.pbAvisoDeleteArquivos);
            this.Controls.Add(this.lblPercentual);
            this.Controls.Add(this.PBLoading);
            this.Controls.Add(this.btnExcluir);
            this.Controls.Add(this.tcListaArquivos);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.pbButtonPesquisar);
            this.Controls.Add(this.TbPesquisar);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.lblAviso);
            this.Controls.Add(this.pBSgMaster);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fUtilitarios";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SGBr - Utilitário para download de arquivos";
            this.Load += new System.EventHandler(this.fUtilitarios_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FUtilitarios_KeyDown);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.fUtilitarios_MouseClick);
            this.tcListaArquivos.ResumeLayout(false);
            this.tbpDowwnload.ResumeLayout(false);
            this.tbpUpload.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbAvisoDeleteArquivos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbButtonPesquisar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBSgMaster)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pBSgMaster;
        private System.Windows.Forms.Label lblAviso;
        public CenteredListBox listBoxDownload;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.TextBox TbPesquisar;
        private System.Windows.Forms.PictureBox pbButtonPesquisar;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.TabControl tcListaArquivos;
        private System.Windows.Forms.TabPage tbpDowwnload;
        private System.Windows.Forms.TabPage tbpUpload;
        public CenteredListBox listBoxUpload;
        private System.Windows.Forms.Button btnExcluir;
        private System.Windows.Forms.ProgressBar PBLoading;
        private System.Windows.Forms.Label lblPercentual;
        private System.Windows.Forms.PictureBox pbAvisoDeleteArquivos;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

