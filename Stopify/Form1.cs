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

        private List<string> listaCanciones = new List<string>();
        private int indiceActual = -1;

        private bool arrastrandoProgreso = false;

        public Form1()
        {
            InitializeComponent();
            ConfigurarGrid();

            timer1.Interval = 500;
            timer1.Start();

            tbVolumen.Value = 50;
        }

        // ================================
        // CONFIGURAR GRID
        // ================================
        private void ConfigurarGrid()
        {
            dgvCanciones.Columns.Clear();

            dgvCanciones.Columns.Add("Titulo", "Título");
            dgvCanciones.Columns.Add("Artista", "Artista");
            dgvCanciones.Columns.Add("Album", "Álbum");
            dgvCanciones.Columns.Add("Duracion", "Duración");
            dgvCanciones.Columns.Add("Ruta", "Ruta");

            dgvCanciones.Columns["Ruta"].Visible = false;
        }

        // ================================
        // ABRIR CARPETA Y CARGAR MP3
        // ================================
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
            listaCanciones.Clear();

            var archivos = Directory.GetFiles(carpeta, "*.mp3");

            foreach (var ruta in archivos)
            {
                try
                {
                    var mp3 = TagFile.Create(ruta);

                    string titulo = mp3.Tag.Title ?? Path.GetFileNameWithoutExtension(ruta);
                    string artista = mp3.Tag.FirstPerformer ?? "Desconocido";
                    string album = mp3.Tag.Album ?? "Desconocido";
                    string duracion = mp3.Properties.Duration.ToString(@"mm\:ss");

                    dgvCanciones.Rows.Add(titulo, artista, album, duracion, ruta);
                    listaCanciones.Add(ruta);
                }
                catch
                {
                    // Ignorar archivos dañados
                }
            }

            if (dgvCanciones.Rows.Count > 0)
            {
                dgvCanciones.Rows[0].Selected = true;
                indiceActual = 0;
            }
        }

        // ================================
        // REPRODUCIR SELECCIONADO
        // ================================
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

            audioFile = new AudioFileReader(ruta);
            outputDevice = new WaveOutEvent();
            outputDevice.Init(audioFile);

            outputDevice.Volume = tbVolumen.Value / 100f;

            outputDevice.Play();

            tbProgreso.Maximum = (int)audioFile.TotalTime.TotalSeconds;

            lblTiempoActual.Text = "00:00";
            lblTiempoTotal.Text = audioFile.TotalTime.ToString(@"mm\:ss");

            rtbLyrics.Clear();

            await CargarCover(ruta, artista, titulo);
        }

        // ================================
        // CARGAR COVER
        // ================================
        private async Task CargarCover(string ruta, string artista, string titulo)
        {
            pbCover.Image = null;

            try
            {
                var mp3 = TagFile.Create(ruta);

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
                    await DescargarCoverITunes(artista, titulo);
                }
            }
            catch
            {
                pbCover.Image = null;
            }
        }

        private async Task DescargarCoverITunes(string artista, string titulo)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string query = Uri.EscapeDataString($"{artista} {titulo}");
                    string url = $"https://itunes.apple.com/search?term={query}&limit=1";

                    string json = await client.GetStringAsync(url);

                    using (JsonDocument doc = JsonDocument.Parse(json))
                    {
                        var results = doc.RootElement.GetProperty("results");

                        if (results.GetArrayLength() > 0)
                        {
                            string coverUrl = results[0].GetProperty("artworkUrl100").GetString();

                            coverUrl = coverUrl.Replace("100x100", "500x500");

                            byte[] imgBytes = await client.GetByteArrayAsync(coverUrl);

                            using (var ms = new MemoryStream(imgBytes))
                            {
                                pbCover.Image = Image.FromStream(ms);
                            }
                        }
                    }
                }
            }
            catch
            {
                pbCover.Image = null;
            }
        }

        // ================================
        // BOTONES PLAY PAUSE STOP
        // ================================
        private async void btnPlay_Click(object sender, EventArgs e)
        {
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
            outputDevice?.Pause();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            DetenerCancion();
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

        // ================================
        // NEXT / PREV
        // ================================
        private async void btnNext_Click(object sender, EventArgs e)
        {
            if (dgvCanciones.Rows.Count == 0) return;

            indiceActual++;

            if (indiceActual >= dgvCanciones.Rows.Count)
                indiceActual = 0;

            dgvCanciones.Rows[indiceActual].Selected = true;

            await ReproducirCancion(indiceActual);
        }

        private async void btnPrev_Click(object sender, EventArgs e)
        {
            if (dgvCanciones.Rows.Count == 0) return;

            indiceActual--;

            if (indiceActual < 0)
                indiceActual = dgvCanciones.Rows.Count - 1;

            dgvCanciones.Rows[indiceActual].Selected = true;

            await ReproducirCancion(indiceActual);
        }

        // ================================
        // TIMER PARA ACTUALIZAR PROGRESO
        // ================================
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (audioFile != null && outputDevice != null && !arrastrandoProgreso)
            {
                tbProgreso.Value = Math.Min(tbProgreso.Maximum, (int)audioFile.CurrentTime.TotalSeconds);
                lblTiempoActual.Text = audioFile.CurrentTime.ToString(@"mm\:ss");
            }

            if (audioFile != null && outputDevice != null)
            {
                if (audioFile.CurrentTime >= audioFile.TotalTime)
                {
                    btnNext.PerformClick();
                }
            }
        }

        // ================================
        // TRACKBAR VOLUMEN
        // ================================
        private void tbVolumen_Scroll(object sender, EventArgs e)
        {
            if (outputDevice != null)
            {
                outputDevice.Volume = tbVolumen.Value / 100f;
            }
        }

        // ================================
        // TRACKBAR PROGRESO (SEEK)
        // ================================
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

        // ================================
        // GUARDAR PLAYLIST TXT
        // ================================
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
                    List<string> rutas = new List<string>();

                    foreach (DataGridViewRow row in dgvCanciones.Rows)
                    {
                        rutas.Add(row.Cells["Ruta"].Value.ToString());
                    }

                    IOFile.WriteAllLines(sfd.FileName, rutas);
                    MessageBox.Show("Playlist guardada correctamente.");
                }
            }
        }

        // ================================
        // CARGAR PLAYLIST TXT
        // ================================
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
            listaCanciones.Clear();

            string[] rutas = IOFile.ReadAllLines(archivo);

            foreach (string ruta in rutas)
            {
                if (!IOFile.Exists(ruta))
                    continue;

                try
                {
                    var mp3 = TagFile.Create(ruta);

                    string titulo = mp3.Tag.Title ?? Path.GetFileNameWithoutExtension(ruta);
                    string artista = mp3.Tag.FirstPerformer ?? "Desconocido";
                    string album = mp3.Tag.Album ?? "Desconocido";
                    string duracion = mp3.Properties.Duration.ToString(@"mm\:ss");

                    dgvCanciones.Rows.Add(titulo, artista, album, duracion, ruta);
                    listaCanciones.Add(ruta);
                }
                catch
                {
                    // ignorar
                }
            }

            if (dgvCanciones.Rows.Count > 0)
            {
                indiceActual = 0;
                dgvCanciones.Rows[0].Selected = true;
            }

            MessageBox.Show("Playlist cargada correctamente.");
        }

        // ================================
        // ABRIR EN WINDOWS MEDIA PLAYER
        // ================================
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error abriendo WMP: " + ex.Message);
            }
        }

        // ================================
        // NORMALIZAR TEXTO (IMPORTANTE)
        // ================================
        private string NormalizarTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "";

            texto = texto.Replace("\r", "").Replace("\n", "").Trim().ToLower();

            // quitar acentos
            string normalized = texto.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            texto = sb.ToString().Normalize(NormalizationForm.FormC);

            // quitar texto basura
            string[] basura =
            {
                "feat.", "ft.", "featuring", "remix", "official", "video", "audio",
                "lyrics", "letra", "hq", "4k", "prod.", "producer", "album version",
                "version", "edit", "extended", "radio edit"
            };

            foreach (var b in basura)
            {
                int index = texto.IndexOf(b);
                if (index != -1)
                    texto = texto.Substring(0, index);
            }

            // quitar paréntesis
            while (texto.Contains("(") && texto.Contains(")"))
            {
                int a = texto.IndexOf("(");
                int b = texto.IndexOf(")");

                if (b > a)
                    texto = texto.Remove(a, b - a + 1);
                else break;
            }

            // quitar corchetes
            while (texto.Contains("[") && texto.Contains("]"))
            {
                int a = texto.IndexOf("[");
                int b = texto.IndexOf("]");

                if (b > a)
                    texto = texto.Remove(a, b - a + 1);
                else break;
            }

            // quitar símbolos comunes
            texto = texto.Replace("&", "and");
            texto = texto.Replace("'", "");
            texto = texto.Replace("\"", "");
            texto = texto.Replace(".", "");
            texto = texto.Replace(",", "");
            texto = texto.Replace(":", "");
            texto = texto.Replace(";", "");
            texto = texto.Replace("-", " ");

            // quitar doble espacios
            while (texto.Contains("  "))
                texto = texto.Replace("  ", " ");

            return texto.Trim();
        }

        // ================================
        // BUSCAR LETRA API lyrics.ovh
        // ================================
        private async void btnLetra_Click(object sender, EventArgs e)
        {
            if (dgvCanciones.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona una canción primero.");
                return;
            }

            string artista = dgvCanciones.SelectedRows[0].Cells["Artista"].Value.ToString();
            string titulo = dgvCanciones.SelectedRows[0].Cells["Titulo"].Value.ToString();

            artista = NormalizarTexto(artista);
            titulo = NormalizarTexto(titulo);

            if (string.IsNullOrWhiteSpace(artista) || string.IsNullOrWhiteSpace(titulo))
            {
                MessageBox.Show("No se puede buscar letra sin artista y título.");
                return;
            }

            rtbLyrics.Text = "🔎 Buscando letra...";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = $"https://api.lyrics.ovh/v1/{Uri.EscapeDataString(artista)}/{Uri.EscapeDataString(titulo)}";

                    var response = await client.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        rtbLyrics.Text = $"❌ No se encontró letra.\n\nArtista: {artista}\nCanción: {titulo}";
                        return;
                    }

                    string json = await response.Content.ReadAsStringAsync();

                    using (JsonDocument doc = JsonDocument.Parse(json))
                    {
                        if (doc.RootElement.TryGetProperty("lyrics", out JsonElement lyricsElement))
                        {
                            string lyrics = lyricsElement.GetString();

                            if (string.IsNullOrWhiteSpace(lyrics))
                                rtbLyrics.Text = "❌ No se encontró letra.";
                            else
                                rtbLyrics.Text = lyrics;
                        }
                        else
                        {
                            rtbLyrics.Text = "❌ No se encontró letra.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error buscando letra: " + ex.Message);
            }
        }
    }
}

