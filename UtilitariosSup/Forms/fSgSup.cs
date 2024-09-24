using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.WebSockets;
using System.Windows.Forms;
using System.Diagnostics;
using FluentFTP;
using static System.Net.Mime.MediaTypeNames;


namespace UtilitariosSup
{
    public partial class fUtilitarios : Form
    {
        #region Estilização de barra superior, arrendodamento de botões e centralização de itens na ListBox

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
        #endregion

        private FtpClient ftpClient;

        private List<DownloadItem> _downloadItems = new List<DownloadItem>();

        public class DownloadResponse
        {
            public List<DownloadItem> Data { get; set; }
        }

        public class DownloadItem
        {
            public string Nome { get; set; }
            public string Url { get; set; }
        }

        public string dirPadraoFtp { get; set; } = "/dados/sgbr.com.br/interno/arquivos/";
        public string sitema = "SGBr Sistemas";

        public fUtilitarios()
        {
            InitializeComponent();
            LoadDownloadItemsAsync();

            this.MouseClick += fUtilitarios_MouseClick;
            this.pBSgMaster.Click += pBSgMaster_Click;

            ftpClient = new FtpClient("ftp://files.sgbr.com.br", "publico", "96#s!G@86");
            ftpClient.Connect();
        }
        private void fUtilitarios_Load(object sender, EventArgs e)
        {
            btnDownload.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnDownload.Width, btnDownload.Height, 7, 7));
            btnUpload.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnUpload.Width, btnUpload.Height, 7, 7));
            btnExcluir.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnExcluir.Width, btnExcluir.Height, 7, 7));



            TbPesquisar.TextChanged += TbPesquisar_TextChanged;
            TbPesquisar.LostFocus += TbPesquisar_LostFocus;
            TbPesquisar.KeyDown += TbPesquisar_KeyDown;
            this.KeyDown += FUtilitarios_KeyDown;
            pbButtonPesquisar.MouseClick += pbButtonPesquisar_MouseClick;
        }


        private void FUtilitarios_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                TbPesquisar.Clear();
                listBoxDownload.ClearSelected();
                TbPesquisar.TextAlign = HorizontalAlignment.Left;
                TbPesquisar.Focus();
                TbPesquisar.ForeColor = Color.Black;
            }

            if(e.KeyCode == Keys.F8)
            {
                if(tcListaArquivos.SelectedIndex == 0)
                {
                    if (listBoxDownload.SelectedIndex != -1 || TbPesquisar.Tag != null)
                    {
                        IniciarDownload();
                    }
                    else
                    {
                        if (listBoxDownload.Items.Count > 0)
                        {
                            MessageBox.Show("SELECIONE UM ARQUIVO ANTES DE INICIAR O DOWNLOAD", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            AjudantedeEstilo.ReformulaLblAviso(lblAviso, "*OU DUPLO CLICK / ENTER NO NOME PARA INICIAR DOWNLOAD", 6);
                            AjudantedeEstilo.ReformulaTxtBox(TbPesquisar, 12, HorizontalAlignment.Center, "BUSCAR (F2)", FontStyle.Bold, Color.DarkGray);
                            listBoxDownload.SelectedIndex = 0;
                            listBoxDownload.Focus();
                        }
                        else
                        {
                            MessageBox.Show("NENHUM ARQUIVO DISPONÍVEL PARA DOWNLOAD", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    if(listBoxUpload.SelectedIndex != -1)
                        IniciarDownloadFtp();
                }
            }
        }

        private void fUtilitarios_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !listBoxDownload.ClientRectangle.Contains(listBoxDownload.PointToClient(e.Location)) && listBoxDownload.Items.Count > 0)
            {
                if (listBoxDownload.SelectedIndex == -1)
                {
                    listBoxDownload.SelectedIndex = 0;
                }

                listBoxDownload.Focus();
                TbPesquisar.Font = new Font(lblAviso.Font.FontFamily, 12, FontStyle.Bold);
                TbPesquisar.ForeColor = Color.DarkGray;
                TbPesquisar.TextAlign = HorizontalAlignment.Center;
                TbPesquisar.Text = "BUSCAR (F2)";
            }
        }


        #region Conexão com API e listagem na ListBox
        private async void LoadDownloadItemsAsync()
        {
            btnDownload.Enabled = false;
            //btnUpload.Enabled = false;
            TbPesquisar.Enabled = false;
            pbButtonPesquisar.Enabled = false;
            listBoxDownload.Enabled = false;
            listBoxDownload.Items.Clear();
            listBoxDownload.Items.Add("CARREGANDO...");

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

                    btnDownload.Enabled = true;
                    TbPesquisar.Enabled = true;
                    pbButtonPesquisar.Enabled = true;
                    listBoxDownload.Enabled = true;
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
                    listBoxDownload.Items.Clear();
                    if (_downloadItems.Count > 0)
                    {
                        foreach (var item in _downloadItems)
                        {
                            if (!string.IsNullOrEmpty(item.Nome))
                            {
                                listBoxDownload.Items.Add(item.Nome);
                            }
                        }
                    }
                    else
                    {
                        listBoxDownload.Items.Add("NENHUM ITEM ENCONTRADO.");
                    }
                }));
            }
        }

        #endregion


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
            listBoxDownload.ClearSelected();
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

                if (listBoxDownload.SelectedIndex != -1)
                {
                    AjudantedeEstilo.ReformulaLblAviso(lblAviso, "*OU DUPLO CLICK / ENTER NO NOME PARA INICIAR DOWNLOAD", 6);
                    AjudantedeEstilo.ReformulaTxtBox(TbPesquisar, 12, HorizontalAlignment.Center, "BUSCAR (F2)",FontStyle.Bold,Color.DarkGray);
                }
                else
                {
                    AjudantedeEstilo.ReformulaLblAviso(lblAviso, "ARQUIVO NÃO ENCONTRADO! TENTE NOVAMENTE!", 7);
                    TbPesquisar.Clear();
                    AjudantedeEstilo.ReformulaTxtBox(TbPesquisar, 12, HorizontalAlignment.Left, null, FontStyle.Bold, Color.Black);
                    TbPesquisar.Focus();
                }


                if (listBoxDownload.SelectedIndex != -1)
                {
                    var selectedItem = _downloadItems[listBoxDownload.SelectedIndex];
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

            if (listBoxDownload.SelectedIndex != -1)
            {
                AjudantedeEstilo.ReformulaLblAviso(lblAviso, "*OU DUPLO CLICK / ENTER NO NOME PARA INICIAR DOWNLOAD", 6);
                AjudantedeEstilo.ReformulaTxtBox(TbPesquisar, 12, HorizontalAlignment.Center, "BUSCAR (F2)", FontStyle.Bold, Color.DarkGray);
            }
            else
            {
                AjudantedeEstilo.ReformulaLblAviso(lblAviso, "ARQUIVO NÃO ENCONTRADO! TENTE NOVAMENTE!", 7);
                TbPesquisar.Clear();
                AjudantedeEstilo.ReformulaTxtBox(TbPesquisar, 12, HorizontalAlignment.Left, null, FontStyle.Bold, Color.Black);
                TbPesquisar.Focus();
            }
        }

        private void TbPesquisar_TextChanged(object sender, EventArgs e)
        {
            string filtro = TbPesquisar.Text.ToUpper();

            int index = listBoxDownload.FindString(filtro);
        }

        private void TbPesquisar_LostFocus(object sender, EventArgs e)
        {
            string filtro = TbPesquisar.Text.ToLower();

            int index = listBoxDownload.FindStringExact(filtro);

            if (index != ListBox.NoMatches)
            {
                listBoxDownload.SelectedIndex = index;
                listBoxDownload.TopIndex = index;
            }

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

        private void pBSgMaster_Click(object sender, EventArgs e)
        {
            if (listBoxDownload.Items.Count > 0)
            {
                if (listBoxDownload.SelectedIndex == -1)
                {
                    listBoxDownload.SelectedIndex = 0;
                }

                listBoxDownload.Focus();
                AjudantedeEstilo.ReformulaTxtBox(TbPesquisar, 12, HorizontalAlignment.Center, "BUSCAR (F2)", FontStyle.Bold, Color.DarkGray);
            }
        }


        private void lblAviso_MouseClick(object sender, MouseEventArgs e)
        {
            if (listBoxDownload.Items.Count > 0)
            {
                if (listBoxDownload.SelectedIndex == -1)
                {
                    listBoxDownload.SelectedIndex = 0;
                }

                listBoxDownload.Focus();
                AjudantedeEstilo.ReformulaTxtBox(TbPesquisar, 12, HorizontalAlignment.Center, "BUSCAR (F2)", FontStyle.Bold, Color.DarkGray);
            }
        }

        private void Pesquisar()
        {
            string textoPesquisa = TbPesquisar.Text.Trim().ToLower();

            int indiceSelecionadoAnterior = listBoxDownload.SelectedIndex;

            listBoxDownload.ClearSelected();

            if (string.IsNullOrEmpty(textoPesquisa))
            {
                TbPesquisar.Tag = null;
            }

            if (!string.IsNullOrEmpty(textoPesquisa))
            {
                for (int i = 0; i < listBoxDownload.Items.Count; i++)
                {
                    string item = listBoxDownload.Items[i].ToString().ToLower();
                    if (item.Contains(textoPesquisa))
                    {
                        listBoxDownload.SelectedIndex = i;
                        listBoxDownload.TopIndex = i;
                        listBoxDownload.Focus();

                        var selectedItem = _downloadItems[listBoxDownload.SelectedIndex];
                        TbPesquisar.Tag = selectedItem;


                        return;
                    }
                }
            }

            listBoxDownload.SelectedIndex = indiceSelecionadoAnterior;
        }

        private void BtnDownload_Click(object sender, EventArgs e)
        {
            {
                if (listBoxDownload.SelectedIndex != -1 || TbPesquisar.Tag != null)
                {
                    if (tcListaArquivos.SelectedIndex == 0)
                        IniciarDownload();
                    else
                        IniciarDownloadFtp();
                }
                else
                {
                    if (listBoxDownload.Items.Count > 0)
                    {
                        MessageBox.Show("SELECIONE UM ARQUIVO ANTES DE INICIAR O DOWNLOAD", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        AjudantedeEstilo.ReformulaLblAviso(lblAviso, "*OU DUPLO CLICK / ENTER NO NOME PARA INICIAR DOWNLOAD", 6);
                        AjudantedeEstilo.ReformulaTxtBox(TbPesquisar, 12, HorizontalAlignment.Center, "BUSCAR (F2)", FontStyle.Bold, Color.DarkGray);
                        listBoxDownload.SelectedIndex = 0;
                        listBoxDownload.Focus();
                    }
                }
            }
        }

        #region Download itens API
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

                   AjudantedeEstilo.ReformulaLblAviso(lblAviso, "      AGUARDE, FINALIZANDO O DOWNLOAD ...", 7);

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

                    MessageBox.Show("DOWNLOAD CONCLUÍDO COM SUCESSO!", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERRO AO BAIXAR O ARQUIVO: {ex.Message}", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                Invoke(new Action(() =>
                {
                    floading.Close();
                    AjudantedeEstilo.ReformulaLblAviso(lblAviso, "*OU DUPLO CLICK / ENTER NO NOME PARA INICIAR DOWNLOAD", 6);
                }));
            }
        }


        private void IniciarDownload()
        {
            DownloadItem selectedItem = null;
            int selectedIndex = -1;

            if (listBoxDownload.SelectedIndex != -1)
            {
                selectedItem = _downloadItems[listBoxDownload.SelectedIndex];
                selectedIndex = listBoxDownload.SelectedIndex;
            }
            else if (TbPesquisar.Tag != null)
            {
                selectedItem = (DownloadItem)TbPesquisar.Tag;
                selectedIndex = _downloadItems.IndexOf(selectedItem);
                if (selectedIndex != -1)
                {
                    listBoxDownload.SelectedIndex = selectedIndex;
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
                MessageBox.Show("SELECIONE UM ARQUIVO ANTES DE INICIAR O DOWNLOAD", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region FTP

        private void btnUpload_Click(object sender, EventArgs e)
        {
            IniciarUpload();
            carregarListaFTP(dirPadraoFtp);
        }

        private void tcListaArquivos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tcListaArquivos.SelectedIndex == 1)
            {
                //using (FLogin login = new FLogin())
                //{
                //    login.StartPosition = FormStartPosition.Manual;
                //    login.Location = new Point(
                //        this.Location.X + (this.Width - login.Width) / 2,
                //        this.Location.Y + (this.Height - login.Height) / 2
                //    );

                //    login.ShowDialog();
                //}

                carregarListaFTP(dirPadraoFtp);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            RemoverArquivoFtp();
        }

        private void listBoxUpload_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            IniciarDownloadFtp();
        }
        private void carregarListaFTP(string directoryPath)
        {
            try
            {
                var items = ftpClient.GetListing(directoryPath, FtpListOption.AllFiles);

                listBoxUpload.Items.Clear();

                foreach (var item in items)
                {
                    listBoxUpload.Items.Add(item.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao listar arquivos do FTP : {ex.Message}");
            }
        }

        private void IniciarUpload()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Selecione um arquivo para enviar",
                Filter = "Todos os arquivos (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                string fileName = Path.GetFileName(filePath);
                string remotePath = dirPadraoFtp + fileName;

                using (var ftp = new FtpClient("ftp://files.sgbr.com.br", "publico", "96#s!G@86"))
                {
                    ftp.Connect();

                    ftp.UploadFile(filePath, remotePath);
                }
            }
        }
        private void IniciarDownloadFtp()
        {
            if (listBoxUpload.SelectedIndex != -1)
            {
                string arquivoSelecionadoFtp = dirPadraoFtp + listBoxUpload.SelectedItem.ToString();

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = @"C:\SGBR\Master\Utilitarios";
                saveFileDialog.FileName = System.IO.Path.GetFileName(arquivoSelecionadoFtp);
                saveFileDialog.Filter = GetFileFilter(arquivoSelecionadoFtp);

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string localPath = saveFileDialog.FileName;

                    try
                    {
                        ftpClient.DownloadFile(localPath, arquivoSelecionadoFtp);
                        MessageBox.Show("Download conclúido com sucesso!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao baixar o arquivo: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecione um arquivo para baixar!", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RemoverArquivoFtp()
        {
            if (listBoxUpload.SelectedIndex != -1)
            {
                string arquivoSelecionadoFtp = dirPadraoFtp + listBoxUpload.SelectedItem.ToString();

                try
                {
                    if (ftpClient.FileExists(arquivoSelecionadoFtp))
                    {
                        ftpClient.DeleteFile(arquivoSelecionadoFtp);
                        MessageBox.Show("Arquivo deletado com sucesso!");
                        carregarListaFTP(dirPadraoFtp);
                    }
                    else
                    {
                        MessageBox.Show("Arquivo não encontrado no FTP.", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao deletar o arquivo: {ex.Message}", sitema, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Selecione um arquivo para remover!", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion
        private string GetFileFilter(string arquivoSelecionado)
        {
            string fileExtension = System.IO.Path.GetExtension(arquivoSelecionado).ToLower();
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


        // By Zequi :)

    }
}
