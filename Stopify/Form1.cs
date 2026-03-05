using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;

using TagFile = TagLib.File;
using IOFile = System.IO.File;

namespace Stopify
{
    public partial class Form1 : Form
    {
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;
        private int indiceActual = -1;
        private bool arrastrandoProgreso = false;

        // DRAG DROP
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        private Rectangle dragBoxFromMouseDown;
        private int dragRowIndex = -1;

        // ALEATORIO
        private bool modoAleatorio = false;
        private ArrayManager _shuffleManager;
        private int _shufflePosition = 0;

        // REPETIR:  0=off  1=lista  2=canción
        private int modoRepetir = 0;

        // COVERS
        private CoverSearcher _coverSearcher;

        // SCROLLBAR personalizado para el grid
        private CustomScrollBar _gridScroll;

        // SCROLLBAR personalizado para la letra
        private CustomScrollBar _lyricsScroll;
        private bool _sincronizandoLetra = false; // guard anti-reentrada

        // PANTALLA COMPLETA
        private bool _fullscreen = false;
        private FormWindowState _prevState;

        //  Colores de estado 
        private static readonly Color _playNormal = Color.FromArgb(30, 160, 30);
        private static readonly Color _playHover = Color.FromArgb(40, 200, 40);
        private static readonly Color _pauseNormal = Color.FromArgb(180, 130, 0);
        private static readonly Color _pauseHover = Color.FromArgb(210, 160, 0);
        private static readonly Color _activeNormal = Color.FromArgb(20, 130, 20);
        private static readonly Color _activeHover = Color.FromArgb(28, 170, 28);
        private static readonly Color _repeatNormal = Color.FromArgb(140, 80, 0);
        private static readonly Color _repeatHover = Color.FromArgb(185, 110, 0);
        private static readonly Color _offNormal = Color.FromArgb(38, 38, 38);
        private static readonly Color _offHover = Color.FromArgb(58, 58, 58);
        private static readonly Color _offFore = Color.FromArgb(155, 155, 155);

        public Form1()
        {
            InitializeComponent();
            ConfigurarGrid();
            ConfigurarScrollBarPersonalizado();
            ConfigurarScrollBarLetra();

            _coverSearcher = new CoverSearcher();

            timer1.Interval = 500;
            timer1.Start();

            tbVolumen.Value = 50;

            ConfigurarDragDrop();

            // F11 → pantalla completa
            KeyPreview = true;
            KeyDown += Form1_KeyDown;

            // Redibujar scrollbars cuando cambia el tamaño
            Resize += (s, e) => { ActualizarScrollBar(); ActualizarScrollBarLetra(); };
        }

        //  PANTALLA COMPLETA 

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
                ToggleFullscreen();
        }

        private void ToggleFullscreen()
        {
            _fullscreen = !_fullscreen;
            if (_fullscreen)
            {
                _prevState = WindowState;
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                WindowState = _prevState;
            }
        }

        //  SCROLLBAR PERSONALIZADO 

        private void ConfigurarScrollBarPersonalizado()
        {
            // Ocultar el scrollbar nativo del DataGridView
            dgvCanciones.ScrollBars = ScrollBars.None;

            _gridScroll = new CustomScrollBar
            {
                Width = 8,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right,
                TrackColor = Color.FromArgb(28, 28, 28),
                ThumbColor = Color.FromArgb(72, 72, 72),
                ThumbHover = Color.FromArgb(110, 110, 110),
                ThumbActive = Color.LimeGreen,
            };

            Controls.Add(_gridScroll);
            _gridScroll.BringToFront();

            _gridScroll.Scroll += (s, e) =>
            {
                if (dgvCanciones.Rows.Count > 0)
                {
                    int idx = Math.Min(_gridScroll.Value, dgvCanciones.Rows.Count - 1);
                    dgvCanciones.FirstDisplayedScrollingRowIndex = idx;
                }
            };

            dgvCanciones.Scroll += (s, e) => SincronizarScrollBar();
            dgvCanciones.MouseWheel += (s, e) => SincronizarScrollBar();
            dgvCanciones.RowsAdded += (s, e) => ActualizarScrollBar();
            dgvCanciones.RowsRemoved += (s, e) => ActualizarScrollBar();

            ActualizarScrollBar();
        }

        private void ActualizarScrollBar()
        {
            if (_gridScroll == null) return;
            _gridScroll.Location = new Point(dgvCanciones.Right + 4, dgvCanciones.Top);
            _gridScroll.Height = dgvCanciones.Height;

            int total = dgvCanciones.Rows.Count;
            int visible = dgvCanciones.DisplayedRowCount(false);

            if (total > visible)
            {
                _gridScroll.Minimum = 0;
                _gridScroll.Maximum = total;
                _gridScroll.LargeChange = Math.Max(1, visible);
                _gridScroll.Visible = true;
            }
            else _gridScroll.Visible = false;
        }

        private void SincronizarScrollBar()
        {
            if (_gridScroll == null || !_gridScroll.Visible) return;
            if (dgvCanciones.Rows.Count == 0) return;
            _gridScroll.Value = dgvCanciones.FirstDisplayedScrollingRowIndex;
        }

        //  SCROLLBAR PERSONALIZADO LETRA 

        private void ConfigurarScrollBarLetra()
        {
            _lyricsScroll = new CustomScrollBar
            {
                Width = 6,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right,
                TrackColor = Color.FromArgb(22, 22, 22),
                ThumbColor = Color.FromArgb(70, 70, 70),
                ThumbHover = Color.FromArgb(105, 105, 105),
                ThumbActive = Color.LimeGreen,
            };

            Controls.Add(_lyricsScroll);
            _lyricsScroll.BringToFront();

            // Scroll custom  mover el RichTextBox (con guard)
            _lyricsScroll.Scroll += (s, e) =>
            {
                if (_sincronizandoLetra) return;
                _sincronizandoLetra = true;
                try
                {
                    int totalLines = rtbLyrics.Lines.Length;
                    int targetLine = Math.Min(_lyricsScroll.Value, Math.Max(0, totalLines - 1));
                    int charIndex = rtbLyrics.GetFirstCharIndexFromLine(targetLine);
                    if (charIndex >= 0)
                    {
                        rtbLyrics.SelectionStart = charIndex;
                        rtbLyrics.ScrollToCaret();
                    }
                }
                finally { _sincronizandoLetra = false; }
            };

            // RichTextBox → actualizar scrollbar (con guard)
            rtbLyrics.VScroll += (s, e) => SincronizarScrollBarLetra();
            rtbLyrics.MouseWheel += (s, e) => { this.BeginInvoke((Action)SincronizarScrollBarLetra); };
            rtbLyrics.TextChanged += (s, e) => ActualizarScrollBarLetra();

            ActualizarScrollBarLetra();
        }

        private void ActualizarScrollBarLetra()
        {
            if (_lyricsScroll == null) return;

            _lyricsScroll.Location = new Point(rtbLyrics.Right + 2, rtbLyrics.Top);
            _lyricsScroll.Height = rtbLyrics.Height;

            int totalLines = rtbLyrics.Lines.Length;
            int visibleLines = Math.Max(1, rtbLyrics.Height / Math.Max(1, rtbLyrics.Font.Height + 2));

            if (totalLines > visibleLines)
            {
                _lyricsScroll.Minimum = 0;
                _lyricsScroll.Maximum = totalLines;
                _lyricsScroll.LargeChange = visibleLines;
                _lyricsScroll.Visible = true;
            }
            else _lyricsScroll.Visible = false;
        }

        private void SincronizarScrollBarLetra()
        {
            if (_lyricsScroll == null || !_lyricsScroll.Visible) return;
            if (_sincronizandoLetra) return;
            _sincronizandoLetra = true;
            try
            {
                int firstChar = rtbLyrics.GetCharIndexFromPosition(new Point(1, 1));
                int firstLine = rtbLyrics.GetLineFromCharIndex(firstChar);
                _lyricsScroll.Value = firstLine;
            }
            finally { _sincronizandoLetra = false; }
        }

        // DRAG & DROP 

        private void ConfigurarDragDrop()
        {
            dgvCanciones.AllowDrop = true;
            dgvCanciones.MouseDown += dgvCanciones_MouseDown;
            dgvCanciones.MouseMove += dgvCanciones_MouseMove;
            dgvCanciones.DragOver += dgvCanciones_DragOver;
            dgvCanciones.DragDrop += dgvCanciones_DragDrop;
            dgvCanciones.Paint += dgvCanciones_Paint;
            dgvCanciones.RowsAdded += (s, e) => ActualizarScrollBar();
            dgvCanciones.RowsRemoved += (s, e) => ActualizarScrollBar();
        }

        private void dgvCanciones_MouseDown(object sender, MouseEventArgs e)
        {
            rowIndexFromMouseDown = dgvCanciones.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown != -1)
            {
                Size sz = SystemInformation.DragSize;
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - sz.Width / 2, e.Y - sz.Height / 2), sz);
            }
            else dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void dgvCanciones_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left &&
                dragBoxFromMouseDown != Rectangle.Empty &&
                !dragBoxFromMouseDown.Contains(e.X, e.Y) &&
                rowIndexFromMouseDown != -1)
                dgvCanciones.DoDragDrop(dgvCanciones.Rows[rowIndexFromMouseDown], DragDropEffects.Move);
        }

        private void dgvCanciones_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            Point cp = dgvCanciones.PointToClient(new Point(e.X, e.Y));
            dragRowIndex = dgvCanciones.HitTest(cp.X, cp.Y).RowIndex;
            dgvCanciones.Invalidate();
        }

        private void dgvCanciones_Paint(object sender, PaintEventArgs e)
        {
            if (dragRowIndex >= 0 && dragRowIndex < dgvCanciones.Rows.Count)
            {
                Rectangle rect = dgvCanciones.GetRowDisplayRectangle(dragRowIndex, true);
                using (Pen pen = new Pen(Color.LimeGreen, 2))
                    e.Graphics.DrawLine(pen, rect.Left, rect.Top, rect.Right, rect.Top);
            }
        }

        private void dgvCanciones_DragDrop(object sender, DragEventArgs e)
        {
            Point cp = dgvCanciones.PointToClient(new Point(e.X, e.Y));
            rowIndexOfItemUnderMouseToDrop = dgvCanciones.HitTest(cp.X, cp.Y).RowIndex;

            if (e.Effect == DragDropEffects.Move && rowIndexOfItemUnderMouseToDrop != -1)
            {
                var row = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
                if (row != null && rowIndexFromMouseDown != rowIndexOfItemUnderMouseToDrop)
                {
                    string t = row.Cells["Titulo"].Value.ToString();
                    string a = row.Cells["Artista"].Value.ToString();
                    string al = row.Cells["Album"].Value.ToString();
                    string d = row.Cells["Duracion"].Value.ToString();
                    string r = row.Cells["Ruta"].Value.ToString();

                    dgvCanciones.Rows.RemoveAt(rowIndexFromMouseDown);
                    int ti = rowIndexOfItemUnderMouseToDrop;
                    if (rowIndexFromMouseDown < rowIndexOfItemUnderMouseToDrop) ti--;

                    dgvCanciones.Rows.Insert(ti, t, a, al, d, r);
                    dgvCanciones.Rows[ti].Selected = true;
                    dgvCanciones.CurrentCell = dgvCanciones.Rows[ti].Cells[0];

                    if (indiceActual == rowIndexFromMouseDown) indiceActual = ti;
                    else if (rowIndexFromMouseDown < indiceActual && ti >= indiceActual) indiceActual--;
                    else if (rowIndexFromMouseDown > indiceActual && ti <= indiceActual) indiceActual++;
                }
            }
        }

        //GRID 

        private void ConfigurarGrid()
        {
            dgvCanciones.Columns.Clear();
            dgvCanciones.Columns.Add("Titulo", "Título");
            dgvCanciones.Columns.Add("Artista", "Artista");
            dgvCanciones.Columns.Add("Album", "Álbum");
            dgvCanciones.Columns.Add("Duracion", "Duración");
            dgvCanciones.Columns.Add("Ruta", "Ruta");
            dgvCanciones.Columns["Ruta"].Visible = false;

            dgvCanciones.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            dgvCanciones.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCanciones.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvCanciones.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCanciones.EnableHeadersVisualStyles = false;

            dgvCanciones.DefaultCellStyle.BackColor = Color.FromArgb(32, 32, 32);
            dgvCanciones.DefaultCellStyle.ForeColor = Color.White;
            dgvCanciones.DefaultCellStyle.SelectionBackColor = Color.FromArgb(30, 160, 30);
            dgvCanciones.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvCanciones.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(42, 42, 42);
            dgvCanciones.GridColor = Color.FromArgb(55, 55, 55);
        }

        private void AgregarCancionAlGrid(string ruta)
        {
            try
            {
                var mp3 = TagFile.Create(ruta);
                dgvCanciones.Rows.Add(
                    LimpiarTitulo(mp3.Tag.Title ?? Path.GetFileNameWithoutExtension(ruta)),
                    LimpiarNombreArtista(mp3.Tag.FirstPerformer ?? "Desconocido"),
                    mp3.Tag.Album ?? "Desconocido",
                    mp3.Properties.Duration.ToString(@"mm\:ss"),
                    ruta);
            }
            catch { }
        }

        private string LimpiarNombreArtista(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "Desconocido";
            return System.Text.RegularExpressions.Regex.Replace(s,
                @"\s*-?\s*(Topic|VEVO|Official)$", "",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase).Trim();
        }

        private string LimpiarTitulo(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "Sin título";
            s = System.Text.RegularExpressions.Regex.Replace(s,
                @"\s*\([^)]*?(Official|Audio|Video|Lyrics|Lyric|Music Video|HD|4K|Visualizer|Explicit|Clean|Remaster)[^)]*?\)", "",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            s = System.Text.RegularExpressions.Regex.Replace(s,
                @"\s*\[[^\]]*?(Official|Audio|Video|Lyrics|Lyric|Music Video|HD|4K|Visualizer|Explicit|Clean|Remaster)[^\]]*?\]", "",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            s = System.Text.RegularExpressions.Regex.Replace(s,
                @"\s*-?\s*(Official Audio|Official Video|Music Video|Official Music Video|Lyrics|Lyric Video|Official Lyric Video|Audio|Video|Visualizer|Explicit|Clean|Remastered)$", "",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return s.Trim();
        }

        //CARPETA / PLAYLIST

        private void btnAbrirCarpeta_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
                CargarCancionesDeCarpeta(fbd.SelectedPath);
        }

        private void CargarCancionesDeCarpeta(string carpeta)
        {
            dgvCanciones.Rows.Clear();
            foreach (var r in Directory.GetFiles(carpeta, "*.mp3"))
                AgregarCancionAlGrid(r);
            if (dgvCanciones.Rows.Count > 0) { dgvCanciones.Rows[0].Selected = true; indiceActual = 0; }
            ActualizarScrollBar();
        }

        private void btnGuardarPlaylist_Click(object sender, EventArgs e)
        {
            if (dgvCanciones.Rows.Count == 0) { MessageBox.Show("No hay canciones cargadas."); return; }
            using var sfd = new SaveFileDialog { Filter = "Playlist (*.txt)|*.txt" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                IOFile.WriteAllLines(sfd.FileName,
                    dgvCanciones.Rows.Cast<DataGridViewRow>()
                        .Select(r => r.Cells["Ruta"].Value.ToString()));
                MessageBox.Show("Playlist guardada.");
            }
        }

        private void btnCargarPlaylist_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog { Filter = "Playlist (*.txt)|*.txt" };
            if (ofd.ShowDialog() == DialogResult.OK) CargarPlaylist(ofd.FileName);
        }

        private void CargarPlaylist(string archivo)
        {
            dgvCanciones.Rows.Clear();
            foreach (string r in IOFile.ReadAllLines(archivo))
                if (IOFile.Exists(r)) AgregarCancionAlGrid(r);
            if (dgvCanciones.Rows.Count > 0) { indiceActual = 0; dgvCanciones.Rows[0].Selected = true; }
            ActualizarScrollBar();
            MessageBox.Show("Playlist cargada.");
        }

        //REPRODUCCIÓN 

        private async void dgvCanciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) { indiceActual = e.RowIndex; await ReproducirCancion(indiceActual); }
        }

        private async Task ReproducirCancion(int index)
        {
            if (index < 0 || index >= dgvCanciones.Rows.Count) return;
            DetenerCancion();

            string ruta = dgvCanciones.Rows[index].Cells["Ruta"].Value.ToString();
            string titulo = dgvCanciones.Rows[index].Cells["Titulo"].Value.ToString();
            string artista = dgvCanciones.Rows[index].Cells["Artista"].Value.ToString();

            rtbLyrics.Clear();
            await CargarCover(ruta, artista, titulo);

            audioFile = new AudioFileReader(ruta);
            outputDevice = new WaveOutEvent();
            outputDevice.Init(audioFile);
            outputDevice.Volume = tbVolumen.Value / 100f;
            outputDevice.Play();

            tbProgreso.Maximum = (int)audioFile.TotalTime.TotalSeconds;
            lblTiempoActual.Text = "00:00";
            lblTiempoTotal.Text = audioFile.TotalTime.ToString(@"mm\:ss");

            SetPlayPauseState(true);
        }

        private void DetenerCancion()
        {
            outputDevice?.Stop(); outputDevice?.Dispose(); outputDevice = null;
            audioFile?.Dispose(); audioFile = null;
            tbProgreso.Value = 0;
            lblTiempoActual.Text = "00:00";
            SetPlayPauseState(false);
        }

        //Estado visual de botones 

        private void SetPlayPauseState(bool playing)
        {
            if (playing)
            {
                btnPlayPause.Text = "⏸   Pause";
                btnPlayPause.NormalColor = _pauseNormal;
                btnPlayPause.HoverColor = _pauseHover;
            }
            else
            {
                btnPlayPause.Text = "▶   Play";
                btnPlayPause.NormalColor = _playNormal;
                btnPlayPause.HoverColor = _playHover;
            }
            btnPlayPause.ForeColor = Color.White;
            btnPlayPause.Invalidate();
        }

        private void SetAleatorioState(bool on)
        {
            btnAleatorio.NormalColor = on ? _activeNormal : _offNormal;
            btnAleatorio.HoverColor = on ? _activeHover : _offHover;
            btnAleatorio.ForeColor = on ? Color.White : _offFore;
            btnAleatorio.Text = on ? "🔀  ON" : "🔀  Aleatorio";
            btnAleatorio.Invalidate();
        }

        private void SetRepetirState(int estado)
        {
            switch (estado)
            {
                case 0:
                    btnRepetir.NormalColor = _offNormal; btnRepetir.HoverColor = _offHover;
                    btnRepetir.ForeColor = _offFore; btnRepetir.Text = "🔁  Repetir"; break;
                case 1:
                    btnRepetir.NormalColor = _activeNormal; btnRepetir.HoverColor = _activeHover;
                    btnRepetir.ForeColor = Color.White; btnRepetir.Text = "🔁  Lista"; break;
                case 2:
                    btnRepetir.NormalColor = _repeatNormal; btnRepetir.HoverColor = _repeatHover;
                    btnRepetir.ForeColor = Color.White; btnRepetir.Text = "🔂  Canción"; break;
            }
            btnRepetir.Invalidate();
        }

        //CAMBIAR CANCIÓN 

        private async Task CambiarCancion(int direccion)
        {
            if (dgvCanciones.Rows.Count == 0) return;

            if (modoRepetir == 2) { await ReproducirCancion(indiceActual); return; }

            if (modoAleatorio)
            {
                if (_shuffleManager == null || _shuffleManager.Vector.Length != dgvCanciones.Rows.Count)
                {
                    _shuffleManager = new ArrayManager(dgvCanciones.Rows.Count);
                    _shuffleManager.FillArray(); _shuffleManager.ShuffleArray(); _shufflePosition = 0;
                }
                if (_shufflePosition >= _shuffleManager.Vector.Length)
                {
                    if (modoRepetir == 0) { DetenerCancion(); return; }
                    _shuffleManager.ShuffleArray(); _shufflePosition = 0;
                }
                indiceActual = _shuffleManager.Vector[_shufflePosition] - 1;
                _shufflePosition++;
            }
            else
            {
                int sig = indiceActual + direccion;
                if (sig >= dgvCanciones.Rows.Count) { if (modoRepetir == 0) { DetenerCancion(); return; } indiceActual = 0; }
                else if (sig < 0) indiceActual = dgvCanciones.Rows.Count - 1;
                else indiceActual = sig;
            }

            dgvCanciones.Rows[indiceActual].Selected = true;
            dgvCanciones.CurrentCell = dgvCanciones.Rows[indiceActual].Cells[0];
            await ReproducirCancion(indiceActual);
        }

        //BOTONES 

        private async void btnPlayPause_Click(object sender, EventArgs e)
        {
            if (!ValidarCancionesCargadas()) return;
            if (outputDevice == null || audioFile == null)
            {
                indiceActual = dgvCanciones.SelectedRows.Count > 0 ? dgvCanciones.SelectedRows[0].Index : 0;
                await ReproducirCancion(indiceActual);
                return;
            }
            if (outputDevice.PlaybackState == PlaybackState.Playing)
            { outputDevice.Pause(); SetPlayPauseState(false); }
            else
            { outputDevice.Play(); SetPlayPauseState(true); }
        }

        private void btnStop_Click(object sender, EventArgs e) { if (!ValidarCancionesCargadas()) return; DetenerCancion(); }

        private async void btnNext_Click(object sender, EventArgs e) { if (!ValidarCancionesCargadas()) return; await CambiarCancion(1); }

        private async void btnPrev_Click(object sender, EventArgs e) { if (!ValidarCancionesCargadas()) return; await CambiarCancion(-1); }

        private void btnAleatorio_Click(object sender, EventArgs e)
        {
            modoAleatorio = !modoAleatorio;
            if (modoAleatorio)
            {
                _shuffleManager = new ArrayManager(dgvCanciones.Rows.Count);
                _shuffleManager.FillArray(); _shuffleManager.ShuffleArray(); _shufflePosition = 0;
            }
            SetAleatorioState(modoAleatorio);
        }

        private void btnRepetir_Click(object sender, EventArgs e) { modoRepetir = (modoRepetir + 1) % 3; SetRepetirState(modoRepetir); }

        private void btnEliminarCancion_Click(object sender, EventArgs e)
        {
            if (!ValidarCancionesCargadas()) return;
            if (dgvCanciones.SelectedRows.Count == 0) { MessageBox.Show("Selecciona una canción primero."); return; }

            int idx = dgvCanciones.SelectedRows[0].Index;
            string titulo = dgvCanciones.Rows[idx].Cells["Titulo"].Value.ToString();
            string artista = dgvCanciones.Rows[idx].Cells["Artista"].Value.ToString();

            if (MessageBox.Show($"¿Eliminar de la lista?\n\n{artista} - {titulo}",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (indiceActual == idx) { DetenerCancion(); indiceActual = -1; }
                dgvCanciones.Rows.RemoveAt(idx);
                if (indiceActual > idx) indiceActual--;
                if (dgvCanciones.Rows.Count > 0)
                    dgvCanciones.Rows[Math.Min(idx, dgvCanciones.Rows.Count - 1)].Selected = true;
                ActualizarScrollBar();
            }
        }

        private async void btnEditarTags_Click(object sender, EventArgs e)
        {
            if (!ValidarCancionesCargadas()) return;
            if (dgvCanciones.SelectedRows.Count == 0) { MessageBox.Show("Selecciona una canción primero."); return; }

            int idx = dgvCanciones.SelectedRows[0].Index;
            string ruta = dgvCanciones.Rows[idx].Cells["Ruta"].Value.ToString();

            bool estabaReproduciendo = audioFile != null && idx == indiceActual &&
                                       outputDevice?.PlaybackState == PlaybackState.Playing;
            if (audioFile != null && idx == indiceActual) DetenerCancion();

            try
            {
                var mp3 = TagFile.Create(ruta);
                using var fe = new FormEditarTags(mp3.Tag.Title ?? "", mp3.Tag.FirstPerformer ?? "", mp3.Tag.Album ?? "");
                if (fe.ShowDialog() == DialogResult.OK)
                {
                    mp3.Tag.Title = fe.Titulo; mp3.Tag.Performers = new[] { fe.Artista }; mp3.Tag.Album = fe.Album;
                    mp3.Save();
                    dgvCanciones.Rows[idx].Cells["Titulo"].Value = fe.Titulo;
                    dgvCanciones.Rows[idx].Cells["Artista"].Value = fe.Artista;
                    dgvCanciones.Rows[idx].Cells["Album"].Value = fe.Album;
                    MessageBox.Show("Tags actualizados.");
                }
                if (estabaReproduciendo) await ReproducirCancion(indiceActual);
            }
            catch (Exception ex) { MessageBox.Show($"Error al editar tags: {ex.Message}"); }
        }

        //cOVER

        private async Task CargarCover(string ruta, string artista, string titulo)
        {
            pbCover.Image?.Dispose(); pbCover.Image = null;
            try
            {
                var mp3 = TagFile.Create(ruta);
                if (mp3.Tag.Pictures.Length > 0)
                {
                    using var ms = new System.IO.MemoryStream(mp3.Tag.Pictures[0].Data.Data);
                    pbCover.Image = System.Drawing.Image.FromStream(ms);
                }
                else await BuscarYDescargarCover(ruta, artista, titulo);
            }
            catch { pbCover.Image = null; }
        }

        private async Task BuscarYDescargarCover(string ruta, string artista, string titulo)
        {
            try
            {
                var res = await _coverSearcher.BuscarMejorCover(artista, titulo);
                if (res != null && res.Similitud > 0.5)
                {
                    using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
                    try
                    {
                        byte[] img = await client.GetByteArrayAsync(res.Url);
                        using var ms = new System.IO.MemoryStream(img);
                        pbCover.Image = System.Drawing.Image.FromStream(ms);
                        await GuardarCoverEnMP3(ruta, img);
                    }
                    catch { }
                }
            }
            catch { pbCover.Image = null; }
        }

        private async Task GuardarCoverEnMP3(string ruta, byte[] img)
        {
            await Task.Run(() =>
            {
                try
                {
                    var mp3 = TagFile.Create(ruta);
                    mp3.Tag.Pictures = new TagLib.IPicture[]
                    {
                        new TagLib.Picture(img) { Type = TagLib.PictureType.FrontCover, MimeType = "image/png", Description = "Cover" }
                    };
                    mp3.Save();
                }
                catch { }
            });
        }

        //LETRA

        private async void btnLetra_Click(object sender, EventArgs e)
        {
            if (dgvCanciones.SelectedRows.Count == 0) { MessageBox.Show("Selecciona una canción primero."); return; }

            string artista = _coverSearcher.NormalizarTexto(ExtraerArtistaPrincipal(
                dgvCanciones.SelectedRows[0].Cells["Artista"].Value.ToString()));
            string titulo = _coverSearcher.NormalizarTexto(
                dgvCanciones.SelectedRows[0].Cells["Titulo"].Value.ToString());

            if (string.IsNullOrWhiteSpace(artista) || string.IsNullOrWhiteSpace(titulo))
            { MessageBox.Show("No se puede buscar letra sin artista y título."); return; }

            rtbLyrics.Text = "Buscando letra..."; btnLetra.Enabled = false;
            try
            {
                using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(25) };
                client.DefaultRequestHeaders.Add("User-Agent", "Stopify/1.0");
                var response = await client.GetAsync(
                    $"https://lrclib.net/api/get?artist_name={Uri.EscapeDataString(artista)}&track_name={Uri.EscapeDataString(titulo)}");
                if (!response.IsSuccessStatusCode) { rtbLyrics.Text = "No se encontró letra."; return; }
                using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                rtbLyrics.Text = doc.RootElement.TryGetProperty("plainLyrics", out var lyr) && !string.IsNullOrWhiteSpace(lyr.GetString())
                    ? lyr.GetString() : "No se encontró letra.";
                ActualizarScrollBarLetra();
            }
            catch (Exception ex) { rtbLyrics.Text = $"Error: {ex.Message}"; }
            finally { btnLetra.Enabled = true; }
        }

        //WMP 

        private void btnAbrirWMP_Click(object sender, EventArgs e)
        {
            if (dgvCanciones.SelectedRows.Count == 0) { MessageBox.Show("Selecciona una canción primero."); return; }
            try
            {
                System.Diagnostics.Process.Start("wmplayer.exe",
                    $"\"{dgvCanciones.SelectedRows[0].Cells["Ruta"].Value}\"");
                outputDevice?.Pause();
                SetPlayPauseState(false);
            }
            catch (Exception ex) { MessageBox.Show("Error abriendo WMP: " + ex.Message); }
        }

        //TIMER 

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (audioFile != null && outputDevice != null && !arrastrandoProgreso)
            {
                tbProgreso.Value = Math.Min(tbProgreso.Maximum, (int)audioFile.CurrentTime.TotalSeconds);
                lblTiempoActual.Text = audioFile.CurrentTime.ToString(@"mm\:ss");
            }

            if (audioFile != null && outputDevice != null &&
                audioFile.TotalTime.TotalSeconds - audioFile.CurrentTime.TotalSeconds <= 1.0 &&
                outputDevice.PlaybackState == PlaybackState.Playing)
            {
                _ = Task.Run(async () =>
                {
                    await Task.Delay(100);
                    this.Invoke((MethodInvoker)async delegate { await CambiarCancion(1); });
                });
                outputDevice.Pause();
            }
        }

        private void tbVolumen_ValueChanged(object sender, EventArgs e)
        { if (outputDevice != null) outputDevice.Volume = tbVolumen.Value / 100f; }

        private void tbProgreso_MouseDown(object sender, MouseEventArgs e) => arrastrandoProgreso = true;

        private void tbProgreso_MouseUp(object sender, MouseEventArgs e)
        {
            if (audioFile != null) audioFile.CurrentTime = TimeSpan.FromSeconds(tbProgreso.Value);
            arrastrandoProgreso = false;
        }

        //HELPERS 

        private bool ValidarCancionesCargadas()
        {
            if (dgvCanciones.Rows.Count == 0)
            {
                MessageBox.Show("Selecciona una carpeta primero.", "Sin canciones",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private string ExtraerArtistaPrincipal(string artista)
        {
            if (string.IsNullOrWhiteSpace(artista)) return "";
            foreach (var sep in new[] { " feat.", " feat ", " ft.", " ft ", " featuring ", " & ", " and ", "," })
            {
                int i = artista.IndexOf(sep, StringComparison.OrdinalIgnoreCase);
                if (i > 0) return artista.Substring(0, i).Trim();
            }
            return artista.Trim();
        }
    }
}