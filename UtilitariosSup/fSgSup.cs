using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

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

        private List<DownloadItem> _downloadItems = new List<DownloadItem>();

        public fUtilitarios()
        {
            InitializeComponent();
            LoadDownloadItemsAsync();

            this.MouseClick += fUtilitarios_MouseClick;
            this.pBSgMaster.Click +=  pBSgMaster_Click;
        }

        private async void LoadDownloadItemsAsync()
        {
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

        private void listBoxArquivos_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBoxArquivos.SelectedIndex != -1)
            {
                var selectedItem = _downloadItems[listBoxArquivos.SelectedIndex];
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = @"C:\SGBR\Master\Utilitarios";
                saveFileDialog.FileName = selectedItem.Nome;
                saveFileDialog.Filter = GetFileFilter(selectedItem.Url);

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DownloadFileAsync(selectedItem.Url, saveFileDialog.FileName);
                }
            }
        }

        private async void DownloadFileAsync(string url, string filePath)
        {
            Floading floading = new Floading();
            try
            {
                Invoke(new Action(() =>
                {
                    floading.StartPosition = FormStartPosition.Manual;

                    floading.Location = new Point(
                    this.Location.X + (this.Width - floading.Width) / 2,
                    this.Location.Y + (this.Height - floading.Height) / 2
                    );

                    floading.Show();
                    lblAviso.Text = "    AGUARDE, FINALIZANDO O DOWNLOAD...";
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
                    lblAviso.Text = "*DUPLO CLICK OU F8 PARA INICIAR DOWNLOAD";
                }));
            }
        }

        // By Zequi :)

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F8)
            {
                if (listBoxArquivos.SelectedIndex != -1)
                {
                    var selectedItem = _downloadItems[listBoxArquivos.SelectedIndex];
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.InitialDirectory = @"C:\SGBR\Master\Utilitarios";
                    saveFileDialog.FileName = selectedItem.Nome;
                    saveFileDialog.Filter = GetFileFilter(selectedItem.Url);

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        DownloadFileAsync(selectedItem.Url, saveFileDialog.FileName);
                    }
                }

                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }


        private string GetFileFilter(string url)
        {
            string fileExtension = System.IO.Path.GetExtension(url);
            if (!string.IsNullOrEmpty(fileExtension))
            {
                return $"Files ({fileExtension})|{fileExtension}";
            }
            else
            {
                return "All files (.)|.";
            }
        }

        public class DownloadItem
        {
            public string Nome { get; set; }
            public string Url { get; set; }
        }
        private void fUtilitarios_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !listBoxArquivos.ClientRectangle.Contains(listBoxArquivos.PointToClient(e.Location)))
            { 
                listBoxArquivos.Focus();
            }
        }
        private void pBSgMaster_Click(object sender, EventArgs e)
        {
            listBoxArquivos.Focus();
        }

        public class DownloadResponse
        {
            public List<DownloadItem> Data { get; set; }
        }

        public class CenteredListBox : ListBox
        {

            protected override void OnDrawItem(DrawItemEventArgs e)
            {
                if (e.Index < 0 || e.Index >= this.Items.Count)
                    return;

                e.DrawBackground();
                e.DrawFocusRectangle();

                var itemText = this.Items[e.Index].ToString();
                var textSize = e.Graphics.MeasureString(itemText, e.Font);

                
                var localizacao = new PointF(e.Bounds.X + (e.Bounds.Width - textSize.Width) / 2,
                                          e.Bounds.Y + (e.Bounds.Height - textSize.Height) / 2);

                e.Graphics.DrawString(itemText, e.Font, new SolidBrush(e.ForeColor), localizacao);
            }
        }

    }
}