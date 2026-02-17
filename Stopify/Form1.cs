using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;  
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;

// Alias para evitar conflictos
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

        // VARIABLES PARA DRAG DROP
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        private Rectangle dragBoxFromMouseDown;
        private int dragRowIndex = -1;

        // ALEATORIO Y REPETIR
        private bool modoAleatorio = false;
        private bool modoRepetir = false;
        private Random random = new Random();

        // BUSCADOR DE COVERS MEJORADO
        private CoverSearcher _coverSearcher;

        public Form1()
        {
            InitializeComponent();
            ConfigurarGrid();

            // Inicializar el buscador de covers
            _coverSearcher = new CoverSearcher();

            timer1.Interval = 500;
            timer1.Start();

            tbVolumen.Value = 50;

            // HABILITAR DRAG DROP
            ConfigurarDragDrop();
        }

        /// //////////////////////////////////////////////
        // CONFIGURAR DRAG & DROP
        ////// 
        private void ConfigurarDragDrop()
        {
            dgvCanciones.AllowDrop = true;
            dgvCanciones.MouseDown += dgvCanciones_MouseDown;
            dgvCanciones.MouseMove += dgvCanciones_MouseMove;
            dgvCanciones.DragOver += dgvCanciones_DragOver;
            dgvCanciones.DragDrop += dgvCanciones_DragDrop;
            dgvCanciones.Paint += dgvCanciones_Paint;  
        }

        // EVENTOS DE DRAG & DROP
        private void dgvCanciones_MouseDown(object sender, MouseEventArgs e)
        {
            rowIndexFromMouseDown = dgvCanciones.HitTest(e.X, e.Y).RowIndex;

            if (rowIndexFromMouseDown != -1)
            {
                Size dragSize = SystemInformation.DragSize;
                dragBoxFromMouseDown = new Rectangle(
                    new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)),
                    dragSize);
            }
            else
            {
                dragBoxFromMouseDown = Rectangle.Empty;
            }
        }

        private void dgvCanciones_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    if (rowIndexFromMouseDown != -1)
                    {
                        DataGridViewRow rowToMove = dgvCanciones.Rows[rowIndexFromMouseDown];
                        dgvCanciones.DoDragDrop(rowToMove, DragDropEffects.Move);
                    }
                }
            }
        }

        private void dgvCanciones_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            Point clientPoint = dgvCanciones.PointToClient(new Point(e.X, e.Y));
            dragRowIndex = dgvCanciones.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            dgvCanciones.Invalidate();
        }

        private void dgvCanciones_Paint(object sender, PaintEventArgs e)
        {
            if (dragRowIndex >= 0 && dragRowIndex < dgvCanciones.Rows.Count)
            {
                Graphics g = e.Graphics;
                Rectangle rect = dgvCanciones.GetRowDisplayRectangle(dragRowIndex, true);

                using (Pen pen = new Pen(Color.CornflowerBlue, 2))
                {
                    g.DrawLine(pen, rect.Left, rect.Top, rect.Right, rect.Top);
                }
            }
        }

        private void dgvCanciones_DragDrop(object sender, DragEventArgs e)
        {
            Point clientPoint = dgvCanciones.PointToClient(new Point(e.X, e.Y));
            rowIndexOfItemUnderMouseToDrop = dgvCanciones.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            if (e.Effect == DragDropEffects.Move && rowIndexOfItemUnderMouseToDrop != -1)
            {
                DataGridViewRow rowToMove = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;

                if (rowToMove != null && rowIndexFromMouseDown != rowIndexOfItemUnderMouseToDrop)
                {
                    string titulo = rowToMove.Cells["Titulo"].Value.ToString();
                    string artista = rowToMove.Cells["Artista"].Value.ToString();
                    string album = rowToMove.Cells["Album"].Value.ToString();
                    string duracion = rowToMove.Cells["Duracion"].Value.ToString();
                    string ruta = rowToMove.Cells["Ruta"].Value.ToString();

                    dgvCanciones.Rows.RemoveAt(rowIndexFromMouseDown);

                    int targetIndex = rowIndexOfItemUnderMouseToDrop;
                    if (rowIndexFromMouseDown < rowIndexOfItemUnderMouseToDrop)
                    {
                        targetIndex--;
                    }

                    dgvCanciones.Rows.Insert(targetIndex, titulo, artista, album, duracion, ruta);

                    dgvCanciones.Rows[targetIndex].Selected = true;
                    dgvCanciones.CurrentCell = dgvCanciones.Rows[targetIndex].Cells[0];

                    if (indiceActual == rowIndexFromMouseDown)
                    {
                        indiceActual = targetIndex;
                    }
                    else if (rowIndexFromMouseDown < indiceActual && targetIndex >= indiceActual)
                    {
                        indiceActual--;
                    }
                    else if (rowIndexFromMouseDown > indiceActual && targetIndex <= indiceActual)
                    {
                        indiceActual++;
                    }
                }
            }
        }

        // CONFIGURAR GRID
        private void ConfigurarGrid()
        {
            dgvCanciones.Columns.Clear();

            dgvCanciones.Columns.Add("Titulo", "Título");
            dgvCanciones.Columns.Add("Artista", "Artista");
            dgvCanciones.Columns.Add("Album", "Álbum");
            dgvCanciones.Columns.Add("Duracion", "Duración");
            dgvCanciones.Columns.Add("Ruta", "Ruta");

            dgvCanciones.Columns["Ruta"].Visible = false;

            dgvCanciones.ColumnHeadersDefaultCellStyle.BackColor = Color.SlateGray;
            dgvCanciones.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCanciones.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvCanciones.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvCanciones.EnableHeadersVisualStyles = false; 

            dgvCanciones.DefaultCellStyle.BackColor = Color.White;
            dgvCanciones.DefaultCellStyle.ForeColor = Color.Black;
            dgvCanciones.DefaultCellStyle.SelectionBackColor = Color.LimeGreen;
            dgvCanciones.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvCanciones.AlternatingRowsDefaultCellStyle.BackColor = Color.SlateGray;

            dgvCanciones.GridColor = Color.Black;
        }

        //AGREGAR CANCIÓN AL GRID
        private void AgregarCancionAlGrid(string ruta)
        {
            try
            {
                var mp3 = TagFile.Create(ruta);

                string titulo = mp3.Tag.Title ?? Path.GetFileNameWithoutExtension(ruta);
                string artista = mp3.Tag.FirstPerformer ?? "Desconocido";
                string album = mp3.Tag.Album ?? "Desconocido";
                string duracion = mp3.Properties.Duration.ToString(@"mm\:ss");

                artista = LimpiarNombreArtista(artista);
                titulo = LimpiarTitulo(titulo);

                dgvCanciones.Rows.Add(titulo, artista, album, duracion, ruta);
            }
            catch
            {
                // Ignorar archivos dañados
            }
        }

        // LIMPIAR NOMBRE DE ARTISTA
        private string LimpiarNombreArtista(string artista)
        {
            if (string.IsNullOrWhiteSpace(artista))
                return "Desconocido";

            artista = System.Text.RegularExpressions.Regex.Replace(
                artista,
                @"\s*-?\s*(Topic|VEVO|Official)$",
                "",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );

            return artista.Trim();
        }

        // LIMPIAR TÍTULO
        private string LimpiarTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                return "Sin título";

            titulo = System.Text.RegularExpressions.Regex.Replace(
                titulo,
                @"\s*\([^)]*?(Official|Audio|Video|Lyrics|Lyric|Music Video|HD|4K|Visualizer|Explicit|Clean|Remaster)[^)]*?\)",
                "",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );

            titulo = System.Text.RegularExpressions.Regex.Replace(
                titulo,
                @"\s*\[[^\]]*?(Official|Audio|Video|Lyrics|Lyric|Music Video|HD|4K|Visualizer|Explicit|Clean|Remaster)[^\]]*?\]",
                "",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );

            titulo = System.Text.RegularExpressions.Regex.Replace(
                titulo,
                @"\s*-?\s*(Official Audio|Official Video|Music Video|Official Music Video|Lyrics|Lyric Video|Official Lyric Video|Audio|Video|Visualizer|Explicit|Clean|Remastered)$",
                "",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );

            return titulo.Trim();
        }

        // ABRIR CARPETA Y CARGAR MP3
        private void btnAbrirCarpeta_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    CargarCancionesDeCarpeta(fbd.SelectedPath);
                }
            }
        }

        private void CargarCancionesDeCarpeta(string carpeta)
        {
            dgvCanciones.Rows.Clear();

            var archivos = Directory.GetFiles(carpeta, "*.mp3");

            foreach (var ruta in archivos)
            {
                AgregarCancionAlGrid(ruta);
            }

            if (dgvCanciones.Rows.Count > 0)
            {
                dgvCanciones.Rows[0].Selected = true;
                indiceActual = 0;
            }
        }

        // REPRODUCIR SELECCIONADO
        private async void dgvCanciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                indiceActual = e.RowIndex;
                await ReproducirCancion(indiceActual);
            }
        }

        private async Task ReproducirCancion(int index)
        {
            if (index < 0 || index >= dgvCanciones.Rows.Count)
                return;

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
        }

        // CARGAR COVER - MEJORADO
        private async Task CargarCover(string ruta, string artista, string titulo)
        {
            // Liberar imagen anterior
            if (pbCover.Image != null)
            {
                pbCover.Image.Dispose();
                pbCover.Image = null;
            }

            try
            {
                var mp3 = TagFile.Create(ruta);

                // Intentar cargar desde metadata primero
                if (mp3.Tag.Pictures.Length > 0)
                {
                    var bin = mp3.Tag.Pictures[0].Data.Data;
                    using (var ms = new MemoryStream(bin))
                    {
                        pbCover.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    // Buscar en internet con el nuevo buscador
                    await BuscarYDescargarCover(ruta, artista, titulo);
                }
            }
            catch
            {
                pbCover.Image = null;
            }
        }

        // BUSCAR Y DESCARGAR COVER - VERSIÓN MEJORADA
        private async Task BuscarYDescargarCover(string ruta, string artista, string titulo)
        {
            try
            {
                pbCover.Text = "Buscando portada...";
                pbCover.Refresh();

                // Usar el nuevo buscador
                var resultado = await _coverSearcher.BuscarMejorCover(artista, titulo);

                if (resultado != null && resultado.Similitud > 0.5)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.Timeout = TimeSpan.FromSeconds(10);

                        try
                        {
                            byte[] imgBytes = await client.GetByteArrayAsync(resultado.Url);

                            using (var ms = new MemoryStream(imgBytes))
                            {
                                pbCover.Image = Image.FromStream(ms);
                            }

                            // Guardar cover en el MP3
                            await GuardarCoverEnMP3(ruta, imgBytes);
                        }
                        catch
                        {
                            pbCover.Text = "Error descargando";
                        }
                    }
                }
                else
                {
                    pbCover.Text = "No encontrado";
                }
            }
            catch
            {
                pbCover.Image = null;
            }
        }

        // GUARDAR COVER EN MP3
        private async Task GuardarCoverEnMP3(string ruta, byte[] imagenBytes)
        {
            await Task.Run(() =>
            {
                try
                {
                    var mp3 = TagFile.Create(ruta);

                    var picture = new TagLib.Picture(imagenBytes)
                    {
                        Type = TagLib.PictureType.FrontCover,
                        MimeType = "image/png",
                        Description = "Cover"
                    };

                    mp3.Tag.Pictures = new TagLib.IPicture[] { picture };
                    mp3.Save();
                }
                catch
                {
                    // Si falla, no hacer nada (el cover ya se muestra en la interfaz)
                }
            });
        }

        // MÉTODO DE VALIDACIÓN 
        private bool ValidarCancionesCargadas()
        {
            if (dgvCanciones.Rows.Count == 0)
            {
                MessageBox.Show("Por favor, selecciona una carpeta primero.",
                                "Sin canciones",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        // BOTONES PLAY PAUSE STOP
        private async void btnPlay_Click(object sender, EventArgs e)
        {
            if (!ValidarCancionesCargadas()) return;
            if (outputDevice == null || audioFile == null)
            {
                if (dgvCanciones.SelectedRows.Count > 0)
                {
                    indiceActual = dgvCanciones.SelectedRows[0].Index;
                    await ReproducirCancion(indiceActual);
                }
                return;
            }

            outputDevice.Play();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (!ValidarCancionesCargadas()) return;
            outputDevice?.Pause();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (!ValidarCancionesCargadas()) return;
            DetenerCancion();
        }

        // BOTÓN ALEATORIO (SHUFFLE)
        private void btnAleatorio_Click(object sender, EventArgs e)
        {
            modoAleatorio = !modoAleatorio;

            if (modoAleatorio)
            {
                btnAleatorio.BackColor = Color.Green;
                btnAleatorio.Text = "🔀 ON";
            }
            else
            {
                btnAleatorio.BackColor = SystemColors.Control;
                btnAleatorio.Text = "🔀 Aleatorio";
            }
        }

        private void DetenerCancion()
        {
            if (outputDevice != null)
            {
                outputDevice.Stop();
                outputDevice.Dispose();
                outputDevice = null;
            }

            if (audioFile != null)
            {
                audioFile.Dispose();
                audioFile = null;
            }

            tbProgreso.Value = 0;
            lblTiempoActual.Text = "00:00";
        }

        // CAMBIAR CANCIÓN (MODIFICADO)
        private async Task CambiarCancion(int direccion)
        {
            if (dgvCanciones.Rows.Count == 0) return;

            if (modoRepetir)
            {
                await ReproducirCancion(indiceActual);
                return;
            }

            if (modoAleatorio)
            {
                int nuevoIndice;
                do
                {
                    nuevoIndice = random.Next(0, dgvCanciones.Rows.Count);
                } while (nuevoIndice == indiceActual && dgvCanciones.Rows.Count > 1);

                indiceActual = nuevoIndice;
            }
            else
            {
                indiceActual += direccion;

                if (indiceActual >= dgvCanciones.Rows.Count)
                    indiceActual = 0;
                else if (indiceActual < 0)
                    indiceActual = dgvCanciones.Rows.Count - 1;
            }

            dgvCanciones.Rows[indiceActual].Selected = true;
            dgvCanciones.CurrentCell = dgvCanciones.Rows[indiceActual].Cells[0];

            await ReproducirCancion(indiceActual);
        }

        // BOTÓN REPETIR UNA CANCIÓN
        private void btnRepetir_Click(object sender, EventArgs e)
        {
            modoRepetir = !modoRepetir;

            if (modoRepetir)
            {
                btnRepetir.BackColor = Color.Orange;
                btnRepetir.Text = "🔂 ON";
            }
            else
            {
                btnRepetir.BackColor = SystemColors.Control;
                btnRepetir.Text = "🔁 Repetir";
            }
        }

        // ELIMINAR CANCIÓN SELECCIONADA
        private void btnEliminarCancion_Click(object sender, EventArgs e)
        {
            if (!ValidarCancionesCargadas()) return;

            if (dgvCanciones.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona una canción primero.");
                return;
            }

            int indiceEliminar = dgvCanciones.SelectedRows[0].Index;
            string titulo = dgvCanciones.Rows[indiceEliminar].Cells["Titulo"].Value.ToString();
            string artista = dgvCanciones.Rows[indiceEliminar].Cells["Artista"].Value.ToString();

            var resultado = MessageBox.Show(
                $"¿Eliminar de la lista?\n\n{artista} - {titulo}\n\n(El archivo no se eliminará del disco)",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                if (indiceActual == indiceEliminar)
                {
                    DetenerCancion();
                    indiceActual = -1;
                }

                dgvCanciones.Rows.RemoveAt(indiceEliminar);

                if (indiceActual > indiceEliminar)
                {
                    indiceActual--;
                }

                if (dgvCanciones.Rows.Count > 0)
                {
                    int nuevoIndice = Math.Min(indiceEliminar, dgvCanciones.Rows.Count - 1);
                    dgvCanciones.Rows[nuevoIndice].Selected = true;
                }
            }
        }

        // EDITAR TAGS DE CANCIÓN
        private async void btnEditarTags_Click(object sender, EventArgs e)
        {
            if (!ValidarCancionesCargadas()) return;

            if (dgvCanciones.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona una canción primero.");
                return;
            }

            int indice = dgvCanciones.SelectedRows[0].Index;
            string ruta = dgvCanciones.Rows[indice].Cells["Ruta"].Value.ToString();

            bool estabaReproduciendo = false;
            if (audioFile != null && indice == indiceActual)
            {
                estabaReproduciendo = outputDevice?.PlaybackState == PlaybackState.Playing;
                DetenerCancion();
            }

            try
            {
                var mp3 = TagFile.Create(ruta);

                string tituloActual = mp3.Tag.Title ?? "";
                string artistaActual = mp3.Tag.FirstPerformer ?? "";
                string albumActual = mp3.Tag.Album ?? "";

                using (var formEditor = new FormEditarTags(tituloActual, artistaActual, albumActual))
                {
                    if (formEditor.ShowDialog() == DialogResult.OK)
                    {
                        mp3.Tag.Title = formEditor.Titulo;
                        mp3.Tag.Performers = new[] { formEditor.Artista };
                        mp3.Tag.Album = formEditor.Album;

                        mp3.Save();

                        dgvCanciones.Rows[indice].Cells["Titulo"].Value = formEditor.Titulo;
                        dgvCanciones.Rows[indice].Cells["Artista"].Value = formEditor.Artista;
                        dgvCanciones.Rows[indice].Cells["Album"].Value = formEditor.Album;

                        MessageBox.Show("Tags actualizados correctamente.");
                    }
                }

                if (estabaReproduciendo)
                {
                    await ReproducirCancion(indiceActual);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar tags: {ex.Message}");
            }
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            if (!ValidarCancionesCargadas()) return;
            await CambiarCancion(1);
        }

        private async void btnPrev_Click(object sender, EventArgs e)
        {
            if (!ValidarCancionesCargadas()) return;
            await CambiarCancion(-1);
        }

        // TIMER PARA ACTUALIZAR PROGRESO
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (audioFile != null && outputDevice != null && !arrastrandoProgreso)
            {
                tbProgreso.Value = Math.Min(tbProgreso.Maximum, (int)audioFile.CurrentTime.TotalSeconds);
                lblTiempoActual.Text = audioFile.CurrentTime.ToString(@"mm\:ss");
            }

            // Detectar cuando termina la canción
            if (audioFile != null && outputDevice != null)
            {
                if (audioFile.TotalTime.TotalSeconds - audioFile.CurrentTime.TotalSeconds <= 1.0)
                {
                    if (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        _ = Task.Run(async () =>
                        {
                            await Task.Delay(100);

                            this.Invoke((MethodInvoker)async delegate
                            {
                                await CambiarCancion(1);
                            });
                        });

                        outputDevice.Pause();
                    }
                }
            }
        }

        // TRACKBAR VOLUMEN
        private void tbVolumen_Scroll(object sender, EventArgs e)
        {
            if (outputDevice != null)
            {
                outputDevice.Volume = tbVolumen.Value / 100f;
            }
        }

        // TRACKBAR PROGRESO
        private void tbProgreso_MouseDown(object sender, MouseEventArgs e)
        {
            arrastrandoProgreso = true;
        }

        private void tbProgreso_MouseUp(object sender, MouseEventArgs e)
        {
            if (audioFile != null)
            {
                audioFile.CurrentTime = TimeSpan.FromSeconds(tbProgreso.Value);
            }

            arrastrandoProgreso = false;
        }

        // GUARDAR PLAYLIST TXT
        private void btnGuardarPlaylist_Click(object sender, EventArgs e)
        {
            if (dgvCanciones.Rows.Count == 0)
            {
                MessageBox.Show("No hay canciones cargadas.");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Playlist (*.txt)|*.txt";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var rutas = dgvCanciones.Rows
                        .Cast<DataGridViewRow>()
                        .Select(row => row.Cells["Ruta"].Value.ToString());

                    IOFile.WriteAllLines(sfd.FileName, rutas);
                    MessageBox.Show("Playlist guardada correctamente.");
                }
            }
        }

        // CARGAR PLAYLIST TXT
        private void btnCargarPlaylist_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Playlist (*.txt)|*.txt";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    CargarPlaylist(ofd.FileName);
                }
            }
        }

        private void CargarPlaylist(string archivo)
        {
            dgvCanciones.Rows.Clear();

            string[] rutas = IOFile.ReadAllLines(archivo);

            foreach (string ruta in rutas)
            {
                if (IOFile.Exists(ruta))
                {
                    AgregarCancionAlGrid(ruta);
                }
            }

            if (dgvCanciones.Rows.Count > 0)
            {
                indiceActual = 0;
                dgvCanciones.Rows[0].Selected = true;
            }

            MessageBox.Show("Playlist cargada correctamente.");
        }

        // ABRIR EN WINDOWS MEDIA PLAYER
        private void btnAbrirWMP_Click(object sender, EventArgs e)
        {
            if (dgvCanciones.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona una canción primero.");
                return;
            }

            string ruta = dgvCanciones.SelectedRows[0].Cells["Ruta"].Value.ToString();

            try
            {
                System.Diagnostics.Process.Start("wmplayer.exe", $"\"{ruta}\"");
                outputDevice?.Pause();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error abriendo WMP: " + ex.Message);
            }
        }

        // BUSCAR LETRA API lyrics.ovh
        private async void btnLetra_Click(object sender, EventArgs e)
        {
            if (dgvCanciones.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona una canción primero.");
                return;
            }

            string artista = dgvCanciones.SelectedRows[0].Cells["Artista"].Value.ToString();
            string titulo = dgvCanciones.SelectedRows[0].Cells["Titulo"].Value.ToString();

            artista = ExtraerArtistaPrincipal(artista);
            artista = _coverSearcher.NormalizarTexto(artista);
            titulo = _coverSearcher.NormalizarTexto(titulo);

            if (string.IsNullOrWhiteSpace(artista) || string.IsNullOrWhiteSpace(titulo))
            {
                MessageBox.Show("No se puede buscar letra sin artista y título.");
                return;
            }

            rtbLyrics.Text = "Buscando letra...";
            btnLetra.Enabled = false;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(25);

                    string url = $"https://api.lyrics.ovh/v1/{Uri.EscapeDataString(artista)}/{Uri.EscapeDataString(titulo)}";

                    var response = await client.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        rtbLyrics.Text = $"No se encontró letra.\n\nArtista buscado: {artista}\nCanción: {titulo}\n\nIntenta buscar manualmente o verifica los metadatos del MP3.";
                        return;
                    }

                    string json = await response.Content.ReadAsStringAsync();

                    using (JsonDocument doc = JsonDocument.Parse(json))
                    {
                        if (doc.RootElement.TryGetProperty("lyrics", out JsonElement lyricsElement))
                        {
                            string lyrics = lyricsElement.GetString();

                            if (string.IsNullOrWhiteSpace(lyrics))
                                rtbLyrics.Text = "No se encontró letra.";
                            else
                                rtbLyrics.Text = lyrics;
                        }
                        else
                        {
                            rtbLyrics.Text = "No se encontró letra.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                rtbLyrics.Text = $"Error buscando letra: {ex.Message}\n\nArtista: {artista}\nCanción: {titulo}";
            }
            finally
            {
                btnLetra.Enabled = true;
            }
        }

        // EXTRAER ARTISTA PRINCIPAL
        private string ExtraerArtistaPrincipal(string artista)
        {
            if (string.IsNullOrWhiteSpace(artista))
                return "";

            string[] separadores = { " feat.", " feat ", " ft.", " ft ", " featuring ", " & ", " and ", "," };

            string resultado = artista;

            foreach (var sep in separadores)
            {
                int index = resultado.IndexOf(sep, StringComparison.OrdinalIgnoreCase);
                if (index > 0)
                {
                    resultado = resultado.Substring(0, index);
                    break;
                }
            }

            return resultado.Trim();
        }
    }
}