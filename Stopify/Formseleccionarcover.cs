using Stopify;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stopify
{
    public partial class FormSeleccionarCover : Form
    {
        public string CoverUrlSeleccionado { get; private set; }
        private List<CoverSearchResult> _covers;
        private HttpClient _httpClient;

        public FormSeleccionarCover(List<CoverSearchResult> covers)
        {
           // InitializeComponent();
            _covers = covers.OrderByDescending(c => c.Similitud).ToList();
            _httpClient = new HttpClient();

            this.Text = "Seleccionar Portada";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            ConstruirUI();
        }

        private void ConstruirUI()
        {
            var panelPrincipal = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = _covers.Count + 2,
                AutoScroll = true,
                Padding = new Padding(10)
            };

            // Encabezado
            var lblTitulo = new Label
            {
                Text = $"Se encontraron {_covers.Count} portadas. Selecciona la mejor:",
                Dock = DockStyle.Top,
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            panelPrincipal.Controls.Add(lblTitulo, 0, 0);
            panelPrincipal.SetColumnSpan(lblTitulo, 3);

            // Mostrar cada cover
            int fila = 1;
            foreach (var cover in _covers)
            {
                try
                {
                    // PictureBox
                    var pb = new PictureBox
                    {
                        Size = new Size(100, 100),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        BorderStyle = BorderStyle.FixedSingle
                    };

                    // Cargar imagen de forma asíncrona
                    var _ = CargarImagenAsync(pb, cover.Url);

                    // Label con información
                    var lblInfo = new Label
                    {
                        Text = $"{cover.Artista}\n{cover.Titulo}\n{cover.Fuente}\nSimilitud: {(cover.Similitud * 100):F1}%",
                        AutoSize = false,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Font = new Font("Segoe UI", 9)
                    };

                    // Botón seleccionar
                    var btnSeleccionar = new Button
                    {
                        Text = "Seleccionar",
                        Size = new Size(100, 30),
                        Tag = cover.Url
                    };
                    btnSeleccionar.Click += (s, e) =>
                    {
                        CoverUrlSeleccionado = (string)btnSeleccionar.Tag;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    };

                    panelPrincipal.Controls.Add(pb, 0, fila);
                    panelPrincipal.Controls.Add(lblInfo, 1, fila);
                    panelPrincipal.Controls.Add(btnSeleccionar, 2, fila);

                    fila++;
                }
                catch { }
            }

            // Botón cancelar
            var btnCancelar = new Button
            {
                Text = "Cancelar",
                Dock = DockStyle.Bottom,
                Size = new Size(100, 40),
                DialogResult = DialogResult.Cancel
            };

            this.Controls.Add(panelPrincipal);
            this.Controls.Add(btnCancelar);
        }

        private async Task CargarImagenAsync(PictureBox pb, string url)
        {
            try
            {
                var bytes = await _httpClient.GetByteArrayAsync(url);
                using (var ms = new System.IO.MemoryStream(bytes))
                {
                    pb.Image = Image.FromStream(ms);
                }
            }
            catch
            {
                pb.Text = "Error";
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _httpClient?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}





/// ///////////////// OPCIONAL, IMPLEMTAR DESPUES?????
///// ///private async Task BuscarYDescargarCover(string ruta, string artista, string titulo)
//{
//    try
//    {
//        pbCover.Text = "Buscando portada...";
//        pbCover.Refresh();

//        // 🆕 Obtener TODOS los resultados (no solo el mejor)
//        var resultados = await _coverSearcher.BuscarTodosLosCovers(artista, titulo);

//        CoverSearchResult seleccionado = null;

//        // Si hay múltiples opciones buenas, dejar que el usuario elija
//        if (resultados != null && resultados.Count > 1)
//        {
//            // 🆕 Abre el formulario de selección
//            using (var formSeleccionar = new FormSeleccionarCover(resultados))
//            {
//                if (formSeleccionar.ShowDialog() == DialogResult.OK)
//                {
//                    seleccionado = resultados.FirstOrDefault(
//                        c => c.Url == formSeleccionar.CoverUrlSeleccionado
//                    );
//                }
//            }
//        }
//        else if (resultados?.Count == 1)
//        {
//            // Si hay solo una opción, usarla directamente
//            seleccionado = resultados[0];
//        }

//        // Descargar el cover seleccionado
//        if (seleccionado != null && seleccionado.Similitud > 0.5)
//        {
//            using (HttpClient client = new HttpClient())
//            {
//                client.Timeout = TimeSpan.FromSeconds(10);

//                try
//                {
//                    byte[] imgBytes = await client.GetByteArrayAsync(seleccionado.Url);

//                    using (var ms = new MemoryStream(imgBytes))
//                    {
//                        pbCover.Image = Image.FromStream(ms);
//                    }

//                    await GuardarCoverEnMP3(ruta, imgBytes);
//                }
//                catch
//                {
//                    pbCover.Text = "Error descargando";
//                }
//            }
//        }
//        else
//        {
//            pbCover.Text = "No encontrado";
//        }
//    }
//    catch
//    {
//        pbCover.Image = null;
//    }
//}


///////////////////////////////// AGREGAR ESTO SI SE VA A PONER LA SELECCION DE COVER
/// <summary>
/// Retorna TODOS los covers encontrados (para que el usuario elija)
/// </summary>
//public async Task<List<CoverSearchResult>> BuscarTodosLosCovers(string artista, string titulo)
//{
//    string artistaLimpio = ExtraerArtistaPrincipal(artista);
//    artistaLimpio = NormalizarTexto(artistaLimpio);
//    string tituloLimpio = NormalizarTexto(titulo);

//    var resultados = new List<CoverSearchResult>();

//    // Buscar en múltiples fuentes
//    var itunesResults = await BuscarEnITunes(artistaLimpio, tituloLimpio, artista, titulo);
//    if (itunesResults != null) resultados.AddRange(itunesResults);

//    var lastFmResults = await BuscarEnLastFm(artistaLimpio, tituloLimpio, artista, titulo);
//    if (lastFmResults != null) resultados.AddRange(lastFmResults);

//    var spotifyResults = await BuscarEnSpotify(artistaLimpio, tituloLimpio, artista, titulo);
//    if (spotifyResults != null) resultados.AddRange(spotifyResults);

//    // Retornar TODOS ordenados por similitud
//    return resultados
//        .OrderByDescending(r => r.Similitud)
//        .ToList();
//}