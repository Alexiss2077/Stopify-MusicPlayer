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
//////////////////////////////////ejh
///
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

        // VARIABLES PARA DRRAAG  DROP
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        private Rectangle dragBoxFromMouseDown;
        private int dragRowIndex = -1;

        public Form1()
        {
            InitializeComponent();
            ConfigurarGrid();

            timer1.Interval = 500;
            timer1.Start();

            tbVolumen.Value = 50;

            // HABILITAR DRAG & DROP
            ConfigurarDragDrop();
        }



        /// //////////////////////////////////////////////
        // CONFIGURAR DRAG  DROP
        ////// EXTRA PENDIENTEEEEEEEE /\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        private void ConfigurarDragDrop()
        {
            dgvCanciones.AllowDrop = true;
            dgvCanciones.MouseDown += dgvCanciones_MouseDown;
            dgvCanciones.MouseMove += dgvCanciones_MouseMove;
            dgvCanciones.DragOver += dgvCanciones_DragOver;
            dgvCanciones.DragDrop += dgvCanciones_DragDrop;
            dgvCanciones.Paint += dgvCanciones_Paint;  
        }

        ///  //////////////////////////////////////////////////////////////////
        ///  

        // EVENTOS DE DRAG & DROP
        private void dgvCanciones_MouseDown(object sender, MouseEventArgs e)
        {
            // Obtener el índice de la fila donde se hizo clic
            rowIndexFromMouseDown = dgvCanciones.HitTest(e.X, e.Y).RowIndex;

            if (rowIndexFromMouseDown != -1)
            {
                // Obtener el tamaño del rectángulo de arrastre
                Size dragSize = SystemInformation.DragSize;

                // Crear un rectángulo alrededor del punto donde se hizo clic
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
                // Si el mouse se mueve fuera del rectángulo de arrastre, iniciar el drag & drop
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    if (rowIndexFromMouseDown != -1)
                    {
                        // Iniciar operación de arrastre
                        DataGridViewRow rowToMove = dgvCanciones.Rows[rowIndexFromMouseDown];
                        dgvCanciones.DoDragDrop(rowToMove, DragDropEffects.Move);
                    }
                }
            }
        }

        private void dgvCanciones_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;

            // Obtener el punto del mouse
            Point clientPoint = dgvCanciones.PointToClient(new Point(e.X, e.Y));
            dragRowIndex = dgvCanciones.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // Redibujar para mostrar el indicador
            dgvCanciones.Invalidate();
        }

        private void dgvCanciones_Paint(object sender, PaintEventArgs e)
        {
            if (dragRowIndex >= 0 && dragRowIndex < dgvCanciones.Rows.Count)
            {
                // Dibujar una línea donde se insertará la canción
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
            // Obtener el punto donde se soltó
            Point clientPoint = dgvCanciones.PointToClient(new Point(e.X, e.Y));

            // Obtener el índice de la fila donde se soltó
            rowIndexOfItemUnderMouseToDrop = dgvCanciones.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // Si se soltó en una fila válida y no es la misma fila
            if (e.Effect == DragDropEffects.Move && rowIndexOfItemUnderMouseToDrop != -1)
            {
                DataGridViewRow rowToMove = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;

                if (rowToMove != null && rowIndexFromMouseDown != rowIndexOfItemUnderMouseToDrop)
                {
                    // Guardar los datos de la fila
                    string titulo = rowToMove.Cells["Titulo"].Value.ToString();
                    string artista = rowToMove.Cells["Artista"].Value.ToString();
                    string album = rowToMove.Cells["Album"].Value.ToString();
                    string duracion = rowToMove.Cells["Duracion"].Value.ToString();
                    string ruta = rowToMove.Cells["Ruta"].Value.ToString();

                    // Eliminar la fila de su posición original
                    dgvCanciones.Rows.RemoveAt(rowIndexFromMouseDown);

                    // Ajustar el índice de destino si es necesario
                    int targetIndex = rowIndexOfItemUnderMouseToDrop;
                    if (rowIndexFromMouseDown < rowIndexOfItemUnderMouseToDrop)
                    {
                        targetIndex--;
                    }

                    // Insertar la fila en la nueva posición
                    dgvCanciones.Rows.Insert(targetIndex, titulo, artista, album, duracion, ruta);

                    // Seleccionar la fila movida
                    dgvCanciones.Rows[targetIndex].Selected = true;
                    dgvCanciones.CurrentCell = dgvCanciones.Rows[targetIndex].Cells[0];

                    // Actualizar índice actual si es necesario
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


        /// /////////////////////////////////////////////////////////////
        /// 
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



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


            // PERSONALIZAR ENCABEZADOS   ///// CAMBIOS PENDIENTEEEEE
            

            // Estilo del encabezado 
            dgvCanciones.ColumnHeadersDefaultCellStyle.BackColor = Color.SlateGray;
            dgvCanciones.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCanciones.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvCanciones.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvCanciones.EnableHeadersVisualStyles = false; 

            // Estilo de las filas 
            dgvCanciones.DefaultCellStyle.BackColor = Color.White;
            dgvCanciones.DefaultCellStyle.ForeColor = Color.Black;
            dgvCanciones.DefaultCellStyle.SelectionBackColor = Color.LimeGreen;  // color de la cancion q se esta sonando
            dgvCanciones.DefaultCellStyle.SelectionForeColor = Color.White;

            // Filas alternadas
            dgvCanciones.AlternatingRowsDefaultCellStyle.BackColor = Color.DarkSlateGray;

            // Bordes
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


                //LIMPIAR ARTISTA Y TÍTULO
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

            // Eliminar " - Topic", "VEVO", etc.
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

            // Eliminar paréntesis con contenido al final
            titulo = System.Text.RegularExpressions.Regex.Replace(
                titulo,
                @"\s*\([^)]*?(Official|Audio|Video|Lyrics|Lyric|Music Video|HD|4K|Visualizer|Explicit|Clean|Remaster)[^)]*?\)",
                "",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );

            // Eliminar corchetes con contenido al final
            titulo = System.Text.RegularExpressions.Regex.Replace(
                titulo,
                @"\s*\[[^\]]*?(Official|Audio|Video|Lyrics|Lyric|Music Video|HD|4K|Visualizer|Explicit|Clean|Remaster)[^\]]*?\]",
                "",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );

            // Eliminar texto suelto al final sin paréntesis
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

        // CARGAR COVER
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
                    client.Timeout = TimeSpan.FromSeconds(10);

                    // Normalizar artista y título
                    string artistaLimpio = ExtraerArtistaPrincipal(artista);
                    artistaLimpio = NormalizarTexto(artistaLimpio);
                    string tituloLimpio = NormalizarTexto(titulo);

                    // Buscar en iTunes
                    string query = Uri.EscapeDataString($"{artistaLimpio} {tituloLimpio}");
                    string url = $"https://itunes.apple.com/search?term={query}&limit=10&entity=song";

                    string json = await client.GetStringAsync(url);

                    using (JsonDocument doc = JsonDocument.Parse(json))
                    {
                        var results = doc.RootElement.GetProperty("results");

                        if (results.GetArrayLength() == 0)
                        {
                            pbCover.Image = null;
                            return;
                        }

                        // Buscar la mejor coincidencia
                        double mejorScore = 0;
                        string mejorCoverUrl = null;

                        for (int i = 0; i < results.GetArrayLength(); i++)
                        {
                            var item = results[i];

                            string artistaAPI = NormalizarTexto(item.GetProperty("artistName").GetString());
                            string tituloAPI = NormalizarTexto(item.GetProperty("trackName").GetString());

                            // Calcular similitud
                            double scoreArtista = CalcularSimilitud(artistaLimpio, artistaAPI);
                            double scoreTitulo = CalcularSimilitud(tituloLimpio, tituloAPI);

                            // combinado 
                            double scoreTotal = (scoreTitulo * 0.7) + (scoreArtista * 0.3);

                            if (scoreTotal > mejorScore)
                            {
                                mejorScore = scoreTotal;
                                mejorCoverUrl = item.GetProperty("artworkUrl100").GetString();
                            }
                        }

                        // mbral moderado: 60%
                        if (mejorScore >= 0.60 && mejorCoverUrl != null)
                        {
                            string coverUrl = mejorCoverUrl.Replace("100x100", "600x600");

                            byte[] imgBytes = await client.GetByteArrayAsync(coverUrl);

                            using (var ms = new MemoryStream(imgBytes))
                            {
                                pbCover.Image = Image.FromStream(ms);
                            }
                        }
                        else
                        {
                            pbCover.Image = null;
                        }
                    }
                }
            }
            catch
            {
                pbCover.Image = null;
            }
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

        // NEXT / PREv
        private async Task CambiarCancion(int direccion)
        {
            if (dgvCanciones.Rows.Count == 0) return;

            indiceActual += direccion;

            // Circular
            if (indiceActual >= dgvCanciones.Rows.Count)
                indiceActual = 0;
            else if (indiceActual < 0)
                indiceActual = dgvCanciones.Rows.Count - 1;

            dgvCanciones.Rows[indiceActual].Selected = true;

            await ReproducirCancion(indiceActual);
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            if (!ValidarCancionesCargadas()) return;
            await CambiarCancion(1);
        }

        private async void btnPrev_Click(object sender, EventArgs e)
        {
            if (!ValidarCancionesCargadas()) return;
            await CambiarCancion(-1);   // Cambiar a la canción anterior
        }



        // TIMER PARA ACTUALIZAR PROGRESO
        private async void timer1_Tick(object sender, EventArgs e)
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
                    await CambiarCancion(1);
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

       
        
        // GUARDAR playlist TXT
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




        // CARGAR playlist TXT
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
               // System.Diagnostics.Process.
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error abriendo WMP: " + ex.Message);
            }
        }

        // EXTRAER ARTISTA PRINCIPAL para MEJORAR BÚSQUEDAS DE COVER 
        private string ExtraerArtistaPrincipal(string artista)
        {
            if (string.IsNullOrWhiteSpace(artista))
                return "";

            // Separadores comunes de featurings
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



        // NORMALIZAR TEXTO
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

        // CALCULAR SIMILITUD ENTRE TEXTOS
        private double CalcularSimilitud(string texto1, string texto2)//////////////////////8362bf¡¡'//+_.;1|.
        {
            if (string.IsNullOrEmpty(texto1) || string.IsNullOrEmpty(texto2))
                return 0;

            // Si son idénticos
            if (texto1 == texto2)
                return 1.0;

            // Dividir en palabras
            var palabras1 = texto1.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var palabras2 = texto2.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // Contar palabras en común
            int palabrasComunes = palabras1.Intersect(palabras2).Count();
            int totalPalabras = Math.Max(palabras1.Length, palabras2.Length);

            if (totalPalabras == 0)
                return 0;

            double similitudPalabras = (double)palabrasComunes / totalPalabras;

            // Calcular similitud de Levenshtein
            int distancia = DistanciaLevenshtein(texto1, texto2);
            int maxLongitud = Math.Max(texto1.Length, texto2.Length);
            double similitudCaracteres = 1.0 - ((double)distancia / maxLongitud);

            // Combinar ambas métricas
            return (similitudPalabras * 0.6) + (similitudCaracteres * 0.4);
        }



        // DISTANCIA LEVENSHTEIN
        private int DistanciaLevenshtein(string s1, string s2)
        {
            int[,] d = new int[s1.Length + 1, s2.Length + 1];

            for (int i = 0; i <= s1.Length; i++)
                d[i, 0] = i;

            for (int j = 0; j <= s2.Length; j++)
                d[0, j] = j;

            for (int j = 1; j <= s2.Length; j++)
            {
                for (int i = 1; i <= s1.Length; i++)
                {
                    int cost = (s1[i - 1] == s2[j - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost
                    );
                }
            }

            return d[s1.Length, s2.Length];
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

            artista = NormalizarTexto(artista);
            titulo = NormalizarTexto(titulo);

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
                    client.Timeout = TimeSpan.FromSeconds(10);

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
    }
}