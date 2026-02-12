using System;
using System.Windows.Forms;

namespace Stopify
{
    public partial class FormEditarTags : Form
    {
        public string Titulo { get; private set; }
        public string Artista { get; private set; }
        public string Album { get; private set; }

        private TextBox txtTitulo;
        private TextBox txtArtista;
        private TextBox txtAlbum;
        private Button btnGuardar;
        private Button btnCancelar;

        public FormEditarTags(string titulo, string artista, string album)
        {
            InitializeComponent();

            this.Text = "Editar Tags";
            this.Size = new System.Drawing.Size(400, 220);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Labels y TextBoxes
            int y = 20;

            var lblTitulo = new Label { Text = "Título:", Location = new System.Drawing.Point(20, y), AutoSize = true };
            txtTitulo = new TextBox { Location = new System.Drawing.Point(100, y), Size = new System.Drawing.Size(260, 20), Text = titulo };

            y += 40;
            var lblArtista = new Label { Text = "Artista:", Location = new System.Drawing.Point(20, y), AutoSize = true };
            txtArtista = new TextBox { Location = new System.Drawing.Point(100, y), Size = new System.Drawing.Size(260, 20), Text = artista };

            y += 40;
            var lblAlbum = new Label { Text = "Álbum:", Location = new System.Drawing.Point(20, y), AutoSize = true };
            txtAlbum = new TextBox { Location = new System.Drawing.Point(100, y), Size = new System.Drawing.Size(260, 20), Text = album };

            // Botones
            btnGuardar = new Button
            {
                Text = "Guardar",
                Location = new System.Drawing.Point(200, 140),
                Size = new System.Drawing.Size(80, 30),
                DialogResult = DialogResult.OK
            };
            btnGuardar.Click += BtnGuardar_Click;

            btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new System.Drawing.Point(290, 140),
                Size = new System.Drawing.Size(80, 30),
                DialogResult = DialogResult.Cancel
            };

            // Agregar controles
            Controls.Add(lblTitulo);
            Controls.Add(txtTitulo);
            Controls.Add(lblArtista);
            Controls.Add(txtArtista);
            Controls.Add(lblAlbum);
            Controls.Add(txtAlbum);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);

            AcceptButton = btnGuardar;
            CancelButton = btnCancelar;
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                MessageBox.Show("El título no puede estar vacío.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtArtista.Text))
            {
                MessageBox.Show("El artista no puede estar vacío.");
                return;
            }

            Titulo = txtTitulo.Text.Trim();
            Artista = txtArtista.Text.Trim();
            Album = txtAlbum.Text.Trim();
        }
    }
}