namespace UtilitariosSup
{
    partial class Floading
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PBLoading = new System.Windows.Forms.ProgressBar();
            this.lbloading = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PBLoading
            // 
            this.PBLoading.Location = new System.Drawing.Point(24, 59);
            this.PBLoading.Name = "PBLoading";
            this.PBLoading.Size = new System.Drawing.Size(189, 29);
            this.PBLoading.TabIndex = 0;
            // 
            // lbloading
            // 
            this.lbloading.AutoSize = true;
            this.lbloading.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbloading.Location = new System.Drawing.Point(72, 21);
            this.lbloading.Name = "lbloading";
            this.lbloading.Size = new System.Drawing.Size(98, 20);
            this.lbloading.TabIndex = 1;
            this.lbloading.Text = "Baixando...";
            // 
            // Floading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(238, 111);
            this.Controls.Add(this.lbloading);
            this.Controls.Add(this.PBLoading);
            this.Cursor = System.Windows.Forms.Cursors.No;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Floading";
            this.ShowInTaskbar = false;
            this.Text = "Floading";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar PBLoading;
        public System.Windows.Forms.Label lbloading;
    }
}