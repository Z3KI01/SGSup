using FluentFTP;
using FluentFTP.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilitariosSup.Forms;


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

        public class WorldTimeApiResponse
        {
            public string datetime { get; set; }
        }
        public string sitema { get; set; } = "SGBr Sistemas";
        public string dirPadraoFtp = "/dados/sgbr.com.br/interno/arquivos/";
        public bool logou = false;
        public CenteredListBox listBoxSelecionada;
        public bool upload = false;

        public List<string> itensUploadAnterior = new List<string>();
        public List<string> itensUploadAtual = new List<string>();

        private CancellationTokenSource cancellationTokenSource;

        public enum FtpOperation
        {
            Download,
            Upload
        }

        public fUtilitarios()
        {
            InitializeComponent();
            LoadDownloadItemsAsync();

            this.MouseClick += fUtilitarios_MouseClick;
            this.pBSgMaster.Click += pBSgMaster_Click;

            ftpClient = new FtpClient("ftp://files.sgbr.com.br", "publico", "96#s!G@86");
            ftpClient.Connect();
        }
        private async void fUtilitarios_Load(object sender, EventArgs e)
        {
            RedimensionarForm(false);
            btnDownload.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnDownload.Width, btnDownload.Height, 7, 7));
            btnUpload.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnUpload.Width, btnUpload.Height, 7, 7));
            btnExcluir.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnExcluir.Width, btnExcluir.Height, 7, 7));

            TbPesquisar.LostFocus += TbPesquisar_LostFocus;
            listBoxSelecionada = listBoxDownload;
            await ApagarArquivoFtpTimer();
        }


        private void FUtilitarios_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                TbPesquisar.Clear();
                listBoxSelecionada.ClearSelected();
                TbPesquisar.TextAlign = HorizontalAlignment.Left;
                TbPesquisar.Focus();
                TbPesquisar.ForeColor = Color.Black;
            }

            if (e.KeyCode == Keys.F6)
            {
                btnExcluir.PerformClick();
            }

            if (e.KeyCode == Keys.F8)
            {
                btnDownload.PerformClick();
            }

            if (e.KeyCode == Keys.F9)
            {
                btnUpload.PerformClick();
            }
        }

        private void fUtilitarios_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !listBoxSelecionada.ClientRectangle.Contains(listBoxSelecionada.PointToClient(e.Location)) && listBoxSelecionada.Items.Count > 0)
            {
                if (listBoxSelecionada.SelectedIndex == -1)
                {
                    listBoxSelecionada.SelectedIndex = 0;
                    listBoxSelecionada.Focus();
                }

                AjudantedeEstilo.ReformulaTxtBox(TbPesquisar, 13, HorizontalAlignment.Center, "BUSCAR (F2)", FontStyle.Bold, Color.DarkGray);
            }
        }


        #region Conexão com API e listagem na ListBox
        private async void LoadDownloadItemsAsync()
        {
            pbAvisoDeleteArquivos.Visible = false;
            btnDownload.Enabled = false;
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
                        MessageBox.Show($"Erro ao carregar lista de downloads: {response.StatusCode} - {errorResponse}", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Falha de conexão com a API: {ex.Message}. Verifique sua conexão e tente novamente.", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar lista de downloads: {ex.Message}", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            AjudantedeEstilo.ReformulaTxtBox(TbPesquisar, 13, HorizontalAlignment.Center, "BUSCAR (F2)", FontStyle.Bold, Color.DarkGray);
            AjudantedeEstilo.ReformulaLblAviso(lblAviso, "*Ou duplo click / enter no nome para iniciar download", 9);
        }

        private void TbPesquisar_MouseDown(object sender, MouseEventArgs e)
        {
            TbPesquisar.Clear();
            listBoxSelecionada.ClearSelected();
            AjudantedeEstilo.ReformulaTxtBox(TbPesquisar, 13, HorizontalAlignment.Left, null, FontStyle.Bold, Color.Black);
        }


        private void TbPesquisar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                Pesquisar();

                if (listBoxSelecionada.SelectedIndex != -1)
                {
                    AjudantedeEstilo.ReformulaLblAviso(lblAviso, "*Ou duplo click / enter no nome para iniciar download", 9);
                    AjudantedeEstilo.ReformulaTxtBox(TbPesquisar, 13, HorizontalAlignment.Center, "BUSCAR (F2)", FontStyle.Bold, Color.DarkGray);
                }
                else
                {
                    AjudantedeEstilo.ReformulaLblAviso(lblAviso, "       Arquivo não encontrado! tente novamente!", 9);
                    TbPesquisar.Clear();
                    AjudantedeEstilo.ReformulaTxtBox(TbPesquisar, 13, HorizontalAlignment.Left, null, FontStyle.Bold, Color.Black);
                    TbPesquisar.Focus();
                }

                if (listBoxSelecionada.SelectedIndex != -1)
                {
                    var selectedItem = _downloadItems[listBoxSelecionada.SelectedIndex];
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

            if (listBoxSelecionada.SelectedIndex != -1)
            {
                AjudantedeEstilo.ReformulaLblAviso(lblAviso, "*Ou duplo click / enter no nome para iniciar download", 9);
                AjudantedeEstilo.ReformulaTxtBox(TbPesquisar, 13, HorizontalAlignment.Center, "BUSCAR (F2)", FontStyle.Bold, Color.DarkGray);
            }
            else
            {
                AjudantedeEstilo.ReformulaLblAviso(lblAviso, "       Arquivo não encontrado! tente novamente!", 9);
                TbPesquisar.Clear();
                AjudantedeEstilo.ReformulaTxtBox(TbPesquisar, 13, HorizontalAlignment.Left, null, FontStyle.Bold, Color.Black);
                TbPesquisar.Focus();
            }
        }

        private void TbPesquisar_TextChanged(object sender, EventArgs e)
        {
            string filtro = TbPesquisar.Text.ToUpper();

            int index = listBoxSelecionada.FindString(filtro);
        }

        private void TbPesquisar_LostFocus(object sender, EventArgs e)
        {
            string filtro = TbPesquisar.Text.ToLower();
            int index = 0;

            index = listBoxSelecionada.FindStringExact(filtro);

            if (index != ListBox.NoMatches)
            {
                listBoxSelecionada.SelectedIndex = index;
                listBoxSelecionada.TopIndex = index;
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
            if (listBoxSelecionada.Items.Count > 0)
            {
                if (listBoxSelecionada.SelectedIndex == -1)
                {
                    listBoxSelecionada.SelectedIndex = 0;
                }

                listBoxSelecionada.Focus();
                AjudantedeEstilo.ReformulaTxtBox(TbPesquisar, 13, HorizontalAlignment.Center, "BUSCAR (F2)", FontStyle.Bold, Color.DarkGray);
            }
        }


        private void lblAviso_MouseClick(object sender, MouseEventArgs e)
        {
            if (listBoxSelecionada.Items.Count > 0)
            {
                if (listBoxSelecionada.SelectedIndex == -1)
                {
                    listBoxSelecionada.SelectedIndex = 0;
                }

                listBoxSelecionada.Focus();
                AjudantedeEstilo.ReformulaTxtBox(TbPesquisar, 13, HorizontalAlignment.Center, "BUSCAR (F2)", FontStyle.Bold, Color.DarkGray);
            }
        }

        private void Pesquisar()
        {
            string textoPesquisa = TbPesquisar.Text.Trim().ToLower();

            int indiceSelecionadoAnterior = listBoxSelecionada.SelectedIndex;

            listBoxSelecionada.ClearSelected();

            if (string.IsNullOrEmpty(textoPesquisa))
            {
                TbPesquisar.Tag = null;
            }

            if (!string.IsNullOrEmpty(textoPesquisa))
            {
                for (int i = 0; i < listBoxSelecionada.Items.Count; i++)
                {
                    string item = listBoxSelecionada.Items[i].ToString().ToLower();
                    if (item.Contains(textoPesquisa))
                    {
                        listBoxSelecionada.SelectedIndex = i;
                        listBoxSelecionada.TopIndex = i;
                        listBoxSelecionada.Focus();

                        if (tcListaArquivos.SelectedIndex == 0)
                        {
                            var selectedItem = _downloadItems[listBoxDownload.SelectedIndex];
                            TbPesquisar.Tag = selectedItem;
                        }

                        return;
                    }
                }
            }

            listBoxSelecionada.SelectedIndex = indiceSelecionadoAnterior;
        }

        private void BtnDownload_Click(object sender, EventArgs e)
        {
            upload = false;

            if (listBoxSelecionada.SelectedIndex != -1 || TbPesquisar.Tag != null)
            {
                if (tcListaArquivos.SelectedIndex == 0)
                    IniciarDownload();
                else
                    IniciarDownloadFtp();
            }
            else
            {
                if (listBoxSelecionada.Items.Count > 0)
                {
                    MessageBox.Show("Selecione um arquivo antes de iniciar o download", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AjudantedeEstilo.ReformulaLblAviso(lblAviso, "*Ou duplo click / enter no nome para iniciar download", 9);
                    AjudantedeEstilo.ReformulaTxtBox(TbPesquisar, 13, HorizontalAlignment.Center, "BUSCAR (F2)", FontStyle.Bold, Color.DarkGray);
                    listBoxSelecionada.SelectedIndex = 0;
                    listBoxSelecionada.Focus();
                }
            }
        }

        #region Download itens API

        public void SetProgress(int percentual)
        {
            if (percentual < 0) percentual = 0;
            if (percentual > 100) percentual = 100;

            PBLoading.Value = percentual;
            lblPercentual.Text = percentual.ToString() + "%";
            PBLoading.Refresh();
        }

        private async void DownloadFileAsync(string url, string filePath)
        {
            SetProgress(0);

            try
            {
                Invoke(new Action(() =>
                {
                    AjudantedeEstilo.ReformulaLblAviso(lblAviso, "                Aguarde, finalizando o download ...", 9);
                    RedimensionarForm(true);
                    InativarComponentes(true);

                }));

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
                                PBLoading.Invoke(new Action(() => { SetProgress(progress); }));
                            }
                        }
                    }

                    AjudantedeEstilo.ReformulaLblAviso(lblAviso, "               Download concluído com sucesso!", 9);
                    await Task.Delay(1000);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Houve um problema ao baixar o arquivo: {ex.Message}. Por favor, tente novamente mais tarde. Se o problema persistir, reinicie a aplicação.", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                Invoke(new Action(() =>
                {
                    AjudantedeEstilo.ReformulaLblAviso(lblAviso, "*Ou duplo click / enter no nome para iniciar download", 9);
                    RedimensionarForm(false);
                    InativarComponentes(false);
                }));
            }
        }


        private void IniciarDownload()
        {
            DownloadItem selectedItem = null;
            int selectedIndex = -1;

            if (listBoxSelecionada.SelectedIndex != -1)
            {
                selectedItem = _downloadItems[listBoxSelecionada.SelectedIndex];
                selectedIndex = listBoxSelecionada.SelectedIndex;
            }
            else if (TbPesquisar.Tag != null)
            {
                selectedItem = (DownloadItem)TbPesquisar.Tag;
                selectedIndex = _downloadItems.IndexOf(selectedItem);
                if (selectedIndex != -1)
                {
                    listBoxSelecionada.SelectedIndex = selectedIndex;
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
                MessageBox.Show("Selecione um arquivo antes de iniciar o download", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        private void RedimensionarForm(bool mostrar)
        {
            if (mostrar)
            {
                PBLoading.Visible = true;
                lblPercentual.Visible = true;
                this.Size = new Size(374, 495);
            }
            else
            {
                PBLoading.Visible = false;
                lblPercentual.Visible = false;
                this.Size = new Size(374, 465);
            }
        }

        private void InativarComponentes(bool baixando)
        {
            if (baixando)
            {
                tcListaArquivos.Enabled = false;
                listBoxSelecionada.Enabled = false;
                TbPesquisar.Enabled = false;
                btnDownload.Enabled = false;
                btnExcluir.Enabled = false;
                btnUpload.Enabled = false;
            }
            else
            {
                tcListaArquivos.Enabled = true;
                listBoxSelecionada.Enabled = true;
                TbPesquisar.Enabled = true;
                btnDownload.Enabled = true;
                btnExcluir.Enabled = true;
                btnUpload.Enabled = true;
            }
        }

        private async Task<bool> ExibirAvisoLabel()
        {
            if (tcListaArquivos.SelectedIndex == 0)
            {
                AjudantedeEstilo.ReformulaLblAviso(lblAviso, "          Acesso negado aos processos de upload.", 9);

                await Task.Delay(5000);

                AjudantedeEstilo.ReformulaLblAviso(lblAviso, "*Ou duplo click / enter no nome para iniciar download", 9);
                return false;
            }


            return true;
        }

        #region FTP

        private async void btnUpload_Click(object sender, EventArgs e)
        {
            upload = true;

            bool eIndexZero = await ExibirAvisoLabel();

            if (!eIndexZero)
                return;

            IniciarUpload();
            CarregarListaFTP(dirPadraoFtp);
        }

        private void tcListaArquivos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tcListaArquivos.SelectedIndex == 1)
            {
                listBoxSelecionada = listBoxUpload;
                pbAvisoDeleteArquivos.Visible = true;

                if (logou != true)
                {
                    using (FLogin login = new FLogin())
                    {
                        login.StartPosition = FormStartPosition.Manual;
                        login.Location = new Point(
                            this.Location.X + (this.Width - login.Width) / 2,
                            this.Location.Y + (this.Height - login.Height) / 2
                        );

                        if (login.ShowDialog() == DialogResult.OK)
                        {
                            logou = true;
                            CarregarListaFTP(dirPadraoFtp);
                        }
                        else
                        {
                            tcListaArquivos.SelectTab(0);
                        }
                    }
                }
            }
            else
            {
                listBoxSelecionada = listBoxDownload;
            }
        }

        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            upload = false;

            bool eIndexZero = await ExibirAvisoLabel();

            if (!eIndexZero)
                return;

            RemoverArquivoFtp();
        }

        private void listBoxUpload_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            IniciarDownloadFtp();
        }
        private void listBoxUpload_MouseClick(object sender, MouseEventArgs e)
        {
            AjudantedeEstilo.ReformulaTxtBox(TbPesquisar, 13, HorizontalAlignment.Center, "BUSCAR (F2)", FontStyle.Bold, Color.DarkGray);
            AjudantedeEstilo.ReformulaLblAviso(lblAviso, "*Ou duplo click / enter no nome para iniciar download", 9);
        }

        private void btnCancelarOperacao_Click(object sender, EventArgs e)
        {
            if (cancellationTokenSource != null && !cancellationTokenSource.Token.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
                upload = false;
                System.Windows.Forms.Application.DoEvents();
            }
        }

        private void listBoxUpload_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                IniciarDownloadFtp();
            }
        }

        private void AtualizarListBoxUpload(string[] arquivos)
        {
            if (listBoxUpload.InvokeRequired)
            {
                listBoxUpload.Invoke(new Action(() => AtualizarListBoxUpload(arquivos)));
            }
            else
            {
                listBoxUpload.Items.Clear();
                foreach (var arquivo in arquivos)
                {
                    listBoxUpload.Items.Add(arquivo);
                }
            }
        }
        private async void CarregarListaFTP(string directoryPath)
        {
            try
            {
                var arquivos = await Task.Run(() =>
                {
                    var items = ftpClient.GetListing(directoryPath, FtpListOption.AllFiles);
                    return items.Select(item => item.Name).ToArray();
                });

                AtualizarListBoxUpload(arquivos);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Houve um problema ao listar os arquivos: {ex.Message}. Por favor, tente novamente mais tarde. Se o problema persistir, reinicie a aplicação.", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void IniciarUpload()
        {
            cancellationTokenSource = new CancellationTokenSource();

            if (listBoxUpload.Items.Count > 0)
            {
                itensUploadAnterior.Clear();

                foreach (var item in listBoxUpload.Items)
                {
                    itensUploadAnterior.Add(item.ToString());
                }
            }

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Selecione um arquivo para enviar",
                Filter = "Arquivos compactados (*.zip; *.rar; *.7z)|*.zip;*.rar;*.7z"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePathTamanhoArquiivo = openFileDialog.FileName;
                FileInfo fileInfo = new FileInfo(filePathTamanhoArquiivo);
                const long tamanhoMaximo = 300 * 1024 * 1024;

                if (fileInfo.Length > tamanhoMaximo)
                {
                    MessageBox.Show("O arquivo selecionado excede o limite de 300 MB.", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string filePath = openFileDialog.FileName;
                string fileName = Path.GetFileName(filePath);
                string remotePath = dirPadraoFtp + fileName;

                AjudantedeEstilo.ReformulaLblAviso(lblAviso, "                Aguarde, finalizando o upload ...", 9);

                await IniciarTransferenciaFtpComProgressoAsync(filePath, remotePath, FtpOperation.Upload);

                foreach (var item in listBoxUpload.Items)
                {
                    itensUploadAtual.Add(item.ToString());
                }

                var novosItens = itensUploadAtual.Except(itensUploadAnterior).ToList();

                if (novosItens.Count > 0)
                {
                    string ultimoItemAdicionado = novosItens.Last();
                    int index = listBoxUpload.Items.IndexOf(ultimoItemAdicionado);

                    if (index != -1)
                    {
                        listBoxUpload.SelectedIndex = index;
                        listBoxUpload.TopIndex = index;
                    }
                }

                AjudantedeEstilo.ReformulaLblAviso(lblAviso, "*Ou duplo click / enter no nome para iniciar download", 9);
                listBoxSelecionada.Focus();
            }
        }

        private async void IniciarDownloadFtp()
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
                        AjudantedeEstilo.ReformulaLblAviso(lblAviso, "                Aguarde, finalizando o download ...", 9);
                        await IniciarTransferenciaFtpComProgressoAsync(localPath, arquivoSelecionadoFtp, FtpOperation.Download);
                        
                        AjudantedeEstilo.ReformulaLblAviso(lblAviso, "               Download concluído com sucesso!", 9);
                        await Task.Delay(1000);

                        AjudantedeEstilo.ReformulaLblAviso(lblAviso, "*Ou duplo click / enter no nome para iniciar download", 9);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao baixar o arquivo: {ex.Message}, reinicie a aplicação e tente novamente, se necessário mais tarde.", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private async Task IniciarTransferenciaFtpComProgressoAsync(string localPath, string remotePath, FtpOperation operacao)
        {
            bool operacaoConcluida = false;
            int retryCount = 0;
            const int maxRetries = 3;

            try
            {
                RedimensionarForm(true);
                InativarComponentes(true);

                var progress = new Progress<FtpProgress>(p =>
                {
                    int percentual = (int)p.Progress;
                    PBLoading.Invoke(new Action(() => SetProgress(percentual)));
                });
                Action<FtpProgress> progressAction = (FtpProgress p) => ((IProgress<FtpProgress>)progress).Report(p);

                while (retryCount < maxRetries && !operacaoConcluida)
                {
                    try
                    {
                        await Task.Run(() =>
                        {
                            if (operacao == FtpOperation.Download)
                            {
                                ftpClient.DownloadFile(localPath, remotePath, FtpLocalExists.Overwrite, FtpVerify.None, progressAction);
                                operacaoConcluida = true;
                            }
                            else if (operacao == FtpOperation.Upload)
                            {
                                using (var ftp = new FtpClient("ftp://files.sgbr.com.br", "publico", "96#s!G@86"))
                                {
                                    try
                                    {
                                        ftp.Connect();  
                                        ftp.SetWorkingDirectory("/dados/sgbr.com.br/interno/arquivos");  

                                        ftp.UploadFile(localPath, remotePath, FtpRemoteExists.Overwrite, false, FtpVerify.None, progressAction);

                                        CarregarListaFTP(dirPadraoFtp);

                                        operacaoConcluida = true;  
                                    }
                                    catch (FtpException ex)
                                    {
                                        MessageBox.Show($"Houve um problema ao fazer upload do arquivo: {ex.Message}. Por favor, tente novamente mais tarde. Se o problema persistir, reinicie a aplicação.", sitema , MessageBoxButtons.OK, MessageBoxIcon.Information);

                                        if(ftp.FileExists(remotePath))
                                            ftp.DeleteFile(remotePath);
                                    }
                                    finally
                                    {
                                        ftp.Disconnect(); 
                                    }
                                }
                            }
                        });
                    }
                    catch (SocketException ex)
                    {
                        retryCount++;
                        if (retryCount < maxRetries)
                        {
                            MessageBox.Show($"Erro de rede, tentando novamente... ({retryCount}/{maxRetries})", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ftpClient.Disconnect();  
                            await Task.Delay(2000); 
                        }
                        else
                        {
                            MessageBox.Show("Erro de rede: a conexão foi encerrada pelo servidor. Tente novamente mais tarde.", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);

                            if(ftpClient.FileExists(remotePath))
                                ftpClient.DeleteFile(remotePath);

                            throw;
                        }
                    }
                }

                await Task.Delay(500);  
            }
            catch (FtpCommandException ex)
            {
                MessageBox.Show($"Erro FTP: {ex.Message}", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                if (ftpClient.FileExists(remotePath))
                    ftpClient.DeleteFile(remotePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro durante a operação de FTP: {ex.Message}", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                if (ftpClient.FileExists(remotePath))
                    ftpClient.DeleteFile(remotePath);
            }
            finally
            {
                SetProgress(0);  
                RedimensionarForm(false);
                InativarComponentes(false);
            }
        }



        private async void RemoverArquivoFtp()
        {
            if (listBoxUpload.Items.Count > 0)
            {
                if (listBoxUpload.SelectedIndex == -1)
                    listBoxUpload.SelectedIndex = 0;

                var arquivoSelecionadoFtp = dirPadraoFtp + listBoxUpload.SelectedItem.ToString();


                if (arquivoSelecionadoFtp != null)
                {
                    try
                    {
                        if (ftpClient.FileExists(arquivoSelecionadoFtp))
                        {
                            btnExcluir.Enabled = false;

                            AjudantedeEstilo.ReformulaLblAviso(lblAviso, "                         Removendo arquivo...", 9);

                            await Task.Delay(1000);

                            await Task.Run(() => ftpClient.DeleteFile(arquivoSelecionadoFtp));

                            listBoxUpload.Focus();

                            btnExcluir.Enabled = true;

                            AjudantedeEstilo.ReformulaLblAviso(lblAviso, "                   Arquivo deletado com sucesso!", 9);

                            await Task.Delay(1000);

                            CarregarListaFTP(dirPadraoFtp);

                            if (listBoxUpload.Items.Count > 0)
                            {
                                listBoxUpload.SelectedIndex = 0;
                                listBoxUpload.Focus();
                            }


                            AjudantedeEstilo.ReformulaLblAviso(lblAviso, "*Ou duplo click / enter no nome para iniciar download", 9);
                        }
                        else
                        {
                            MessageBox.Show("Arquivo não encontrado no FTP.", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Houve um problema ao deletar o arquivo: {ex.Message}. Por favor, tente novamente mais tarde. Se o problema persistir, reinicie a aplicação.", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        AjudantedeEstilo.ReformulaLblAviso(lblAviso, "*Ou duplo click / enter no nome para iniciar download", 9);
                    }
                }
                else
                {
                    MessageBox.Show("Selecione um arquivo para remover!", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Não há nenhum arquivo disponível para exclusão.", sitema, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        public async Task<DateTime> ObterHoraBrasiliaAsync()
        {
            using (var httpClient = new HttpClient())
            {
                string url = "http://worldtimeapi.org/api/timezone/America/Sao_Paulo";
                var response = await httpClient.GetStringAsync(url);

                var timeData = JsonConvert.DeserializeObject<WorldTimeApiResponse>(response);

                return DateTime.Parse(timeData.datetime);
            }
        }


        // Apaga os arquivos a cada 24h de acordo com sua data de modificação e a data e hora atual de Brasília 
        private async Task ApagarArquivoFtpTimer()
        {
            DateTime horaAtualBrasilia = await ObterHoraBrasiliaAsync();

            foreach (var item in ftpClient.GetListing(dirPadraoFtp))
            {
                if (item.Type == FtpObjectType.File)
                {
                    DateTime dataModificao = ftpClient.GetModifiedTime(item.FullName);

                    if (dataModificao.Kind == DateTimeKind.Local)
                    {
                        dataModificao = dataModificao.ToUniversalTime();
                    }
                    else if (dataModificao.Kind != DateTimeKind.Utc)
                    {
                        dataModificao = DateTime.SpecifyKind(dataModificao, DateTimeKind.Utc);
                    }

                    if ((horaAtualBrasilia - dataModificao).TotalHours > 24)
                    {
                        ftpClient.DeleteFile(item.FullName);
                    }
                }
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
