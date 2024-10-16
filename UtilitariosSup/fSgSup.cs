﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace UtilitariosSup
{
    public partial class fUtilitarios : Form
    {
        [DllImport("DwmApi")]
        private static extern int DwmSetWindowAttribute(IntPtr hwn, int attr, int[] attrValue, int attriSize);

        protected override void OnHandleCreated(EventArgs e)
        {
            if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
            {
                DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
            }
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeft, int nTop, int nRight, int nBottom, int nWidhtEllipse, int nHeightEllipse
        );

        private void fUtilitarios_Load(object sender, EventArgs e)
        {
            BtnDownload.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, BtnDownload.Width, BtnDownload.Height, 7, 7));

            TbPesquisar.TextChanged += TbPesquisar_TextChanged;
            TbPesquisar.LostFocus += TbPesquisar_LostFocus;
            TbPesquisar.KeyDown += TbPesquisar_KeyDown;
            this.KeyDown += FUtilitarios_KeyDown;
            pbButtonPesquisar.MouseClick += pbButtonPesquisar_MouseClick;
        }
        public fUtilitarios()
        {
            InitializeComponent();
            LoadDownloadItemsAsync();

            this.MouseClick += fUtilitarios_MouseClick;
            this.pBSgMaster.Click += pBSgMaster_Click;
        }

        private List<DownloadItem> _downloadItems = new List<DownloadItem>();


        private async void LoadDownloadItemsAsync()
        {
            BtnDownload.Enabled = false;
            TbPesquisar.Enabled = false;
            pbButtonPesquisar.Enabled = false;
            listBoxArquivos.Enabled = false;
            listBoxArquivos.Items.Clear();
            listBoxArquivos.Items.Add("CARREGANDO...");

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, "https://sgbr.com.br/acessorestrito/api/utilitario/downloads/listar");
                    request.Headers.Add("token", "5L/8K//}#$@dwKgf/)XqqQB6ZnrhFHX4qm[Y*=wX&%sxSvU5S;nsBm&U=K=7");

                    HttpResponseMessage response = await client.SendAsync(request);
                    if (!response.IsSuccessStatusCode)
                    {
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"ERRO AO CARREGAR LISTA DE DOWNLOADS: {response.StatusCode} - {errorResponse}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var downloadResponse = JsonConvert.DeserializeObject<DownloadResponse>(jsonResponse);
                    _downloadItems = downloadResponse.Data;

                    BtnDownload.Enabled = true;
                    TbPesquisar.Enabled = true;
                    pbButtonPesquisar.Enabled = true;
                    listBoxArquivos.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERRO AO CARREGAR LISTA DE DOWNLOADS: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                Invoke(new Action(() =>
                {
                    listBoxArquivos.Items.Clear();
                    if (_downloadItems.Count > 0)
                    {
                        foreach (var item in _downloadItems)
                        {
                            if (!string.IsNullOrEmpty(item.Nome))
                            {
                                listBoxArquivos.Items.Add(item.Nome);
                            }
                        }
                    }
                    else
                    {
                        listBoxArquivos.Items.Add("NENHUM ITEM ENCONTRADO.");
                    }
                }));
            }
        }


        private void FUtilitarios_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                TbPesquisar.Clear();
                listBoxArquivos.ClearSelected();
                TbPesquisar.TextAlign = HorizontalAlignment.Left;
                TbPesquisar.Focus();
                TbPesquisar.ForeColor = Color.Black;
            }
        }

        private void listBoxArquivos_MouseClick(object sender, MouseEventArgs e)
        {
            TbPesquisar.Font = new Font(lblAviso.Font.FontFamily, 12, FontStyle.Bold);
            TbPesquisar.ForeColor = Color.DarkGray;
            TbPesquisar.TextAlign = HorizontalAlignment.Center;
            TbPesquisar.Text = "BUSCAR (F2)";
        }

        private void TbPesquisar_MouseDown(object sender, MouseEventArgs e)
        {
            TbPesquisar.Clear();
            listBoxArquivos.ClearSelected();
            TbPesquisar.Font = new Font(TbPesquisar.Font.FontFamily, 12, FontStyle.Bold);
            TbPesquisar.ForeColor = Color.Black;
            TbPesquisar.TextAlign = HorizontalAlignment.Left;
        }


        private void TbPesquisar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                Pesquisar();

                if (listBoxArquivos.SelectedIndex != -1)
                {
                    lblAviso.Font = new Font(lblAviso.Font.FontFamily, 6, FontStyle.Bold);
                    lblAviso.Text = "*OU DUPLO CLICK / ENTER NO NOME PARA INICIAR DOWNLOAD";
                    TbPesquisar.Font = new Font(TbPesquisar.Font.FontFamily, 12, FontStyle.Bold);
                    TbPesquisar.ForeColor = Color.DarkGray;
                    TbPesquisar.TextAlign = HorizontalAlignment.Center;
                    TbPesquisar.Text = "BUSCAR (F2)";
                }
                else
                {
                    lblAviso.Font = new Font(lblAviso.Font.FontFamily, 7, FontStyle.Bold);
                    lblAviso.Text = "ARQUIVO NÃO ENCONTRADO! TENTE NOVAMENTE!";
                    TbPesquisar.Clear();
                    TbPesquisar.ForeColor = Color.Black;
                    TbPesquisar.TextAlign = HorizontalAlignment.Left;
                    TbPesquisar.Font = new Font(lblAviso.Font.FontFamily, 12, FontStyle.Bold);
                    TbPesquisar.Focus();
                }

                
                if (listBoxArquivos.SelectedIndex != -1)
                {
                    var selectedItem = _downloadItems[listBoxArquivos.SelectedIndex];
                    TbPesquisar.Tag = selectedItem;
                }
                else
                {
                    TbPesquisar.Tag = null;
                }
            }
        }

        private void pbButtonPesquisar_MouseClick(object sender, MouseEventArgs e)
        {
            Pesquisar();

            if (listBoxArquivos.SelectedIndex != -1)
            {
                lblAviso.Font = new Font(lblAviso.Font.FontFamily, 6, FontStyle.Bold);
                lblAviso.Text = "*OU DUPLO CLICK / ENTER NO NOME PARA INICIAR DOWNLOAD";
                TbPesquisar.Font = new Font(TbPesquisar.Font.FontFamily, 12, FontStyle.Bold);
                TbPesquisar.ForeColor = Color.DarkGray;
                TbPesquisar.TextAlign = HorizontalAlignment.Center;
                TbPesquisar.Text = "BUSCAR (F2)";
            }
            else
            {
                lblAviso.Font = new Font(lblAviso.Font.FontFamily, 7, FontStyle.Bold);
                lblAviso.Text = "ARQUIVO NÃO ENCONTRADO! TENTE NOVAMENTE!";
                TbPesquisar.Clear();
                TbPesquisar.ForeColor = Color.Black;
                TbPesquisar.TextAlign = HorizontalAlignment.Left;
                TbPesquisar.Font = new Font(lblAviso.Font.FontFamily, 12, FontStyle.Bold);
                TbPesquisar.Focus();
            }
        }


        private void Pesquisar()
        {
            string textoPesquisa = TbPesquisar.Text.Trim().ToLower();

            int indiceSelecionadoAnterior = listBoxArquivos.SelectedIndex;

            listBoxArquivos.ClearSelected();

            if (string.IsNullOrEmpty(textoPesquisa))
            {
                TbPesquisar.Tag = null;
            }

            if (!string.IsNullOrEmpty(textoPesquisa))
            {
                for (int i = 0; i < listBoxArquivos.Items.Count; i++)
                {
                    string item = listBoxArquivos.Items[i].ToString().ToLower();
                    if (item.Contains(textoPesquisa))
                    {
                        listBoxArquivos.SelectedIndex = i;
                        listBoxArquivos.TopIndex = i;
                        listBoxArquivos.Focus();

                        var selectedItem = _downloadItems[listBoxArquivos.SelectedIndex];
                        TbPesquisar.Tag = selectedItem;


                        return;
                    }
                }
            }

            listBoxArquivos.SelectedIndex = indiceSelecionadoAnterior;
        }




        private void TbPesquisar_TextChanged(object sender, EventArgs e)
        {
            string filtro = TbPesquisar.Text.ToUpper();

            int index = listBoxArquivos.FindString(filtro);
        }

        private void TbPesquisar_LostFocus(object sender, EventArgs e)
        {
            string filtro = TbPesquisar.Text.ToLower();

            int index = listBoxArquivos.FindStringExact(filtro);

            if (index != ListBox.NoMatches)
            {
                listBoxArquivos.SelectedIndex = index;
                listBoxArquivos.TopIndex = index;
            }

        }
        private void BtnDownload_Click(object sender, EventArgs e)
        {
            if (listBoxArquivos.SelectedIndex != -1 || TbPesquisar.Tag != null)
            {
                IniciarDownload();
            }
            else
            {
                if (listBoxArquivos.Items.Count > 0)
                {
                    MessageBox.Show("SELECIONE UM ARQUIVO ANTES DE INICIAR O DOWNLOAD", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblAviso.Font = new Font(lblAviso.Font.FontFamily, 6, FontStyle.Bold);
                    lblAviso.Text = "*OU DUPLO CLICK / ENTER NO NOME PARA INICIAR DOWNLOAD";
                    TbPesquisar.Font = new Font(lblAviso.Font.FontFamily, 12, FontStyle.Bold);
                    TbPesquisar.ForeColor = Color.DarkGray;
                    TbPesquisar.TextAlign = HorizontalAlignment.Center;
                    TbPesquisar.Text = "BUSCAR (F2)";
                    listBoxArquivos.SelectedIndex = 0;
                    listBoxArquivos.Focus();
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F8)
            {
                if (listBoxArquivos.SelectedIndex != -1 || TbPesquisar.Tag != null)
                {
                    IniciarDownload();
                }
                else
                {
                    if (listBoxArquivos.Items.Count > 0)
                    {
                        MessageBox.Show("SELECIONE UM ARQUIVO ANTES DE INICIAR O DOWNLOAD", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lblAviso.Font = new Font(lblAviso.Font.FontFamily, 6, FontStyle.Bold);
                        lblAviso.Text = "*OU DUPLO CLICK / ENTER NO NOME PARA INICIAR DOWNLOAD";
                        TbPesquisar.Font = new Font(lblAviso.Font.FontFamily, 12, FontStyle.Bold);
                        TbPesquisar.ForeColor = Color.DarkGray;
                        TbPesquisar.TextAlign = HorizontalAlignment.Center;
                        TbPesquisar.Text = "BUSCAR (F2)";
                        listBoxArquivos.SelectedIndex = 0;
                        listBoxArquivos.Focus();
                    }
                    else
                    {
                        MessageBox.Show("NENHUM ITEM DISPONÍVEL PARA DOWNLOAD", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void listBoxArquivos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                IniciarDownload();
            }
        }


        private void listBoxArquivos_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            IniciarDownload();
        }

        private async void DownloadFileAsync(string url, string filePath)
        {
            Floading floading = new Floading();
            try
            {
                Invoke(new Action(() =>
                {
                    floading.StartPosition = FormStartPosition.CenterScreen;
                    floading.Location = new Point(
                        this.Location.X + (this.Width - floading.Width) / 2,
                        this.Location.Y + (this.Height - floading.Height) / 2
                    );

                    floading.Show();

                    lblAviso.Font = new Font(lblAviso.Font.FontFamily, 7, FontStyle.Bold);
                    lblAviso.Text = "      AGUARDE, FINALIZANDO O DOWNLOAD ...";
                }));
                floading.Refresh();

                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();

                    var totalBytes = response.Content.Headers.ContentLength ?? -1L;
                    var canReportProgress = totalBytes != -1;

                    using (var contentStream = await response.Content.ReadAsStreamAsync())
                    using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None, 8192, true))
                    {
                        var buffer = new byte[8192];
                        long totalRead = 0L;
                        int read;

                        while ((read = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, read);
                            totalRead += read;

                            if (canReportProgress)
                            {
                                var progress = (int)((totalRead * 1d) / (totalBytes * 1d) * 100);
                                floading.Invoke(new Action(() => { floading.SetProgress(progress); }));
                            }
                        }
                    }

                    MessageBox.Show("DOWNLOAD CONCLUÍDO COM SUCESSO!", "SUCESSO!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERRO AO BAIXAR O ARQUIVO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                Invoke(new Action(() =>
                {
                    floading.Close();
                    lblAviso.Font = new Font(lblAviso.Font.FontFamily, 6, FontStyle.Bold);
                    lblAviso.Text = "*OU DUPLO CLICK / ENTER NO NOME PARA INICIAR DOWNLOAD";
                }));
            }
        }

        // By Zequi :)


        private void IniciarDownload()
        {
            DownloadItem selectedItem = null;
            int selectedIndex = -1;

            if (listBoxArquivos.SelectedIndex != -1)
            {
                selectedItem = _downloadItems[listBoxArquivos.SelectedIndex];
                selectedIndex = listBoxArquivos.SelectedIndex;
            }
            else if (TbPesquisar.Tag != null)
            {
                selectedItem = (DownloadItem)TbPesquisar.Tag;
                selectedIndex = _downloadItems.IndexOf(selectedItem);
                if (selectedIndex != -1)
                {
                    listBoxArquivos.SelectedIndex = selectedIndex;
                }
            }

            if (selectedItem != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = @"C:\SGBR\Master\Utilitarios";
                saveFileDialog.FileName = selectedItem.Nome;
                saveFileDialog.Filter = GetFileFilter(selectedItem.Url);

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DownloadFileAsync(selectedItem.Url, saveFileDialog.FileName);
                }
            }
            else
            {
                MessageBox.Show("SELECIONE UM ARQUIVO ANTES DE INICIAR O DOWNLOAD", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        private string GetFileFilter(string url)
        {
            string fileExtension = System.IO.Path.GetExtension(url).ToLower();
            string filterDescription = "Arquivo";

            switch (fileExtension)
            {
                case ".pdf":
                    filterDescription = "Documento PDF";
                    break;
                case ".doc":
                case ".docx":
                    filterDescription = "Documento Word";
                    break;
                case ".xls":
                case ".xlsx":
                    filterDescription = "Documento Excel";
                    break;
                case ".png":
                    filterDescription = "Imagem PNG";
                    break;
                case ".jpg":
                case ".jpeg":
                    filterDescription = "Imagem JPEG";
                    break;
                case ".txt":
                    filterDescription = "Documento de Texto";
                    break;
                case ".zip":
                    filterDescription = "Arquivo ZIP";
                    break;
                case ".rar":
                    filterDescription = "Arquivo RAR";
                    break;
                case ".dll":
                    filterDescription = "Arquivo DLL";
                    break;
                default:
                    filterDescription = "Arquivo";
                    break;
            }

            return $"{filterDescription} (*{fileExtension})|*{fileExtension}";
        }


        public class DownloadItem
        {
            public string Nome { get; set; }
            public string Url { get; set; }
        }

        private void fUtilitarios_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !listBoxArquivos.ClientRectangle.Contains(listBoxArquivos.PointToClient(e.Location)) && listBoxArquivos.Items.Count > 0)
            {
                if (listBoxArquivos.SelectedIndex == -1)
                {
                    listBoxArquivos.SelectedIndex = 0;
                }

                listBoxArquivos.Focus();
                TbPesquisar.Font = new Font(lblAviso.Font.FontFamily, 12, FontStyle.Bold);
                TbPesquisar.ForeColor = Color.DarkGray;
                TbPesquisar.TextAlign = HorizontalAlignment.Center;
                TbPesquisar.Text = "BUSCAR (F2)";
            }
        }

        private void pBSgMaster_Click(object sender, EventArgs e)
        {
            if (listBoxArquivos.Items.Count > 0)
            {
                if (listBoxArquivos.SelectedIndex == -1)
                {
                    listBoxArquivos.SelectedIndex = 0;
                }

                listBoxArquivos.Focus();
                TbPesquisar.Font = new Font(lblAviso.Font.FontFamily, 12, FontStyle.Bold);
                TbPesquisar.ForeColor = Color.DarkGray;
                TbPesquisar.TextAlign = HorizontalAlignment.Center;
                TbPesquisar.Text = "BUSCAR (F2)";
            }
        }


        private void lblAviso_MouseClick(object sender, MouseEventArgs e)
        {
            if (listBoxArquivos.Items.Count > 0)
            {
                if (listBoxArquivos.SelectedIndex == -1)
                {
                    listBoxArquivos.SelectedIndex = 0;
                }

                listBoxArquivos.Focus();
                TbPesquisar.Font = new Font(lblAviso.Font.FontFamily, 12, FontStyle.Bold);
                TbPesquisar.ForeColor = Color.DarkGray;
                TbPesquisar.TextAlign = HorizontalAlignment.Center;
                TbPesquisar.Text = "BUSCAR (F2)";
            }
        }

        public class DownloadResponse
        {
            public List<DownloadItem> Data { get; set; }
        }

        public class CenteredListBox : ListBox
        {
            public CenteredListBox()
            {
                this.DrawMode = DrawMode.OwnerDrawFixed;
            }

            protected override void OnDrawItem(DrawItemEventArgs e)
            {
                if (e.Index < 0 || e.Index >= this.Items.Count)
                    return;

                e.DrawBackground();

                bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                bool hasFocus = (e.State & DrawItemState.Focus) == DrawItemState.Focus;
                Color backgroundColor = (isSelected || hasFocus) ? SystemColors.Highlight : e.BackColor;
                Color foregroundColor = (isSelected || hasFocus) ? SystemColors.HighlightText : e.ForeColor;

                using (Brush backgroundBrush = new SolidBrush(backgroundColor))
                {
                    e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
                }

                string itemText = this.Items[e.Index].ToString();
                SizeF textSize = e.Graphics.MeasureString(itemText, e.Font);
                PointF textLocation = new PointF(
                    e.Bounds.X + (e.Bounds.Width - textSize.Width) / 2,
                    e.Bounds.Y + (e.Bounds.Height - textSize.Height) / 2
                );

                using (Brush textBrush = new SolidBrush(foregroundColor))
                {
                    e.Graphics.DrawString(itemText, e.Font, textBrush, textLocation);
                }

                e.DrawFocusRectangle();
            }

            protected override void OnGotFocus(EventArgs e)
            {
                base.OnGotFocus(e);
                Invalidate();
            }

            protected override void OnLostFocus(EventArgs e)
            {
                base.OnLostFocus(e);
                Invalidate();
            }

            protected override void OnSelectedIndexChanged(EventArgs e)
            {
                base.OnSelectedIndexChanged(e);
                Invalidate();
            }
        }

        
    }
}