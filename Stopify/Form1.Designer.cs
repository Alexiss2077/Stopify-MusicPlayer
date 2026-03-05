namespace Stopify
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.DataGridView dgvCanciones;

        // Todos los botones ahora son CustomButton
        private CustomButton btnAbrirCarpeta;
        private CustomButton btnPlayPause;
        private CustomButton btnStop;
        private CustomButton btnPrev;
        private CustomButton btnNext;
        private CustomButton btnAleatorio;
        private CustomButton btnRepetir;
        private CustomButton btnGuardarPlaylist;
        private CustomButton btnCargarPlaylist;
        private CustomButton btnAbrirWMP;
        private CustomButton btnLetra;
        private CustomButton btnEliminarCancion;
        private CustomButton btnEditarTags;

        private System.Windows.Forms.PictureBox pbCover;
        private System.Windows.Forms.RichTextBox rtbLyrics;
        private System.Windows.Forms.Label lblCanciones;
        private System.Windows.Forms.Label lblCover;
        private System.Windows.Forms.Label lblLyrics;

        private CustomTrackBar tbProgreso;
        private CustomTrackBar tbVolumen;

        private System.Windows.Forms.Label lblTiempoActual;
        private System.Windows.Forms.Label lblTiempoTotal;
        private System.Windows.Forms.Label lblVolumen;

        private System.Windows.Forms.Timer timer1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(Form1));

            // Instancias
            dgvCanciones = new System.Windows.Forms.DataGridView();
            btnAbrirCarpeta = new CustomButton();
            btnPlayPause = new CustomButton();
            btnStop = new CustomButton();
            btnPrev = new CustomButton();
            btnNext = new CustomButton();
            btnAleatorio = new CustomButton();
            btnRepetir = new CustomButton();
            btnGuardarPlaylist = new CustomButton();
            btnCargarPlaylist = new CustomButton();
            btnAbrirWMP = new CustomButton();
            btnLetra = new CustomButton();
            btnEliminarCancion = new CustomButton();
            btnEditarTags = new CustomButton();
            pbCover = new System.Windows.Forms.PictureBox();
            rtbLyrics = new System.Windows.Forms.RichTextBox();
            lblCanciones = new System.Windows.Forms.Label();
            lblCover = new System.Windows.Forms.Label();
            lblLyrics = new System.Windows.Forms.Label();
            tbProgreso = new CustomTrackBar();
            tbVolumen = new CustomTrackBar();
            lblTiempoActual = new System.Windows.Forms.Label();
            lblTiempoTotal = new System.Windows.Forms.Label();
            lblVolumen = new System.Windows.Forms.Label();
            timer1 = new System.Windows.Forms.Timer(components);

            ((System.ComponentModel.ISupportInitialize)dgvCanciones).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbCover).BeginInit();
            SuspendLayout();

            // ── dgvCanciones ──────────────────────────────────────────────────
            dgvCanciones.AllowUserToAddRows = false;
            dgvCanciones.AllowUserToDeleteRows = false;
            dgvCanciones.Anchor = System.Windows.Forms.AnchorStyles.Top
                                                     | System.Windows.Forms.AnchorStyles.Bottom
                                                     | System.Windows.Forms.AnchorStyles.Left
                                                     | System.Windows.Forms.AnchorStyles.Right;
            dgvCanciones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvCanciones.BackgroundColor = System.Drawing.Color.FromArgb(28, 28, 28);
            dgvCanciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCanciones.Location = new System.Drawing.Point(14, 107);
            dgvCanciones.MultiSelect = false;
            dgvCanciones.Name = "dgvCanciones";
            dgvCanciones.ReadOnly = true;
            dgvCanciones.RowHeadersVisible = false;
            dgvCanciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvCanciones.Size = new System.Drawing.Size(766, 355);
            dgvCanciones.TabIndex = 0;
            dgvCanciones.CellDoubleClick += dgvCanciones_CellDoubleClick;

            // ── FILA 1: Controles de sesión (Y=14) ───────────────────────────

            // btnAbrirCarpeta
            btnAbrirCarpeta.Location = new System.Drawing.Point(14, 14);
            btnAbrirCarpeta.Size = new System.Drawing.Size(155, 46);
            btnAbrirCarpeta.Text = "📂  Abrir Carpeta";
            btnAbrirCarpeta.NormalColor = System.Drawing.Color.FromArgb(38, 38, 38);
            btnAbrirCarpeta.HoverColor = System.Drawing.Color.FromArgb(58, 58, 58);
            btnAbrirCarpeta.PressColor = System.Drawing.Color.FromArgb(20, 20, 20);
            btnAbrirCarpeta.BorderColor = System.Drawing.Color.FromArgb(72, 72, 72);
            btnAbrirCarpeta.ForeColor = System.Drawing.Color.White;
            btnAbrirCarpeta.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnAbrirCarpeta.CornerRadius = 8;
            btnAbrirCarpeta.Name = "btnAbrirCarpeta";
            btnAbrirCarpeta.TabIndex = 1;
            btnAbrirCarpeta.Click += btnAbrirCarpeta_Click;

            // btnPlayPause — acento verde, más grande
            btnPlayPause.Location = new System.Drawing.Point(182, 14);
            btnPlayPause.Size = new System.Drawing.Size(148, 46);
            btnPlayPause.Text = "▶   Play";
            btnPlayPause.NormalColor = System.Drawing.Color.FromArgb(30, 160, 30);
            btnPlayPause.HoverColor = System.Drawing.Color.FromArgb(40, 200, 40);
            btnPlayPause.PressColor = System.Drawing.Color.FromArgb(18, 110, 18);
            btnPlayPause.BorderColor = System.Drawing.Color.FromArgb(20, 130, 20);
            btnPlayPause.ForeColor = System.Drawing.Color.White;
            btnPlayPause.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            btnPlayPause.CornerRadius = 8;
            btnPlayPause.Name = "btnPlayPause";
            btnPlayPause.TabIndex = 2;
            btnPlayPause.Click += btnPlayPause_Click;

            // btnStop
            btnStop.Location = new System.Drawing.Point(343, 14);
            btnStop.Size = new System.Drawing.Size(90, 46);
            btnStop.Text = "⏹  Stop";
            btnStop.NormalColor = System.Drawing.Color.FromArgb(38, 38, 38);
            btnStop.HoverColor = System.Drawing.Color.FromArgb(90, 28, 28);
            btnStop.PressColor = System.Drawing.Color.FromArgb(60, 15, 15);
            btnStop.BorderColor = System.Drawing.Color.FromArgb(72, 72, 72);
            btnStop.ForeColor = System.Drawing.Color.FromArgb(210, 210, 210);
            btnStop.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnStop.CornerRadius = 8;
            btnStop.Name = "btnStop";
            btnStop.TabIndex = 3;
            btnStop.Click += btnStop_Click;

            // btnPrev
            btnPrev.Location = new System.Drawing.Point(446, 14);
            btnPrev.Size = new System.Drawing.Size(90, 46);
            btnPrev.Text = "⏮  Prev";
            btnPrev.NormalColor = System.Drawing.Color.FromArgb(38, 38, 38);
            btnPrev.HoverColor = System.Drawing.Color.FromArgb(58, 58, 58);
            btnPrev.PressColor = System.Drawing.Color.FromArgb(20, 20, 20);
            btnPrev.BorderColor = System.Drawing.Color.FromArgb(72, 72, 72);
            btnPrev.ForeColor = System.Drawing.Color.White;
            btnPrev.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnPrev.CornerRadius = 8;
            btnPrev.Name = "btnPrev";
            btnPrev.TabIndex = 4;
            btnPrev.Click += btnPrev_Click;

            // btnNext
            btnNext.Location = new System.Drawing.Point(549, 14);
            btnNext.Size = new System.Drawing.Size(90, 46);
            btnNext.Text = "Next ⏭";
            btnNext.NormalColor = System.Drawing.Color.FromArgb(38, 38, 38);
            btnNext.HoverColor = System.Drawing.Color.FromArgb(58, 58, 58);
            btnNext.PressColor = System.Drawing.Color.FromArgb(20, 20, 20);
            btnNext.BorderColor = System.Drawing.Color.FromArgb(72, 72, 72);
            btnNext.ForeColor = System.Drawing.Color.White;
            btnNext.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnNext.CornerRadius = 8;
            btnNext.Name = "btnNext";
            btnNext.TabIndex = 5;
            btnNext.Click += btnNext_Click;

            // ── lblCanciones ──────────────────────────────────────────────────
            lblCanciones.AutoSize = true;
            lblCanciones.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblCanciones.ForeColor = System.Drawing.Color.White;
            lblCanciones.BackColor = System.Drawing.Color.Transparent;
            lblCanciones.Location = new System.Drawing.Point(14, 76);
            lblCanciones.Name = "lblCanciones";
            lblCanciones.TabIndex = 6;
            lblCanciones.Text = "Lista de Canciones:";

            // ── tbProgreso ────────────────────────────────────────────────────
            tbProgreso.Location = new System.Drawing.Point(14, 480);
            tbProgreso.Name = "tbProgreso";
            tbProgreso.Size = new System.Drawing.Size(766, 22);
            tbProgreso.Minimum = 0;
            tbProgreso.Maximum = 100;
            tbProgreso.Value = 0;
            tbProgreso.TrackColor = System.Drawing.Color.FromArgb(65, 65, 65);
            tbProgreso.FillColor = System.Drawing.Color.LimeGreen;
            tbProgreso.ThumbColor = System.Drawing.Color.White;
            tbProgreso.ThumbHoverColor = System.Drawing.Color.LimeGreen;
            tbProgreso.TabIndex = 7;
            tbProgreso.MouseDown += tbProgreso_MouseDown;
            tbProgreso.MouseUp += tbProgreso_MouseUp;

            // ── lblTiempoActual ───────────────────────────────────────────────
            lblTiempoActual.AutoSize = true;
            lblTiempoActual.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblTiempoActual.ForeColor = System.Drawing.Color.LimeGreen;
            lblTiempoActual.BackColor = System.Drawing.Color.Transparent;
            lblTiempoActual.Location = new System.Drawing.Point(14, 507);
            lblTiempoActual.Name = "lblTiempoActual";
            lblTiempoActual.TabIndex = 8;
            lblTiempoActual.Text = "00:00";

            // ── lblTiempoTotal ────────────────────────────────────────────────
            lblTiempoTotal.AutoSize = true;
            lblTiempoTotal.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblTiempoTotal.ForeColor = System.Drawing.Color.FromArgb(150, 150, 150);
            lblTiempoTotal.BackColor = System.Drawing.Color.Transparent;
            lblTiempoTotal.Location = new System.Drawing.Point(735, 507);
            lblTiempoTotal.Name = "lblTiempoTotal";
            lblTiempoTotal.TabIndex = 9;
            lblTiempoTotal.Text = "00:00";

            // ── lblVolumen ────────────────────────────────────────────────────
            lblVolumen.AutoSize = true;
            lblVolumen.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblVolumen.ForeColor = System.Drawing.Color.FromArgb(150, 150, 150);
            lblVolumen.BackColor = System.Drawing.Color.Transparent;
            lblVolumen.Location = new System.Drawing.Point(14, 540);
            lblVolumen.Name = "lblVolumen";
            lblVolumen.TabIndex = 10;
            lblVolumen.Text = "🔊  Volumen";

            // ── tbVolumen ─────────────────────────────────────────────────────
            tbVolumen.Location = new System.Drawing.Point(110, 538);
            tbVolumen.Name = "tbVolumen";
            tbVolumen.Size = new System.Drawing.Size(300, 22);
            tbVolumen.Minimum = 0;
            tbVolumen.Maximum = 100;
            tbVolumen.Value = 50;
            tbVolumen.TrackColor = System.Drawing.Color.FromArgb(65, 65, 65);
            tbVolumen.FillColor = System.Drawing.Color.LimeGreen;
            tbVolumen.ThumbColor = System.Drawing.Color.White;
            tbVolumen.ThumbHoverColor = System.Drawing.Color.LimeGreen;
            tbVolumen.TabIndex = 11;
            tbVolumen.ValueChanged += tbVolumen_ValueChanged;

            // ── FILA 3: Aleatorio + Repetir (Y=530) ──────────────────────────

            // btnAleatorio — apagado: gris, encendido: verde (se cambia en código)
            btnAleatorio.Location = new System.Drawing.Point(432, 528);
            btnAleatorio.Size = new System.Drawing.Size(128, 36);
            btnAleatorio.Text = "🔀  Aleatorio";
            btnAleatorio.NormalColor = System.Drawing.Color.FromArgb(38, 38, 38);
            btnAleatorio.HoverColor = System.Drawing.Color.FromArgb(58, 58, 58);
            btnAleatorio.PressColor = System.Drawing.Color.FromArgb(20, 20, 20);
            btnAleatorio.BorderColor = System.Drawing.Color.FromArgb(72, 72, 72);
            btnAleatorio.ForeColor = System.Drawing.Color.FromArgb(155, 155, 155);
            btnAleatorio.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnAleatorio.CornerRadius = 18;  // completamente redondeado (pill)
            btnAleatorio.Name = "btnAleatorio";
            btnAleatorio.TabIndex = 20;
            btnAleatorio.Click += btnAleatorio_Click;

            // btnRepetir
            btnRepetir.Location = new System.Drawing.Point(572, 528);
            btnRepetir.Size = new System.Drawing.Size(128, 36);
            btnRepetir.Text = "🔁  Repetir";
            btnRepetir.NormalColor = System.Drawing.Color.FromArgb(38, 38, 38);
            btnRepetir.HoverColor = System.Drawing.Color.FromArgb(58, 58, 58);
            btnRepetir.PressColor = System.Drawing.Color.FromArgb(20, 20, 20);
            btnRepetir.BorderColor = System.Drawing.Color.FromArgb(72, 72, 72);
            btnRepetir.ForeColor = System.Drawing.Color.FromArgb(155, 155, 155);
            btnRepetir.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnRepetir.CornerRadius = 18;
            btnRepetir.Name = "btnRepetir";
            btnRepetir.TabIndex = 21;
            btnRepetir.Click += btnRepetir_Click;

            // ── FILA 4: Playlist + WMP + Letra (Y=576) ───────────────────────

            btnGuardarPlaylist.Location = new System.Drawing.Point(14, 576);
            btnGuardarPlaylist.Size = new System.Drawing.Size(155, 42);
            btnGuardarPlaylist.Text = "💾  Guardar Playlist";
            btnGuardarPlaylist.NormalColor = System.Drawing.Color.FromArgb(38, 38, 38);
            btnGuardarPlaylist.HoverColor = System.Drawing.Color.FromArgb(58, 58, 58);
            btnGuardarPlaylist.PressColor = System.Drawing.Color.FromArgb(20, 20, 20);
            btnGuardarPlaylist.BorderColor = System.Drawing.Color.FromArgb(72, 72, 72);
            btnGuardarPlaylist.ForeColor = System.Drawing.Color.White;
            btnGuardarPlaylist.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnGuardarPlaylist.CornerRadius = 8;
            btnGuardarPlaylist.Name = "btnGuardarPlaylist";
            btnGuardarPlaylist.TabIndex = 12;
            btnGuardarPlaylist.Click += btnGuardarPlaylist_Click;

            btnCargarPlaylist.Location = new System.Drawing.Point(182, 576);
            btnCargarPlaylist.Size = new System.Drawing.Size(155, 42);
            btnCargarPlaylist.Text = "📂  Cargar Playlist";
            btnCargarPlaylist.NormalColor = System.Drawing.Color.FromArgb(38, 38, 38);
            btnCargarPlaylist.HoverColor = System.Drawing.Color.FromArgb(58, 58, 58);
            btnCargarPlaylist.PressColor = System.Drawing.Color.FromArgb(20, 20, 20);
            btnCargarPlaylist.BorderColor = System.Drawing.Color.FromArgb(72, 72, 72);
            btnCargarPlaylist.ForeColor = System.Drawing.Color.White;
            btnCargarPlaylist.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnCargarPlaylist.CornerRadius = 8;
            btnCargarPlaylist.Name = "btnCargarPlaylist";
            btnCargarPlaylist.TabIndex = 13;
            btnCargarPlaylist.Click += btnCargarPlaylist_Click;

            btnAbrirWMP.Location = new System.Drawing.Point(350, 576);
            btnAbrirWMP.Size = new System.Drawing.Size(155, 42);
            btnAbrirWMP.Text = "🎵  Abrir en WMP";
            btnAbrirWMP.NormalColor = System.Drawing.Color.FromArgb(38, 38, 38);
            btnAbrirWMP.HoverColor = System.Drawing.Color.FromArgb(58, 58, 58);
            btnAbrirWMP.PressColor = System.Drawing.Color.FromArgb(20, 20, 20);
            btnAbrirWMP.BorderColor = System.Drawing.Color.FromArgb(72, 72, 72);
            btnAbrirWMP.ForeColor = System.Drawing.Color.White;
            btnAbrirWMP.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnAbrirWMP.CornerRadius = 8;
            btnAbrirWMP.Name = "btnAbrirWMP";
            btnAbrirWMP.TabIndex = 14;
            btnAbrirWMP.Click += btnAbrirWMP_Click;

            btnLetra.Location = new System.Drawing.Point(518, 576);
            btnLetra.Size = new System.Drawing.Size(155, 42);
            btnLetra.Text = "🎤  Buscar Letra";
            btnLetra.NormalColor = System.Drawing.Color.FromArgb(38, 38, 38);
            btnLetra.HoverColor = System.Drawing.Color.FromArgb(58, 58, 58);
            btnLetra.PressColor = System.Drawing.Color.FromArgb(20, 20, 20);
            btnLetra.BorderColor = System.Drawing.Color.FromArgb(72, 72, 72);
            btnLetra.ForeColor = System.Drawing.Color.White;
            btnLetra.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnLetra.CornerRadius = 8;
            btnLetra.Name = "btnLetra";
            btnLetra.TabIndex = 15;
            btnLetra.Click += btnLetra_Click;

            // ── Panel derecho — Eliminar / Editar Tags ────────────────────────

            btnEliminarCancion.Location = new System.Drawing.Point(800, 14);
            btnEliminarCancion.Size = new System.Drawing.Size(128, 46);
            btnEliminarCancion.Text = "❌  Eliminar";
            btnEliminarCancion.NormalColor = System.Drawing.Color.FromArgb(110, 25, 25);
            btnEliminarCancion.HoverColor = System.Drawing.Color.FromArgb(160, 35, 35);
            btnEliminarCancion.PressColor = System.Drawing.Color.FromArgb(75, 15, 15);
            btnEliminarCancion.BorderColor = System.Drawing.Color.FromArgb(140, 35, 35);
            btnEliminarCancion.ForeColor = System.Drawing.Color.White;
            btnEliminarCancion.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnEliminarCancion.CornerRadius = 8;
            btnEliminarCancion.Name = "btnEliminarCancion";
            btnEliminarCancion.TabIndex = 22;
            btnEliminarCancion.Click += btnEliminarCancion_Click;

            btnEditarTags.Location = new System.Drawing.Point(942, 14);
            btnEditarTags.Size = new System.Drawing.Size(128, 46);
            btnEditarTags.Text = "✏️  Editar Tags";
            btnEditarTags.NormalColor = System.Drawing.Color.FromArgb(22, 72, 130);
            btnEditarTags.HoverColor = System.Drawing.Color.FromArgb(32, 100, 175);
            btnEditarTags.PressColor = System.Drawing.Color.FromArgb(14, 50, 95);
            btnEditarTags.BorderColor = System.Drawing.Color.FromArgb(38, 90, 155);
            btnEditarTags.ForeColor = System.Drawing.Color.White;
            btnEditarTags.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnEditarTags.CornerRadius = 8;
            btnEditarTags.Name = "btnEditarTags";
            btnEditarTags.TabIndex = 23;
            btnEditarTags.Click += btnEditarTags_Click;

            // ── pbCover ───────────────────────────────────────────────────────
            pbCover.Anchor = System.Windows.Forms.AnchorStyles.Top
                                | System.Windows.Forms.AnchorStyles.Bottom
                                | System.Windows.Forms.AnchorStyles.Left
                                | System.Windows.Forms.AnchorStyles.Right;
            pbCover.BackColor = System.Drawing.Color.FromArgb(28, 28, 28);
            pbCover.BorderStyle = System.Windows.Forms.BorderStyle.None;
            pbCover.Location = new System.Drawing.Point(800, 75);
            pbCover.Name = "pbCover";
            pbCover.Size = new System.Drawing.Size(270, 330);
            pbCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pbCover.TabStop = false;

            // ── lblCover ──────────────────────────────────────────────────────
            lblCover.AutoSize = true;
            lblCover.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblCover.ForeColor = System.Drawing.Color.White;
            lblCover.BackColor = System.Drawing.Color.Transparent;
            lblCover.Location = new System.Drawing.Point(800, 48);
            lblCover.Name = "lblCover";
            lblCover.Text = "Portada:";

            // ── lblLyrics ─────────────────────────────────────────────────────
            lblLyrics.AutoSize = true;
            lblLyrics.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblLyrics.ForeColor = System.Drawing.Color.White;
            lblLyrics.BackColor = System.Drawing.Color.Transparent;
            lblLyrics.Location = new System.Drawing.Point(800, 420);
            lblLyrics.Name = "lblLyrics";
            lblLyrics.Text = "Letra:";

            // ── rtbLyrics ─────────────────────────────────────────────────────
            rtbLyrics.BackColor = System.Drawing.Color.FromArgb(22, 22, 22);
            rtbLyrics.ForeColor = System.Drawing.Color.White;
            rtbLyrics.BorderStyle = System.Windows.Forms.BorderStyle.None;
            rtbLyrics.Font = new System.Drawing.Font("Segoe UI", 10F);
            rtbLyrics.Location = new System.Drawing.Point(800, 448);
            rtbLyrics.Name = "rtbLyrics";
            rtbLyrics.Size = new System.Drawing.Size(270, 170);
            rtbLyrics.Text = "";

            // ── timer1 ────────────────────────────────────────────────────────
            timer1.Tick += timer1_Tick;

            // ── Form1 ─────────────────────────────────────────────────────────
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            BackgroundImage = (System.Drawing.Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            ClientSize = new System.Drawing.Size(1090, 636);

            Controls.Add(btnEditarTags);
            Controls.Add(btnEliminarCancion);
            Controls.Add(btnRepetir);
            Controls.Add(btnAleatorio);
            Controls.Add(rtbLyrics);
            Controls.Add(lblLyrics);
            Controls.Add(lblCover);
            Controls.Add(pbCover);
            Controls.Add(btnLetra);
            Controls.Add(btnAbrirWMP);
            Controls.Add(btnCargarPlaylist);
            Controls.Add(btnGuardarPlaylist);
            Controls.Add(tbVolumen);
            Controls.Add(lblVolumen);
            Controls.Add(lblTiempoTotal);
            Controls.Add(lblTiempoActual);
            Controls.Add(tbProgreso);
            Controls.Add(lblCanciones);
            Controls.Add(btnNext);
            Controls.Add(btnPrev);
            Controls.Add(btnStop);
            Controls.Add(btnPlayPause);
            Controls.Add(btnAbrirCarpeta);
            Controls.Add(dgvCanciones);

            DoubleBuffered = true;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Stopify Music Player";

            ((System.ComponentModel.ISupportInitialize)dgvCanciones).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbCover).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}